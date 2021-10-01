using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryCatalog.Data.Migrations
{
    public partial class UpdateReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Books_BookId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_BookId",
                table: "Reviews",
                newName: "IX_Reviews_BookId");

            migrationBuilder.AlterColumn<int>(
                name: "Rating",
                table: "Reviews",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewDate",
                table: "Reviews",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Review",
                newName: "IX_Review_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BookId",
                table: "Review",
                newName: "IX_Review_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "Rating",
                table: "Review",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Books_BookId",
                table: "Review",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_UserId",
                table: "Review",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
