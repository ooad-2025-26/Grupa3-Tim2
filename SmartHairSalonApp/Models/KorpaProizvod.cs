
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class KorpaProizvod
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Korpa")]
        public int KorpaId { get; set; }
        public Korpa Korpa { get; set; }

        [ForeignKey("Proizvod")]
        public int ProizvodId { get; set; }
        public Proizvod Proizvod { get; set; }

        public int Kolicina { get; set; }

        public KorpaProizvod() { }
    }
}
