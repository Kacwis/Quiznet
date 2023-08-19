using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class AddedBlockedPlayerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedPlayers_Players_PlayerId",
                table: "BlockedPlayers");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "BlockedPlayers",
                newName: "PlayerBlockedId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedPlayers_PlayerId",
                table: "BlockedPlayers",
                newName: "IX_BlockedPlayers_PlayerBlockedId");

            migrationBuilder.AddColumn<int>(
                name: "BlockingPlayerId",
                table: "BlockedPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlockedPlayers_BlockingPlayerId",
                table: "BlockedPlayers",
                column: "BlockingPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedPlayers_Players_BlockingPlayerId",
                table: "BlockedPlayers",
                column: "BlockingPlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedPlayers_Players_PlayerBlockedId",
                table: "BlockedPlayers",
                column: "PlayerBlockedId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedPlayers_Players_BlockingPlayerId",
                table: "BlockedPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockedPlayers_Players_PlayerBlockedId",
                table: "BlockedPlayers");

            migrationBuilder.DropIndex(
                name: "IX_BlockedPlayers_BlockingPlayerId",
                table: "BlockedPlayers");

            migrationBuilder.DropColumn(
                name: "BlockingPlayerId",
                table: "BlockedPlayers");

            migrationBuilder.RenameColumn(
                name: "PlayerBlockedId",
                table: "BlockedPlayers",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockedPlayers_PlayerBlockedId",
                table: "BlockedPlayers",
                newName: "IX_BlockedPlayers_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedPlayers_Players_PlayerId",
                table: "BlockedPlayers",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
