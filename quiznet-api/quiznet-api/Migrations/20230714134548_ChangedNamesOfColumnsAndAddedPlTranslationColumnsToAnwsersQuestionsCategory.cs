using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNamesOfColumnsAndAddedPlTranslationColumnsToAnwsersQuestionsCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Questions",
                newName: "TextPl");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "NamePl");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextPl",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Text",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TextPl",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "TextPl",
                table: "Questions",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "NamePl",
                table: "Categories",
                newName: "CategoryName");
        }
    }
}
