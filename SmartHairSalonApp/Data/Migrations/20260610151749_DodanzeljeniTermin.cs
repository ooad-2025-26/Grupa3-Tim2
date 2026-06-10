using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHairSalonApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodanzeljeniTermin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ZeljeniTermin",
                table: "Rezervacija",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZeljeniTermin",
                table: "Rezervacija");
        }
    }
}
