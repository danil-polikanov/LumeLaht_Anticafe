using Mapster;
using MapsterMapper;
using LumaCove_RoomApi.Controllers;
using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Application.IServices;
using LumeLaht_RoomApi.Application.Mapping;
using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
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
    /// <summary>
    /// Полные тесты для RoomController
    /// </summary>
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly Mock<IActivityService> _activityServiceMock;
        private readonly Mock<ILogger<RoomController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly RoomController _controller;

        // Фиксированные Guid
        private readonly Guid Room1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        private readonly Guid Room2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

        public RoomControllerTests()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _activityServiceMock = new Mock<IActivityService>();
            _loggerMock = new Mock<ILogger<RoomController>>();

            var config = new TypeAdapterConfig();
            new RoomProfile().Register(config);
            _mapper = new Mapper(config);

            _controller = new RoomController(
                _mapper,
                _roomServiceMock.Object,
                _activityServiceMock.Object,
                _loggerMock.Object);
        }

        #region GetAll Tests

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoRoomsExist()
        {
            // Arrange
            _roomServiceMock
                .Setup(s => s.GetAllRoomsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<RoomResponse>)null);

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithRooms_WhenRoomsExist()
        {
            // Arrange
            var rooms = new List<RoomResponse>
            {
                new RoomResponse { RoomId = Room1Id, Name = "Room 1" },
                new RoomResponse { RoomId = Room2Id, Name = "Room 2" }
            };

            _roomServiceMock
                .Setup(s => s.GetAllRoomsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rooms);

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRooms = Assert.IsAssignableFrom<IEnumerable<RoomResponse>>(okResult.Value);
            Assert.Equal(2, returnedRooms.Count());
            _roomServiceMock.Verify(s => s.GetAllRoomsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoRooms()
        {
            // Arrange
            _roomServiceMock
                .Setup(s => s.GetAllRoomsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RoomResponse>());

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedRooms = Assert.IsAssignableFrom<IEnumerable<RoomResponse>>(okResult.Value);
            Assert.Empty(returnedRooms);
        }

        #endregion

        #region GetRoomById Tests

        [Fact]
        public async Task GetRoomById_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            _roomServiceMock
                .Setup(s => s.GetRoomByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoomResponse)null);

            // Act
            var result = await _controller.GetRoomById(Room1Id, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetRoomById_ReturnsOkWithRoom_WhenRoomExists()
        {
            // Arrange
            var room = new RoomResponse
            {
                RoomId = Room1Id,
                Name = "Test Room",
                Description = "Test Description",
                PricePerHour = 100
            };

            _roomServiceMock
                .Setup(s => s.GetRoomByIdAsync(Room1Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            // Act
            var result = await _controller.GetRoomById(Room1Id, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoom = Assert.IsType<RoomResponse>(okResult.Value);
            Assert.Equal(Room1Id, returnedRoom.RoomId);
            Assert.Equal("Test Room", returnedRoom.Name);
            _roomServiceMock.Verify(s => s.GetRoomByIdAsync(Room1Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")]
        public async Task GetRoomById_AcceptsAnyValidGuid(string guidString)
        {
            // Arrange
            var guid = Guid.Parse(guidString);
            _roomServiceMock
                .Setup(s => s.GetRoomByIdAsync(guid, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoomResponse)null);

            // Act
            var result = await _controller.GetRoomById(guid, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.Create(null, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Request is null", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenRoomIsCreated()
        {
            // Arrange
            var request = new CreateRoomRequest
            {
                Name = "New Room",
                Description = "Test",
                PricePerHour = 100,
                Status = "Available",
                AddressId = Guid.NewGuid()
            };

            var createdRoom = new RoomResponse
            {
                RoomId = Room1Id,
                Name = "New Room",
                Description = "Test",
                PricePerHour = 100
            };

            _roomServiceMock
                .Setup(s => s.CreateRoomAsync(It.IsAny<CreateRoomRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdRoom);

            // Act
            var result = await _controller.Create(request, CancellationToken.None);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetRoomById), createdResult.ActionName);
            Assert.Equal(Room1Id, ((dynamic)createdResult.RouteValues)["id"]);

            var returnedRoom = Assert.IsType<RoomResponse>(createdResult.Value);
            Assert.Equal("New Room", returnedRoom.Name);
            _roomServiceMock.Verify(s => s.CreateRoomAsync(It.IsAny<CreateRoomRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsRoomWithGeneratedId()
        {
            // Arrange
            var request = new CreateRoomRequest { Name = "Test" };
            var generatedId = Guid.NewGuid();

            _roomServiceMock
                .Setup(s => s.CreateRoomAsync(It.IsAny<CreateRoomRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RoomResponse { RoomId = generatedId, Name = "Test" });

            // Act
            var result = await _controller.Create(request, CancellationToken.None);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var room = Assert.IsType<RoomResponse>(createdResult.Value);
            Assert.Equal(generatedId, room.RoomId);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdIsEmpty()
        {
            // Arrange
            var request = new CreateRoomRequest { Name = "Test" };

            // Act
            var result = await _controller.Update(Guid.Empty, request, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Room ID", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Act
            var result = await _controller.Update(Room1Id, null, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Request is null", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            var request = new CreateRoomRequest { Name = "Test" };

            _roomServiceMock
                .Setup(s => s.UpdateRoomAsync(Room1Id, request, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RoomResponse)null);

            // Act
            var result = await _controller.Update(Room1Id, request, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedRoom_WhenUpdateSucceeds()
        {
            // Arrange
            var request = new CreateRoomRequest
            {
                Name = "Updated Room",
                Description = "Updated",
                PricePerHour = 150
            };

            var updatedRoom = new RoomResponse
            {
                RoomId = Room1Id,
                Name = "Updated Room",
                Description = "Updated",
                PricePerHour = 150
            };

            _roomServiceMock
                .Setup(s => s.UpdateRoomAsync(Room1Id, request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedRoom);

            // Act
            var result = await _controller.Update(Room1Id, request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoom = Assert.IsType<RoomResponse>(okResult.Value);
            Assert.Equal("Updated Room", returnedRoom.Name);
            Assert.Equal(150, returnedRoom.PricePerHour);
            _roomServiceMock.Verify(s => s.UpdateRoomAsync(Room1Id, request, It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_ReturnsNoContent_Always()
        {
            // Arrange
            _roomServiceMock
                .Setup(s => s.DeleteRoomAsync(Room1Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(Room1Id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _roomServiceMock.Verify(s => s.DeleteRoomAsync(Room1Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_EvenWhenRoomNotFound()
        {
            // Arrange
            _roomServiceMock
                .Setup(s => s.DeleteRoomAsync(Room1Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(Room1Id, CancellationToken.None);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        #endregion

        #region GetRoomFilterOptionsAsync Tests

        [Fact]
        public async Task GetRoomFilterOptionsAsync_ReturnsBadRequest_WhenFiltersAreInvalid()
        {
            // Arrange
            var filterDto = new RoomFilterDto();

            _roomServiceMock
                .Setup(s => s.GetFilteredRoomsAsync(filterDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PagedResult<RoomResponse>)null);

            // Act
            var result = await _controller.GetRoomFilterOptionsAsync(filterDto, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Filters", badRequestResult.Value);
        }

        [Fact]
        public async Task GetRoomFilterOptionsAsync_ReturnsOkWithFilteredRooms()
        {
            // Arrange
            var filterDto = new RoomFilterDto
            {
                Page = 1,
                PageSize = 10,
                roomOptionDTO = new RoomOptionDTO { MinPrice = 50, MaxPrice = 200 }
            };

            var pagedResult = new PagedResult<RoomResponse>
            {
                Items = new List<RoomResponse>
                {
                    new RoomResponse { RoomId = Room1Id, Name = "Room 1", PricePerHour = 100 },
                    new RoomResponse { RoomId = Room2Id, Name = "Room 2", PricePerHour = 150 }
                },
                pagination = new PaginationOptions
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalItems = 2
                }
            };

            _roomServiceMock
                .Setup(s => s.GetFilteredRoomsAsync(filterDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetRoomFilterOptionsAsync(filterDto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResult = Assert.IsType<PagedResult<RoomResponse>>(okResult.Value);
            Assert.Equal(2, returnedResult.Items.Count);
            Assert.Equal(1, returnedResult.pagination.CurrentPage);
            _roomServiceMock.Verify(s => s.GetFilteredRoomsAsync(filterDto, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetRoomFilterOptionsAsync_ReturnsEmptyList_WhenNoMatchingRooms()
        {
            // Arrange
            var filterDto = new RoomFilterDto();
            var emptyResult = new PagedResult<RoomResponse>
            {
                Items = new List<RoomResponse>(),
                pagination = new PaginationOptions
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalItems = 0
                }
            };

            _roomServiceMock
                .Setup(s => s.GetFilteredRoomsAsync(filterDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.GetRoomFilterOptionsAsync(filterDto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResult = Assert.IsType<PagedResult<RoomResponse>>(okResult.Value);
            Assert.Empty(returnedResult.Items);
        }

        #endregion

        #region GetActivities Tests

        [Fact]
        public async Task GetActivities_ReturnsOkWithActivities()
        {
            // Arrange
            var activities = new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), Name = "Yoga" },
                new Activity { ActivityId = Guid.NewGuid(), Name = "Pilates" }
            };

            _activityServiceMock
                .Setup(s => s.GetAllActivitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(activities);

            // Act
            var result = await _controller.GetActivities(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActivities = Assert.IsAssignableFrom<IEnumerable<Activity>>(okResult.Value);
            Assert.Equal(2, returnedActivities.Count());
            _activityServiceMock.Verify(s => s.GetAllActivitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetActivities_ReturnsEmptyList_WhenNoActivities()
        {
            // Arrange
            _activityServiceMock
                .Setup(s => s.GetAllActivitiesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Activity>());

            // Act
            var result = await _controller.GetActivities(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedActivities = Assert.IsAssignableFrom<IEnumerable<Activity>>(okResult.Value);
            Assert.Empty(returnedActivities);
        }

        #endregion

        #region CancellationToken Tests

        [Fact]
        public async Task GetAll_PassesCancellationToken_ToService()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            _roomServiceMock
                .Setup(s => s.GetAllRoomsAsync(token))
                .ReturnsAsync(new List<RoomResponse>());

            // Act
            await _controller.GetAll(token);

            // Assert
            _roomServiceMock.Verify(
                s => s.GetAllRoomsAsync(token),
                Times.Once);
        }

        #endregion
    }
}
