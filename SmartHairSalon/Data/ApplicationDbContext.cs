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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Salon>().ToTable("Salon");
            modelBuilder.Entity<Termin>().ToTable("Termin");
            modelBuilder.Entity<Usluga>().ToTable("Usluga");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Korpa>().ToTable("Korpa");
            modelBuilder.Entity<Proizvod>().ToTable("Proizvod");
            modelBuilder.Entity<Narudzba>().ToTable("Narudzba");
            modelBuilder.Entity<Obavijest>().ToTable("Obavijest");
        }
    }
}