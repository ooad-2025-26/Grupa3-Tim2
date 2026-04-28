using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        public string Lokacija { get; set; } = string.Empty;

        public string RadnoVrijeme { get; set; } = string.Empty;

        public ICollection<Termin> Termini { get; set; } = new List<Termin>();
        public ICollection<Usluga> Usluge { get; set; } = new List<Usluga>();
    }
}