using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class RemovedIsSearchingFromPlayerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSearching",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSearching",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
