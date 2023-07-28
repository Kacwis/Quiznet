using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPlayerAnswerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "PlayerAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "PlayerAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
