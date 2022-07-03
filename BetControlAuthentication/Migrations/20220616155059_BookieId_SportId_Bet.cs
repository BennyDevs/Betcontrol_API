using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BookieId_SportId_Bet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets");

            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets");

            migrationBuilder.AlterColumn<int>(
                name: "TipsterId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets",
                column: "TipsterId",
                principalTable: "Tipsters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets");

            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets");

            migrationBuilder.AlterColumn<int>(
                name: "TipsterId",
                table: "Bets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SportId",
                table: "Bets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tipsters_TipsterId",
                table: "Bets",
                column: "TipsterId",
                principalTable: "Tipsters",
                principalColumn: "Id");
        }
    }
}
