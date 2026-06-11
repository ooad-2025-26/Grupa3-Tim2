using System.ComponentModel.DataAnnotations;

namespace SmartHairSalonApp.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Naziv { get; set; }

        public string Lokacija { get; set; } 

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string RadnoVrijeme { get; set; }

        public Salon() { }
    }
}