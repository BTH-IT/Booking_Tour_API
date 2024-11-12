using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.API.Migrations
{
    /// <inheritdoc />
    public partial class addStatusToBookingRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BookingRooms",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingRooms");
        }
    }
}
