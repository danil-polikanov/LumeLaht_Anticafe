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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Repositories
{
    public class RoomServiceTests
    {
        private readonly Mock<IRoomRepository> _roomRepoMock;
        private readonly IMapper _mapper;
        private readonly RoomService _service;

        public RoomServiceTests()
        {
            _roomRepoMock = new Mock<IRoomRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>(); //Using Same Profile
            });
            _mapper = config.CreateMapper();

            _service = new RoomService(_roomRepoMock.Object, _mapper);
        }
        //Get by Id Tests
        [Fact]
        public async Task GetRoomByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Room)null);

            // Act
            var result = await _service.GetRoomByIdAsync(123);

            // Assert
            Assert.Null(result);
        }
        // Get Include Tests

        [Fact]
        public async Task GetAllRoomsAsync_ShouldReturnMappedRoomList()
        {        
            // Arrange
            var rooms = new List<Room>
            {
                new Room { RoomId = 1, Name = "Room 1", AddressId = 1 },
                new Room { RoomId = 2, Name = "Room 2", AddressId = 2 }
            };
            _roomRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(rooms);

            // Act
            var result = await _service.GetAllRoomsAsync();
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Room 1", result[0].Name);
        }
        //Create Tests
        [Fact]
        public async Task CreateRoomAsync_ShouldReturnMappedRoomResponse()
        {
            // Arrange
            var request = new CreateRoomRequest
            {
                Name = "Test Room",
                Description = "Description",
                PricePerHour = 15.5,
                IsActive = true,
                AddressId = 1
            };

            Room capturedRoom = null;
            _roomRepoMock.Setup(r => r.AddAsync(It.IsAny<Room>()))
                .Callback<Room>(room => capturedRoom = room)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateRoomAsync(request);

            // Assert
            Assert.Equal(request.Name, result.Name);
            Assert.NotNull(capturedRoom);
            _roomRepoMock.Verify(r => r.AddAsync(It.IsAny<Room>()), Times.Once);
        }

        //Update Tests
        [Fact]
        public async Task UpdateRoomAsync_ShouldReturnFalse_WhenRoomNotFound()
        {
            // Arrange
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Room)null);
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
            _roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Room)null);
            // Act
            var result = await _service.DeleteRoomAsync(1);
            // Assert
            Assert.False(result);
        }
    }

}
