#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization; // 🔥 Dodano za zaključavanje pojedinačnih akcija

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

        // Otvoreno za sve: Gosti mogu gledati Web Shop
        public async Task<IActionResult> Index()
        {
            var proizvodi = await _context.Proizvodi.ToListAsync();
            return View(proizvodi);
        }

        // Otvoreno za sve: Gosti mogu gledati detalje proizvoda
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

        public IActionResult Create()
        {
            return View();
        }

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

        private KorpaObracunViewModel PokreniAlgoritamObracuna(List<StavkaKorpeViewModel> osnovnaKorpa)
        {
            var obracun = new KorpaObracunViewModel
            {
                Stavke = osnovnaKorpa
            };

            foreach (var stavka in obracun.Stavke)
            {
                if (stavka.Kolicina >= 3)
                {
                    stavka.PopustProcenat = 15;
                }
                else
                {
                    stavka.PopustProcenat = 0;
                }
            }

            double privremeniSumaNakonPopusta = obracun.Stavke.Sum(s => s.UkupnoStavka);
            if (privremeniSumaNakonPopusta > 150)
            {
                obracun.PoklonPoruka = "Čestitamo! Ostvarili ste pravo na GRATIS uzorak premium maske za kosu uz vaše preuzimanje.";
            }
            else
            {
                obracun.PoklonPoruka = string.Empty;
            }

            return obracun;
        }

        // =========================================================================
        // 🔥 ZAKLJUČANE AKCIJE ZA KORPU - ZAHTIJEVAJU LOGIN
        // =========================================================================

        [Authorize] // Zabrana za goste
        [HttpGet]
        [Route("Proizvod/Basket")]
        public IActionResult Basket()
        {
            var korpa = DohvatiKorpuIzSesije();
            var obracunataKorpa = PokreniAlgoritamObracuna(korpa);
            return View(obracunataKorpa);
        }

        [Authorize] // Zabrana za goste
        [HttpGet, HttpPost]
        [Route("Proizvod/DohvatiKorpu")]
        public IActionResult DohvatiKorpu()
        {
            var korpa = DohvatiKorpuIzSesije();
            return Json(korpa);
        }

        [Authorize] // 🔥 KLJUČNO: Ako gost klikne kupi, sistem mu ne da već ga šalje na Login!
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
                    Naziv = proizvod.Naziv ?? "Proizvod",
                    Cijena = (double)proizvod.Cijena,
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

        [Authorize] // Zabrana za goste
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

        [Authorize] // Zabrana za goste
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

        [Authorize] // Zabrana za goste
        [HttpPost]
        public async Task<IActionResult> ZavrsiNarudzbu()
        {
            var korisnikId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(korisnikId))
            {
                return Json(new { success = false, redirectUrl = "/Identity/Account/Login" });
            }

            var sesijaKorpa = DohvatiKorpuIzSesije();
            if (sesijaKorpa == null || !sesijaKorpa.Any())
            {
                return Json(new { success = false, message = "Vaša korpa je prazna." });
            }

            var finalniObracun = PokreniAlgoritamObracuna(sesijaKorpa);
            double ukupnaCijena = finalniObracun.Total;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var novaKorpa = new Korpa
                    {
                        UkupnaCijena = ukupnaCijena,
                        KorisnikId = korisnikId
                    };
                    _context.Korpe.Add(novaKorpa);
                    await _context.SaveChangesAsync();

                    foreach (var stavka in sesijaKorpa)
                    {
                        var korpaProizvod = new KorpaProizvod
                        {
                            KorpaId = novaKorpa.Id,
                            ProizvodId = stavka.ProizvodId,
                            Kolicina = stavka.Kolicina
                        };

                        _context.KorpaProizvodi.Add(korpaProizvod);

                        var proizvodUBazi = await _context.Proizvodi.FindAsync(stavka.ProizvodId);
                        if (proizvodUBazi != null)
                        {
                            proizvodUBazi.Kolicina -= stavka.Kolicina;
                            _context.Proizvodi.Update(proizvodUBazi);
                        }
                    }
                    await _context.SaveChangesAsync();

                    var novaNarudzba = new Narudzba
                    {
                        StatusNarudzbe = StatusNarudzbe.UObradi,
                        KorisnikId = korisnikId,
                        KorpaId = novaKorpa.Id
                    };
                    _context.Narudzbe.Add(novaNarudzba);
                    await _context.SaveChangesAsync();

                    var obavijestZaSalon = new Obavijest
                    {
                        Poruka = $"SALON: Nova rezervacija proizvoda #{novaNarudzba.Id} spremna za preuzimanje.",
                        KorisnikId = korisnikId,
                        Datum = DateTime.Now
                    };
                    _context.Obavijesti.Add(obavijestZaSalon);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    HttpContext.Session.Remove(SessionKey);

                    return Json(new { success = true, message = "Vaša narudžba je rezervisana! Proizvodi vas čekaju na preuzimanju u salonu." });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    return Json(new { success = false, message = "Greška prilikom obrade baze podataka: " + innerMessage });
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
        public double PopustProcenat { get; set; }
        public double CijenaSaPopustom => Cijena * (1 - (PopustProcenat / 100));
        public double UkupnoStavka => CijenaSaPopustom * Kolicina;
    }

    public class KorpaObracunViewModel
    {
        public List<StavkaKorpeViewModel> Stavke { get; set; } = new List<StavkaKorpeViewModel>();
        public double SubTotal => Stavke.Sum(s => s.Cijena * s.Kolicina);
        public double UkupniPopust => Stavke.Sum(s => (s.Cijena - s.CijenaSaPopustom) * s.Kolicina);
        public double Total => SubTotal - UkupniPopust;
        public string PoklonPoruka { get; set; } = string.Empty;
    }
}