using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiznet_api.Migrations
{
    /// <inheritdoc />
    public partial class AddedAvatarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarId",
                table: "Players",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Avatars",
                columns: new[] { "Name", "ImagePath" },
                values: new object[,]
                {
                    { "Avatar 1", "/Img/Avatar1.png" },
                    { "Avatar 2", "/Img/Avatar2.png" },
                    { "Avatar 3", "/Img/Avatar3.png" },
                    { "Avatar 4", "/Img/Avatar4.png" },
                    { "Avatar 5", "/Img/Avatar5.png" },
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_AvatarId",
                table: "Players",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Avatars_AvatarId",
                table: "Players",
                column: "AvatarId",
                principalTable: "Avatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Avatars_AvatarId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropIndex(
                name: "IX_Players_AvatarId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Players");
        }
    }
}
