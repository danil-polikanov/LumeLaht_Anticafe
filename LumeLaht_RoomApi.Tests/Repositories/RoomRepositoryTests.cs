using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using LumeLaht_RoomApi.Core_.Entities.Filters;
using LumeLaht_RoomApi.Infrastructure.Data;
using LumeLaht_RoomApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static LumeLaht_RoomApi.Tests.Helpers.TestDataFactory;

namespace LumeLaht_RoomApi.Tests.Repositories   
{
    public class RoomRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly RoomRepository _repository;
        public RoomRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _repository = new RoomRepository(_context);
        }
        // Dispose
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        #region GetAllAsync Tests
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoRoomsExists()
        {
            //Act
            var result = await _repository.GetAllAsync(CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRoomsRelatedData()
        {
            //Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();
            //Act
            var result = await _repository.GetAllAsync(CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(2,result.Count);
            Assert.All(result, room =>
            {
                Assert.NotNull(room.Address);
                Assert.NotNull(room.Images);
                Assert.NotNull(room.RoomActivity);
            });
        }
        #endregion

        #region GetByIdAsync Tests
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRoomDoesNotExist()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRoomsWithReliatedData_WhenRoomExists()
        {
            // Arrange
            var testData= CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();
            var targetRoom = testData[0];
            //Act
            var result = await _repository.GetByIdAsync(targetRoom.RoomId, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(targetRoom.RoomId, result.RoomId);
            Assert.Equal(targetRoom.Name, result.Name);

            // Check includes
            Assert.NotNull(result.Address);
            Assert.Equal(targetRoom.Address.City, result.Address.City);
            Assert.NotNull(result.Images);
            Assert.Equal(targetRoom.Images.Count, result.Images.Count);
            Assert.NotNull(result.RoomActivity);
        }
        #endregion

        #region GetFilteredRoomsAsync Tests
        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldReturnAllRooms_WhenFiltersApplied()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Page = 1,
                PageSize = 3,
                SortOptions = new SortOptions { Field = "name", Direction = "asc" }
            };

            // Act
            var result =await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(2, result.pagination.TotalItems);
        }
        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldFilterBySearch()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Search = "Cozy Corner",
                Page = 1,
                PageSize = 10,
                SortOptions = new SortOptions()
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Contains("Cozy Corner", result.Items[0].Name);
        }

        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldFilterByStatus()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Page = 1,
                PageSize = 10,
                Filters = new Dictionary<string, object> { { "Status", "Available" } },
                SortOptions = new SortOptions()
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Available", result.Items[0].Status);
        }

        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldFilterByCity()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterDto = new RoomFilterDto
            {
                Page = 1,
                PageSize = 10,
                SortOptions = new SortOptions(),
                roomOptionDTO = new RoomOptionDTO
                {
                    City = "Tallinn"
                }
            };

            var filterOptions = filterDto.ToFilterOptions();

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Tallinn", result.Items[0].Address.City);
        }
        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldFilterByPriceRange()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterDto = new RoomFilterDto
            {
                Page = 1,
                PageSize = 10,
                SortOptions = new SortOptions(),
                roomOptionDTO = new RoomOptionDTO
                {
                    MinPrice = 3.0m,
                    MaxPrice = 8.0m
                }
            };

            var filterOptions = filterDto.ToFilterOptions();

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.InRange(result.Items[0].PricePerHour, 3.0m, 8.0m);
        }

        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldFilterByActivityIds()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var activityId = testData[0].RoomActivity[0].ActivityId;
            var filterOptions = new FilterOptions
            {
                Page = 1,
                PageSize = 10,
                Filters = new Dictionary<string, object>
                {
                    { "ActivitiesIds", new List<Guid> { activityId } }
                },
                SortOptions = new SortOptions()
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Contains(result.Items[0].RoomActivity, ra => ra.ActivityId == activityId);
        }

        [Theory]
        [InlineData("pricePerHour", "asc", "Cozy Corner")]
        [InlineData("pricePerHour", "desc", "Game Hub")]
        [InlineData("name", "asc", "Cozy Corner")]
        [InlineData("name", "desc", "Game Hub")]
        public async Task GetFilteredRoomsAsync_ShouldSortCorrectly(string sortBy, string sortOrder, string expectedFirstName)
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Page = 1,
                PageSize = 10,
                SortOptions = new SortOptions { Field = sortBy, Direction = sortOrder }
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Equal(expectedFirstName, result.Items[0].Name);
        }

        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldPaginateCorrectly()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Page = 1,
                PageSize = 1,
                SortOptions = new SortOptions { Field = "name", Direction = "asc" }
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal(2, result.pagination.TotalItems);
            Assert.Equal(1, result.pagination.CurrentPage);
            Assert.Equal(1, result.pagination.PageSize);
        }

        [Fact]
        public async Task GetFilteredRoomsAsync_ShouldApplyMultipleFilters()
        {
            // Arrange
            var testData = CreateTestRooms();
            await _context.Rooms.AddRangeAsync(testData);
            await _context.SaveChangesAsync();

            var filterOptions = new FilterOptions
            {
                Search = "Cozy",
                Page = 1,
                PageSize = 10,
                Filters = new Dictionary<string, object>
                {
                    { "Status", "Available" },
                    { "MinPrice", 3.0m }
                },
                SortOptions = new SortOptions { Field = "pricePerHour", Direction = "asc" }
            };

            // Act
            var result = await _repository.GetFilteredRoomsAsync(filterOptions, CancellationToken.None);

            // Assert
            Assert.Single(result.Items);
            Assert.Contains("Cozy", result.Items[0].Name);
            Assert.Equal("Available", result.Items[0].Status);
            Assert.True(result.Items[0].PricePerHour >= 3.0m);
        }

        #endregion

    }
}
