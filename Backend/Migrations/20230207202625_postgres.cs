using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    public partial class postgres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    CodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.CodeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Powerplants",
                columns: table => new
                {
                    PowerplantId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    PowerplantType = table.Column<int>(type: "integer", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    ConnectionStatus = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Powerplants", x => x.PowerplantId);
                    table.ForeignKey(
                        name: "FK_Powerplants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnergyProductions",
                columns: table => new
                {
                    EnergyProductionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentProduction = table.Column<string>(type: "text", nullable: true),
                    DailyProduction = table.Column<string>(type: "text", nullable: true),
                    CurrentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    PowerplantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyProductions", x => x.EnergyProductionId);
                    table.ForeignKey(
                        name: "FK_EnergyProductions_Powerplants_PowerplantId",
                        column: x => x.PowerplantId,
                        principalTable: "Powerplants",
                        principalColumn: "PowerplantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indications",
                columns: table => new
                {
                    IndicationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "text", nullable: true),
                    Azimuth = table.Column<float>(type: "real", nullable: false),
                    Elevation = table.Column<float>(type: "real", nullable: false),
                    WindSpeed = table.Column<float>(type: "real", nullable: false),
                    State = table.Column<string>(type: "text", nullable: true),
                    PowerplantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indications", x => x.IndicationId);
                    table.ForeignKey(
                        name: "FK_Indications_Powerplants_PowerplantId",
                        column: x => x.PowerplantId,
                        principalTable: "Powerplants",
                        principalColumn: "PowerplantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnergyProductions_PowerplantId",
                table: "EnergyProductions",
                column: "PowerplantId");

            migrationBuilder.CreateIndex(
                name: "IX_Indications_PowerplantId",
                table: "Indications",
                column: "PowerplantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Powerplants_UserId",
                table: "Powerplants",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "EnergyProductions");

            migrationBuilder.DropTable(
                name: "Indications");

            migrationBuilder.DropTable(
                name: "Powerplants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
