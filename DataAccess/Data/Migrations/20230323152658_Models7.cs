using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTransactionSizes",
                table: "StockTransactionSizes");

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumns: new[] { "ItemId", "SizeId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTransactionSizes",
                table: "StockTransactionSizes",
                column: "TransactionId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "2005b9c4-26a7-48fc-bf57-f77baf7c620a");

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

            migrationBuilder.UpdateData(
                table: "StockTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 3, 23, 16, 26, 58, 385, DateTimeKind.Local).AddTicks(2257));

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactionSizes_ItemId",
                table: "StockTransactionSizes",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTransactionSizes",
                table: "StockTransactionSizes");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactionSizes_ItemId",
                table: "StockTransactionSizes");

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StockTransactionSizes",
                keyColumn: "TransactionId",
                keyValue: 6);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTransactionSizes",
                table: "StockTransactionSizes",
                columns: new[] { "ItemId", "SizeId" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "71fb510d-fb25-4629-be6b-174181b2dc05");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ee124651-710c-457a-955a-375bebe56b67", "7473fdd3-dfa4-4c02-9103-86ffca068dc8" });

            migrationBuilder.InsertData(
                table: "StockTransactionSizes",
                columns: new[] { "ItemId", "SizeId", "Quantity", "TransactionDate", "TransactionId", "TransactionType" },
                values: new object[,]
                {
                    { 1, 1, 18, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "In" },
                    { 1, 2, 4, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "In" },
                    { 1, 3, 0, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "In" },
                    { 2, 1, 13, new DateTime(2023, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "In" },
                    { 2, 2, 0, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "In" },
                    { 2, 3, 23, new DateTime(2023, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "In" }
                });

            migrationBuilder.UpdateData(
                table: "StockTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 3, 20, 22, 59, 56, 585, DateTimeKind.Local).AddTicks(5349));
        }
    }
}
