using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgenciesController : Controller
    {
        private readonly AppDbContext _context;

        public AgenciesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Agencies
        public async Task<IActionResult> Index()
        {
              return _context.Agencies != null ? 
                          View(await _context.Agencies.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Agencies'  is null.");
        }

        // GET: Admin/Agencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Agencies == null)
            {
                return NotFound();
            }

            var agencies = await _context.Agencies
                .FirstOrDefaultAsync(m => m.AgenciesId == id);
            if (agencies == null)
            {
                return NotFound();
            }

            return View(agencies);
        }

        // GET: Admin/Agencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Agencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgenciesId,Address,Phone")] Agencies agencies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agencies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agencies);
        }

        // GET: Admin/Agencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Agencies == null)
            {
                return NotFound();
            }

            var agencies = await _context.Agencies.FindAsync(id);
            if (agencies == null)
            {
                return NotFound();
            }
            return View(agencies);
        }

        // POST: Admin/Agencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AgenciesId,Address,Phone")] Agencies agencies)
        {
            if (id != agencies.AgenciesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agencies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgenciesExists(agencies.AgenciesId))
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
            return View(agencies);
        }

        // GET: Admin/Agencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Agencies == null)
            {
                return NotFound();
            }

            var agencies = await _context.Agencies
                .FirstOrDefaultAsync(m => m.AgenciesId == id);
            if (agencies == null)
            {
                return NotFound();
            }

            return View(agencies);
        }

        // POST: Admin/Agencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Agencies == null)
            {
                return Problem("Entity set 'AppDbContext.Agencies'  is null.");
            }
            var agencies = await _context.Agencies.FindAsync(id);
            if (agencies != null)
            {
                _context.Agencies.Remove(agencies);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgenciesExists(int id)
        {
          return (_context.Agencies?.Any(e => e.AgenciesId == id)).GetValueOrDefault();
        }
    }
}
