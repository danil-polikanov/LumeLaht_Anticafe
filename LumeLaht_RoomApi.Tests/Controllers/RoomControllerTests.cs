using AutoMapper;
using LumaCove_RoomApi.Controllers;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Application.Mapping;
using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly Mock<ILogger<RoomController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _loggerMock = new Mock<ILogger<RoomController>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
            });
            _mapper = config.CreateMapper();

            _controller = new RoomController(_mapper, _roomServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRoomIsNull()
        {
            _roomServiceMock
                .Setup(s => s.GetRoomByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoomResponse)null);

            var result = await _controller.GetRoomById(Guid.NewGuid(), CancellationToken.None);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnFirstRoom_WhenRoomIsFirst()
        {
            var room = new RoomResponse { RoomId = Guid.NewGuid(), Name = "Room 1" };
            _roomServiceMock
                .Setup(s => s.GetRoomByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            var result = await _controller.GetRoomById(room.RoomId, CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoom = Assert.IsType<RoomResponse>(okResult.Value);
            Assert.Equal("Room 1", returnedRoom.Name);
        }

        [Fact]
        public async Task GetRooms_ShouldReturnRooms_WhenRoomExist()
        {
            var rooms = new List<RoomResponse>
        {
            new RoomResponse { RoomId = Guid.NewGuid(), Name = "Room" },
            new RoomResponse { RoomId = Guid.NewGuid(), Name = "Room 2" }
        };

            _roomServiceMock
                .Setup(s => s.GetAllRoomsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rooms);

            var result = await _controller.GetAll(CancellationToken.None);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<RoomResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task CreateRoom_ShouldCreateRoom_WhenRoomCreated()
        {
            var request = new CreateRoomRequest { Name = "Room", AddressId = Guid.NewGuid() };
            var response = new RoomResponse { RoomId = Guid.NewGuid(), Name = "Room" };

            _roomServiceMock
                .Setup(s => s.CreateRoomAsync(It.IsAny<CreateRoomRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await _controller.Create(request, CancellationToken.None);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdRoom = Assert.IsType<RoomResponse>(createdResult.Value);
            Assert.Equal("Room", createdRoom.Name);
        }
    }
}
