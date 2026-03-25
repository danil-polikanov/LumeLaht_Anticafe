using LumeLaht_RoomApi.Core_.Entities;
using Microsoft.EntityFrameworkCore;

namespace LumeLaht_RoomApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<RoomActivity> RoomsActivity { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        private static Guid GenerateDeterministicGuid(char prefix, int index)
        {
            // Преобразуем индекс в 8-значную строку
            string prefixPart = ((int)prefix).ToString("X2") + index.ToString("D6"); 
            string guidStr = $"{prefixPart}-0000-0000-0000-000000000000";
            return Guid.ParseExact(guidStr, "D");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicit column type for PricePerHour
            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerHour)
                .HasColumnType("decimal(18,2)");

            // Связь many-to-many
            modelBuilder.Entity<RoomActivity>()
                .HasKey(ra => new { ra.RoomId, ra.ActivityId });

            // Фиксированные AddressId
            var addressIds = Enumerable.Range(1, 6).ToDictionary(
                i => i,
                i => GenerateDeterministicGuid('B', i)
            );

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = addressIds[1], City = "Narva", Region = "Ida-Virumaa", AddressName = "Gagarini 11", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                new Address { AddressId = addressIds[2], City = "Narva", Region = "Ida-Virumaa", AddressName = "Narva mnt 32", PostalCode = "12342", Country = "Estonia", PhoneNumber = "+37254351534" },
                new Address { AddressId = addressIds[3], City = "Jõhvi", Region = "Ida-Virumaa", AddressName = "Taamsaare 24", PostalCode = "12343", Country = "Estonia", PhoneNumber = "+37254326535" },
                new Address { AddressId = addressIds[4], City = "Narva", Region = "Ida-Virumaa", AddressName = "Vaba mnt 105", PostalCode = "12344", Country = "Estonia", PhoneNumber = "+37254365536" },
                new Address { AddressId = addressIds[5], City = "Tallinn", Region = "Tallinn", AddressName = "Tallina mnt 25", PostalCode = "12345", Country = "Estonia", PhoneNumber = "+37254356537" },
                new Address { AddressId = addressIds[6], City = "Tallinn", Region = "Tallinn", AddressName = "Tallina mnt 23", PostalCode = "12346", Country = "Estonia", PhoneNumber = "+37254356538" }
            );

            // Фиксированные ActivityId
            var activityIds = Enumerable.Range(1, 12).ToDictionary(
                  i => i,
                  i => GenerateDeterministicGuid('C', i)
              );    
            modelBuilder.Entity<Activity>().HasData(
                new Activity { ActivityId = activityIds[1], Name = "Monopolia", Description = "Table Game" },
                new Activity { ActivityId = activityIds[2], Name = "Uno", Description = "Table Game" },
                new Activity { ActivityId = activityIds[3], Name = "Chess", Description = "Table Game" },
                new Activity { ActivityId = activityIds[4], Name = "Scrabble", Description = "Table Game" },
                new Activity { ActivityId = activityIds[5], Name = "Ping Pong", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[6], Name = "Foosball", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[7], Name = "Billiards", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[8], Name = "Darts", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[9], Name = "Poker", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[10], Name = "Blackjack", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[11], Name = "Bowling", Description = "Sport Game" },
                new Activity { ActivityId = activityIds[12], Name = "Table Tennis", Description = "Sport Game" }
            );

            // Фиксированные RoomId
            var roomIds = Enumerable.Range(1, 6).ToDictionary(
                i => i,
                i => GenerateDeterministicGuid('F', i)
            );

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = roomIds[1], Name = "Room A", Description = "Description A", PricePerHour = 25.5M, Status = "Available", AddressId = addressIds[1] },
                new Room { RoomId = roomIds[2], Name = "Room B", Description = "Description B", PricePerHour = 30, Status = "Available", AddressId = addressIds[2] },
                new Room { RoomId = roomIds[3], Name = "Room C", Description = "Description C", PricePerHour = 35, Status = "Available", AddressId = addressIds[3] },
                new Room { RoomId = roomIds[4], Name = "Room D", Description = "Description D", PricePerHour = 40, Status = "Maintenance", AddressId = addressIds[4] },
                new Room { RoomId = roomIds[5], Name = "Room E", Description = "Description E", PricePerHour = 45, Status = "Available", AddressId = addressIds[5] },
                new Room { RoomId = roomIds[6], Name = "Room F", Description = "Description F", PricePerHour = 50, Status = "Available", AddressId = addressIds[6] }
            );

            // RoomActivity связи
            modelBuilder.Entity<RoomActivity>().HasData(
                new RoomActivity { RoomId = roomIds[1], ActivityId = activityIds[1] },
                new RoomActivity { RoomId = roomIds[1], ActivityId = activityIds[2] },
                new RoomActivity { RoomId = roomIds[2], ActivityId = activityIds[3] },
                new RoomActivity { RoomId = roomIds[2], ActivityId = activityIds[4] },
                new RoomActivity { RoomId = roomIds[3], ActivityId = activityIds[5] },
                new RoomActivity { RoomId = roomIds[3], ActivityId = activityIds[6] },
                new RoomActivity { RoomId = roomIds[4], ActivityId = activityIds[7] },
                new RoomActivity { RoomId = roomIds[4], ActivityId = activityIds[8] },
                new RoomActivity { RoomId = roomIds[5], ActivityId = activityIds[9] },
                new RoomActivity { RoomId = roomIds[5], ActivityId = activityIds[10] },
                new RoomActivity { RoomId = roomIds[6], ActivityId = activityIds[11] },
                new RoomActivity { RoomId = roomIds[6], ActivityId = activityIds[12] }
            );
            // RoomImages
            var ImagesId = Enumerable.Range(1, 18).ToDictionary(
                i => i,
                i => GenerateDeterministicGuid('D', i)
            );
            modelBuilder.Entity<RoomImage>().HasData(
                new RoomImage { ImageId = ImagesId[1], Url = "https://edef6.pcloud.com/DLZWFU6B6ZErvSOj7ZOjDfZZyHl10kZNVZZQYVZZn7M7ZzRZN4ZXLZaRIc8C2DvgjCj7EOMDkcnu0LrCFy/chairs-2181980_1920.jpg", IsMain = true, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[2], Url = "https://edef10.pcloud.com/cfZGzI1DZt6wfOj7ZOjDfZZTHl10kZNVZZQYVZZvRpCZwHZPZc7ZEHZoZQFZZx7Zp7Z7LZTVZe7ZWLZ4kZJ0xXJLg2QHRU1NeDLwgBCFGb5AY7/inter-er-restorana.jpg", IsMain = true, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[3], Url = "https://edef10.pcloud.com/cfZv1PIsZzxwfOj7ZOjDfZZwzl10kZNVZZQYVZZExBtZRXZ0RZcLZD7Zk5ZU7ZS7Z0zZL7ZTkZDVZ2Z3kZKFZSMQ4xFQQzF495zWLLGbSyHWUq2WV/inter-er-vystrel-iz-kafe-so-stul-ami-vozle-bara-s-derevannymi-stolami.jpg", IsMain = true, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[4], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZJRl10kZNVZZQYVZZvwBZCRZNRZT8ZeLyDsVPx5FhSJdO3T1lkfY2PaiwX/photo-1497366811353-6870744d04b2.jpg", IsMain = true, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[5], Url = "https://edef8.pcloud.com/DLZu0sOB6Zq6wfOj7ZOjDfZZPRl10kZNVZZQYVZZhOE7ZdLZi5ZE8Z7I5tNY0r9qVPGtCmfXBDPmca9Osk/vintage-aesthetic-7131604_1920.jpg", IsMain = true, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[6], Url = "https://edef4.pcloud.com/DLZzMPOB6ZjMifOj7ZOjDfZZrRl10kZNVZZQYVZZTUmZzJZg8ZA4Z2Ko1Jk2Ac8hX3jSG8WtmwkFK20v7/photo-1497366216548-37526070297c.jpg", IsMain = true, RoomId = roomIds[6] },
                new RoomImage { ImageId = ImagesId[7], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZSLl10kZNVZZQYVZZ0DjZzzZH4Z44ZxUM7yrRl9X4uS0QPdocoem0EWnik/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[8], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZz4l10kZNVZZQYVZZvwBZCRZT8ZNRZuWuQtIxC4JyR37GkGAycnjp68Gfy/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[9], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZLsl10kZNVZZQYVZZ0DjZH4ZzzZ44ZqmYhXTdU6l5cOmHyJpPBe0tIXs7k/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[10], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZisl10kZNVZZQYVZZvwBZCRZT8ZNRZVIJlV2Lv1qQDm8LQHxofDBv02GDX/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[11], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZqDl10kZNVZZQYVZZ0DjZ44ZH4ZzzZEkzylnQCisBKyhTJUaMP6jxozogX/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[12], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZH1l10kZNVZZQYVZZvwBZCRZNRZT8Zbc0hNNc8KdjJFtTJ3hyUwp3xeAbX/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[13], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZC1l10kZNVZZQYVZZ0DjZH4Z44ZzzZ4df3TUten8yDWnGx26XlCmeS10yk/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[14], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZH1l10kZNVZZQYVZZvwBZCRZNRZT8Zbc0hNNc8KdjJFtTJ3hyUwp3xeAbX/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[15], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZC1l10kZNVZZQYVZZ0DjZH4Z44ZzzZ4df3TUten8yDWnGx26XlCmeS10yk/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[16], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZH1l10kZNVZZQYVZZvwBZCRZNRZT8Zbc0hNNc8KdjJFtTJ3hyUwp3xeAbX/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[17], Url = "https://edef11.pcloud.com/DLZTTPOB6Z2gifOj7ZOjDfZZC1l10kZNVZZQYVZZ0DjZH4Z44ZzzZ4df3TUten8yDWnGx26XlCmeS10yk/photo-1497366754035-f200968a6e72.jpg", IsMain = false, RoomId = roomIds[6] },
                new RoomImage { ImageId = ImagesId[18], Url = "https://edef10.pcloud.com/DLZUTPOB6ZSgifOj7ZOjDfZZH1l10kZNVZZQYVZZvwBZCRZNRZT8Zbc0hNNc8KdjJFtTJ3hyUwp3xeAbX/photo-1497366811353-6870744d04b2.jpg", IsMain = false, RoomId = roomIds[6] }
                );
        }



    }
}
