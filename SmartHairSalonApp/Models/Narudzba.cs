
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Narudzba
    {
        [Key]
        public int Id { get; set; }

        public StatusNarudzbe StatusNarudzbe { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }

        public Korisnik Korisnik { get; set; }

        [ForeignKey("Korpa")]
        public int KorpaId { get; set; }

        public Korpa Korpa { get; set; }

        public Narudzba() { }
    }
}