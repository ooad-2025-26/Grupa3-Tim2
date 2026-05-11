using System.ComponentModel.DataAnnotations;

namespace SmartHairSalonApp.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }

        public string Lokacija { get; set; }

        public string RadnoVrijeme { get; set; }

        public Salon() { }
    }
}