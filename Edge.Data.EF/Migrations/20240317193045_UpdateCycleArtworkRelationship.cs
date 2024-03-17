using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCycleArtworkRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Cycles_CycleId",
                table: "Artworks");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Cycles_CycleId",
                table: "Artworks",
                column: "CycleId",
                principalTable: "Cycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artworks_Cycles_CycleId",
                table: "Artworks");

            migrationBuilder.AddForeignKey(
                name: "FK_Artworks_Cycles_CycleId",
                table: "Artworks",
                column: "CycleId",
                principalTable: "Cycles",
                principalColumn: "Id");
        }
    }
}
