using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartHairSalonApp.Data;
using SmartHairSalonApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHairSalonApp.Controllers
{
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Termin
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termini.Include(t => t.Korisnik).Include(t => t.Salon);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Termin/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Korisnik)
                .Include(t => t.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // GET: Termin/Create

        [Authorize(Roles = "admin,zaposlenik")]
        public IActionResult Create()
        {
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id");
            return View();
        }

        // POST: Termin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,zaposlenik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vrijeme,StatusTermina,KorisnikId,SalonId")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Id", termin.KorisnikId);
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", termin.SalonId);
            return View(termin);
        }

        // GET: Termin/Edit/5

        [Authorize(Roles = "admin,zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Id", termin.KorisnikId);
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", termin.SalonId);
            return View(termin);
        }

        // POST: Termin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,zaposlenik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vrijeme,StatusTermina,KorisnikId,SalonId")] Termin termin)
        {
            if (id != termin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.Id))
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
            ViewData["KorisnikId"] = new SelectList(_context.Users, "Id", "Id", termin.KorisnikId);
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", termin.SalonId);
            return View(termin);
        }

        // GET: Termin/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Korisnik)
                .Include(t => t.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // POST: Termin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termini.FindAsync(id);
            if (termin != null)
            {
                _context.Termini.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termini.Any(e => e.Id == id);
        }
    }
}
