using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edge.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditColumnsToSmtpSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SmtpSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "SmtpSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "SmtpSettings");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SmtpSettings");
        }
    }
}
