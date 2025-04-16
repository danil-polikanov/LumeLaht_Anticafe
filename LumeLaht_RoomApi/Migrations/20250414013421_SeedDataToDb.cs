using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LumeLaht_RoomApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "ActivityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Table Game", "Monopolia" },
                    { 2, "Table Game", "Uno" },
                    { 3, "Table Game", "Chess" },
                    { 4, "Table Game", "Scrabble" },
                    { 5, "Sport Game", "Ping Pong" },
                    { 6, "Sport Game", "Foosball" },
                    { 7, "Sport Game", "Billiards" },
                    { 8, "Sport Game", "Darts" },
                    { 9, "Sport Game", "Poker" },
                    { 10, "Sport Game", "Blackjack" },
                    { 11, "Sport Game", "Bowling" },
                    { 12, "Sport Game", "Table Tennis" }
                });

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "AddressId", "City", "Country", "PhoneNumber", "PostalCode", "Region" },
                values: new object[,]
                {
                    { 1, "Narva", "Estonia", "+37254356533", "12341", "Ida-Virumaa" },
                    { 2, "Narva", "Estonia", "+37254351534", "12342", "Ida-Virumaa" },
                    { 3, "Jõhvi", "Estonia", "+37254326535", "12343", "Ida-Virumaa" },
                    { 4, "Narva", "Estonia", "+37254365536", "12344", "Ida-Virumaa" },
                    { 5, "Tallinn", "Estonia", "+37254356537", "12345", "Tallinn" },
                    { 6, "Tallinn", "Estonia", "+37254356538", "12346", "Tallinn" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "AddressId", "Description", "IsActive", "Name", "PricePerHour" },
                values: new object[,]
                {
                    { 1, 1, "Description A", true, "Room A", 25.5 },
                    { 2, 2, "Description B", true, "Room B", 30.0 },
                    { 3, 3, "Description C", true, "Room C", 35.0 },
                    { 4, 4, "Description D", true, "Room D", 40.0 },
                    { 5, 5, "Description E", true, "Room E", 45.0 },
                    { 6, 6, "Description F", true, "Room F", 50.0 }
                });

            migrationBuilder.InsertData(
                table: "RoomsActivity",
                columns: new[] { "ActivityId", "RoomId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 4 },
                    { 8, 4 },
                    { 9, 5 },
                    { 10, 5 },
                    { 11, 6 },
                    { 12, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 9, 5 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 10, 5 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "RoomsActivity",
                keyColumns: new[] { "ActivityId", "RoomId" },
                keyValues: new object[] { 12, 6 });

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 6);
        }
    }
}
