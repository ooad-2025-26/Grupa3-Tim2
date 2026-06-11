using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using System.Text.Json;

namespace SmartHairSalonApp.Controllers
{
    public class ProizvodController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Korisnik> _userManager;
        private const string SessionKey = "MojaKorpa";

        public ProizvodController(ApplicationDbContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================================
        // STANDARDNE CRUD AKCIJE ZA PROIZVOD (SHOP)
        // ==========================================================

        // GET: Proizvod
        public async Task<IActionResult> Index()
        {
            var proizvodi = await _context.Proizvodi.ToListAsync();
            return View(proizvodi);
        }

        // GET: Proizvod/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi
                .FirstOrDefaultAsync(m => m.Id == id);

            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }

        // GET: Proizvod/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proizvod/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Cijena,Opis,Kolicina")] Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proizvod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proizvod);
        }

        // GET: Proizvod/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod == null)
            {
                return NotFound();
            }
            return View(proizvod);
        }

        // POST: Proizvod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Cijena,Opis,Kolicina")] Proizvod proizvod)
        {
            if (id != proizvod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proizvod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProizvodExists(proizvod.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proizvod);
        }

        // GET: Proizvod/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proizvod = await _context.Proizvodi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proizvod == null)
            {
                return NotFound();
            }

            return View(proizvod);
        }

        // POST: Proizvod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proizvod = await _context.Proizvodi.FindAsync(id);
            if (proizvod != null)
            {
                _context.Proizvodi.Remove(proizvod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProizvodExists(int id)
        {
            return _context.Proizvodi.Any(e => e.Id == id);
        }

        // ==========================================================
        // POMOĆNE METODE ZA UPRAVLJANJE KORPOM (SESIJA)
        // ==========================================================

        private List<StavkaKorpeViewModel> DohvatiKorpuIzSesije()
        {
            var sessionData = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(sessionData))
            {
                return new List<StavkaKorpeViewModel>();
            }
            return JsonSerializer.Deserialize<List<StavkaKorpeViewModel>>(sessionData) ?? new List<StavkaKorpeViewModel>();
        }

        private void SpasiKorpuUSesiju(List<StavkaKorpeViewModel> korpa)
        {
            var sessionData = JsonSerializer.Serialize(korpa);
            HttpContext.Session.SetString(SessionKey, sessionData);
        }

        // ==========================================================
        // AKCIJE ZA UPRAVLJANJE KORPOM (SA FIKSNIM RUTAMA)
        // ==========================================================

        // GET: /Proizvod/Basket (Prikazuje cijelu "Your Beauty Basket" stranicu)
        [HttpGet]
        [Route("Proizvod/Basket")]
        public IActionResult Basket()
        {
            var korpa = DohvatiKorpuIzSesije();
            return View(korpa);
        }

        // GET ili POST: /Proizvod/DohvatiKorpu
        [HttpGet, HttpPost]
        [Route("Proizvod/DohvatiKorpu")]
        public IActionResult DohvatiKorpu()
        {
            var korpa = DohvatiKorpuIzSesije();
            return Json(korpa);
        }

        // GET ili POST: /Proizvod/DodajUKorpu?id=X
        [HttpGet, HttpPost]
        [Route("Proizvod/DodajUKorpu")]
        public IActionResult DodajUKorpu(int id)
        {
            var proizvod = _context.Proizvodi.FirstOrDefault(p => p.Id == id);
            if (proizvod == null)
            {
                return NotFound("Proizvod ne postoji u bazi podataka.");
            }

            var korpa = DohvatiKorpuIzSesije();
            var stavka = korpa.FirstOrDefault(s => s.ProizvodId == id);

            if (stavka == null)
            {
                korpa.Add(new StavkaKorpeViewModel
                {
                    ProizvodId = proizvod.Id,
                    Naziv = proizvod.Naziv,
                    Cijena = proizvod.Cijena,
                    Kolicina = 1
                });
            }
            else
            {
                stavka.Kolicina++;
            }

            SpasiKorpuUSesiju(korpa);
            return Json(korpa);
        }

        // GET ili POST: /Proizvod/PromijeniKolicinu?id=X&promjena=Y
        [HttpGet, HttpPost]
        [Route("Proizvod/PromijeniKolicinu")]
        public IActionResult PromijeniKolicinu(int id, int promjena)
        {
            var korpa = DohvatiKorpuIzSesije();
            var stavka = korpa.FirstOrDefault(s => s.ProizvodId == id);

            if (stavka != null)
            {
                stavka.Kolicina += promjena;
                if (stavka.Kolicina <= 0)
                {
                    korpa.Remove(stavka);
                }
            }

            SpasiKorpuUSesiju(korpa);
            return Json(korpa);
        }

        // GET ili POST: /Proizvod/UkloniIzKorpe?id=X
        [HttpGet, HttpPost]
        [Route("Proizvod/UkloniIzKorpe")]
        public IActionResult UkloniIzKorpe(int id)
        {
            var korpa = DohvatiKorpuIzSesije();
            var stavka = korpa.FirstOrDefault(s => s.ProizvodId == id);

            if (stavka != null)
            {
                korpa.Remove(stavka);
            }

            SpasiKorpuUSesiju(korpa);
            return Json(korpa);
        }

        // ==========================================================
        // FINALE: ZAVRŠETAK NARUDŽBE I SPAŠAVANJE U BAZU
        // ==========================================================
        [HttpPost]
        public async Task<IActionResult> ZavrsiNarudzbu()
        {
            // 1. Dohvati trenutno ulogovanog korisnika preko UserManager-a
            var korisnikId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(korisnikId))
            {
                return Json(new { success = false, redirectUrl = "/Identity/Account/Login" });
            }

            // 2. Povuci podatke iz sesije
            var sesijaKorpa = DohvatiKorpuIzSesije();
            if (sesijaKorpa == null || !sesijaKorpa.Any())
            {
                return Json(new { success = false, message = "Vaša korpa je prazna." });
            }

            // Računamo ukupnu cijenu za bazu
            double ukupnaCijena = sesijaKorpa.Sum(s => s.Cijena * s.Kolicina);

            // Koristimo DB transakciju kako bismo osigurali integritet podataka (sve ili ništa)
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 3. Kreiraj novu Korpu u bazi sa svim potrebnim poljima
                    var novaKorpa = new Korpa
                    {
                        UkupnaCijena = ukupnaCijena,
                        KorisnikId = korisnikId
                    };
                    _context.Korpe.Add(novaKorpa);
                    await _context.SaveChangesAsync(); // Generiše se novaKorpa.Id

                    // 4. Napuni veznu tabelu KorpaProizvod za svaki artikal
                    foreach (var stavka in sesijaKorpa)
                    {
                        var korpaProizvod = new KorpaProizvod
                        {
                            KorpaId = novaKorpa.Id,
                            ProizvodId = stavka.ProizvodId,
                            Kolicina = stavka.Kolicina
                        };
                        _context.KorpaProizvodi.Add(korpaProizvod);

                        // Smanji skladište u bazi (upravljanje zalihama)
                        var proizvodUBazi = await _context.Proizvodi.FindAsync(stavka.ProizvodId);
                        if (proizvodUBazi != null)
                        {
                            proizvodUBazi.Kolicina -= stavka.Kolicina;
                            _context.Proizvodi.Update(proizvodUBazi);
                        }
                    }
                    await _context.SaveChangesAsync();

                    // 5. Kreiraj finalnu Narudžbu sa ispravnim enum statusom
                    var novaNarudzba = new Narudzba
                    {
                        StatusNarudzbe = StatusNarudzbe.UObradi, // Postavljeno na tvoj enum "UObradi"
                        KorisnikId = korisnikId,
                        KorpaId = novaKorpa.Id
                    };
                    _context.Narudzbe.Add(novaNarudzba);
                    await _context.SaveChangesAsync();

                    // Potvrdi transakciju u bazi podataka
                    await transaction.CommitAsync();

                    // 6. Isprazni sesiju koristeći ispravan ključ
                    HttpContext.Session.Remove(SessionKey);

                    return Json(new { success = true, message = "Uspješno ste izvršili narudžbu!" });
                }
                catch (Exception ex)
                {
                    // U slučaju greške brišu se svi djelimični upisi
                    await transaction.RollbackAsync();
                    return Json(new { success = false, message = "Greška prilikom obrade baze podataka: " + ex.Message });
                }
            }
        }
    }

    public class StavkaKorpeViewModel
    {
        public int ProizvodId { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public double Cijena { get; set; }
        public int Kolicina { get; set; }
    }
}