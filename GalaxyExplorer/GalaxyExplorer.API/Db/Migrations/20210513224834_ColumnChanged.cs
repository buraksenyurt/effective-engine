using Microsoft.EntityFrameworkCore.Migrations;

namespace GalaxyExplorer.API.Db.Migrations
{
    public partial class ColumnChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnMission",
                table: "Voyagers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnMission",
                table: "Voyagers");
        }
    }
}
