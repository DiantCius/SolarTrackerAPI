using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class Codes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirebaseCollectionId",
                table: "PowerPlants");

            migrationBuilder.AddColumn<string>(
                name: "FirebaseCollectionCode",
                table: "PowerPlants",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConnectionCode = table.Column<string>(type: "text", nullable: true),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropColumn(
                name: "FirebaseCollectionCode",
                table: "PowerPlants");

            migrationBuilder.AddColumn<int>(
                name: "FirebaseCollectionId",
                table: "PowerPlants",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
