using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        public StatusRezervacije Status { get; set; }

        public string KorisnikId { get; set; } = null!;
        public Korisnik Korisnik { get; set; } = null!;

        public int TerminId { get; set; }
        public Termin Termin { get; set; } = null!;

        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; } = null!;
    }
}