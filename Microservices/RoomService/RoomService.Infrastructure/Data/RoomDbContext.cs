using RoomService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace RoomService.Infrastructure.Data
{
    public class RoomDbContext : DbContext
    {
        public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<RoomActivity> RoomsActivity { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }

        private static Guid GenerateDeterministicGuid(char prefix, int index)
        {
            string prefixPart = ((int)prefix).ToString("X2") + index.ToString("D6");
            string guidStr = $"{prefixPart}-0000-0000-0000-000000000000";
            return Guid.ParseExact(guidStr, "D");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerHour)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RoomActivity>()
                .HasKey(ra => new { ra.RoomId, ra.ActivityId });

            // ── Addresses (7) ──
            var addr = Enumerable.Range(1, 7).ToDictionary(i => i, i => GenerateDeterministicGuid('B', i));

            modelBuilder.Entity<Address>().HasData(
                new Address { AddressId = addr[1], City = "Tallinn", Region = "Harju", AddressName = "Viru 15", PostalCode = "10140", Country = "Estonia", PhoneNumber = "+37255501001" },
                new Address { AddressId = addr[2], City = "Tallinn", Region = "Harju", AddressName = "Telliskivi 60a", PostalCode = "10412", Country = "Estonia", PhoneNumber = "+37255501002" },
                new Address { AddressId = addr[3], City = "Tartu", Region = "Tartu", AddressName = "Riia 2", PostalCode = "51004", Country = "Estonia", PhoneNumber = "+37255502001" },
                new Address { AddressId = addr[4], City = "Tartu", Region = "Tartu", AddressName = "Küüni 5b", PostalCode = "51003", Country = "Estonia", PhoneNumber = "+37255502002" },
                new Address { AddressId = addr[5], City = "Narva", Region = "Ida-Virumaa", AddressName = "Pushkini 20", PostalCode = "20309", Country = "Estonia", PhoneNumber = "+37255503001" },
                new Address { AddressId = addr[6], City = "Pärnu", Region = "Pärnu", AddressName = "Rüütli 40", PostalCode = "80011", Country = "Estonia", PhoneNumber = "+37255504001" },
                new Address { AddressId = addr[7], City = "Jõhvi", Region = "Ida-Virumaa", AddressName = "Keskväljak 4", PostalCode = "41531", Country = "Estonia", PhoneNumber = "+37255505001" }
            );

            // ── Activities (16) ──
            var act = Enumerable.Range(1, 16).ToDictionary(i => i, i => GenerateDeterministicGuid('C', i));

            modelBuilder.Entity<Activity>().HasData(
                new Activity { ActivityId = act[1],  Name = "Monopoly",          Description = "Classic property trading board game",            Category = "Board Game" },
                new Activity { ActivityId = act[2],  Name = "Uno",               Description = "Fast-paced card matching game",                  Category = "Board Game" },
                new Activity { ActivityId = act[3],  Name = "Chess",             Description = "Classic strategy game for two players",           Category = "Board Game" },
                new Activity { ActivityId = act[4],  Name = "Scrabble",          Description = "Word-building board game",                       Category = "Board Game" },
                new Activity { ActivityId = act[5],  Name = "Catan",             Description = "Resource trading and settlement building game",   Category = "Board Game" },
                new Activity { ActivityId = act[6],  Name = "Dungeons & Dragons", Description = "Tabletop role-playing adventure game",           Category = "Board Game" },
                new Activity { ActivityId = act[7],  Name = "Dixit",             Description = "Creative storytelling card game",                 Category = "Board Game" },
                new Activity { ActivityId = act[8],  Name = "Ticket to Ride",    Description = "Railway route-building board game",               Category = "Board Game" },
                new Activity { ActivityId = act[9],  Name = "Poker",             Description = "Classic bluffing and betting card game",          Category = "Card Game" },
                new Activity { ActivityId = act[10], Name = "Mafia",             Description = "Social deduction party game",                    Category = "Card Game" },
                new Activity { ActivityId = act[11], Name = "Billiards",         Description = "Classic cue sport on a felt-covered table",       Category = "Sport Game" },
                new Activity { ActivityId = act[12], Name = "Foosball",          Description = "Table football for two or four players",          Category = "Sport Game" },
                new Activity { ActivityId = act[13], Name = "PlayStation",       Description = "Sony gaming console with popular titles",         Category = "Console" },
                new Activity { ActivityId = act[14], Name = "Nintendo Switch",   Description = "Portable gaming console for group play",          Category = "Console" },
                new Activity { ActivityId = act[15], Name = "Karaoke",           Description = "Sing along with music and lyrics on screen",      Category = "Entertainment" },
                new Activity { ActivityId = act[16], Name = "Movie Screening",   Description = "Watch films on a big screen with surround sound", Category = "Entertainment" }
            );

            // ── Rooms (15) ──
            var room = Enumerable.Range(1, 15).ToDictionary(i => i, i => GenerateDeterministicGuid('F', i));
            var now = new DateTime(2025, 12, 15, 12, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Room>().HasData(
                // Tallinn (5)
                new Room { RoomId = room[1],  Name = "Cozy Corner",      Description = "A warm space with board games and soft sofas for quiet evenings",               PricePerHour = 5.00m,  Capacity = 6,  Status = "Available",   AddressId = addr[1], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[2],  Name = "Game Hub",         Description = "Main gaming zone with billiards, foosball and board games",                     PricePerHour = 12.00m, Capacity = 12, Status = "Available",   AddressId = addr[1], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[3],  Name = "VIP Lounge",       Description = "Premium private room with projector, sound system and exclusive atmosphere",    PricePerHour = 25.00m, Capacity = 8,  Status = "Available",   AddressId = addr[2], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[4],  Name = "Cinema Room",      Description = "Cozy room with a big screen for watching movies and series together",           PricePerHour = 15.00m, Capacity = 10, Status = "Occupied",    AddressId = addr[2], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[5],  Name = "Console Corner",   Description = "PlayStation and Nintendo Switch with comfortable seating",                      PricePerHour = 10.00m, Capacity = 6,  Status = "Available",   AddressId = addr[2], CreatedAt = now, UpdatedAt = now },
                // Tartu (4)
                new Room { RoomId = room[6],  Name = "Board Game Vault", Description = "A collection of 100+ board games in a cozy setting",                           PricePerHour = 6.00m,  Capacity = 8,  Status = "Available",   AddressId = addr[3], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[7],  Name = "Creative Space",   Description = "Spacious room for workshops, drawing and creative meetups",                    PricePerHour = 8.00m,  Capacity = 15, Status = "Available",   AddressId = addr[3], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[8],  Name = "Study Lounge",     Description = "Quiet zone for studying and remote work with fast Wi-Fi",                      PricePerHour = 4.00m,  Capacity = 6,  Status = "Available",   AddressId = addr[4], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[9],  Name = "Chill Zone",       Description = "Bean bags, soft cushions and calm atmosphere for reading and relaxation",       PricePerHour = 5.00m,  Capacity = 8,  Status = "Maintenance", AddressId = addr[4], CreatedAt = now, UpdatedAt = now },
                // Narva (3)
                new Room { RoomId = room[10], Name = "Party Room",       Description = "Large hall for birthdays, celebrations and group events",                       PricePerHour = 20.00m, Capacity = 25, Status = "Available",   AddressId = addr[5], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[11], Name = "Family Room",      Description = "Room for family fun with games suitable for all ages",                         PricePerHour = 7.00m,  Capacity = 10, Status = "Occupied",    AddressId = addr[5], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[12], Name = "Sport Arena",      Description = "Active zone with billiards, foosball and darts",                               PricePerHour = 10.00m, Capacity = 10, Status = "Available",   AddressId = addr[5], CreatedAt = now, UpdatedAt = now },
                // Pärnu (2)
                new Room { RoomId = room[13], Name = "Loft Space",       Description = "Spacious upper-floor area with board games and card games",                    PricePerHour = 8.00m,  Capacity = 12, Status = "Available",   AddressId = addr[6], CreatedAt = now, UpdatedAt = now },
                new Room { RoomId = room[14], Name = "Green Room",       Description = "Room filled with live plants and a calm atmosphere",                           PricePerHour = 6.00m,  Capacity = 6,  Status = "Maintenance", AddressId = addr[6], CreatedAt = now, UpdatedAt = now },
                // Jõhvi (1)
                new Room { RoomId = room[15], Name = "Workshop Studio",  Description = "Drawing, clay modeling and DIY projects with all materials included",           PricePerHour = 9.00m,  Capacity = 10, Status = "Available",   AddressId = addr[7], CreatedAt = now, UpdatedAt = now }
            );

            // ── RoomActivity links ──
            modelBuilder.Entity<RoomActivity>().HasData(
                // Cozy Corner: board games
                new RoomActivity { RoomId = room[1], ActivityId = act[1] },  // Monopoly
                new RoomActivity { RoomId = room[1], ActivityId = act[2] },  // Uno
                new RoomActivity { RoomId = room[1], ActivityId = act[7] },  // Dixit
                // Game Hub: sport + board
                new RoomActivity { RoomId = room[2], ActivityId = act[11] }, // Billiards
                new RoomActivity { RoomId = room[2], ActivityId = act[12] }, // Foosball
                new RoomActivity { RoomId = room[2], ActivityId = act[1] },  // Monopoly
                new RoomActivity { RoomId = room[2], ActivityId = act[3] },  // Chess
                // VIP Lounge: entertainment + cards
                new RoomActivity { RoomId = room[3], ActivityId = act[15] }, // Karaoke
                new RoomActivity { RoomId = room[3], ActivityId = act[16] }, // Movie Screening
                new RoomActivity { RoomId = room[3], ActivityId = act[9] },  // Poker
                // Cinema Room: movie + console
                new RoomActivity { RoomId = room[4], ActivityId = act[16] }, // Movie Screening
                new RoomActivity { RoomId = room[4], ActivityId = act[13] }, // PlayStation
                // Console Corner: consoles
                new RoomActivity { RoomId = room[5], ActivityId = act[13] }, // PlayStation
                new RoomActivity { RoomId = room[5], ActivityId = act[14] }, // Nintendo Switch
                // Board Game Vault: many board games
                new RoomActivity { RoomId = room[6], ActivityId = act[1] },  // Monopoly
                new RoomActivity { RoomId = room[6], ActivityId = act[3] },  // Chess
                new RoomActivity { RoomId = room[6], ActivityId = act[4] },  // Scrabble
                new RoomActivity { RoomId = room[6], ActivityId = act[5] },  // Catan
                new RoomActivity { RoomId = room[6], ActivityId = act[6] },  // D&D
                new RoomActivity { RoomId = room[6], ActivityId = act[8] },  // Ticket to Ride
                // Creative Space: board games + social
                new RoomActivity { RoomId = room[7], ActivityId = act[7] },  // Dixit
                new RoomActivity { RoomId = room[7], ActivityId = act[10] }, // Mafia
                // Study Lounge: chess + scrabble (quiet)
                new RoomActivity { RoomId = room[8], ActivityId = act[3] },  // Chess
                new RoomActivity { RoomId = room[8], ActivityId = act[4] },  // Scrabble
                // Chill Zone: light games
                new RoomActivity { RoomId = room[9], ActivityId = act[2] },  // Uno
                new RoomActivity { RoomId = room[9], ActivityId = act[7] },  // Dixit
                // Party Room: social + entertainment
                new RoomActivity { RoomId = room[10], ActivityId = act[10] }, // Mafia
                new RoomActivity { RoomId = room[10], ActivityId = act[15] }, // Karaoke
                new RoomActivity { RoomId = room[10], ActivityId = act[9] },  // Poker
                new RoomActivity { RoomId = room[10], ActivityId = act[2] },  // Uno
                // Family Room: family-friendly
                new RoomActivity { RoomId = room[11], ActivityId = act[1] },  // Monopoly
                new RoomActivity { RoomId = room[11], ActivityId = act[2] },  // Uno
                new RoomActivity { RoomId = room[11], ActivityId = act[14] }, // Nintendo Switch
                new RoomActivity { RoomId = room[11], ActivityId = act[8] },  // Ticket to Ride
                // Sport Arena: sport + active
                new RoomActivity { RoomId = room[12], ActivityId = act[11] }, // Billiards
                new RoomActivity { RoomId = room[12], ActivityId = act[12] }, // Foosball
                // Loft Space: board + card
                new RoomActivity { RoomId = room[13], ActivityId = act[5] },  // Catan
                new RoomActivity { RoomId = room[13], ActivityId = act[6] },  // D&D
                new RoomActivity { RoomId = room[13], ActivityId = act[9] },  // Poker
                // Green Room: calm games
                new RoomActivity { RoomId = room[14], ActivityId = act[3] },  // Chess
                new RoomActivity { RoomId = room[14], ActivityId = act[7] },  // Dixit
                // Workshop Studio: creative + social
                new RoomActivity { RoomId = room[15], ActivityId = act[7] },  // Dixit
                new RoomActivity { RoomId = room[15], ActivityId = act[10] }  // Mafia
            );

            // ── RoomImages (30) ──
            var img = Enumerable.Range(1, 30).ToDictionary(i => i, i => GenerateDeterministicGuid('D', i));

            modelBuilder.Entity<RoomImage>().HasData(
                // Cozy Corner
                new RoomImage { ImageId = img[1],  RoomId = room[1],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482586/home/LumeLaht/qpjpigt9nqnnwigx2we3.jpg",  CloudinaryPublicId = "home/LumeLaht/qpjpigt9nqnnwigx2we3" },
                new RoomImage { ImageId = img[2],  RoomId = room[1],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482590/home/LumeLaht/pgwxgmmghfsc7ngiahxb.jpg",  CloudinaryPublicId = "home/LumeLaht/pgwxgmmghfsc7ngiahxb" },
                // Game Hub
                new RoomImage { ImageId = img[3],  RoomId = room[2],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482803/home/LumeLaht/gwhzr9ytxlrquih6m5at.jpg",  CloudinaryPublicId = "home/LumeLaht/gwhzr9ytxlrquih6m5at" },
                new RoomImage { ImageId = img[4],  RoomId = room[2],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482604/home/LumeLaht/hvv7upflpq1cgjk15gh3.jpg",  CloudinaryPublicId = "home/LumeLaht/hvv7upflpq1cgjk15gh3" },
                // VIP Lounge
                new RoomImage { ImageId = img[5],  RoomId = room[3],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482767/home/LumeLaht/ysv9byftiovr4d4rjgfu.jpg",  CloudinaryPublicId = "home/LumeLaht/ysv9byftiovr4d4rjgfu" },
                new RoomImage { ImageId = img[6],  RoomId = room[3],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482791/home/LumeLaht/ancef5r4tiybxbemcria.jpg",  CloudinaryPublicId = "home/LumeLaht/ancef5r4tiybxbemcria" },
                // Cinema Room
                new RoomImage { ImageId = img[7],  RoomId = room[4],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482699/home/LumeLaht/bqelx6s1kxpr0phuajl5.jpg",  CloudinaryPublicId = "home/LumeLaht/bqelx6s1kxpr0phuajl5" },
                new RoomImage { ImageId = img[8],  RoomId = room[4],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482704/home/LumeLaht/hjdaotzh7ks8vtdw5qni.jpg",  CloudinaryPublicId = "home/LumeLaht/hjdaotzh7ks8vtdw5qni" },
                // Console Corner
                new RoomImage { ImageId = img[9],  RoomId = room[5],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482613/home/LumeLaht/msc2bmhotims1emimplv.jpg",  CloudinaryPublicId = "home/LumeLaht/msc2bmhotims1emimplv" },
                new RoomImage { ImageId = img[10], RoomId = room[5],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482783/home/LumeLaht/r6vocb2tofb58wzrbutm.jpg",  CloudinaryPublicId = "home/LumeLaht/r6vocb2tofb58wzrbutm" },
                // Board Game Vault
                new RoomImage { ImageId = img[11], RoomId = room[6],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482600/home/LumeLaht/yqimpvuynmvft8ge1xb1.jpg",  CloudinaryPublicId = "home/LumeLaht/yqimpvuynmvft8ge1xb1" },
                new RoomImage { ImageId = img[12], RoomId = room[6],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482797/home/LumeLaht/e0fggn0wdjnncsfzu5ph.jpg",  CloudinaryPublicId = "home/LumeLaht/e0fggn0wdjnncsfzu5ph" },
                // Creative Space
                new RoomImage { ImageId = img[13], RoomId = room[7],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482754/home/LumeLaht/kvn5gcm4ykrxr94xhjpg.jpg",  CloudinaryPublicId = "home/LumeLaht/kvn5gcm4ykrxr94xhjpg" },
                new RoomImage { ImageId = img[14], RoomId = room[7],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482758/home/LumeLaht/lu3g8tbm51681draqqsk.jpg",  CloudinaryPublicId = "home/LumeLaht/lu3g8tbm51681draqqsk" },
                // Study Lounge
                new RoomImage { ImageId = img[15], RoomId = room[8],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482770/home/LumeLaht/ossh3aqbxmlpowedshjy.jpg",  CloudinaryPublicId = "home/LumeLaht/ossh3aqbxmlpowedshjy" },
                new RoomImage { ImageId = img[16], RoomId = room[8],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482788/home/LumeLaht/tqhyz03tx9h8prrhotbc.jpg",  CloudinaryPublicId = "home/LumeLaht/tqhyz03tx9h8prrhotbc" },
                // Chill Zone
                new RoomImage { ImageId = img[17], RoomId = room[9],  IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482619/home/LumeLaht/jikegxhcjgjimghpceou.jpg",  CloudinaryPublicId = "home/LumeLaht/jikegxhcjgjimghpceou" },
                new RoomImage { ImageId = img[18], RoomId = room[9],  IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482774/home/LumeLaht/ktf5vq0dnyt1fhugwgja.jpg",  CloudinaryPublicId = "home/LumeLaht/ktf5vq0dnyt1fhugwgja" },
                // Party Room
                new RoomImage { ImageId = img[19], RoomId = room[10], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482752/home/LumeLaht/wxvbynaa8r2nydcsvi6n.jpg",  CloudinaryPublicId = "home/LumeLaht/wxvbynaa8r2nydcsvi6n" },
                new RoomImage { ImageId = img[20], RoomId = room[10], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482777/home/LumeLaht/eiohgfhu3tyw3ywbyrat.jpg",  CloudinaryPublicId = "home/LumeLaht/eiohgfhu3tyw3ywbyrat" },
                // Family Room
                new RoomImage { ImageId = img[21], RoomId = room[11], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482595/home/LumeLaht/e6ey92ny3mlqjojfkdvd.jpg",  CloudinaryPublicId = "home/LumeLaht/e6ey92ny3mlqjojfkdvd" },
                new RoomImage { ImageId = img[22], RoomId = room[11], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482761/home/LumeLaht/kwl77thrfdqoxt6sq9be.jpg",  CloudinaryPublicId = "home/LumeLaht/kwl77thrfdqoxt6sq9be" },
                // Sport Arena
                new RoomImage { ImageId = img[23], RoomId = room[12], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482803/home/LumeLaht/gwhzr9ytxlrquih6m5at.jpg",  CloudinaryPublicId = "home/LumeLaht/gwhzr9ytxlrquih6m5at" },
                new RoomImage { ImageId = img[24], RoomId = room[12], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482604/home/LumeLaht/hvv7upflpq1cgjk15gh3.jpg",  CloudinaryPublicId = "home/LumeLaht/hvv7upflpq1cgjk15gh3" },
                // Loft Space
                new RoomImage { ImageId = img[25], RoomId = room[13], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482777/home/LumeLaht/eiohgfhu3tyw3ywbyrat.jpg",  CloudinaryPublicId = "home/LumeLaht/eiohgfhu3tyw3ywbyrat" },
                new RoomImage { ImageId = img[26], RoomId = room[13], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482797/home/LumeLaht/e0fggn0wdjnncsfzu5ph.jpg",  CloudinaryPublicId = "home/LumeLaht/e0fggn0wdjnncsfzu5ph" },
                // Green Room
                new RoomImage { ImageId = img[27], RoomId = room[14], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482774/home/LumeLaht/ktf5vq0dnyt1fhugwgja.jpg",  CloudinaryPublicId = "home/LumeLaht/ktf5vq0dnyt1fhugwgja" },
                new RoomImage { ImageId = img[28], RoomId = room[14], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482794/home/LumeLaht/sqqbapuwso9etgppxwrw.jpg",  CloudinaryPublicId = "home/LumeLaht/sqqbapuwso9etgppxwrw" },
                // Workshop Studio
                new RoomImage { ImageId = img[29], RoomId = room[15], IsMain = true,  Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482758/home/LumeLaht/lu3g8tbm51681draqqsk.jpg",  CloudinaryPublicId = "home/LumeLaht/lu3g8tbm51681draqqsk" },
                new RoomImage { ImageId = img[30], RoomId = room[15], IsMain = false, Url = "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482765/home/LumeLaht/frd5jjvloef3ura6hppd.jpg",  CloudinaryPublicId = "home/LumeLaht/frd5jjvloef3ura6hppd" }
            );
        }
    }
}
