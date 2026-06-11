using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization; // 🔥 Dodano za zaključavanje kontrolera

namespace SmartHairSalonApp.Controllers
{
    [Authorize] // 🔥 Kompletan kontroler narudžbi je sada nevidljiv za goste
    public class NarudzbaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public NarudzbaController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Narudzba
        public async Task<IActionResult> Index()
        {
            var korisnikId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(korisnikId))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            List<Narudzba> narudzbe;

            if (User.IsInRole("admin") || User.IsInRole("zaposlenik"))
            {
                narudzbe = await _context.Narudzbe
                    .Include(n => n.Korisnik)
                    .Include(n => n.Korpa)
                    .OrderByDescending(n => n.Id)
                    .ToListAsync();
            }
            else
            {
                narudzbe = await _context.Narudzbe
                    .Include(n => n.Korisnik)
                    .Include(n => n.Korpa)
                    .Where(n => n.KorisnikId == korisnikId)
                    .OrderByDescending(n => n.Id)
                    .ToListAsync();
            }

            return View(narudzbe);
        }

        // POST: Narudzba/Odluci
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Odluci(int id, string odluka)
        {
            if (!User.IsInRole("zaposlenik"))
            {
                return Forbid();
            }

            var narudzba = await _context.Narudzbe.FindAsync(id);
            if (narudzba == null) return NotFound();

            if (odluka == "prihvati")
            {
                narudzba.StatusNarudzbe = StatusNarudzbe.Potvrdjena;

                var obavijestKupac = new Obavijest
                {
                    Poruka = $"Vaša narudžba #{narudzba.Id} je PRIHVAĆENA od strane našeg tima!",
                    KorisnikId = narudzba.KorisnikId,
                    Datum = DateTime.Now
                };
                _context.Obavijesti.Add(obavijestKupac);
            }
            else if (odluka == "odbij")
            {
                narudzba.StatusNarudzbe = StatusNarudzbe.Odbijena;

                var obavijestKupac = new Obavijest
                {
                    Poruka = $"Vaša narudžba #{narudzba.Id} je nažalost ODBIJENA (artikli trenutno nisu na stanju).",
                    KorisnikId = narudzba.KorisnikId,
                    Datum = DateTime.Now
                };
                _context.Obavijesti.Add(obavijestKupac);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}