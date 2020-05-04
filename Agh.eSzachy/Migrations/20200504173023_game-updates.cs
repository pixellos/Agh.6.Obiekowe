using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class gameupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActualGameId",
                table: "Rooms",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GameEntity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RoomId = table.Column<string>(nullable: false),
                    PlayerOneId = table.Column<string>(nullable: true),
                    PlayerTwoId = table.Column<string>(nullable: true),
                    Moves = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEntity_AspNetUsers_PlayerOneId",
                        column: x => x.PlayerOneId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameEntity_AspNetUsers_PlayerTwoId",
                        column: x => x.PlayerTwoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameEntity_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEntity_PlayerOneId",
                table: "GameEntity",
                column: "PlayerOneId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntity_PlayerTwoId",
                table: "GameEntity",
                column: "PlayerTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEntity_RoomId",
                table: "GameEntity",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameEntity");

            migrationBuilder.DropColumn(
                name: "ActualGameId",
                table: "Rooms");
        }
    }
}
