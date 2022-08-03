using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirebaseCollectionCode",
                table: "PowerPlants",
                newName: "FirebaseDocumentId");

            migrationBuilder.RenameColumn(
                name: "ConnectionCode",
                table: "Codes",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Codes",
                newName: "CodeId");

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "PowerPlants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "PowerPlants");

            migrationBuilder.RenameColumn(
                name: "FirebaseDocumentId",
                table: "PowerPlants",
                newName: "FirebaseCollectionCode");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Codes",
                newName: "ConnectionCode");

            migrationBuilder.RenameColumn(
                name: "CodeId",
                table: "Codes",
                newName: "Id");
        }
    }
}
