using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToUri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlPath",
                table: "Songs",
                newName: "Uri");

            migrationBuilder.RenameColumn(
                name: "UrlPath",
                table: "Mixes",
                newName: "Uri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Songs",
                newName: "UrlPath");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "Mixes",
                newName: "UrlPath");
        }
    }
}
