using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManagementApplication.Migrations
{
    public partial class TransactionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderStatus" },
                values: new object[] { new DateTime(2024, 5, 31, 9, 43, 47, 450, DateTimeKind.Local).AddTicks(7754), 3 });

            migrationBuilder.InsertData(
                table: "Refunds",
                columns: new[] { "RefundId", "ClosedBy", "ClosedByMemberId", "CreatedBy", "CreatedByMemberId", "CreatedDate", "OrderId", "PaymentId", "PaymentId1", "Reason", "RefundAmount", "RefundStatus" },
                values: new object[] { 1, null, null, 101, null, new DateTime(2024, 5, 31, 9, 43, 47, 450, DateTimeKind.Local).AddTicks(7765), 1, null, null, "Damaged", 1000.0, 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "MemberId", "Password", "PasswordHashKey", "Status" },
                values: new object[,]
                {
                    { 101, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, "Disabled" },
                    { 102, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, "Active" },
                    { 103, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, new byte[] { 121, 111, 117, 114, 80, 97, 115, 115, 119, 111, 114, 100 }, "Disabled" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Refunds",
                keyColumn: "RefundId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "MemberId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "MemberId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "MemberId",
                keyValue: 103);

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "OrderStatus" },
                values: new object[] { new DateTime(2024, 5, 29, 10, 10, 46, 757, DateTimeKind.Local).AddTicks(6953), 0 });
        }
    }
}
