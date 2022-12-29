using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roulette.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionType = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    StakeAmount = table.Column<double>(type: "REAL", nullable: false),
                    OutcomeAmount = table.Column<double>(type: "REAL", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OutcomeDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDetails", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTransactions_Reference",
                table: "GameTransactions",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDetails_PlayerName",
                table: "PlayerDetails",
                column: "PlayerName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTransactions");

            migrationBuilder.DropTable(
                name: "PlayerDetails");
        }
    }
}
