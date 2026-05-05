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


	}
}