using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Proizvod
    {
        [Key]
        public int Id { get; set; }

        public string Naziv { get; set; }

        public double Cijena { get; set; }

        public string Opis { get; set; }

        public int Kolicina { get; set; }

        public Proizvod() { }
    }
}