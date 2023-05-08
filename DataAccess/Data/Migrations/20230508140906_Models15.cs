using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemHasSize",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemHasSize", x => new { x.ItemId, x.SizeId });
                    table.ForeignKey(
                        name: "FK_ItemHasSize_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemHasSize_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "SizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "06a2d2e6-29df-4be9-9ef8-218d728a8d06");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a0ba29b6-a71d-497f-928f-13dbe94b6e1a", "d35365de-57d9-473e-a14d-e787cc7beb41" });

            migrationBuilder.InsertData(
                table: "ItemHasSize",
                columns: new[] { "ItemId", "SizeId" },
                values: new object[] { 36, 1 });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 8, 16, 9, 6, 334, DateTimeKind.Local).AddTicks(3120));

            migrationBuilder.CreateIndex(
                name: "IX_ItemHasSize_SizeId",
                table: "ItemHasSize",
                column: "SizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemHasSize");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "b355e326-ec32-4c96-b972-18348525c6e9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ba96d32d-92cd-49ed-bee7-bfe752397908", "d494d5f6-1806-499c-89d1-86a32d9d7610" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 8, 14, 15, 5, 15, DateTimeKind.Local).AddTicks(3278));
        }
    }
}
