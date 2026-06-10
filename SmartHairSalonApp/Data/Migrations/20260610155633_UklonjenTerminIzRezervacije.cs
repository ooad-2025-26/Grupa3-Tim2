using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHairSalonApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UklonjenTerminIzRezervacije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "TerminId",
                table: "Rezervacija");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TerminId",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_TerminId",
                table: "Rezervacija",
                column: "TerminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id");
        }
    }
}
