using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Models;

namespace SmartHairSalonApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<Korisnik>(options)
    {
        public DbSet<Narudzba> Narudzbe { get; set; }
        public DbSet<Korpa> Korpe { get; set; }
        public DbSet<Rezervacija> Rezervacije { get; set; }
        public DbSet<Termin> Termini { get; set; }
        public DbSet<Salon> Saloni { get; set; }
        public DbSet<Obavijest> Obavijesti { get; set; }
        public DbSet<Usluga> Usluge { get; set; }
        public DbSet<Proizvod> Proizvodi { get; set; }
        public DbSet<KorpaProizvod> KorpaProizvodi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Narudzba>().ToTable("Narudzba");
            modelBuilder.Entity<Korpa>().ToTable("Korpa");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Termin>().ToTable("Termin");
            modelBuilder.Entity<Salon>().ToTable("Salon");
            modelBuilder.Entity<Obavijest>().ToTable("Obavijest");
            modelBuilder.Entity<Usluga>().ToTable("Usluga");
            modelBuilder.Entity<Proizvod>().ToTable("Proizvod");
            modelBuilder.Entity<KorpaProizvod>().ToTable("KorpaProizvod");

            
            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Usluga)
                .WithMany()
                .HasForeignKey(r => r.UslugaId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Narudzba>()
                .HasOne(n => n.Korpa)
                .WithMany()
                .HasForeignKey(n => n.KorpaId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}