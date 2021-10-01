using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryCatalog.Data.Migrations
{
    public partial class ReviewFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "Reviews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewDate",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
