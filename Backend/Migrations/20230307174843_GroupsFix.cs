using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class GroupsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants");

            migrationBuilder.AlterColumn<int>(
                name: "PowerplantGroupId",
                table: "Powerplants",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PowerplantGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PowerplantGroups_UserId",
                table: "PowerplantGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerplantGroups_Users_UserId",
                table: "PowerplantGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants",
                column: "PowerplantGroupId",
                principalTable: "PowerplantGroups",
                principalColumn: "PowerplantGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerplantGroups_Users_UserId",
                table: "PowerplantGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants");

            migrationBuilder.DropIndex(
                name: "IX_PowerplantGroups_UserId",
                table: "PowerplantGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PowerplantGroups");

            migrationBuilder.AlterColumn<int>(
                name: "PowerplantGroupId",
                table: "Powerplants",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Powerplants_PowerplantGroups_PowerplantGroupId",
                table: "Powerplants",
                column: "PowerplantGroupId",
                principalTable: "PowerplantGroups",
                principalColumn: "PowerplantGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
