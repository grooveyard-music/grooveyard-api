using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Mixes_MediaId",
                table: "Tracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Songs_MediaId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_MediaId",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Tracks",
                newName: "SongId");

            migrationBuilder.AddColumn<string>(
                name: "MixId",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_MixId",
                table: "Tracks",
                column: "MixId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_SongId",
                table: "Tracks",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Mixes_MixId",
                table: "Tracks",
                column: "MixId",
                principalTable: "Mixes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Songs_SongId",
                table: "Tracks",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Mixes_MixId",
                table: "Tracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Songs_SongId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_MixId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_SongId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "MixId",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "SongId",
                table: "Tracks",
                newName: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_MediaId",
                table: "Tracks",
                column: "MediaId",
                unique: true,
                filter: "[MediaId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Mixes_MediaId",
                table: "Tracks",
                column: "MediaId",
                principalTable: "Mixes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Songs_MediaId",
                table: "Tracks",
                column: "MediaId",
                principalTable: "Songs",
                principalColumn: "Id");
        }
    }
}
