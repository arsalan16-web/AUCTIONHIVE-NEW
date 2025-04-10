using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUCTIONHIVE.Data.Migrations
{
    /// <inheritdoc />
    public partial class productViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductViews",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductViews",
                table: "Products");
        }
    }
}
