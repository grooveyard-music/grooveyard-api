using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class REVAMPTRACK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicboxTracks_Songs_SongId",
                table: "MusicboxTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracklists_Mixes_MixId",
                table: "Tracklists");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Mixes_MixId",
                table: "Tracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Songs_SongId",
                table: "Tracks");

            migrationBuilder.DropTable(
                name: "TracklistSongs");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_MixId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_SongId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracklists_MixId",
                table: "Tracklists");

            migrationBuilder.DropIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_MusicboxTracks_SongId",
                table: "MusicboxTracks");

            migrationBuilder.DropIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "MixId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "MixId",
                table: "Tracklists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tracklists");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "MusicboxTracks");

            migrationBuilder.AlterColumn<string>(
                name: "UrlPath",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MusicFileId",
                table: "Songs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackId",
                table: "Songs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UrlPath",
                table: "Mixes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TracklistId",
                table: "Mixes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MusicFileId",
                table: "Mixes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackId",
                table: "Mixes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TracklistTracks",
                columns: table => new
                {
                    TracklistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TracklistTracks", x => new { x.TracklistId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_TracklistTracks_Tracklists_TracklistId",
                        column: x => x.TracklistId,
                        principalTable: "Tracklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TracklistTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Songs_TrackId",
                table: "Songs",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mixes_TrackId",
                table: "Mixes",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixes_TracklistId",
                table: "Mixes",
                column: "TracklistId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TracklistTracks_TrackId",
                table: "TracklistTracks",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mixes_Tracklists_TracklistId",
                table: "Mixes",
                column: "TracklistId",
                principalTable: "Tracklists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mixes_Tracks_TrackId",
                table: "Mixes",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_Tracks_TrackId",
                table: "Songs",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Mixes_Tracklists_TracklistId",
                table: "Mixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Mixes_Tracks_TrackId",
                table: "Mixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_Tracks_TrackId",
                table: "Songs");

            migrationBuilder.DropTable(
                name: "TracklistTracks");

            migrationBuilder.DropIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_TrackId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropIndex(
                name: "IX_Mixes_TrackId",
                table: "Mixes");

            migrationBuilder.DropIndex(
                name: "IX_Mixes_TracklistId",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "TrackId",
                table: "Mixes");

            migrationBuilder.AddColumn<string>(
                name: "MixId",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SongId",
                table: "Tracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MixId",
                table: "Tracklists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tracklists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UrlPath",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MusicFileId",
                table: "Songs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "SongId",
                table: "MusicboxTracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UrlPath",
                table: "Mixes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TracklistId",
                table: "Mixes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "MusicFileId",
                table: "Mixes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "TracklistSongs",
                columns: table => new
                {
                    TracklistId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SongId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TracklistSongs", x => new { x.TracklistId, x.SongId });
                    table.ForeignKey(
                        name: "FK_TracklistSongs_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TracklistSongs_Tracklists_TracklistId",
                        column: x => x.TracklistId,
                        principalTable: "Tracklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_MixId",
                table: "Tracks",
                column: "MixId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_SongId",
                table: "Tracks",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracklists_MixId",
                table: "Tracklists",
                column: "MixId",
                unique: true,
                filter: "[MixId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                unique: true,
                filter: "[MusicFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MusicboxTracks_SongId",
                table: "MusicboxTracks",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                unique: true,
                filter: "[MusicFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TracklistSongs_SongId",
                table: "TracklistSongs",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicboxTracks_Songs_SongId",
                table: "MusicboxTracks",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracklists_Mixes_MixId",
                table: "Tracklists",
                column: "MixId",
                principalTable: "Mixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
