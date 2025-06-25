using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class ExtendArtworkWithHeightAndWidth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Artworks");
        }
    }
}
