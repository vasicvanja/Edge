using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ExtendArtworkModelWithQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "082a9d54-dd2c-4783-963e-9dd49557a2c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7790143f-cf2e-45be-b69b-abf7d82c6595");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Artworks");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "082a9d54-dd2c-4783-963e-9dd49557a2c3", "2", "User", "User" },
                    { "7790143f-cf2e-45be-b69b-abf7d82c6595", "1", "Admin", "Admin" }
                });
        }
    }
}
