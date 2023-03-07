using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class Groups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Powerplants",
                newName: "Tariff");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Powerplants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LatLong",
                table: "Powerplants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PowerplantGroupId",
                table: "Powerplants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PowerplantGroups",
                columns: table => new
                {
                    PowerplantGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerplantGroups", x => x.PowerplantGroupId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Powerplants_PowerplantGroupId",
                table: "Powerplants",
                column: "PowerplantGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants",
                column: "PowerplantGroupId",
                principalTable: "PowerplantGroups",
                principalColumn: "PowerplantGroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants");

            migrationBuilder.DropTable(
                name: "PowerplantGroups");

            migrationBuilder.DropIndex(
                name: "IX_Powerplants_PowerplantGroupId",
                table: "Powerplants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Powerplants");

            migrationBuilder.DropColumn(
                name: "LatLong",
                table: "Powerplants");

            migrationBuilder.DropColumn(
                name: "PowerplantGroupId",
                table: "Powerplants");

            migrationBuilder.RenameColumn(
                name: "Tariff",
                table: "Powerplants",
                newName: "Location");
        }
    }
}
