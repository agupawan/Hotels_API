using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelsAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedHotelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Amenity", "CreatedAt", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedAt" },
                values: new object[] { 1, "", new DateTime(2023, 12, 21, 14, 50, 14, 12, DateTimeKind.Local).AddTicks(3560), "lorem ipsum  mndjkh mwswjbhbshw ydvyv hvbduyg mn gduyb uygy", "", "Royal Villa", 5, 200.0, 550, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
