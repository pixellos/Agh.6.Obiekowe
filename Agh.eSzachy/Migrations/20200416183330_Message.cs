using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ClientIdId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ClientIdId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ClientIdId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Messages",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClientId",
                table: "Messages",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ClientId",
                table: "Messages",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ClientId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ClientId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ClientIdId",
                table: "Messages",
                type: "varchar(95) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClientIdId",
                table: "Messages",
                column: "ClientIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ClientIdId",
                table: "Messages",
                column: "ClientIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
