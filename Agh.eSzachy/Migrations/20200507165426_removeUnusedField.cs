using Microsoft.EntityFrameworkCore.Migrations;

namespace Agh.eSzachy.Migrations
{
    public partial class removeUnusedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEntity_AspNetUsers_PlayerOneId",
                table: "GameEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_GameEntity_AspNetUsers_PlayerTwoId",
                table: "GameEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_GameEntity_Rooms_RoomId",
                table: "GameEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEntity",
                table: "GameEntity");

            migrationBuilder.DropColumn(
                name: "ActualGameId",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "GameEntity",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_GameEntity_RoomId",
                table: "Games",
                newName: "IX_Games_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEntity_PlayerTwoId",
                table: "Games",
                newName: "IX_Games_PlayerTwoId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEntity_PlayerOneId",
                table: "Games",
                newName: "IX_Games_PlayerOneId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PlayerOneId",
                table: "Games",
                column: "PlayerOneId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PlayerTwoId",
                table: "Games",
                column: "PlayerTwoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Rooms_RoomId",
                table: "Games",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PlayerOneId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PlayerTwoId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Rooms_RoomId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "GameEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Games_RoomId",
                table: "GameEntity",
                newName: "IX_GameEntity_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_PlayerTwoId",
                table: "GameEntity",
                newName: "IX_GameEntity_PlayerTwoId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_PlayerOneId",
                table: "GameEntity",
                newName: "IX_GameEntity_PlayerOneId");

            migrationBuilder.AddColumn<string>(
                name: "ActualGameId",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEntity",
                table: "GameEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEntity_AspNetUsers_PlayerOneId",
                table: "GameEntity",
                column: "PlayerOneId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEntity_AspNetUsers_PlayerTwoId",
                table: "GameEntity",
                column: "PlayerTwoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEntity_Rooms_RoomId",
                table: "GameEntity",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
