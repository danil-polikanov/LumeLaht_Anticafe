using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LumeLaht_RoomApi.Migrations
{
    /// <inheritdoc />
    public partial class Added_Address_Names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressName",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 1,
                column: "AddressName",
                value: "Gagarini 11");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 2,
                column: "AddressName",
                value: "Narva mnt 32");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 3,
                column: "AddressName",
                value: "Taamsaare 24");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 4,
                column: "AddressName",
                value: "Vaba mnt 105");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 5,
                column: "AddressName",
                value: "Tallina mnt 25");

            migrationBuilder.UpdateData(
                table: "Address",
                keyColumn: "AddressId",
                keyValue: 6,
                column: "AddressName",
                value: "Tallina mnt 23");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressName",
                table: "Address");
        }
    }
}
