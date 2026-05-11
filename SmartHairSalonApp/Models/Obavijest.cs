
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Obavijest
    {
        [Key]
        public int Id { get; set; }

        public string Poruka { get; set; }

        public DateTime Datum { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public Obavijest() { }
    }
}