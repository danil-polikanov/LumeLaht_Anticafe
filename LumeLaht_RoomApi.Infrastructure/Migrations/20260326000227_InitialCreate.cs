using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LumeLaht_RoomApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityId);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CloudinaryPublicId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_RoomImages_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsActivity",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsActivity", x => new { x.RoomId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_RoomsActivity_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsActivity_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "ActivityId", "Category", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("43000001-0000-0000-0000-000000000000"), "Board Game", "Classic property trading board game", "Monopoly" },
                    { new Guid("43000002-0000-0000-0000-000000000000"), "Board Game", "Fast-paced card matching game", "Uno" },
                    { new Guid("43000003-0000-0000-0000-000000000000"), "Board Game", "Classic strategy game for two players", "Chess" },
                    { new Guid("43000004-0000-0000-0000-000000000000"), "Board Game", "Word-building board game", "Scrabble" },
                    { new Guid("43000005-0000-0000-0000-000000000000"), "Board Game", "Resource trading and settlement building game", "Catan" },
                    { new Guid("43000006-0000-0000-0000-000000000000"), "Board Game", "Tabletop role-playing adventure game", "Dungeons & Dragons" },
                    { new Guid("43000007-0000-0000-0000-000000000000"), "Board Game", "Creative storytelling card game", "Dixit" },
                    { new Guid("43000008-0000-0000-0000-000000000000"), "Board Game", "Railway route-building board game", "Ticket to Ride" },
                    { new Guid("43000009-0000-0000-0000-000000000000"), "Card Game", "Classic bluffing and betting card game", "Poker" },
                    { new Guid("43000010-0000-0000-0000-000000000000"), "Card Game", "Social deduction party game", "Mafia" },
                    { new Guid("43000011-0000-0000-0000-000000000000"), "Sport Game", "Classic cue sport on a felt-covered table", "Billiards" },
                    { new Guid("43000012-0000-0000-0000-000000000000"), "Sport Game", "Table football for two or four players", "Foosball" },
                    { new Guid("43000013-0000-0000-0000-000000000000"), "Console", "Sony gaming console with popular titles", "PlayStation" },
                    { new Guid("43000014-0000-0000-0000-000000000000"), "Console", "Portable gaming console for group play", "Nintendo Switch" },
                    { new Guid("43000015-0000-0000-0000-000000000000"), "Entertainment", "Sing along with music and lyrics on screen", "Karaoke" },
                    { new Guid("43000016-0000-0000-0000-000000000000"), "Entertainment", "Watch films on a big screen with surround sound", "Movie Screening" }
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "AddressId", "AddressName", "City", "Country", "PhoneNumber", "PostalCode", "Region" },
                values: new object[,]
                {
                    { new Guid("42000001-0000-0000-0000-000000000000"), "Viru 15", "Tallinn", "Estonia", "+37255501001", "10140", "Harju" },
                    { new Guid("42000002-0000-0000-0000-000000000000"), "Telliskivi 60a", "Tallinn", "Estonia", "+37255501002", "10412", "Harju" },
                    { new Guid("42000003-0000-0000-0000-000000000000"), "Riia 2", "Tartu", "Estonia", "+37255502001", "51004", "Tartu" },
                    { new Guid("42000004-0000-0000-0000-000000000000"), "Küüni 5b", "Tartu", "Estonia", "+37255502002", "51003", "Tartu" },
                    { new Guid("42000005-0000-0000-0000-000000000000"), "Pushkini 20", "Narva", "Estonia", "+37255503001", "20309", "Ida-Virumaa" },
                    { new Guid("42000006-0000-0000-0000-000000000000"), "Rüütli 40", "Pärnu", "Estonia", "+37255504001", "80011", "Pärnu" },
                    { new Guid("42000007-0000-0000-0000-000000000000"), "Keskväljak 4", "Jõhvi", "Estonia", "+37255505001", "41531", "Ida-Virumaa" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "AddressId", "Capacity", "CreatedAt", "Description", "Name", "PricePerHour", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("46000001-0000-0000-0000-000000000000"), new Guid("42000001-0000-0000-0000-000000000000"), 6, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "A warm space with board games and soft sofas for quiet evenings", "Cozy Corner", 5.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000002-0000-0000-0000-000000000000"), new Guid("42000001-0000-0000-0000-000000000000"), 12, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Main gaming zone with billiards, foosball and board games", "Game Hub", 12.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000003-0000-0000-0000-000000000000"), new Guid("42000002-0000-0000-0000-000000000000"), 8, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Premium private room with projector, sound system and exclusive atmosphere", "VIP Lounge", 25.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000004-0000-0000-0000-000000000000"), new Guid("42000002-0000-0000-0000-000000000000"), 10, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Cozy room with a big screen for watching movies and series together", "Cinema Room", 15.00m, "Occupied", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000005-0000-0000-0000-000000000000"), new Guid("42000002-0000-0000-0000-000000000000"), 6, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "PlayStation and Nintendo Switch with comfortable seating", "Console Corner", 10.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000006-0000-0000-0000-000000000000"), new Guid("42000003-0000-0000-0000-000000000000"), 8, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "A collection of 100+ board games in a cozy setting", "Board Game Vault", 6.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000007-0000-0000-0000-000000000000"), new Guid("42000003-0000-0000-0000-000000000000"), 15, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Spacious room for workshops, drawing and creative meetups", "Creative Space", 8.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000008-0000-0000-0000-000000000000"), new Guid("42000004-0000-0000-0000-000000000000"), 6, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Quiet zone for studying and remote work with fast Wi-Fi", "Study Lounge", 4.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000009-0000-0000-0000-000000000000"), new Guid("42000004-0000-0000-0000-000000000000"), 8, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Bean bags, soft cushions and calm atmosphere for reading and relaxation", "Chill Zone", 5.00m, "Maintenance", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000010-0000-0000-0000-000000000000"), new Guid("42000005-0000-0000-0000-000000000000"), 25, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Large hall for birthdays, celebrations and group events", "Party Room", 20.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000011-0000-0000-0000-000000000000"), new Guid("42000005-0000-0000-0000-000000000000"), 10, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Room for family fun with games suitable for all ages", "Family Room", 7.00m, "Occupied", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000012-0000-0000-0000-000000000000"), new Guid("42000005-0000-0000-0000-000000000000"), 10, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Active zone with billiards, foosball and darts", "Sport Arena", 10.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000013-0000-0000-0000-000000000000"), new Guid("42000006-0000-0000-0000-000000000000"), 12, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Spacious upper-floor area with board games and card games", "Loft Space", 8.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000014-0000-0000-0000-000000000000"), new Guid("42000006-0000-0000-0000-000000000000"), 6, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Room filled with live plants and a calm atmosphere", "Green Room", 6.00m, "Maintenance", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("46000015-0000-0000-0000-000000000000"), new Guid("42000007-0000-0000-0000-000000000000"), 10, new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc), "Drawing, clay modeling and DIY projects with all materials included", "Workshop Studio", 9.00m, "Available", new DateTime(2025, 12, 15, 12, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "ImageId", "CloudinaryPublicId", "IsMain", "RoomId", "Url" },
                values: new object[,]
                {
                    { new Guid("44000001-0000-0000-0000-000000000000"), "home/LumeLaht/qpjpigt9nqnnwigx2we3", true, new Guid("46000001-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482586/home/LumeLaht/qpjpigt9nqnnwigx2we3.jpg" },
                    { new Guid("44000002-0000-0000-0000-000000000000"), "home/LumeLaht/pgwxgmmghfsc7ngiahxb", false, new Guid("46000001-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482590/home/LumeLaht/pgwxgmmghfsc7ngiahxb.jpg" },
                    { new Guid("44000003-0000-0000-0000-000000000000"), "home/LumeLaht/gwhzr9ytxlrquih6m5at", true, new Guid("46000002-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482803/home/LumeLaht/gwhzr9ytxlrquih6m5at.jpg" },
                    { new Guid("44000004-0000-0000-0000-000000000000"), "home/LumeLaht/hvv7upflpq1cgjk15gh3", false, new Guid("46000002-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482604/home/LumeLaht/hvv7upflpq1cgjk15gh3.jpg" },
                    { new Guid("44000005-0000-0000-0000-000000000000"), "home/LumeLaht/ysv9byftiovr4d4rjgfu", true, new Guid("46000003-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482767/home/LumeLaht/ysv9byftiovr4d4rjgfu.jpg" },
                    { new Guid("44000006-0000-0000-0000-000000000000"), "home/LumeLaht/ancef5r4tiybxbemcria", false, new Guid("46000003-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482791/home/LumeLaht/ancef5r4tiybxbemcria.jpg" },
                    { new Guid("44000007-0000-0000-0000-000000000000"), "home/LumeLaht/bqelx6s1kxpr0phuajl5", true, new Guid("46000004-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482699/home/LumeLaht/bqelx6s1kxpr0phuajl5.jpg" },
                    { new Guid("44000008-0000-0000-0000-000000000000"), "home/LumeLaht/hjdaotzh7ks8vtdw5qni", false, new Guid("46000004-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482704/home/LumeLaht/hjdaotzh7ks8vtdw5qni.jpg" },
                    { new Guid("44000009-0000-0000-0000-000000000000"), "home/LumeLaht/msc2bmhotims1emimplv", true, new Guid("46000005-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482613/home/LumeLaht/msc2bmhotims1emimplv.jpg" },
                    { new Guid("44000010-0000-0000-0000-000000000000"), "home/LumeLaht/r6vocb2tofb58wzrbutm", false, new Guid("46000005-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482783/home/LumeLaht/r6vocb2tofb58wzrbutm.jpg" },
                    { new Guid("44000011-0000-0000-0000-000000000000"), "home/LumeLaht/yqimpvuynmvft8ge1xb1", true, new Guid("46000006-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482600/home/LumeLaht/yqimpvuynmvft8ge1xb1.jpg" },
                    { new Guid("44000012-0000-0000-0000-000000000000"), "home/LumeLaht/e0fggn0wdjnncsfzu5ph", false, new Guid("46000006-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482797/home/LumeLaht/e0fggn0wdjnncsfzu5ph.jpg" },
                    { new Guid("44000013-0000-0000-0000-000000000000"), "home/LumeLaht/kvn5gcm4ykrxr94xhjpg", true, new Guid("46000007-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482754/home/LumeLaht/kvn5gcm4ykrxr94xhjpg.jpg" },
                    { new Guid("44000014-0000-0000-0000-000000000000"), "home/LumeLaht/lu3g8tbm51681draqqsk", false, new Guid("46000007-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482758/home/LumeLaht/lu3g8tbm51681draqqsk.jpg" },
                    { new Guid("44000015-0000-0000-0000-000000000000"), "home/LumeLaht/ossh3aqbxmlpowedshjy", true, new Guid("46000008-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482770/home/LumeLaht/ossh3aqbxmlpowedshjy.jpg" },
                    { new Guid("44000016-0000-0000-0000-000000000000"), "home/LumeLaht/tqhyz03tx9h8prrhotbc", false, new Guid("46000008-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482788/home/LumeLaht/tqhyz03tx9h8prrhotbc.jpg" },
                    { new Guid("44000017-0000-0000-0000-000000000000"), "home/LumeLaht/jikegxhcjgjimghpceou", true, new Guid("46000009-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482619/home/LumeLaht/jikegxhcjgjimghpceou.jpg" },
                    { new Guid("44000018-0000-0000-0000-000000000000"), "home/LumeLaht/ktf5vq0dnyt1fhugwgja", false, new Guid("46000009-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482774/home/LumeLaht/ktf5vq0dnyt1fhugwgja.jpg" },
                    { new Guid("44000019-0000-0000-0000-000000000000"), "home/LumeLaht/wxvbynaa8r2nydcsvi6n", true, new Guid("46000010-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482752/home/LumeLaht/wxvbynaa8r2nydcsvi6n.jpg" },
                    { new Guid("44000020-0000-0000-0000-000000000000"), "home/LumeLaht/eiohgfhu3tyw3ywbyrat", false, new Guid("46000010-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482777/home/LumeLaht/eiohgfhu3tyw3ywbyrat.jpg" },
                    { new Guid("44000021-0000-0000-0000-000000000000"), "home/LumeLaht/e6ey92ny3mlqjojfkdvd", true, new Guid("46000011-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482595/home/LumeLaht/e6ey92ny3mlqjojfkdvd.jpg" },
                    { new Guid("44000022-0000-0000-0000-000000000000"), "home/LumeLaht/kwl77thrfdqoxt6sq9be", false, new Guid("46000011-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482761/home/LumeLaht/kwl77thrfdqoxt6sq9be.jpg" },
                    { new Guid("44000023-0000-0000-0000-000000000000"), "home/LumeLaht/gwhzr9ytxlrquih6m5at", true, new Guid("46000012-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482803/home/LumeLaht/gwhzr9ytxlrquih6m5at.jpg" },
                    { new Guid("44000024-0000-0000-0000-000000000000"), "home/LumeLaht/hvv7upflpq1cgjk15gh3", false, new Guid("46000012-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482604/home/LumeLaht/hvv7upflpq1cgjk15gh3.jpg" },
                    { new Guid("44000025-0000-0000-0000-000000000000"), "home/LumeLaht/eiohgfhu3tyw3ywbyrat", true, new Guid("46000013-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482777/home/LumeLaht/eiohgfhu3tyw3ywbyrat.jpg" },
                    { new Guid("44000026-0000-0000-0000-000000000000"), "home/LumeLaht/e0fggn0wdjnncsfzu5ph", false, new Guid("46000013-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482797/home/LumeLaht/e0fggn0wdjnncsfzu5ph.jpg" },
                    { new Guid("44000027-0000-0000-0000-000000000000"), "home/LumeLaht/ktf5vq0dnyt1fhugwgja", true, new Guid("46000014-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482774/home/LumeLaht/ktf5vq0dnyt1fhugwgja.jpg" },
                    { new Guid("44000028-0000-0000-0000-000000000000"), "home/LumeLaht/sqqbapuwso9etgppxwrw", false, new Guid("46000014-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482794/home/LumeLaht/sqqbapuwso9etgppxwrw.jpg" },
                    { new Guid("44000029-0000-0000-0000-000000000000"), "home/LumeLaht/lu3g8tbm51681draqqsk", true, new Guid("46000015-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482758/home/LumeLaht/lu3g8tbm51681draqqsk.jpg" },
                    { new Guid("44000030-0000-0000-0000-000000000000"), "home/LumeLaht/frd5jjvloef3ura6hppd", false, new Guid("46000015-0000-0000-0000-000000000000"), "https://res.cloudinary.com/dw8apd46g/image/upload/v1774482765/home/LumeLaht/frd5jjvloef3ura6hppd.jpg" }
                });

            migrationBuilder.InsertData(
                table: "RoomsActivity",
                columns: new[] { "ActivityId", "RoomId" },
                values: new object[,]
                {
                    { new Guid("43000001-0000-0000-0000-000000000000"), new Guid("46000001-0000-0000-0000-000000000000") },
                    { new Guid("43000002-0000-0000-0000-000000000000"), new Guid("46000001-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000001-0000-0000-0000-000000000000") },
                    { new Guid("43000001-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000003-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000011-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000012-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000009-0000-0000-0000-000000000000"), new Guid("46000003-0000-0000-0000-000000000000") },
                    { new Guid("43000015-0000-0000-0000-000000000000"), new Guid("46000003-0000-0000-0000-000000000000") },
                    { new Guid("43000016-0000-0000-0000-000000000000"), new Guid("46000003-0000-0000-0000-000000000000") },
                    { new Guid("43000013-0000-0000-0000-000000000000"), new Guid("46000004-0000-0000-0000-000000000000") },
                    { new Guid("43000016-0000-0000-0000-000000000000"), new Guid("46000004-0000-0000-0000-000000000000") },
                    { new Guid("43000013-0000-0000-0000-000000000000"), new Guid("46000005-0000-0000-0000-000000000000") },
                    { new Guid("43000014-0000-0000-0000-000000000000"), new Guid("46000005-0000-0000-0000-000000000000") },
                    { new Guid("43000001-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000003-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000004-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000005-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000006-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000008-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000007-0000-0000-0000-000000000000") },
                    { new Guid("43000010-0000-0000-0000-000000000000"), new Guid("46000007-0000-0000-0000-000000000000") },
                    { new Guid("43000003-0000-0000-0000-000000000000"), new Guid("46000008-0000-0000-0000-000000000000") },
                    { new Guid("43000004-0000-0000-0000-000000000000"), new Guid("46000008-0000-0000-0000-000000000000") },
                    { new Guid("43000002-0000-0000-0000-000000000000"), new Guid("46000009-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000009-0000-0000-0000-000000000000") },
                    { new Guid("43000002-0000-0000-0000-000000000000"), new Guid("46000010-0000-0000-0000-000000000000") },
                    { new Guid("43000009-0000-0000-0000-000000000000"), new Guid("46000010-0000-0000-0000-000000000000") },
                    { new Guid("43000010-0000-0000-0000-000000000000"), new Guid("46000010-0000-0000-0000-000000000000") },
                    { new Guid("43000015-0000-0000-0000-000000000000"), new Guid("46000010-0000-0000-0000-000000000000") },
                    { new Guid("43000001-0000-0000-0000-000000000000"), new Guid("46000011-0000-0000-0000-000000000000") },
                    { new Guid("43000002-0000-0000-0000-000000000000"), new Guid("46000011-0000-0000-0000-000000000000") },
                    { new Guid("43000008-0000-0000-0000-000000000000"), new Guid("46000011-0000-0000-0000-000000000000") },
                    { new Guid("43000014-0000-0000-0000-000000000000"), new Guid("46000011-0000-0000-0000-000000000000") },
                    { new Guid("43000011-0000-0000-0000-000000000000"), new Guid("46000012-0000-0000-0000-000000000000") },
                    { new Guid("43000012-0000-0000-0000-000000000000"), new Guid("46000012-0000-0000-0000-000000000000") },
                    { new Guid("43000005-0000-0000-0000-000000000000"), new Guid("46000013-0000-0000-0000-000000000000") },
                    { new Guid("43000006-0000-0000-0000-000000000000"), new Guid("46000013-0000-0000-0000-000000000000") },
                    { new Guid("43000009-0000-0000-0000-000000000000"), new Guid("46000013-0000-0000-0000-000000000000") },
                    { new Guid("43000003-0000-0000-0000-000000000000"), new Guid("46000014-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000014-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000015-0000-0000-0000-000000000000") },
                    { new Guid("43000010-0000-0000-0000-000000000000"), new Guid("46000015-0000-0000-0000-000000000000") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomImages_RoomId",
                table: "RoomImages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AddressId",
                table: "Rooms",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsActivity_ActivityId",
                table: "RoomsActivity",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomImages");

            migrationBuilder.DropTable(
                name: "RoomsActivity");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
