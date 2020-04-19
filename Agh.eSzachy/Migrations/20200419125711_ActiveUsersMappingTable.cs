using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class ActiveUsersMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomEntityId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "RoomUsers",
                columns: table => new
                {
                    RoomId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUsers", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoomUsers_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomUsers_UserId",
                table: "RoomUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomUsers");

            migrationBuilder.AddColumn<string>(
                name: "RoomEntityId",
                table: "AspNetUsers",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomEntityId",
                table: "AspNetUsers",
                column: "RoomEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomEntityId",
                table: "AspNetUsers",
                column: "RoomEntityId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
