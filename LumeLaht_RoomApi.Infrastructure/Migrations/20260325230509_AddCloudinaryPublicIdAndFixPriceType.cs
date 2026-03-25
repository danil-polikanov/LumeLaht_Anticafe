using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LumeLaht_RoomApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCloudinaryPublicIdAndFixPriceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix PricePerHour column type (was float in DB, should be decimal)
            migrationBuilder.Sql("ALTER TABLE [Rooms] ALTER COLUMN [PricePerHour] decimal(18,2) NOT NULL;");

            migrationBuilder.AddColumn<string>(
                name: "CloudinaryPublicId",
                table: "RoomImages",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000001-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000002-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000003-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000004-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000005-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000006-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000007-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000008-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000009-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000010-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000011-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000012-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000013-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000014-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000015-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000016-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000017-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "ImageId",
                keyValue: new Guid("44000018-0000-0000-0000-000000000000"),
                column: "CloudinaryPublicId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudinaryPublicId",
                table: "RoomImages");
        }
    }
}
