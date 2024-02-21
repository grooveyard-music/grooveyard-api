using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grooveyard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class profileCoverUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "UserProfiles");
        }
    }
}
