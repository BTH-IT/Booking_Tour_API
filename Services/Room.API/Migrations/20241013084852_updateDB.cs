using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.API.Migrations
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Videos",
                table: "Rooms",
                newName: "Video");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Video",
                table: "Rooms",
                newName: "Videos");
        }
    }
}
