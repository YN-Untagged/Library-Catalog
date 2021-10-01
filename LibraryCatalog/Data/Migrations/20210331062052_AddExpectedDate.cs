using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryCatalog.Data.Migrations
{
    public partial class AddExpectedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDate",
                table: "Reservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedDate",
                table: "Reservations");
        }
    }
}
