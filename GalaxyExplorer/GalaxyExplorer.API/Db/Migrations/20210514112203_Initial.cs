using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GalaxyExplorer.API.Db.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpaceshipId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlannedDuration = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.MissionId);
                });

            migrationBuilder.CreateTable(
                name: "Spaceships",
                columns: table => new
                {
                    SpaceshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Range = table.Column<double>(type: "float", nullable: false),
                    OnMission = table.Column<bool>(type: "bit", nullable: false),
                    MaxCrewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spaceships", x => x.SpaceshipId);
                });

            migrationBuilder.CreateTable(
                name: "Voyagers",
                columns: table => new
                {
                    VoyagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstMissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnMission = table.Column<bool>(type: "bit", nullable: false),
                    MissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voyagers", x => x.VoyagerId);
                    table.ForeignKey(
                        name: "FK_Voyagers_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "MissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Spaceships",
                columns: new[] { "SpaceshipId", "MaxCrewCount", "Name", "OnMission", "Range" },
                values: new object[,]
                {
                    { 1, 2, "Saturn IV Rocket", false, 1.2 },
                    { 2, 5, "Pathfinder", true, 2.6000000000000001 },
                    { 3, 3, "Event Horizon", false, 9.9000000000000004 },
                    { 4, 7, "Captain Marvel", false, 3.1400000000000001 },
                    { 5, 7, "Lucky Tortiinn", false, 7.7000000000000002 },
                    { 6, 5, "Battle Master", false, 10.0 },
                    { 7, 3, "Zerash Guidah", true, 3.3500000000000001 },
                    { 8, 4, "Ayran Hayd", false, 5.0999999999999996 },
                    { 9, 7, "Nebukadnezar", false, 9.0 },
                    { 10, 7, "Sifiyus Alpha Siera", false, 7.7000000000000002 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voyagers_MissionId",
                table: "Voyagers",
                column: "MissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Spaceships");

            migrationBuilder.DropTable(
                name: "Voyagers");

            migrationBuilder.DropTable(
                name: "Missions");
        }
    }
}
