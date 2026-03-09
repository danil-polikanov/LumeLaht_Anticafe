using LumeLaht_RoomApi.Application.Services;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Interfaces;
using Moq;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Services
{
    /// <summary>
    /// Тесты для ActivityService
    /// </summary>
    public class ActivityServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRepository<Activity>> _activityRepositoryMock;
        private readonly ActivityService _service;

        public ActivityServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _activityRepositoryMock = new Mock<IRepository<Activity>>();
            _unitOfWorkMock.Setup(u => u.Activities).Returns(_activityRepositoryMock.Object);
            _service = new ActivityService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllActivitiesAsync_ReturnsEmptyList_WhenNoActivities()
        {
            // Arrange
            _activityRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Activity>());

            // Act
            var result = await _service.GetAllActivitiesAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllActivitiesAsync_ReturnsSortedActivities_WhenActivitiesExist()
        {
            // Arrange
            var activities = new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), Name = "Yoga" },
                new Activity { ActivityId = Guid.NewGuid(), Name = "Boxing" },
                new Activity { ActivityId = Guid.NewGuid(), Name = "Pilates" }
            };

            _activityRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(activities);

            // Act
            var result = await _service.GetAllActivitiesAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Boxing", result[0].Name);
            Assert.Equal("Pilates", result[1].Name);
            Assert.Equal("Yoga", result[2].Name);
            _activityRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
