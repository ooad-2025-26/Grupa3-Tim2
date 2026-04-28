using System;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Obavijest
    {
        [Key]
        public int Id { get; set; }

        public string Poruka { get; set; } = string.Empty;
        public DateTime Datum { get; set; }

        public string KorisnikId { get; set; } = null!;
        public Korisnik Korisnik { get; set; } = null!;
    }
}