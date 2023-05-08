using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ItemQuantity",
                table: "OrderContainsItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StripeItemId",
                table: "OrderContainsItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Total",
                table: "OrderContainsItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "ItemQuantity", "StripeItemId", "Total" },
                values: new object[] { 1L, "prod_NYcwS1YSGIhUW1", 199L });

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "ItemQuantity", "StripeItemId", "Total" },
                values: new object[] { 2L, "prod_NYcwS1YSGIhUW1", 199L });

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "ItemQuantity", "StripeItemId", "Total" },
                values: new object[] { 1L, "prod_NYcwS1YSGIhUW1", 199L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeItemId",
                table: "OrderContainsItem");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "OrderContainsItem");

            migrationBuilder.AlterColumn<int>(
                name: "ItemQuantity",
                table: "OrderContainsItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "9bb2e4d6-cfa4-4020-8f7a-b06c433ec5f2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b3aeaa40-ba6d-46d6-baca-d8590736e189", "b6acc558-d919-4e02-befb-c019da75690b" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 5, 21, 17, 54, 130, DateTimeKind.Local).AddTicks(6853));

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 2, 1 },
                column: "ItemQuantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 3, 1 },
                column: "ItemQuantity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "OrderContainsItem",
                keyColumns: new[] { "ItemId", "OrderId" },
                keyValues: new object[] { 2, 2 },
                column: "ItemQuantity",
                value: 1);
        }
    }
}
