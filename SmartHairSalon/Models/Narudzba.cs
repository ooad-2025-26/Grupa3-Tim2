using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        public StatusNarudzbe Status { get; set; }

        public string KorisnikId { get; set; } = null!;
        public Korisnik Korisnik { get; set; } = null!;

        public int KorpaId { get; set; }
        public Korpa Korpa { get; set; } = null!;
    }
}