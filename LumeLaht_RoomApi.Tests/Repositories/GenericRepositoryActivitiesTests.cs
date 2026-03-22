using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LumeLaht_RoomApi.Tests.Repositories
{
    /// <summary>
    /// Тесты для базового репозитория с использованием Activity
    /// Показывает, что Repository<T> работает с любыми сущностями
    /// </summary>
    public class GenericRepository_ActivityTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Repository<Activity> _repository;

        public GenericRepository_ActivityTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new Repository<Activity>(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_Activity_WorksCorrectly()
        {
            // Arrange
            var activity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = "Йога",
                Description = "Занятия йогой для начинающих"
            };

            // Act
            var result = await _repository.AddAsync(activity, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(activity.ActivityId, result.ActivityId);
            Assert.Equal("Йога", result.Name);
        }

        [Fact]
        public async Task GetAllAsync_Activities_ReturnsAllActivities()
        {
            // Arrange
            var activities = new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), Name = "Йога", Description = "Desc 1" },
                new Activity { ActivityId = Guid.NewGuid(), Name = "Фитнес", Description = "Desc 2" },
                new Activity { ActivityId = Guid.NewGuid(), Name = "Бокс", Description = "Desc 3" }
            };
            await _context.Activities.AddRangeAsync(activities);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(result, a => a.Name == "Йога");
            Assert.Contains(result, a => a.Name == "Фитнес");
            Assert.Contains(result, a => a.Name == "Бокс");
        }

        [Theory]
        [InlineData("Йога")]
        [InlineData("Пилатес")]
        [InlineData("Кроссфит")]
        public async Task UpdateAsync_Activity_UpdatesName(string newName)
        {
            // Arrange
            var activity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = "Старое название",
                Description = "Описание"
            };
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();

            // Act
            activity.Name = newName;
            await _repository.UpdateAsync(activity, CancellationToken.None);

            // Assert
            var updated = await _repository.GetByIdAsync(activity.ActivityId, CancellationToken.None);
            Assert.Equal(newName, updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_Activity_RemovesFromDatabase()
        {
            // Arrange
            var activity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = "Временная активность",
                Description = "Будет удалена"
            };
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(activity.ActivityId, CancellationToken.None);

            // Assert
            var deleted = await _repository.GetByIdAsync(activity.ActivityId, CancellationToken.None);
            Assert.Null(deleted);
        }
    }
}
