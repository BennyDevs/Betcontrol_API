using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BetUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets");

            migrationBuilder.AlterColumn<string>(
                name: "Selection",
                table: "Bets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Event",
                table: "Bets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "Sport",
                table: "Bets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipster",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Bookies_BookieId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Bookie",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "Tipster",
                table: "Bets");

            migrationBuilder.AlterColumn<string>(
                name: "Selection",
                table: "Bets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Event",
                table: "Bets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

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
    }
}
