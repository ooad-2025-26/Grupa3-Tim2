using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SmartHairSalonApp.Controllers
{
    [Authorize] // Kompletan kontroler narudžbi je nevidljiv za goste
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

            // Pokrivamo i admina i zaposlenika (velika i mala slova za svaki slučaj)
            if (User.IsInRole("admin") || User.IsInRole("Admin") || User.IsInRole("zaposlenik") || User.IsInRole("Zaposlenik"))
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

        // POST: Narudzba/PromijeniStatus
        // Promijenili smo naziv i parametre da savršeno prima Enum sa klijenta preko Ajaxa
        [HttpPost]
        [Authorize(Roles = "admin, Admin, zaposlenik, Zaposlenik")] // Dozvoljeno i adminu i zaposleniku
        public async Task<IActionResult> PromijeniStatus(int id, StatusNarudzbe noviStatus)
        {
            var narudzba = await _context.Narudzbe.FindAsync(id);
            if (narudzba == null) return NotFound();

            // Dodjeljujemo proslijeđeni enum status (Potvrdjena ili Odbijena)
            narudzba.StatusNarudzbe = noviStatus;

            // Kreiranje obavijesti za kupca na osnovu odluke osoblja
            string porukaKupcu = "";
            if (noviStatus == StatusNarudzbe.Potvrdjena)
            {
                porukaKupcu = $"Vaša narudžba #{narudzba.Id} je PRIHVAĆENA od strane našeg tima i spremna je za preuzimanje!";
            }
            else if (noviStatus == StatusNarudzbe.Odbijena)
            {
                porukaKupcu = $"Vaša narudžba #{narudzba.Id} je nažalost ODBIJENA (artikli trenutno nisu na stanju).";
            }

            if (!string.IsNullOrEmpty(porukaKupcu))
            {
                var obavijestKupac = new Obavijest
                {
                    Poruka = porukaKupcu,
                    KorisnikId = narudzba.KorisnikId,
                    Datum = DateTime.Now
                };
                _context.Obavijesti.Add(obavijestKupac);
            }

            await _context.SaveChangesAsync();

            // Vraćamo JSON odgovor umjesto Redirect-a, jer radimo preko Fetch/Ajax-a bez osvježavanja stranice
            return Json(new { success = true, statusNaziv = narudzba.StatusNarudzbe.ToString() });
        }
    }
}