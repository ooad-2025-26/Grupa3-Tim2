
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHairSalonApp.Models
{
    public class Termin
    {
        [Key]
        public int Id { get; set; }

        public DateTime Vrijeme { get; set; }

        public StatusTermina StatusTermina { get; set; }

        [ForeignKey("Korisnik")]
        public string KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        [ForeignKey("Salon")]
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        public Termin() { }

        [NotMapped]
        public string PrikazTermina
        {
            get
            {
                return Vrijeme.ToString("dd.MM.yyyy HH:mm");
            }
        }
    }
}