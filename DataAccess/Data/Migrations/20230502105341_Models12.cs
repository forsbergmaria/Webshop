using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdminPanel.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingStatusId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeItemId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePriceId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippingStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingStatuses", x => x.StatusId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "498e5462-74a0-410e-b5d2-44cef5baa984", "HUVUDADMINISTRATÖR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "42659956-bf97-4e2d-b5a5-6a501474b1ee", "217c8580-7d7f-4f66-9476-d138f81de5e7" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 5, 2, 12, 53, 41, 55, DateTimeKind.Local).AddTicks(7994));

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 1,
                columns: new[] { "StripeItemId", "StripePriceId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 2,
                columns: new[] { "StripeItemId", "StripePriceId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Items",
                keyColumn: "ItemId",
                keyValue: 3,
                columns: new[] { "StripeItemId", "StripePriceId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "ShippingStatusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2,
                column: "ShippingStatusId",
                value: 2);

            migrationBuilder.InsertData(
                table: "ShippingStatuses",
                columns: new[] { "StatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Ohanterad" },
                    { 2, "Pågående" },
                    { 3, "Slutförd" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders",
                column: "ShippingStatusId",
                principalTable: "ShippingStatuses",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingStatuses_ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ShippingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingStatusId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StripeItemId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StripePriceId",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "jdigru",
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "cc078ca1-d0fc-443e-abcb-cd93fae20b82", null });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "jfkdgjk8jd5509",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3ed28d86-632c-4433-832f-357b8c9d2463", "862788ea-8971-4dab-ae28-fc64f572f8f7" });

            migrationBuilder.UpdateData(
                table: "ItemTransactions",
                keyColumn: "TransactionId",
                keyValue: 1,
                column: "TransactionDate",
                value: new DateTime(2023, 4, 14, 14, 46, 47, 731, DateTimeKind.Local).AddTicks(7475));
        }
    }
}
