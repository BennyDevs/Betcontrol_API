using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetControlAuthentication.Migrations
{
    public partial class BetsSports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sport",
                table: "Bets");

            migrationBuilder.AddColumn<int>(
                name: "SportId",
                table: "Bets",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_SportId",
                table: "Bets",
                column: "SportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets",
                column: "SportId",
                principalTable: "Sports",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Sports_SportId",
                table: "Bets");

            migrationBuilder.DropTable(
                name: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_Bets_SportId",
                table: "Bets");

            migrationBuilder.DropColumn(
                name: "SportId",
                table: "Bets");

            migrationBuilder.AddColumn<string>(
                name: "Sport",
                table: "Bets",
                type: "TEXT",
                nullable: true);
        }
    }
}
