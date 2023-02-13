using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace BirdsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Birds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirdId = table.Column<int>(type: "int", nullable: false),
                    ObservationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<Point>(type: "geography", nullable: true),
                    PhotoName = table.Column<string>(name: "Photo_Name", type: "nvarchar(max)", nullable: true),
                    PhotoContentType = table.Column<string>(name: "Photo_ContentType", type: "nvarchar(max)", nullable: true),
                    PhotoData = table.Column<byte[]>(name: "Photo_Data", type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Birds_BirdId",
                        column: x => x.BirdId,
                        principalTable: "Birds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_BirdId",
                table: "Observations",
                column: "BirdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Birds");
        }
    }
}
