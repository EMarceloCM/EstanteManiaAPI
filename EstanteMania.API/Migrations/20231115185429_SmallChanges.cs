using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstanteMania.API.Migrations
{
    /// <inheritdoc />
    public partial class SmallChanges : Migration
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

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "IconCSS",
                table: "Category",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Book",
                type: "int",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Book_BookId",
                table: "CategoryBook");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBook_Category_CategoryId",
                table: "CategoryBook");

            migrationBuilder.DropColumn(
                name: "IconCSS",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Book");

            migrationBuilder.AddColumn<int>(
                name: "MainCategoryId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
