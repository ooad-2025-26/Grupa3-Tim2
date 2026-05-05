using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Korpa
    {
        [Key]
        public int Id { get; set; }

        public double UkupnaCijena { get; set; }

        public string KorisnikId { get; set; } = null!;
        public Korisnik Korisnik { get; set; } = null!;

      
    }
}