using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryCatalog.Data.Migrations
{
    public partial class Fines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmazonUrl",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TakeALotUrl",
                table: "Books");

            migrationBuilder.AddColumn<decimal>(
                name: "FineAmount",
                table: "BookLoans",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "FineSettled",
                table: "BookLoans",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FineAmount",
                table: "BookLoans");

            migrationBuilder.DropColumn(
                name: "FineSettled",
                table: "BookLoans");

            migrationBuilder.AddColumn<string>(
                name: "AmazonUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TakeALotUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
