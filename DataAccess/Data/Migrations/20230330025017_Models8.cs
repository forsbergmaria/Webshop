using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "StockTransactionSizes");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Subcategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ItemTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_ItemTransactions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTransactions_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "SizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "7ef31417-cd6a-4965-8757-9362097025ad", "Huvudadministratör" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "add2dced-ebf7-4f44-abce-031975ef9f95", "94cee179-89be-42e7-9f87-cd8eda549e1a" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "IsPublished",
                value: true);

            migrationBuilder.InsertData(
                table: "ItemTransactions",
                columns: new[] { "TransactionId", "Discriminator", "ItemId", "Quantity", "TransactionDate", "TransactionType" },
                values: new object[] { 1, "ItemTransaction", 3, 16, new DateTime(2023, 3, 30, 4, 50, 17, 445, DateTimeKind.Local).AddTicks(1557), "In" });

            migrationBuilder.InsertData(
                table: "ItemTransactions",
                columns: new[] { "TransactionId", "Discriminator", "ItemId", "Quantity", "SizeId", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { 2, "TransactionWithSizes", 1, 18, 1, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 3, "TransactionWithSizes", 1, 4, 2, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 4, "TransactionWithSizes", 1, 0, 3, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 5, "TransactionWithSizes", 2, 13, 1, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 6, "TransactionWithSizes", 2, 0, 2, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 7, "TransactionWithSizes", 2, 23, 3, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" }
                });

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "SubcategoryId",
                keyValue: 1,
                column: "IsPublished",
                value: true);

            migrationBuilder.UpdateData(
                table: "Subcategories",
                keyColumn: "SubcategoryId",
                keyValue: 2,
                column: "IsPublished",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransactions_ItemId",
                table: "ItemTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTransactions_SizeId",
                table: "ItemTransactions",
                column: "SizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemTransactions");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactionSizes",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactionSizes", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_StockTransactionSizes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactionSizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "SizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "2005b9c4-26a7-48fc-bf57-f77baf7c620a", "Testadmin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4cb93fcf-15f0-406e-81c0-766eba593175", "8989841e-5355-493d-b6fe-bd7d76f21375" });

            migrationBuilder.InsertData(
                table: "StockTransactionSizes",
                columns: new[] { "TransactionId", "ItemId", "Quantity", "SizeId", "TransactionDate", "TransactionType" },
                values: new object[,]
                {
                    { 1, 1, 18, 1, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 2, 1, 4, 2, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 3, 1, 0, 3, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 4, 2, 13, 1, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 5, 2, 0, 2, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" },
                    { 6, 2, 23, 3, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "In" }
                });

            migrationBuilder.InsertData(
                table: "StockTransactions",
                columns: new[] { "TransactionId", "ItemId", "Quantity", "TransactionDate", "TransactionType" },
                values: new object[] { 1, 3, 16, new DateTime(2023, 3, 23, 16, 26, 58, 385, DateTimeKind.Local).AddTicks(2257), "In" });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ItemId",
                table: "StockTransactions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionSizes_ItemId",
                table: "StockTransactionSizes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionSizes_SizeId",
                table: "StockTransactionSizes",
                column: "SizeId");
        }
    }
}
