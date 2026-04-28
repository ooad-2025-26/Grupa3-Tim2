using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Proizvod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; } = string.Empty;

        public double Cijena { get; set; }
        public string Opis { get; set; } = string.Empty;
        public int Kolicina { get; set; }

        public int? KorpaId { get; set; }
        public Korpa? Korpa { get; set; }
    }
}