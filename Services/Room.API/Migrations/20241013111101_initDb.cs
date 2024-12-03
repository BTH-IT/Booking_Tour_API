using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LocationCode = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactInfo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
<<<<<<<< HEAD:Services/Room.API/Migrations/20241105082603_InitialCreate.cs
                    Reviews = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HotelRules = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HotelAmenities = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
========
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Reviews = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HotelRules = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HotelAmenities = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
>>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a:Services/Room.API/Migrations/20241013111101_initDb.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
<<<<<<<< HEAD:Services/Room.API/Migrations/20241105082603_InitialCreate.cs
                    Images = table.Column<string>(type: "JSON", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Video = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
========
>>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a:Services/Room.API/Migrations/20241013111101_initDb.cs
                    Detail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<double>(type: "double", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MaxGuests = table.Column<int>(type: "int", nullable: false),
<<<<<<<< HEAD:Services/Room.API/Migrations/20241105082603_InitialCreate.cs
                    Reviews = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoomAmenities = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
========
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Reviews = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoomAmenities = table.Column<string>(type: "JSON", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Images = table.Column<string>(type: "JSON", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Video = table.Column<string>(type: "JSON", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
>>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a:Services/Room.API/Migrations/20241013111101_initDb.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
