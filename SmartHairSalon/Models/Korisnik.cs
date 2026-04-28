using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
	public class Korisnik : IdentityUser
	{
		[Required]
		[StringLength(100)]
		public string Ime { get; set; } = string.Empty;

		public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
		public ICollection<Narudzba> Narudzbe { get; set; } = new List<Narudzba>();
		public ICollection<Obavijest> Obavijesti { get; set; } = new List<Obavijest>();
		public ICollection<Korpa> Korpe { get; set; } = new List<Korpa>();
	}
}