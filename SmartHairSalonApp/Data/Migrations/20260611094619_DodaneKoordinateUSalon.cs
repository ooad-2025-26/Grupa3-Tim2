using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHairSalonApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodaneKoordinateUSalon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Termin");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Salon",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Salon",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Salon");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Salon");

            migrationBuilder.CreateTable(
                name: "Termin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false),
                    StatusTermina = table.Column<int>(type: "int", nullable: false),
                    Vrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Termin_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Termin_Salon_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Termin_KorisnikId",
                table: "Termin",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_SalonId",
                table: "Termin",
                column: "SalonId");
        }
    }
}
