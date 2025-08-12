using AutoMapper;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Application.Mapping;
using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Repositories
{
    public class RoomServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IRepository<Address>> _addressRepositoryMock;
        private readonly Mock<IRepository<Activity>> _activityRepositorryMock;
        private readonly IMapper _mapper;
        private readonly RoomService _service;

        // Fix Guids for consistency
        private readonly Guid Room1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        private readonly Guid Room2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        private readonly Guid Address1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        private readonly Guid Address2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

        // Test Data
        List<Room> rooms;
        CreateRoomRequest request;

        public RoomServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _addressRepositoryMock = new Mock<IRepository<Address>>();
            _activityRepositorryMock = new Mock<IRepository<Activity>>();

            _unitOfWorkMock.Setup(u => u.Address).Returns(_addressRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Activities).Returns(_activityRepositorryMock.Object);
            _unitOfWorkMock.Setup(u => u.Rooms).Returns(_roomRepositoryMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
            });
            _mapper = config.CreateMapper();
            _service = new RoomService(_unitOfWorkMock.Object, _mapper);

            // Initialize test data with fixed Guids
            rooms = new List<Room>
        {
            new Room { RoomId = Room1Id, Name = "Room 1", AddressId = Address1Id },
            new Room { RoomId = Room2Id, Name = "Room 2", AddressId = Address2Id }
        };

            request = new CreateRoomRequest
            {
                Name = "Test Room",
                Description = "Description",
                PricePerHour = 15.5,
                Status = "Available",
                AddressId = Address1Id,
                Address = new AddressResponse
                {
                    AddressId = Address1Id,
                    City = "Narva",
                    Region = "Ida-Virumaa",
                    AddressName = "Gagarini 11",
                    PostalCode = "12341",
                    Country = "Estonia",
                    PhoneNumber = "+37254356533"
                },
                Activities = new List<ActivityResponse>
            {
                new ActivityResponse { Name = "test", Description = "Test" }
            }
            };
        }

        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenExist()
        {
            // Arrange
            _roomRepositoryMock.Setup(r => r.GetByIdAsync(Room2Id, CancellationToken.None)).ReturnsAsync(rooms[1]);

            // Act
            var result = await _service.GetRoomByIdAsync(Room2Id, CancellationToken.None);

            // Assert
            Assert.Equal(Room2Id, result.RoomId);
        }

        [Fact]
        public async Task GetAllRoomsAsync_ShouldReturnMappedRoomList_WhenExist()
        {
            // Arrange
            _roomRepositoryMock.Setup(r => r.GetAllAsync(CancellationToken.None)).ReturnsAsync(rooms);

            // Act
            var result = await _service.GetAllRoomsAsync(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async Task CreateRoomAsync_ShouldReturnMappedRoomResponse_WhenCreated()
        {
            // Arrange
            Room capturedRoom = null;

            _addressRepositoryMock
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Address, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _activityRepositorryMock
                .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Activity, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _roomRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .Callback<Room>(room => capturedRoom = room) // ✅ Удалена лишняя запятая
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateRoomAsync(request, CancellationToken.None);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.NotNull(capturedRoom);
            _roomRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRoomAsync_ShouldReturnNull_WhenRoomNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(r => r.Rooms.GetByIdAsync(Room1Id, It.IsAny<CancellationToken>())).ReturnsAsync((Room)null);

            // Act
            var result = await _service.UpdateRoomAsync(Room1Id, request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteRoomAsync_ShouldReturnFalse_WhenNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(r => r.Rooms.GetByIdAsync(Room1Id, CancellationToken.None)).ReturnsAsync((Room)null);

            // Act
            var result = await _service.DeleteRoomAsync(Room1Id, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }


}
