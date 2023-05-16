using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingStatusId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                column: "ConcurrencyStamp",
                value: "1dc74195-f4ea-476b-a204-aee6356a0b20");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b78dd81d-d11d-45f2-b327-8cd59dd785e5", "8d547d7a-6a65-472a-a72a-83ba5889878b" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 10, 19, 47, 35, 84, DateTimeKind.Local).AddTicks(334));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId",
                principalTable: "ShippingStatuses",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingStatusId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId",
                principalTable: "ShippingStatuses",
                principalColumn: "StatusId");
        }
    }
}
