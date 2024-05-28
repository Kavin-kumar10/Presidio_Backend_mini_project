using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManagementApplication.Migrations
{
    public partial class conflictRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_RefundId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RefundId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RefundId",
                table: "Orders",
                column: "RefundId",
                unique: true,
                filter: "[RefundId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_RefundId",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RefundId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RefundId",
                table: "Orders",
                column: "RefundId",
                unique: true);
        }
    }
}
