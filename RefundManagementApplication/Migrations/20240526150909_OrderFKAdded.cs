using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManagementApplication.Migrations
{
    public partial class OrderFKAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Refunds_RefundId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RefundId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Refunds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Refunds_Orders_OrderId",
                table: "Refunds",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Refunds_Orders_OrderId",
                table: "Refunds");

            migrationBuilder.DropIndex(
                name: "IX_Refunds_OrderId",
                table: "Refunds");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Refunds");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RefundId",
                table: "Orders",
                column: "RefundId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Refunds_RefundId",
                table: "Orders",
                column: "RefundId",
                principalTable: "Refunds",
                principalColumn: "RefundId");
        }
    }
}
