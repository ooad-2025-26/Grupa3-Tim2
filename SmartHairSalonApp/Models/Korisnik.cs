using Microsoft.AspNetCore.Identity;

namespace SmartHairSalonApp.Models
{
    public class Korisnik : IdentityUser
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
    }
}
