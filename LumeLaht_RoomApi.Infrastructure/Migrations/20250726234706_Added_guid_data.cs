using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LumeLaht_RoomApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_guid_data : Migration
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
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
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
                    PricePerHour = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                columns: new[] { "ActivityId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("43000001-0000-0000-0000-000000000000"), "Table Game", "Monopolia" },
                    { new Guid("43000002-0000-0000-0000-000000000000"), "Table Game", "Uno" },
                    { new Guid("43000003-0000-0000-0000-000000000000"), "Table Game", "Chess" },
                    { new Guid("43000004-0000-0000-0000-000000000000"), "Table Game", "Scrabble" },
                    { new Guid("43000005-0000-0000-0000-000000000000"), "Sport Game", "Ping Pong" },
                    { new Guid("43000006-0000-0000-0000-000000000000"), "Sport Game", "Foosball" },
                    { new Guid("43000007-0000-0000-0000-000000000000"), "Sport Game", "Billiards" },
                    { new Guid("43000008-0000-0000-0000-000000000000"), "Sport Game", "Darts" },
                    { new Guid("43000009-0000-0000-0000-000000000000"), "Sport Game", "Poker" },
                    { new Guid("43000010-0000-0000-0000-000000000000"), "Sport Game", "Blackjack" },
                    { new Guid("43000011-0000-0000-0000-000000000000"), "Sport Game", "Bowling" },
                    { new Guid("43000012-0000-0000-0000-000000000000"), "Sport Game", "Table Tennis" }
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "AddressId", "AddressName", "City", "Country", "PhoneNumber", "PostalCode", "Region" },
                values: new object[,]
                {
                    { new Guid("42000001-0000-0000-0000-000000000000"), "Gagarini 11", "Narva", "Estonia", "+37254356533", "12341", "Ida-Virumaa" },
                    { new Guid("42000002-0000-0000-0000-000000000000"), "Narva mnt 32", "Narva", "Estonia", "+37254351534", "12342", "Ida-Virumaa" },
                    { new Guid("42000003-0000-0000-0000-000000000000"), "Taamsaare 24", "Jõhvi", "Estonia", "+37254326535", "12343", "Ida-Virumaa" },
                    { new Guid("42000004-0000-0000-0000-000000000000"), "Vaba mnt 105", "Narva", "Estonia", "+37254365536", "12344", "Ida-Virumaa" },
                    { new Guid("42000005-0000-0000-0000-000000000000"), "Tallina mnt 25", "Tallinn", "Estonia", "+37254356537", "12345", "Tallinn" },
                    { new Guid("42000006-0000-0000-0000-000000000000"), "Tallina mnt 23", "Tallinn", "Estonia", "+37254356538", "12346", "Tallinn" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "AddressId", "Description", "IsActive", "Name", "PricePerHour" },
                values: new object[,]
                {
                    { new Guid("46000001-0000-0000-0000-000000000000"), new Guid("42000001-0000-0000-0000-000000000000"), "Description A", true, "Room A", 25.5 },
                    { new Guid("46000002-0000-0000-0000-000000000000"), new Guid("42000002-0000-0000-0000-000000000000"), "Description B", true, "Room B", 30.0 },
                    { new Guid("46000003-0000-0000-0000-000000000000"), new Guid("42000003-0000-0000-0000-000000000000"), "Description C", true, "Room C", 35.0 },
                    { new Guid("46000004-0000-0000-0000-000000000000"), new Guid("42000004-0000-0000-0000-000000000000"), "Description D", true, "Room D", 40.0 },
                    { new Guid("46000005-0000-0000-0000-000000000000"), new Guid("42000005-0000-0000-0000-000000000000"), "Description E", true, "Room E", 45.0 },
                    { new Guid("46000006-0000-0000-0000-000000000000"), new Guid("42000006-0000-0000-0000-000000000000"), "Description F", true, "Room F", 50.0 }
                });

            migrationBuilder.InsertData(
                table: "RoomImages",
                columns: new[] { "ImageId", "IsMain", "RoomId", "Url" },
                values: new object[,]
                {
                    { new Guid("44000001-0000-0000-0000-000000000000"), true, new Guid("46000001-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZXn4EZ447cJGzE4mjGFUEd7fHezhKfrzEy" },
                    { new Guid("44000002-0000-0000-0000-000000000000"), true, new Guid("46000002-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZK94EZnlvkoFPgUJhicWK2l9kunhbdFlJy" },
                    { new Guid("44000003-0000-0000-0000-000000000000"), true, new Guid("46000003-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZl94EZcxrwzhmidM7XHfoOqgqtKhGsFzty" },
                    { new Guid("44000004-0000-0000-0000-000000000000"), true, new Guid("46000004-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000005-0000-0000-0000-000000000000"), true, new Guid("46000005-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZ694EZx8NoXjBFg5phHhKrzqVhmXagQNhy" },
                    { new Guid("44000006-0000-0000-0000-000000000000"), true, new Guid("46000006-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZE94EZPoL8U9CtrbzjvKxPqXuQx869cc0k" },
                    { new Guid("44000007-0000-0000-0000-000000000000"), false, new Guid("46000001-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000008-0000-0000-0000-000000000000"), false, new Guid("46000001-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000009-0000-0000-0000-000000000000"), false, new Guid("46000002-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000010-0000-0000-0000-000000000000"), false, new Guid("46000002-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000011-0000-0000-0000-000000000000"), false, new Guid("46000003-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000012-0000-0000-0000-000000000000"), false, new Guid("46000003-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000013-0000-0000-0000-000000000000"), false, new Guid("46000004-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000014-0000-0000-0000-000000000000"), false, new Guid("46000004-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000015-0000-0000-0000-000000000000"), false, new Guid("46000005-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000016-0000-0000-0000-000000000000"), false, new Guid("46000005-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" },
                    { new Guid("44000017-0000-0000-0000-000000000000"), false, new Guid("46000006-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZO94EZauKLzus1OdLUsUg6iOqxdVOTJJAy" },
                    { new Guid("44000018-0000-0000-0000-000000000000"), false, new Guid("46000006-0000-0000-0000-000000000000"), "https://e.pcloud.link/publink/show?code=XZt94EZpHrcKFd0pM4wX1YBvPw7cbgIgUJk" }
                });

            migrationBuilder.InsertData(
                table: "RoomsActivity",
                columns: new[] { "ActivityId", "RoomId" },
                values: new object[,]
                {
                    { new Guid("43000001-0000-0000-0000-000000000000"), new Guid("46000001-0000-0000-0000-000000000000") },
                    { new Guid("43000002-0000-0000-0000-000000000000"), new Guid("46000001-0000-0000-0000-000000000000") },
                    { new Guid("43000003-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000004-0000-0000-0000-000000000000"), new Guid("46000002-0000-0000-0000-000000000000") },
                    { new Guid("43000005-0000-0000-0000-000000000000"), new Guid("46000003-0000-0000-0000-000000000000") },
                    { new Guid("43000006-0000-0000-0000-000000000000"), new Guid("46000003-0000-0000-0000-000000000000") },
                    { new Guid("43000007-0000-0000-0000-000000000000"), new Guid("46000004-0000-0000-0000-000000000000") },
                    { new Guid("43000008-0000-0000-0000-000000000000"), new Guid("46000004-0000-0000-0000-000000000000") },
                    { new Guid("43000009-0000-0000-0000-000000000000"), new Guid("46000005-0000-0000-0000-000000000000") },
                    { new Guid("43000010-0000-0000-0000-000000000000"), new Guid("46000005-0000-0000-0000-000000000000") },
                    { new Guid("43000011-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") },
                    { new Guid("43000012-0000-0000-0000-000000000000"), new Guid("46000006-0000-0000-0000-000000000000") }
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
