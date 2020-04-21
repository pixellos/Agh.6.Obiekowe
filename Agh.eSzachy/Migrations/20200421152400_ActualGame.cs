using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class ActualGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Rooms_RoomEntityId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Games_RoomEntityId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RoomEntityId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms",
                column: "ActualGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_RoomId",
                table: "Games",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Rooms_RoomId",
                table: "Games",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Rooms_RoomId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Games_RoomId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "RoomEntityId",
                table: "Games",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ActualGameId",
                table: "Rooms",
                column: "ActualGameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_RoomEntityId",
                table: "Games",
                column: "RoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Rooms_RoomEntityId",
                table: "Games",
                column: "RoomEntityId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
