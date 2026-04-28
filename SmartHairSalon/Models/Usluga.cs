using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Usluga
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; } = string.Empty;

        public double Cijena { get; set; }
        public int Trajanje { get; set; }

        public int SalonId { get; set; }
        public Salon Salon { get; set; } = null!;

        public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
    }
}