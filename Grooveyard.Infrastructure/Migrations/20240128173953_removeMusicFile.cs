using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeMusicFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs");

            migrationBuilder.DropIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes");

            migrationBuilder.DropColumn(
                name: "MusicFileId",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "MusicFileId",
                table: "Mixes");

            migrationBuilder.AlterColumn<string>(
                name: "SongId",
                table: "MusicFiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixId",
                table: "MusicFiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicFiles_MixId",
                table: "MusicFiles",
                column: "MixId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicFiles_SongId",
                table: "MusicFiles",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicFiles_Mixes_MixId",
                table: "MusicFiles",
                column: "MixId",
                principalTable: "Mixes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicFiles_Songs_SongId",
                table: "MusicFiles",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicFiles_Mixes_MixId",
                table: "MusicFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_MusicFiles_Songs_SongId",
                table: "MusicFiles");

            migrationBuilder.DropIndex(
                name: "IX_MusicFiles_MixId",
                table: "MusicFiles");

            migrationBuilder.DropIndex(
                name: "IX_MusicFiles_SongId",
                table: "MusicFiles");

            migrationBuilder.DropColumn(
                name: "MixId",
                table: "MusicFiles");

            migrationBuilder.AddColumn<string>(
                name: "MusicFileId",
                table: "Songs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SongId",
                table: "MusicFiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MusicFileId",
                table: "Mixes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mixes_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mixes_MusicFiles_MusicFileId",
                table: "Mixes",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Songs_MusicFiles_MusicFileId",
                table: "Songs",
                column: "MusicFileId",
                principalTable: "MusicFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
