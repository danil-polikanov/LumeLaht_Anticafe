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
                new Room { RoomId = roomIds[1], Name = "Room A", Description = "Description A", PricePerHour = 25.5, Status = "Available", AddressId = addressIds[1] },
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
                new RoomImage { ImageId = ImagesId[1], Url = "https://e.pcloud.link/publink/show?code=XZXn4EZ447cJGzE4mjGFUEd7fHezhKfrzEy", IsMain = true, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[2], Url = "https://e.pcloud.link/publink/show?code=XZK94EZnlvkoFPgUJhicWK2l9kunhbdFlJy", IsMain = true, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[3], Url = "https://e.pcloud.link/publink/show?code=XZl94EZcxrwzhmidM7XHfoOqgqtKhGsFzty", IsMain = true, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[4], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = true, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[5], Url = "https://e.pcloud.link/publink/show?code=XZ694EZx8NoXjBFg5phHhKrzqVhmXagQNhy", IsMain = true, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[6], Url = "https://e.pcloud.link/publink/show?code=XZE94EZPoL8U9CtrbzjvKxPqXuQx869cc0k", IsMain = true, RoomId = roomIds[6] },
                new RoomImage { ImageId = ImagesId[7], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[8], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[1] },
                new RoomImage { ImageId = ImagesId[9], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[10], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[2] },
                new RoomImage { ImageId = ImagesId[11], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[12], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[3] },
                new RoomImage { ImageId = ImagesId[13], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[14], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[4] },
                new RoomImage { ImageId = ImagesId[15], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[16], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[5] },
                new RoomImage { ImageId = ImagesId[17], Url = "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy", IsMain = false, RoomId = roomIds[6] },
                new RoomImage { ImageId = ImagesId[18], Url = "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk", IsMain = false, RoomId = roomIds[6] }
                );
        }



    }
}

