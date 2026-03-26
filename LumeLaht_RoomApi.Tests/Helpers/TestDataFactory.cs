using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;

namespace LumeLaht_RoomApi.Tests.Helpers
{
    public static class TestDataFactory
    {
        public static Address CreateAddress(
            string city = "Tallinn",
            string region = "Harju")
        {
            return new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = $"Test Street {Random.Shared.Next(1, 100)}",
                City = city,
                Region = region,
                PostalCode = Random.Shared.Next(10000, 99999).ToString(),
                Country = "Estonia",
                PhoneNumber = $"+3725550{Random.Shared.Next(1000, 9999)}"
            };
        }

        public static Activity CreateActivity(
            string name = "Test Activity",
            string category = "Board Game")
        {
            return new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = name,
                Description = "Test activity description",
                Category = category
            };
        }

        public static Room CreateRoom(
            string name = "Test Room",
            decimal price = 10,
            int capacity = 6,
            string status = "Available",
            Address? address = null,
            List<Activity>? activities = null)
        {
            var room = new Room
            {
                RoomId = Guid.NewGuid(),
                Name = name,
                Description = "Test room description",
                PricePerHour = price,
                Capacity = capacity,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = address ?? CreateAddress(),
                Images = new List<RoomImage>
                {
                    new RoomImage
                    {
                        ImageId = Guid.NewGuid(),
                        Url = "https://example.com/test.jpg",
                        IsMain = true
                    }
                },
                RoomActivity = new List<RoomActivity>()
            };

            room.AddressId = room.Address.AddressId;

            if (activities != null)
            {
                foreach (var activity in activities)
                {
                    room.RoomActivity.Add(new RoomActivity
                    {
                        RoomId = room.RoomId,
                        ActivityId = activity.ActivityId,
                        Room = room,
                        Activity = activity
                    });
                }
            }

            return room;
        }

        public static List<Room> CreateTestRooms()
        {
            var activity1 = CreateActivity("Monopoly", "Board Game");
            var activity2 = CreateActivity("Billiards", "Sport Game");

            var address1 = new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = "Viru 15",
                City = "Tallinn",
                Region = "Harju",
                PostalCode = "10140",
                Country = "Estonia",
                PhoneNumber = "+37255501001"
            };

            var address2 = new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = "Pushkini 20",
                City = "Narva",
                Region = "Ida-Virumaa",
                PostalCode = "20309",
                Country = "Estonia",
                PhoneNumber = "+37255503001"
            };

            var room1Id = Guid.NewGuid();
            var room2Id = Guid.NewGuid();

            var room1 = new Room
            {
                RoomId = room1Id,
                Name = "Cozy Corner",
                Description = "A warm space with board games",
                PricePerHour = 5,
                Capacity = 6,
                Status = "Available",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AddressId = address1.AddressId,
                Address = address1,
                Images = new List<RoomImage>
                {
                    new RoomImage { ImageId = Guid.NewGuid(), Url = "https://example.com/room1.jpg", IsMain = true }
                },
                RoomActivity = new List<RoomActivity>
                {
                    new RoomActivity { RoomId = room1Id, ActivityId = activity1.ActivityId, Activity = activity1 }
                }
            };

            var room2 = new Room
            {
                RoomId = room2Id,
                Name = "Game Hub",
                Description = "Main gaming zone with billiards",
                PricePerHour = 12,
                Capacity = 12,
                Status = "Occupied",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                AddressId = address2.AddressId,
                Address = address2,
                Images = new List<RoomImage>
                {
                    new RoomImage { ImageId = Guid.NewGuid(), Url = "https://example.com/room2.jpg", IsMain = true }
                },
                RoomActivity = new List<RoomActivity>
                {
                    new RoomActivity { RoomId = room2Id, ActivityId = activity2.ActivityId, Activity = activity2 }
                }
            };

            return new List<Room> { room1, room2 };
        }

        public static CreateRoomRequest CreateRoomRequest(List<Guid>? activityIds = null)
        {
            return new CreateRoomRequest
            {
                Name = "Test Room",
                Description = "Test Description",
                PricePerHour = 10,
                Capacity = 6,
                Status = "Available",
                AddressId = Guid.NewGuid(),
                ActivityIds = activityIds
            };
        }
    }
}
