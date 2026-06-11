using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using Microsoft.AspNetCore.Identity;

namespace SmartHairSalonApp.Controllers
{
    public class NarudzbaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        // Kroz konstruktor ubrizgavamo bazu i rad sa korisnicima
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

            // OBRISAN I ZAKOMENTARISAN DIO KOJI JE TRAŽIO 'IsProcitana' DA NE BI KVARIO BUILD

            List<Narudzba> narudzbe;

            // 2. Filtriranje narudžbi zavisno od toga ko gleda stranicu
            if (User.IsInRole("admin") || User.IsInRole("zaposlenik"))
            {
                // Admin i zaposlenik vide sve narudžbe u salonu sa podacima o klijentu i korpi
                narudzbe = await _context.Narudzbe
                    .Include(n => n.Korisnik)
                    .Include(n => n.Korpa)
                    .OrderByDescending(n => n.Id)
                    .ToListAsync();
            }
            else
            {
                // Obični korisnik vidi isključivo svoje narudžbe
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
            // Osiguranje: Samo zaposlenik donosi odluke! Admin i obični korisnici dobijaju "Access Denied"
            if (!User.IsInRole("zaposlenik"))
            {
                return Forbid();
            }

            var narudzba = await _context.Narudzbe.FindAsync(id);
            if (narudzba == null) return NotFound();

            if (odluka == "prihvati")
            {
                narudzba.StatusNarudzbe = StatusNarudzbe.Potvrdjena;

                // Kreiramo obavijest namijenjenu isključivo kupcu koji je napravio narudžbu (PRILAGOĐENO MODELU)
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

                // Kreiramo obavijest namijenjenu kupcu (PRILAGOĐENO MODELU)
                var obavijestKupac = new Obavijest
                {
                    Poruka = $"Vaša narudžba #{narudzba.Id} je nažalost ODBIJENA (artikli trenutno nisu na stanju).",
                    KorisnikId = narudzba.KorisnikId,
                    Datum = DateTime.Now
                };
                _context.Obavijesti.Add(obavijestKupac);
            }

            await _context.SaveChangesAsync();

            // Nakon što donesemo odluku, osvježavamo stranicu (vraćamo se na tabelu)
            return RedirectToAction(nameof(Index));
        }
    }
}