using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Migrations
{
    /// <inheritdoc />
    public partial class DbV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayListOfTour");

            migrationBuilder.RenameColumn(
                name: "dateStart",
                table: "Schedule",
                newName: "DateStart");

            migrationBuilder.RenameColumn(
                name: "dateEnd",
                table: "Schedule",
                newName: "DateEnd");

            migrationBuilder.RenameColumn(
                name: "availableSeats",
                table: "Schedule",
                newName: "AvailableSeats");

            migrationBuilder.AddColumn<string>(
                name: "Days",
                table: "Tour",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Role",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "Tour");

            migrationBuilder.RenameColumn(
                name: "DateStart",
                table: "Schedule",
                newName: "dateStart");

            migrationBuilder.RenameColumn(
                name: "DateEnd",
                table: "Schedule",
                newName: "dateEnd");

            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Schedule",
                newName: "availableSeats");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Status",
                table: "Role",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.CreateTable(
                name: "DayListOfTour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TourId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayListOfTour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayListOfTour_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DayListOfTour_TourId",
                table: "DayListOfTour",
                column: "TourId");
        }
    }
}
