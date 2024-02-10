using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54b69a01-95e5-4bc6-8718-d8357b5b1412");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d24bce7-69e0-41d0-a49d-661d5f79adc0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "082a9d54-dd2c-4783-963e-9dd49557a2c3", "2", "User", "User" },
                    { "7790143f-cf2e-45be-b69b-abf7d82c6595", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "082a9d54-dd2c-4783-963e-9dd49557a2c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7790143f-cf2e-45be-b69b-abf7d82c6595");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "54b69a01-95e5-4bc6-8718-d8357b5b1412", "2", "User", "User" },
                    { "6d24bce7-69e0-41d0-a49d-661d5f79adc0", "1", "Admin", "Admin" }
                });
        }
    }
}
