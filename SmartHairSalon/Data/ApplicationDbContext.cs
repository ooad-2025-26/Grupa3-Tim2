using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHairSalon.Models;

public class ApplicationDbContext : IdentityDbContext<Korisnik>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Korpa> Korpe { get; set; }
    public DbSet<Obavijest> Obavijesti { get; set; }
    public DbSet<Salon> Saloni { get; set; }
    public DbSet<Narudzba> Narudzbe { get; set; }
    public DbSet<Proizvod> Proizvodi { get; set; }
    public DbSet<Termin> Termini { get; set; }
    public DbSet<Usluga> Usluge { get; set; }
    public DbSet<Rezervacija> Rezervacije { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Korpa>().ToTable("Korpe");
        modelBuilder.Entity<Obavijest>().ToTable("Obavijesti");
        modelBuilder.Entity<Salon>().ToTable("Saloni");
        modelBuilder.Entity<Narudzba>().ToTable("Narudzbe");
        modelBuilder.Entity<Proizvod>().ToTable("Proizvodi");
        modelBuilder.Entity<Termin>().ToTable("Termini");
        modelBuilder.Entity<Usluga>().ToTable("Usluge");
        modelBuilder.Entity<Rezervacija>().ToTable("Rezervacije");

        // 🔴 FIX za multiple cascade paths (KLJUČNO)

        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Korisnik)
            .WithMany()
            .HasForeignKey(r => r.KorisnikId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Termin)
            .WithMany()
            .HasForeignKey(r => r.TerminId)
            .OnDelete(DeleteBehavior.NoAction);

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
    }
}