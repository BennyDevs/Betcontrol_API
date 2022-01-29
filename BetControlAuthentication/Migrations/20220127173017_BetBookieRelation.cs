using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BetBookieRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Bookie",
                table: "Bets");

            migrationBuilder.AlterColumn<int>(
                name: "BookieId",
                table: "Bets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets",
                column: "BookieId",
                principalTable: "Bookies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets");

            migrationBuilder.AlterColumn<int>(
                name: "BookieId",
                table: "Bets",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Bookie",
                table: "Bets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets",
                column: "BookieId",
                principalTable: "Bookies",
                principalColumn: "Id");
        }
    }
}
