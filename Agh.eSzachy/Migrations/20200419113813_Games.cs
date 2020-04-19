using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class Games : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualGameId",
                table: "Rooms",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RoomEntityId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Rooms_RoomEntityId",
                        column: x => x.RoomEntityId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms",
                column: "ActualGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RoomEntityId",
                table: "Games",
                column: "RoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Games_ActualGameId",
                table: "Rooms",
                column: "ActualGameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Games_ActualGameId",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ActualGameId",
                table: "Rooms");
        }
    }
}
