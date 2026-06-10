using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;

namespace SmartHairSalonApp.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;

        public RezervacijaController(
            ApplicationDbContext context,
            UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rezervacija
        [Authorize(Roles = "admin,zaposlenik,korisnik")]
        public async Task<IActionResult> Index()
        {
    
            var korisnik = await _userManager.GetUserAsync(User);
            ViewBag.Admin = User.IsInRole("admin");
            ViewBag.Zaposlenik = User.IsInRole("zaposlenik");
            ViewBag.Korisnik = User.IsInRole("korisnik");

            if (User.IsInRole("korisnik"))
            {
                var mojeRezervacije = await _context.Rezervacije
                    .Include(r => r.Korisnik)
                    .Include(r => r.Usluga)
                    .Where(r => r.KorisnikId == korisnik.Id)
                    .ToListAsync();

                return View(mojeRezervacije);
            }

            var sveRezervacije = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Usluga)
                .ToListAsync();

            return View(sveRezervacije);
        }

        // GET: Rezervacija/Details/5
        [Authorize(Roles = "admin,zaposlenik,korisnik")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Usluga)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        [Authorize(Roles = "korisnik")]
        public IActionResult Create()
        {
            ViewData["UslugaId"] = new SelectList(
                _context.Usluge,
                "Id",
                "Naziv");

            return View();
        }

        // POST: Rezervacija/Create
        [Authorize(Roles = "korisnik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("UslugaId,ZeljeniTermin")] Rezervacija rezervacija)
        {
            var korisnik = await _userManager.GetUserAsync(User);

            rezervacija.KorisnikId = korisnik.Id;
            rezervacija.StatusRezervacije = StatusRezervacije.UObradi;

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // POTVRDI REZERVACIJU
        [Authorize(Roles = "admin,zaposlenik")]
        public async Task<IActionResult> Prihvati(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            rezervacija.StatusRezervacije = StatusRezervacije.Potvrdjena;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ODBIJ REZERVACIJU
        [Authorize(Roles = "admin,zaposlenik")]
        public async Task<IActionResult> Odbij(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            rezervacija.StatusRezervacije = StatusRezervacije.Odbijena;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Rezervacija/Edit/5
        [Authorize(Roles = "admin,zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            ViewData["KorisnikId"] =
                new SelectList(_context.Users, "Id", "Id", rezervacija.KorisnikId);

            ViewData["UslugaId"] =
                new SelectList(_context.Usluge, "Id", "Naziv", rezervacija.UslugaId);

            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        [Authorize(Roles = "admin,zaposlenik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,StatusRezervacije,KorisnikId,UslugaId,ZeljeniTermin")]
            Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["KorisnikId"] =
                new SelectList(_context.Users, "Id", "Id", rezervacija.KorisnikId);

            ViewData["UslugaId"] =
                new SelectList(_context.Usluge, "Id", "Naziv", rezervacija.UslugaId);

            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Usluga)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}