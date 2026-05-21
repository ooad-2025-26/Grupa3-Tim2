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
    public class UslugaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UslugaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usluga
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Usluge.Include(u => u.Salon);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Usluga/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluge
                .Include(u => u.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usluga == null)
            {
                return NotFound();
            }

            return View(usluga);
        }

        // GET: Usluga/Create
        [Authorize(Roles = "admin,zaposlenik")]
        public IActionResult Create()
        {
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id");
            return View();
        }

        // POST: Usluga/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,zaposlenik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Cijena,SalonId")] Usluga usluga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usluga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", usluga.SalonId);
            return View(usluga);
        }

        // GET: Usluga/Edit/5
        [Authorize(Roles = "admin,zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluge.FindAsync(id);
            if (usluga == null)
            {
                return NotFound();
            }
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", usluga.SalonId);
            return View(usluga);
        }

        // POST: Usluga/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin,zaposlenik")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Cijena,SalonId")] Usluga usluga)
        {
            if (id != usluga.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usluga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UslugaExists(usluga.Id))
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
            ViewData["SalonId"] = new SelectList(_context.Saloni, "Id", "Id", usluga.SalonId);
            return View(usluga);
        }

        // GET: Usluga/Delete/5

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usluga = await _context.Usluge
                .Include(u => u.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usluga == null)
            {
                return NotFound();
            }

            return View(usluga);
        }

        // POST: Usluga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usluga = await _context.Usluge.FindAsync(id);
            if (usluga != null)
            {
                _context.Usluge.Remove(usluga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UslugaExists(int id)
        {
            return _context.Usluge.Any(e => e.Id == id);
        }
    }
}
