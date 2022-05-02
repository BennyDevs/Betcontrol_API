using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BetBookie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bookie",
                table: "Bets");

            migrationBuilder.AddColumn<int>(
                name: "BookieId",
                table: "Bets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bookies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_BookieId",
                table: "Bets",
                column: "BookieId");

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

            migrationBuilder.DropTable(
                name: "Bookies");

            migrationBuilder.DropIndex(
                name: "IX_Bets_BookieId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "BookieId",
                table: "Bets");

            migrationBuilder.AddColumn<string>(
                name: "Bookie",
                table: "Bets",
                type: "TEXT",
                nullable: true);
        }
    }
}
