using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Room.API.Migrations
{
    /// <inheritdoc />
    public partial class BDupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Videos",
                table: "Rooms",
                type: "JSON",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoomEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Rooms_RoomEntityId",
                        column: x => x.RoomEntityId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Video_RoomEntityId",
                table: "Video",
                column: "RoomEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropColumn(
                name: "Videos",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Video",
                table: "Rooms",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
