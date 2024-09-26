using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.API.Migrations
{
    /// <inheritdoc />
    public partial class changecol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelRules",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "HotelRules",
                table: "Hotels",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelRules",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "HotelRules",
                table: "Rooms",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
