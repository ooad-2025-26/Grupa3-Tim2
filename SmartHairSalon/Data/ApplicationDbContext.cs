using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHairSalon.Models;

namespace SmartHairSalon.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Salon> Saloni { get; set; }
        public DbSet<Termin> Termini { get; set; }
        public DbSet<Usluga> Usluge { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Korpa> Korpe { get; set; }
        public DbSet<Proizvod> Proizvodi { get; set; }
        public DbSet<Narudzba> Narudzbe { get; set; }
        public DbSet<Obavijest> Obavijesti { get; set; }
    }
}