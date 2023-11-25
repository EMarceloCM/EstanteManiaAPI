using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstanteMania.API.Migrations
{
    /// <inheritdoc />
    public partial class BugFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Book_BookId",
                table: "CategoryBook");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Category_CategoryId",
                table: "CategoryBook");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBook_Book_BookId",
                table: "CategoryBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBook_Category_CategoryId",
                table: "CategoryBook",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Book_BookId",
                table: "CategoryBook");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Category_CategoryId",
                table: "CategoryBook");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBook_Book_BookId",
                table: "CategoryBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBook_Category_CategoryId",
                table: "CategoryBook",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
