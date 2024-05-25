using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefundManagementApplication.Migrations
{
    public partial class UserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Member");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Member",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Member",
                keyColumn: "Id",
                keyValue: 101,
                column: "password",
                value: "Kavin@10");

            migrationBuilder.UpdateData(
                table: "Member",
                keyColumn: "Id",
                keyValue: 102,
                column: "password",
                value: "Pravin@25");

            migrationBuilder.UpdateData(
                table: "Member",
                keyColumn: "Id",
                keyValue: 103,
                column: "password",
                value: "collector");
        }
    }
}
