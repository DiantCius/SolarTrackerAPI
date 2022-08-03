using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "PowerPlants");

            migrationBuilder.RenameColumn(
                name: "FirebaseDocumentId",
                table: "PowerPlants",
                newName: "SerialNumber");

            migrationBuilder.AddColumn<int>(
                name: "ConnectionStatus",
                table: "PowerPlants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionStatus",
                table: "PowerPlants");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "PowerPlants",
                newName: "FirebaseDocumentId");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "PowerPlants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
