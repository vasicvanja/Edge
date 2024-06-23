using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class CycleImageDataProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Cycles",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Cycles");
        }
    }
}
