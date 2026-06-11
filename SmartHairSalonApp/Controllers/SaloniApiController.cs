using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using System.Threading.Tasks;

namespace SmartHairSalonApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaloniApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SaloniApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ruta: GET /api/saloniapi
        [HttpGet]
        public async Task<IActionResult> GetSalone()
        {
            // Povlačimo salone iz baze
            var saloni = await _context.Saloni
                .Select(s => new
                {
                    s.Id,
                    s.Naziv,
                    s.Lokacija,
                    s.RadnoVrijeme,
                    s.Latitude,
                    s.Longitude
                })
                .ToListAsync();

            return Ok(saloni);
        }
    }
}