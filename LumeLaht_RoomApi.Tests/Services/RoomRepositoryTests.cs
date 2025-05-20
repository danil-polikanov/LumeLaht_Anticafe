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
        public async Task GetRoomsAsync_ShouldReturnRooms_WhenExists()
        {
            using var context = new AppDbContext(_dbOptions);
            var addresses = new List<Address>
            {
                new Address { AddressId = 1, City = "Narva", Region = "Ida-Virumaa",AddressName="Gagarini 11", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                new Address { AddressId = 2, City = "Narva", Region = "Ida-Virumaa", AddressName = "Narva mnt 32", PostalCode = "12342", Country = "Estonia", PhoneNumber = "+37254351534" },
                new Address { AddressId = 3, City = "Jõhvi", Region = "Ida-Virumaa", AddressName = "Taamsaare 24", PostalCode = "12343", Country = "Estonia", PhoneNumber = "+37254326535" }
            };
            context.Address.AddRange(addresses);

            // Добавим активности (если они обязательны)
            var activity = new Activity { ActivityId = 1, Name = "Chess", Description = "Table Game"};
            context.Activities.Add(activity);

            // Комнаты
            var rooms = new List<Room>
            {
                new Room
                {
                    RoomId = 1,
                    Name = "Test",
                    Description = "A",
                    AddressId = 1,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                },
                new Room
                {
                    RoomId = 2,
                    Name = "Test 2",
                    Description = "B",
                    AddressId = 2,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                },
                new Room
                {
                    RoomId = 3,
                    Name = "Test 3",
                    Description = "C",
                    AddressId = 3,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                }
             };
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();
            var repo = new RoomRepository(context);
            var result = await repo.GetAllAsync();

            Assert.Equal(3, result.Count());
        }
        // Get by id Tests
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRoom_WhenExists()
        {
            using var context = new AppDbContext(_dbOptions);
            var addresses = new List<Address>
            {
                new Address { AddressId = 1, City = "Narva", Region = "Ida-Virumaa",AddressName="Gagarini 11", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                new Address { AddressId = 2, City = "Narva", Region = "Ida-Virumaa", AddressName = "Narva mnt 32", PostalCode = "12342", Country = "Estonia", PhoneNumber = "+37254351534" },
                new Address { AddressId = 3, City = "Jõhvi", Region = "Ida-Virumaa", AddressName = "Taamsaare 24", PostalCode = "12343", Country = "Estonia", PhoneNumber = "+37254326535" }
            };
            context.Address.AddRange(addresses);

            // Добавим активности (если они обязательны)
            var activity = new Activity { ActivityId = 1, Name = "Chess", Description = "Table Game" };
            context.Activities.Add(activity);

            // Комнаты
            var rooms = new List<Room>
            {
                new Room
                {
                    RoomId = 1,
                    Name = "Test",
                    Description = "A",
                    AddressId = 1,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                },
                new Room
                {
                    RoomId = 2,
                    Name = "Test 2",
                    Description = "B",
                    AddressId = 2,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                },
                new Room
                {
                    RoomId = 3,
                    Name = "Test 3",
                    Description = "C",
                    AddressId = 3,
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = 1 }
                    }
                }
             };
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();

            var repo = new RoomRepository(context);
            var result = await repo.GetByIdAsync(rooms[1].RoomId);

            Assert.NotNull(result);
            Assert.Equal(rooms[1].RoomId, result.RoomId);
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
        [Fact]
        public async Task UpdateAsync_ShouldUpdateRoomToDatabase()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { RoomId = 1, Name = "Test", Description = "Desc", PricePerHour = 10.0, IsActive = true, AddressId = 1 };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();
            var roomTwo = await context.Rooms.FirstAsync();
            roomTwo.RoomId = 1;
            roomTwo.Name = "TestTwo";
            roomTwo.Description = "up";
            roomTwo.PricePerHour = 25.0;
            await repo.UpdateAsync(roomTwo);
            var added = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == 1);
            Assert.NotNull(added);
            Assert.Equal("TestTwo", added.Name);
        }
        //Delete Tests
        [Fact]
        public async Task DeleteAsync_ShouldDeleteRoomToDatabase_WhenIdExist()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { RoomId = 1, Name = "Test", Description = "Desc", PricePerHour = 10.0, IsActive = true, AddressId = 1 };
            context.Rooms.Add(room);
            await context.SaveChangesAsync();
            var deleteRoom = await context.Rooms.FindAsync(1);
            await repo.DeleteAsync(deleteRoom);
            var checkRoom = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == 1);
            Assert.Null(checkRoom);
        }
    }
}
