using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class LatLongFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatLong",
                table: "Powerplants");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Powerplants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Powerplants",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Powerplants");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Powerplants");

            migrationBuilder.AddColumn<string>(
                name: "LatLong",
                table: "Powerplants",
                type: "text",
                nullable: true);
        }
    }
}
