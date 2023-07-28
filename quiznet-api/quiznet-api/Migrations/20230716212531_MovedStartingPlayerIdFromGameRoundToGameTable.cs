using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class MovedStartingPlayerIdFromGameRoundToGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingPlayerId",
                table: "Rounds");

            migrationBuilder.AddColumn<int>(
                name: "StartingPlayerId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingPlayerId",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "StartingPlayerId",
                table: "Rounds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
