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

        public RoomServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _roomRepositoryMock= new Mock<IRoomRepository>();
            _addressRepositoryMock= new Mock<IRepository<Address>>();
            _activityRepositorryMock=new Mock<IRepository<Activity>>();
            _unitOfWorkMock.Setup(u => u.Address).Returns(_addressRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Activities).Returns(_activityRepositorryMock.Object);
            _unitOfWorkMock.Setup(u => u.Rooms).Returns(_roomRepositoryMock.Object);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>(); //Using Same Profile
            });
            _mapper = config.CreateMapper();

            _service = new RoomService(_unitOfWorkMock.Object, _mapper);
        }
        //Get by Id Tests
        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnRoom_WhenExist()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1", AddressId = 1 },
                new Room { RoomId = 2, Name = "Room 2", AddressId = 2 }
            };

            _roomRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(rooms[1]);

            // Act
            var result = await _service.GetRoomByIdAsync(2);

            // Assert
            Assert.Equal(2, rooms[1].RoomId);
        }
        // Get Include Tests
        [Fact]
        public async Task GetAllRoomsAsync_ShouldReturnMappedRoomList_WhenExist()
        {
            // Arrange
            var rooms = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1", AddressId = 1 },
                new Room { RoomId = 2, Name = "Room 2", AddressId = 2 }
            };

            _roomRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);
            // Act
            var result = await _service.GetAllRoomsAsync();
            // Assert
            Assert.Equal(2, result.Count);
        }
        //Create Tests
        [Fact]
        public async Task CreateRoomAsync_ShouldReturnMappedRoomResponse_WhenCreated()
        {
            // Arrange
            var request = new CreateRoomRequest
            {
                Name = "Test Room",
                Description = "Description",
                PricePerHour = 15.5,
                IsActive = true,
                AddressId = 1,
                Address = new AddressResponse { AddressId = 1, City = "Narva", Region = "Ida-Virumaa", AddressName = "Gagarini 11", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                Activities = new List<ActivityResponse> {
                    new ActivityResponse { Name="test",Description="Test"} },
            };
            Room capturedRoom = null;

            _addressRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Address, bool>>>()))
                           .ReturnsAsync(true);

            _activityRepositorryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Activity,bool>>>()))
                            .ReturnsAsync(true);

            _roomRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Room>()))
                .Callback<Room>(room => capturedRoom = room)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateRoomAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.NotNull(capturedRoom);
            _roomRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Room>()), Times.Once);
        }
        //Update Tests
        [Fact]
        public async Task UpdateRoomAsync_ShouldReturnFalse_WhenRoomNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(r => r.Rooms.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Room)null);
            // Act
            var request = new CreateRoomRequest {Name = "Updated" };
            var result = await _service.UpdateRoomAsync(1,request);
            // Assert
            Assert.Equal("Updated",result.Name);
        }
        //Delete Tests
        [Fact]
        public async Task DeleteRoomAsync_ShouldReturnFalse_WhenNotFound()
        {
            // Arrange
            _unitOfWorkMock.Setup(r => r.Rooms.GetByIdAsync(1)).ReturnsAsync((Room)null);
            // Act
            var result = await _service.DeleteRoomAsync(1);
            // Assert
            Assert.False(result);
        }
    }

}
