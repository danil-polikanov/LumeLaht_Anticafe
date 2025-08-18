using LumeLaht_RoomApi.Core_.Entities;
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

namespace LumeLaht_RoomApi.Tests.Services
{
    public class RoomRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        //Test Data
        static private readonly Guid Room1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        static private readonly Guid Room2Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        static private readonly Guid Room3Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        static private readonly Guid Address1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        static private readonly Guid Address2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        static private readonly Guid Address3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

        static public List<Address> addresses = new List<Address>
            {
                new Address { AddressId =Address1Id, City = "Narva", Region = "Ida-Virumaa",AddressName="Gagarini 11", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                new Address { AddressId = Address2Id, City = "Narva", Region = "Ida-Virumaa", AddressName = "Narva mnt 32", PostalCode = "12342", Country = "Estonia", PhoneNumber = "+37254351534" },
                new Address { AddressId = Address3Id, City = "Jõhvi", Region = "Ida-Virumaa", AddressName = "Taamsaare 24", PostalCode = "12343", Country = "Estonia", PhoneNumber = "+37254326535" }
            };
        // Добавим активности (если они обязательны)
        static public Activity activity = new Activity { ActivityId = Guid.NewGuid(), Name = "Chess", Description = "Table Game" };

        static public List<Room> rooms = new List<Room>
            {
                new Room
                {
                    RoomId = Room1Id,
                    Name = "Test",
                    Description = "A",
                    AddressId = Address1Id,
                    Status="Available",
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId =activity.ActivityId }
                    }
                },
                new Room
                {
                    RoomId = Room2Id,
                    Name = "Test 2",
                    Description = "B",
                    AddressId = Address2Id,
                       Status="Available",
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = activity.ActivityId }
                    }
                },
                new Room
                {
                    RoomId = Room3Id,
                    Name = "Test 3",
                    Description = "C",
                    AddressId =Address3Id,
                       Status="Available",
                    RoomActivity = new List<RoomActivity>
                    {
                        new RoomActivity { ActivityId = activity.ActivityId }
                    }
                }
             };
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

            context.Address.AddRange(addresses);


            context.Activities.Add(activity);

            // Комнаты            
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync();
            var repo = new RoomRepository(context);
            var result = await repo.GetAllAsync(CancellationToken.None);

            Assert.Equal(3, result.Count());
        }
        // Get by id Tests
        [Fact]
        public async Task GetByIdAsync_ShouldReturnRoom_WhenExists()
        {
            using var context = new AppDbContext(_dbOptions);
           
            context.Address.AddRange(addresses);

            // Добавим активности (если они обязательны)

            context.Activities.Add(activity);
            context.Rooms.AddRange(rooms);
            await context.SaveChangesAsync(CancellationToken.None);

            var repo = new RoomRepository(context);
            var result = await repo.GetByIdAsync(rooms[1].RoomId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(rooms[1].RoomId, result.RoomId);
        }
        //Create Tests
        [Fact]
        public async Task AddAsync_ShouldAddRoomToDatabase()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { Name = "Test", Description = "Desc", PricePerHour = 10.0, Status = "Available", AddressId = Address1Id };

            await repo.AddAsync(room, CancellationToken.None);

            var added = await context.Rooms.FirstOrDefaultAsync(r => r.Name == "Test", CancellationToken.None);
            Assert.NotNull(added);
            Assert.Equal("Test", added.Name);
        }
        //Update Tests
        [Fact]
        public async Task UpdateAsync_ShouldUpdateRoomToDatabase()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { RoomId = Guid.NewGuid(), Name = "Test", Description = "Desc", PricePerHour = 10.0, Status = "Available", AddressId = Address1Id };
            context.Rooms.Add(room);
            await context.SaveChangesAsync(CancellationToken.None);
            var roomTwo = await context.Rooms.FirstOrDefaultAsync(r=>r.RoomId==room.RoomId, CancellationToken.None);
            roomTwo.Name = "TestTwo";
            roomTwo.Description = "up";
            roomTwo.PricePerHour = 25.0;
            await repo.UpdateAsync(roomTwo, CancellationToken.None);
            var added = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == room.RoomId, CancellationToken.None);
            Assert.NotNull(added);
            Assert.Equal("TestTwo", added.Name);
        }
        //Delete Tests
        [Fact]
        public async Task DeleteAsync_ShouldDeleteRoomToDatabase_WhenIdExist()
        {
            using var context = new AppDbContext(_dbOptions);
            var repo = new RoomRepository(context);

            var room = new Room { RoomId = Guid.NewGuid(), Name = "Test", Description = "Desc", PricePerHour = 10.0, Status = "Available", AddressId = Address1Id };
            context.Rooms.Add(room);
            await context.SaveChangesAsync(CancellationToken.None);
            var deleteRoom = await context.Rooms.FindAsync(room.RoomId);
            await repo.DeleteAsync(deleteRoom, CancellationToken.None);
            var checkRoom = await context.Rooms.FirstOrDefaultAsync(r => r.RoomId == room.RoomId);
            Assert.Null(checkRoom);
        }
    }
}
