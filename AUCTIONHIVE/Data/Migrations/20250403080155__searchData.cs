using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUCTIONHIVE.Data.Migrations
{
    /// <inheritdoc />
    public partial class _searchData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_SubCategoryId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "SearchedHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchedHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchedHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchedHistories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SearchedHistories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchedHistories_CategoryId",
                table: "SearchedHistories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchedHistories_SubCategoryId",
                table: "SearchedHistories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchedHistories_UserId",
                table: "SearchedHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "SearchedHistories");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
