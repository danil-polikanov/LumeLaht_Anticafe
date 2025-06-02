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
        //GetRoomById
        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenRoomIsNull()
        {

            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>())).ReturnsAsync((RoomResponse)null);

            var result = await _controller.GetRoomById(1);

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task GetById_ShouldReturnFirstRoom_WhenRoomIsFirst()
        {
            var room = new RoomResponse { RoomId = 1, Name = "Room 1" };
            _roomServiceMock.Setup(s => s.GetRoomByIdAsync(It.IsAny<int>())).ReturnsAsync(room);

            var result = await _controller.GetRoomById(1);

            Assert.IsType <OkObjectResult>(result);
        }
        //GetAllRooms
        [Fact]
        public async Task GetRooms_ShouldReturnRooms_WhenRoomExist()
        {
            var rooms = new List<RoomResponse>
            {
                 new RoomResponse { RoomId = 1, Name = "Room" },
                 new RoomResponse { RoomId = 2, Name = "Room 2" }
            };
            _roomServiceMock.Setup(s => s.GetAllRoomsAsync()).ReturnsAsync(rooms);

            var result = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<RoomResponse>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }
        //CreateRoom
        [Fact]
        public async Task CreateRoom_ShouldCreateRoom_WhenRoomCreated()
        {
            var request = new CreateRoomRequest { Name = "Room", AddressId = 1 };
            var response = new RoomResponse { RoomId = 1, Name = "Room" };
            _roomServiceMock.Setup(s => s.CreateRoomAsync(It.IsAny<CreateRoomRequest>())).ReturnsAsync(response);

            var result = await _controller.Create(request);
            
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
