using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManagementApplication.Migrations
{
    public partial class loopResolve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Orders_OrderId",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds");

            migrationBuilder.AddColumn<int>(
                name: "RefundId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 101,
                column: "Role",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 102,
                column: "Role",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RefundId",
                table: "Orders",
                column: "RefundId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Refunds_RefundId",
                table: "Orders",
                column: "RefundId",
                principalTable: "Refunds",
                principalColumn: "RefundId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Refunds_RefundId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RefundId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RefundId",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 101,
                column: "Role",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 102,
                column: "Role",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Orders_OrderId",
                table: "Refunds",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
