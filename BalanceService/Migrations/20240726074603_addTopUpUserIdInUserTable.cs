using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BalanceService.Migrations
{
    public partial class addTopUpUserIdInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopUpUserId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpUserId",
                table: "Users");
        }
    }
}
