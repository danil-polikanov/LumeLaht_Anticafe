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
using static LumeLaht_RoomApi.Tests.Helpers.TestDataFactory;
using Xunit;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;

namespace LumeLaht_RoomApi.Tests.Services
{
    public class RoomServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IRepository<Address>> _addressRepositoryMock;
        private readonly Mock<IRepository<Activity>> _activityRepositoryMock;
        private readonly IMapper _mapper;
        private readonly RoomService _service;

        public RoomServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _addressRepositoryMock = new Mock<IRepository<Address>>();
            _activityRepositoryMock = new Mock<IRepository<Activity>>();

            _unitOfWorkMock.Setup(u => u.Addresses).Returns(_addressRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Activities).Returns(_activityRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.Rooms).Returns(_roomRepositoryMock.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomProfile>();
            });
            _mapper = config.CreateMapper();
            _service = new RoomService(_unitOfWorkMock.Object, _mapper);

        }
        #region GetAllRoomsAsync Tests
        [Fact]
        public async Task GetAllRoomsAsync_ReturnsEmptyList_WhenNoRoomsExist()
        {
            // Arrange
            _roomRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Room>());

            // Act
            var result = await _service.GetAllRoomsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _roomRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllRoomsAsync_ReturnsMappedRooms_WhenRoomsExist()
        {
            // Arrange
            var rooms = CreateTestRooms();
            _roomRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rooms);

            // Act
            var result = await _service.GetAllRoomsAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Зал А", result[0].Name);
            Assert.Equal("Зал B", result[1].Name);
        }

        [Fact]
        public async Task GetAllRoomsAsync_CallsRepositoryOnce()
        {
            // Arrange
            _roomRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Room>());

            // Act
            await _service.GetAllRoomsAsync(CancellationToken.None);

            // Assert
            _roomRepositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region GetRoomByIdAsync Tests

        [Fact]
        public async Task GetRoomByIdAsync_ReturnsNull_WhenRoomDoesNotExist()
        {
            // Arrange
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Room)null);

            // Act
            var result = await _service.GetRoomByIdAsync(Guid.NewGuid(), CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRoomByIdAsync_ReturnsMappedRoom_WhenRoomExists()
        {
            // Arrange
            var room = CreateRoom ("Test Room");
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            // Act
            var result = await _service.GetRoomByIdAsync(room.RoomId, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Room", result.Name);
        }

        [Fact]
        public async Task GetRoomByIdAsync_CallsRepositoryWithCorrectId()
        {
            // Arrange
            var testId = Guid.NewGuid();
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Room)null);

            // Act
            await _service.GetRoomByIdAsync(testId, CancellationToken.None);

            // Assert
            _roomRepositoryMock.Verify(
                r => r.GetByIdAsync(testId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
        #region CreateRoomAsync Tests

        [Fact]
        public async Task CreateRoomAsync_ThrowsValidationException_WhenAddressDoesNotExist()
        {
            // Arrange
            var request = CreateRoomRequest();
            _addressRepositoryMock
                .Setup(r => r.GetByIdAsync(request.AddressId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Address)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(
                async () => await _service.CreateRoomAsync(request, CancellationToken.None));

            Assert.Contains($"Address with ID {request.AddressId}", exception.Message);
        }

        [Fact]
        public async Task CreateRoomAsync_CreatesRoom_WhenAllDependenciesExist()
        {
            // Arrange
            var request = CreateRoomRequest();
            var address = new Address { AddressId = request.AddressId, City = "Test City" };

            _addressRepositoryMock
                .Setup(r => r.GetByIdAsync(address.AddressId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(address);

            Room capturedRoom = null;
            _roomRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .Callback<Room, CancellationToken>((room, _) => capturedRoom = room)
                .ReturnsAsync(new Room { Name = request.Name });

            // Act
            var result = await _service.CreateRoomAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.NotNull(capturedRoom);
            Assert.Equal(request.Name, capturedRoom.Name);
            _roomRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateRoomAsync_CreatesRoomWithActivities_WhenActivitiesProvided()
        {
            // Arrange
            var activities = new List<ActivityResponse>
            {
                new ActivityResponse { ActivityId = Guid.NewGuid(), Name = "Yoga", Description = "Yoga class" }
            };
            var request = CreateRoomRequest(activities);

            _addressRepositoryMock
                .Setup(r => r.GetByIdAsync(request.AddressId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Address { AddressId = request.AddressId });

            _activityRepositoryMock
                .Setup(r => r.GetByIdAsync(activities[0].ActivityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Activity { ActivityId = activities[0].ActivityId, Name = "Yoga" });

            Room capturedRoom = null;
            _roomRepositoryMock
                 .Setup(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                 .Callback<Room, CancellationToken>((room, _) => capturedRoom = room)
                 .ReturnsAsync(new Room { Name = request.Name });

            // Act
            var result = await _service.CreateRoomAsync(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(capturedRoom);
            Assert.NotNull(capturedRoom.RoomActivity);
            Assert.Single(capturedRoom.RoomActivity);
        }

        [Fact]
        public async Task CreateRoomAsync_ValidatesAllActivities_WhenMultipleActivitiesProvided()
        {
            // Arrange
            var activity1Id = Guid.NewGuid();
            var activity2Id = Guid.NewGuid();
            var activities = new List<ActivityResponse>
            {
                new ActivityResponse { ActivityId = activity1Id, Name = "Yoga" },
                new ActivityResponse { ActivityId = activity2Id, Name = "Pilates" }
            };
            var request = CreateRoomRequest(activities);

            _addressRepositoryMock
                .Setup(r => r.GetByIdAsync(request.AddressId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Address { AddressId = request.AddressId });

            _activityRepositoryMock
                .Setup(r => r.GetByIdAsync(activity1Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Activity { ActivityId = activity1Id, Name = "Yoga" });

            _activityRepositoryMock
                .Setup(r => r.GetByIdAsync(activity2Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Activity { ActivityId = activity2Id, Name = "Pilates" });

            _roomRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Room { Name = request.Name }); 

            // Act
            await _service.CreateRoomAsync(request, CancellationToken.None);

            // Assert
            _activityRepositoryMock.Verify(
                r => r.GetByIdAsync(activity1Id, It.IsAny<CancellationToken>()),
                Times.Once);
            _activityRepositoryMock.Verify(
                r => r.GetByIdAsync(activity2Id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region UpdateRoomAsync Tests

        [Fact]
        public async Task UpdateRoomAsync_UpdatesRoom_Successfully()
        {
            // Arrange
            var request = CreateRoomRequest();
            Room capturedRoom = null;

            _roomRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .Callback<Room, CancellationToken>((room, _) => capturedRoom = room)
                .Returns(Task.CompletedTask);
            var roomId=Guid.NewGuid();
            // Act
            var result = await _service.UpdateRoomAsync(roomId,request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roomId, result.RoomId);
            Assert.Equal(request.Name, result.Name);
            Assert.NotNull(capturedRoom);
            Assert.Equal(roomId, capturedRoom.RoomId);
            _roomRepositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateRoomAsync_PreservesRoomId()
        {
            // Arrange
            var request = CreateRoomRequest();
            Room capturedRoom = null;

            _roomRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Room>(), It.IsAny<CancellationToken>()))
                .Callback<Room, CancellationToken>((room, _) => capturedRoom = room)
                .Returns(Task.CompletedTask);
            var roomId = Guid.NewGuid();
            // Act
            await _service.UpdateRoomAsync(roomId, request, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedRoom);
            Assert.Equal(roomId, capturedRoom.RoomId);
        }

        #endregion

        #region DeleteRoomAsync Tests

        [Fact]
        public async Task DeleteRoomAsync_ReturnsFalse_WhenRoomDoesNotExist()
        {
            var roomId = Guid.NewGuid();
            // Arrange
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(roomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Room)null);

            // Act
            var result = await _service.DeleteRoomAsync(roomId, CancellationToken.None);

            // Assert
            Assert.False(result);
            _roomRepositoryMock.Verify(
                r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteRoomAsync_ReturnsTrue_WhenRoomIsDeleted()
        {
 
            // Arrange
            var room = CreateRoom("Test Room");
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            _roomRepositoryMock
                .Setup(r => r.DeleteAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteRoomAsync(room.RoomId, CancellationToken.None);

            // Assert
            Assert.True(result);
            _roomRepositoryMock.Verify(
                r => r.DeleteAsync(room.RoomId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteRoomAsync_CallsDeleteWithCorrectId()
        {
            var roomId = Guid.NewGuid();
            // Arrange
            var room = CreateRoom("Test Room");
            _roomRepositoryMock
                .Setup(r => r.GetByIdAsync(room.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(room);

            // Act
            await _service.DeleteRoomAsync(room.RoomId, CancellationToken.None);

            // Assert
            _roomRepositoryMock.Verify(
                r => r.DeleteAsync(room.RoomId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion

        #region GetFilteredRoomsAsync Tests     
        [Fact]
        public async Task GetFilteredRoomsAsync_ReturnsMappedPagedResult()
        {
            // Arrange
            var rooms = CreateTestRooms();
            var pagedResult = new PagedResult<Room>
            {
                Items = rooms,
                pagination = new PaginationOptions
                {
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalItems = 2
                }
            };

            var filterDto = new RoomFilterDto
            {
                roomOptionDTO = new RoomOptionDTO {
                },
                SortOptions=new SortOptions
                {

                },
                Page = 1,
                PageSize = 10
            };

            _roomRepositoryMock
                .Setup(r => r.GetFilteredRoomsAsync(It.IsAny<FilterOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _service.GetFilteredRoomsAsync(filterDto, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(1, result.pagination.CurrentPage);
            Assert.Equal(10, result.pagination.PageSize);
            Assert.Equal(2, result.pagination.TotalItems);
        }

        #endregion
    }


}
