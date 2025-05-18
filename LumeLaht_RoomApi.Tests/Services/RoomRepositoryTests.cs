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

namespace LumeLaht_RoomApi.Tests.Services
{
    public class RoomRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public RoomRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }
        // Get All Tests
        [Fact]
        public async Task GetRoomsAsync_ShouldReturnRoom_WhenExists()
        {
            using var context = new AppDbContext(_dbOptions);
            var room = new Room { Name = "Test", AddressId = 1 };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);
            var result = await repo.GetByIdAsync(room.RoomId);

            Assert.NotNull(result);
            Assert.Equal(room.RoomId, result.RoomId);
        }
        // Get by id Tests
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRoom_WhenExists()
        {
            using var context = new AppDbContext(_dbOptions);
            var room = new Room { Name = "Test", AddressId = 1 };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);
            var result = await repo.GetByIdAsync(room.RoomId);

            Assert.NotNull(result);
            Assert.Equal(room.RoomId, result.RoomId);
        }   
        //Create Tests
        [Fact]
        public async Task AddAsync_ShouldAddRoomToDatabase()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { Name = "Test", Description = "Desc", PricePerHour = 10.0, IsActive = true, AddressId = 1 };

            await repo.AddAsync(room);

            var added = await context.Rooms.FirstOrDefaultAsync(r => r.Name == "Test");
            Assert.NotNull(added);
            Assert.Equal("Test", added.Name);
        }
        //Update Tests

        //Delete Tests




    }
}
