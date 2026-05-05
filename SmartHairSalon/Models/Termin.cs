using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartHairSalon.Models
{
    public class Termin
    {
        [Key]
        public int Id { get; set; }

        public DateTime Vrijeme { get; set; }

        public string Status { get; set; } = string.Empty;

        public int SalonId { get; set; }
        public Salon Salon { get; set; } = null!;

      
    }
}