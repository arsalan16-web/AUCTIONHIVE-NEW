using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUCTIONHIVE.Data.Migrations
{
    /// <inheritdoc />
    public partial class addBidPer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions");

            migrationBuilder.CreateTable(
                name: "BidingPercentages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PriceInPer = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidingPercentages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions",
                column: "ProductId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidingPercentages");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledAuctions_ProductId",
                table: "ScheduledAuctions",
                column: "ProductId");
        }
    }
}
