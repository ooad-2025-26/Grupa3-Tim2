using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Usluga
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }

        public double Cijena { get; set; }

        [ForeignKey("Salon")]
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        public Usluga() { }
    }
}