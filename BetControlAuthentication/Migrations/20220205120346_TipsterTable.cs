using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class TipsterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tipster_TipsterId",
                table: "Bets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tipster",
                table: "Tipster");

            migrationBuilder.RenameTable(
                name: "Tipster",
                newName: "Tipsters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tipsters",
                table: "Tipsters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets",
                column: "TipsterId",
                principalTable: "Tipsters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tipsters",
                table: "Tipsters");

            migrationBuilder.RenameTable(
                name: "Tipsters",
                newName: "Tipster");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tipster",
                table: "Tipster",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tipster_TipsterId",
                table: "Bets",
                column: "TipsterId",
                principalTable: "Tipster",
                principalColumn: "Id");
        }
    }
}
