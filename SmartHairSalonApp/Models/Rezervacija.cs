
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        public StatusRezervacije StatusRezervacije { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Usluga")]
        public int UslugaId { get; set; }
        public Usluga Usluga { get; set; }


        [Display(Name = "Željeni termin")]
        public string? ZeljeniTermin { get; set; }
        public Rezervacija() { }
    }
}