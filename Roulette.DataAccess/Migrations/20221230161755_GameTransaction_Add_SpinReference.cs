using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roulette.DataAccess.Migrations
{
    public partial class GameTransaction_Add_SpinReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpinReference",
                table: "GameTransactions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpinReference",
                table: "GameTransactions");
        }
    }
}
