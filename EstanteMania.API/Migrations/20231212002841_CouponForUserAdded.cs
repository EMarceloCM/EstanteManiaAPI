using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstanteMania.API.Migrations
{
    /// <inheritdoc />
    public partial class CouponForUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Carrinho",
                type: "nvarchar(80)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Carrinho");
        }
    }
}
