using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelAmenities",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "HotelAmenities",
                table: "Hotels",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelAmenities",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "HotelAmenities",
                table: "Rooms",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
