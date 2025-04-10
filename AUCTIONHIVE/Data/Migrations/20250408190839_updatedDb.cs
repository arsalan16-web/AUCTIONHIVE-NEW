using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUCTIONHIVE.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ScheduledAuctions_ScheduledAuctionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ScheduledAuctionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ScheduledAuctionId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "ScheduledAuctions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledAuctions_Products_ProductId",
                table: "ScheduledAuctions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledAuctions_Products_ProductId",
                table: "ScheduledAuctions");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ScheduledAuctions");

            migrationBuilder.AddColumn<string>(
                name: "ScheduledAuctionId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ScheduledAuctionId",
                table: "Products",
                column: "ScheduledAuctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ScheduledAuctions_ScheduledAuctionId",
                table: "Products",
                column: "ScheduledAuctionId",
                principalTable: "ScheduledAuctions",
                principalColumn: "Id");
        }
    }
}
