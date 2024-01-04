using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTrack_Musicbox_MusicboxId",
                table: "MusicboxTrack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Musicbox",
                table: "Musicbox");

            migrationBuilder.RenameTable(
                name: "Musicbox",
                newName: "MusicBoxes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicBoxes",
                table: "MusicBoxes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTrack_MusicBoxes_MusicboxId",
                table: "MusicboxTrack",
                column: "MusicboxId",
                principalTable: "MusicBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTrack_MusicBoxes_MusicboxId",
                table: "MusicboxTrack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicBoxes",
                table: "MusicBoxes");

            migrationBuilder.RenameTable(
                name: "MusicBoxes",
                newName: "Musicbox");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Musicbox",
                table: "Musicbox",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTrack_Musicbox_MusicboxId",
                table: "MusicboxTrack",
                column: "MusicboxId",
                principalTable: "Musicbox",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
