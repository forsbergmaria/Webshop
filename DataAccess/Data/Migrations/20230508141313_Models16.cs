using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 8, 16, 9, 6, 334, DateTimeKind.Local).AddTicks(3120));
        }
    }
}
