using LumaCove_Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LumaCove_Api.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<RoomActivity> RoomsActivity { get; set; }
        public DbSet<Address> Address { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomActivity>()
                .HasKey(ra => new { ra.RoomId, ra.ActivityId });

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = 1, City = "Narva", Region = "Ida-Virumaa", PostalCode = "12341", Country = "Estonia", PhoneNumber = "+37254356533" },
                new Address { AddressId = 2, City = "Narva", Region = "Ida-Virumaa", PostalCode = "12342", Country = "Estonia", PhoneNumber = "+37254351534" },
                new Address { AddressId = 3, City = "Jõhvi", Region = "Ida-Virumaa", PostalCode = "12343", Country = "Estonia", PhoneNumber = "+37254326535" },
                new Address { AddressId = 4, City = "Narva", Region = "Ida-Virumaa", PostalCode = "12344", Country = "Estonia", PhoneNumber = "+37254365536" },
                new Address { AddressId = 5, City = "Tallinn", Region = "Tallinn", PostalCode = "12345", Country = "Estonia", PhoneNumber = "+37254356537" },
                new Address { AddressId = 6, City = "Tallinn", Region = "Tallinn", PostalCode = "12346", Country = "Estonia", PhoneNumber = "+37254356538" }
            );

            modelBuilder.Entity<Activity>().HasData(
                new Activity { ActivityId = 1, Name = "Monopolia",Description="Table Game"},
                new Activity { ActivityId = 2, Name = "Uno", Description = "Table Game" },
                new Activity { ActivityId = 3, Name = "Chess", Description = "Table Game" },
                new Activity { ActivityId = 4, Name = "Scrabble", Description = "Table Game" },
                new Activity { ActivityId = 5, Name = "Ping Pong", Description = "Sport Game" },
                new Activity { ActivityId = 6, Name = "Foosball", Description = "Sport Game" },
                new Activity { ActivityId = 7, Name = "Billiards", Description = "Sport Game" },
                new Activity { ActivityId = 8, Name = "Darts", Description = "Sport Game" },
                new Activity { ActivityId = 9, Name = "Poker", Description = "Sport Game" },
                new Activity { ActivityId = 10, Name = "Blackjack", Description = "Sport Game" },
                new Activity { ActivityId = 11, Name = "Bowling", Description = "Sport Game" },
                new Activity { ActivityId = 12, Name = "Table Tennis", Description = "Sport Game" }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = 1, Name = "Room A", Description = "Description A", PricePerHour = 25.5, IsActive = true, AddressId = 1 },
                new Room { RoomId = 2, Name = "Room B", Description = "Description B", PricePerHour = 30, IsActive = true, AddressId = 2 },
                new Room { RoomId = 3, Name = "Room C", Description = "Description C", PricePerHour = 35, IsActive = true, AddressId = 3 },
                new Room { RoomId = 4, Name = "Room D", Description = "Description D", PricePerHour = 40, IsActive = true, AddressId = 4 },
                new Room { RoomId = 5, Name = "Room E", Description = "Description E", PricePerHour = 45, IsActive = true, AddressId = 5 },
                new Room { RoomId = 6, Name = "Room F", Description = "Description F", PricePerHour = 50, IsActive = true, AddressId = 6 }
            );

            modelBuilder.Entity<RoomActivity>().HasData(
                new RoomActivity { RoomId = 1, ActivityId = 1 },
                new RoomActivity { RoomId = 1, ActivityId = 2 },
                new RoomActivity { RoomId = 2, ActivityId = 3 },
                new RoomActivity { RoomId = 2, ActivityId = 4 },
                new RoomActivity { RoomId = 3, ActivityId = 5 },
                new RoomActivity { RoomId = 3, ActivityId = 6 },
                new RoomActivity { RoomId = 4, ActivityId = 7 },
                new RoomActivity { RoomId = 4, ActivityId = 8 },
                new RoomActivity { RoomId = 5, ActivityId = 9 },
                new RoomActivity { RoomId = 5, ActivityId = 10 },
                new RoomActivity { RoomId = 6, ActivityId = 11 },
                new RoomActivity { RoomId = 6, ActivityId = 12 }
            );

        }

    }
}

