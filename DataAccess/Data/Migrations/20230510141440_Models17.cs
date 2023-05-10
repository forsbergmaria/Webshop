using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingMethodId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "aa22b0ef-11e1-43ec-9d59-82bf05e09c60");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "341f627c-0749-4f1d-be65-661c7e566d61", "7b40f05b-c511-4d46-bdc9-a414e8bd032f" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 10, 16, 14, 39, 997, DateTimeKind.Local).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "ShippingMethodId",
                value: "shr_1MnW4gJ9NmDaISNLsDI6gLUz");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "ShippingMethodId",
                value: "shr_1N3cELJ9NmDaISNLaYLSbzBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingMethodId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "0be72039-8bc4-4122-a217-841150763a2a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2846c9af-af1a-4d13-8f53-e1e5f82ee75c", "f061b1a7-9995-49d4-9bea-0a603246ced5" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 8, 16, 13, 13, 545, DateTimeKind.Local).AddTicks(3313));
        }
    }
}
