using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHairSalon.Data.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ime",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Korpe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UkupnaCijena = table.Column<double>(type: "float", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korpe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Korpe_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obavijesti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Poruka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavijesti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obavijesti_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Saloni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lokacija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RadnoVrijeme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saloni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Narudzbe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KorpaId = table.Column<int>(type: "int", nullable: false),
                    KorpaId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzbe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Narudzbe_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Narudzbe_Korpe_KorpaId",
                        column: x => x.KorpaId,
                        principalTable: "Korpe",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Narudzbe_Korpe_KorpaId1",
                        column: x => x.KorpaId1,
                        principalTable: "Korpe",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Proizvodi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cijena = table.Column<double>(type: "float", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    KorpaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proizvodi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proizvodi_Korpe_KorpaId",
                        column: x => x.KorpaId,
                        principalTable: "Korpe",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Termini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Termini_Saloni_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Saloni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cijena = table.Column<double>(type: "float", nullable: false),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usluge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usluge_Saloni_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Saloni",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: false),
                    UslugaId = table.Column<int>(type: "int", nullable: false),
                    KorisnikId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TerminId1 = table.Column<int>(type: "int", nullable: true),
                    UslugaId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_AspNetUsers_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_AspNetUsers_KorisnikId1",
                        column: x => x.KorisnikId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Termini_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termini",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Termini_TerminId1",
                        column: x => x.TerminId1,
                        principalTable: "Termini",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Usluge_UslugaId",
                        column: x => x.UslugaId,
                        principalTable: "Usluge",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Usluge_UslugaId1",
                        column: x => x.UslugaId1,
                        principalTable: "Usluge",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Korpe_KorisnikId",
                table: "Korpe",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbe_KorisnikId",
                table: "Narudzbe",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbe_KorpaId",
                table: "Narudzbe",
                column: "KorpaId");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzbe_KorpaId1",
                table: "Narudzbe",
                column: "KorpaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Obavijesti_KorisnikId",
                table: "Obavijesti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Proizvodi_KorpaId",
                table: "Proizvodi",
                column: "KorpaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KorisnikId",
                table: "Rezervacije",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KorisnikId1",
                table: "Rezervacije",
                column: "KorisnikId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_TerminId",
                table: "Rezervacije",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_TerminId1",
                table: "Rezervacije",
                column: "TerminId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_UslugaId",
                table: "Rezervacije",
                column: "UslugaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_UslugaId1",
                table: "Rezervacije",
                column: "UslugaId1");

            migrationBuilder.CreateIndex(
                name: "IX_Termini_SalonId",
                table: "Termini",
                column: "SalonId");

            migrationBuilder.CreateIndex(
                name: "IX_Usluge_SalonId",
                table: "Usluge",
                column: "SalonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Narudzbe");

            migrationBuilder.DropTable(
                name: "Obavijesti");

            migrationBuilder.DropTable(
                name: "Proizvodi");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Korpe");

            migrationBuilder.DropTable(
                name: "Termini");

            migrationBuilder.DropTable(
                name: "Usluge");

            migrationBuilder.DropTable(
                name: "Saloni");

            migrationBuilder.DropColumn(
                name: "Ime",
                table: "AspNetUsers");
        }
    }
}
