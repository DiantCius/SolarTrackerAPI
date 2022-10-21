using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class IndicationsFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indication_Powerplants_PowerplantId",
                table: "Indication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Indication",
                table: "Indication");

            migrationBuilder.RenameTable(
                name: "Indication",
                newName: "Indications");

            migrationBuilder.RenameIndex(
                name: "IX_Indication_PowerplantId",
                table: "Indications",
                newName: "IX_Indications_PowerplantId");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Indications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Indications",
                table: "Indications",
                column: "IndicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indications_Powerplants_PowerplantId",
                table: "Indications",
                column: "PowerplantId",
                principalTable: "Powerplants",
                principalColumn: "PowerplantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Indications_Powerplants_PowerplantId",
                table: "Indications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Indications",
                table: "Indications");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Indications");

            migrationBuilder.RenameTable(
                name: "Indications",
                newName: "Indication");

            migrationBuilder.RenameIndex(
                name: "IX_Indications_PowerplantId",
                table: "Indication",
                newName: "IX_Indication_PowerplantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Indication",
                table: "Indication",
                column: "IndicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Indication_Powerplants_PowerplantId",
                table: "Indication",
                column: "PowerplantId",
                principalTable: "Powerplants",
                principalColumn: "PowerplantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
