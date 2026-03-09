using LumeLaht_RoomApi.Application.Dto;
using LumeLaht_RoomApi.Core_.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumeLaht_RoomApi.Tests.Helpers
{
    /// <summary>
    /// Фабрика для создания тестовых данных
    /// </summary>
    public static class TestDataFactory
    {
        public static Address CreateAddress(
            string city = "Narva",
            string region = "Ida-Virumaa")
        {
            return new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = $"ул. Тестовая, {Random.Shared.Next(1, 100)}",
                City = city,
                Region = region,
                PostalCode = Random.Shared.Next(100000, 999999).ToString(),
                Country = "Estonia",
                PhoneNumber = $"+7900{Random.Shared.Next(1000000, 9999999)}"
            };
        }

        public static Activity CreateActivity(
            string name = "Тестовая активность")
        {
            return new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = name,
                Description = "Описание активности"
            };
        }

        public static Room CreateRoom(
            string name = "Тестовый зал",
            decimal price = 1000,
            string status = "Available",
            Address? address = null,
            string addressId=null,
            List<Activity>? activities = null)
        {
            var room = new Room
            {
                RoomId = Guid.NewGuid(),
                Name = name,
                Description = "Описание зала",
                PricePerHour = price,
                Status = status,
                Address = address?? CreateAddress(),
                Images = new List<RoomImage>
                {
                    new RoomImage
                    {
                        ImageId = Guid.NewGuid(),                  
                        Url = "test.jpg",
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
        #region Helper Methods

        public static List<Room> CreateTestRooms()
        {
            var activity1 = new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = "Йога",
                Description = "Занятия йогой"
            };

            var activity2 = new Activity
            {
                ActivityId = Guid.NewGuid(),
                Name = "Фитнес",
                Description = "Силовые тренировки"
            };

            var address1 = new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = "ул. Gagarini, 10",
                City = "Tallinn",
                Region = "Tallinn reg",
                PostalCode = "123456",
                Country = "Estonia",
                PhoneNumber = "+38001234567"
            };

            var address2 = new Address
            {
                AddressId = Guid.NewGuid(),
                AddressName = "пр. Мира, 25",
                City = "Narva",
                Region = "Ida-Virumaa",
                PostalCode = "654321",
                Country = "Estonia",
                PhoneNumber = "+372009876543"
            };

            var room1 = new Room
            {
                RoomId = Guid.NewGuid(),
                Name = "Зал А",
                Description = "Просторный зал для йоги",
                PricePerHour = 1000,
                Status = "Available",
                AddressId = address1.AddressId,
                Address = address1,
                Images = new List<RoomImage>
                {
                    new RoomImage { ImageId = Guid.NewGuid(), Url = "image1.jpg", IsMain = true }
                },
                RoomActivity = new List<RoomActivity>
                {
                    new RoomActivity { RoomId = room1.RoomId, ActivityId = activity1.ActivityId, Activity = activity1 }
                }
            };

            var room2 = new Room
            {
                RoomId = Guid.NewGuid(),
                Name = "Зал B",
                Description = "Зал для фитнеса",
                PricePerHour = 1500,
                Status = "Occupied",
                AddressId = address2.AddressId,
                Address = address2,
                Images = new List<RoomImage>
                {
                    new RoomImage { ImageId = Guid.NewGuid(), Url = "image2.jpg", IsMain = true }
                },
                RoomActivity = new List<RoomActivity>
                {
                    new RoomActivity { RoomId = room2.RoomId, ActivityId = activity2.ActivityId, Activity = activity2 }
                }
            };

            return new List<Room> { room1, room2 };
        }
        public static CreateRoomRequest CreateRoomRequest(List<ActivityResponse> activities = null)
        {
            return new CreateRoomRequest
            {
                Name = "Test Room",
                Description = "Test Description",
                PricePerHour = 100,
                Status = "Available",
                AddressId = Guid.NewGuid(),
                Activities = activities
            };
        }


        #endregion
    }
}
