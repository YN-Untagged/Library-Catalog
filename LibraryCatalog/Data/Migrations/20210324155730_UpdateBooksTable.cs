using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryCatalog.Data.Migrations
{
    public partial class UpdateBooksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyUrl",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "AmazonUrl",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TakeALotUrl",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmazonUrl",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TakeALotUrl",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BuyUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
