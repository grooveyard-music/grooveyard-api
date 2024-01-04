using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTrack_MusicBoxes_MusicboxId",
                table: "MusicboxTrack");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTrack_Songs_SongId",
                table: "MusicboxTrack");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTrack_Tracks_TrackId",
                table: "MusicboxTrack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicboxTrack",
                table: "MusicboxTrack");

            migrationBuilder.RenameTable(
                name: "MusicboxTrack",
                newName: "MusicboxTracks");

            migrationBuilder.RenameIndex(
                name: "IX_MusicboxTrack_TrackId",
                table: "MusicboxTracks",
                newName: "IX_MusicboxTracks_TrackId");

            migrationBuilder.RenameIndex(
                name: "IX_MusicboxTrack_SongId",
                table: "MusicboxTracks",
                newName: "IX_MusicboxTracks_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicboxTracks",
                table: "MusicboxTracks",
                columns: new[] { "MusicboxId", "TrackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTracks_MusicBoxes_MusicboxId",
                table: "MusicboxTracks",
                column: "MusicboxId",
                principalTable: "MusicBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTracks_Songs_SongId",
                table: "MusicboxTracks",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTracks_Tracks_TrackId",
                table: "MusicboxTracks",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTracks_MusicBoxes_MusicboxId",
                table: "MusicboxTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTracks_Songs_SongId",
                table: "MusicboxTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTracks_Tracks_TrackId",
                table: "MusicboxTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MusicboxTracks",
                table: "MusicboxTracks");

            migrationBuilder.RenameTable(
                name: "MusicboxTracks",
                newName: "MusicboxTrack");

            migrationBuilder.RenameIndex(
                name: "IX_MusicboxTracks_TrackId",
                table: "MusicboxTrack",
                newName: "IX_MusicboxTrack_TrackId");

            migrationBuilder.RenameIndex(
                name: "IX_MusicboxTracks_SongId",
                table: "MusicboxTrack",
                newName: "IX_MusicboxTrack_SongId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MusicboxTrack",
                table: "MusicboxTrack",
                columns: new[] { "MusicboxId", "TrackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTrack_MusicBoxes_MusicboxId",
                table: "MusicboxTrack",
                column: "MusicboxId",
                principalTable: "MusicBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTrack_Songs_SongId",
                table: "MusicboxTrack",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTrack_Tracks_TrackId",
                table: "MusicboxTrack",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
