using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BetTipster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipster",
                table: "Bets");

            migrationBuilder.AddColumn<int>(
                name: "TipsterId",
                table: "Bets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tipster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_TipsterId",
                table: "Bets",
                column: "TipsterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Tipster_TipsterId",
                table: "Bets",
                column: "TipsterId",
                principalTable: "Tipster",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Tipster_TipsterId",
                table: "Bets");

            migrationBuilder.DropTable(
                name: "Tipster");

            migrationBuilder.DropIndex(
                name: "IX_Bets_TipsterId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "TipsterId",
                table: "Bets");

            migrationBuilder.AddColumn<string>(
                name: "Tipster",
                table: "Bets",
                type: "TEXT",
                nullable: true);
        }
    }
}
