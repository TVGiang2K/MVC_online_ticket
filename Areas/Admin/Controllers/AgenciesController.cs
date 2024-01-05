using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using X.PagedList;

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
        public async Task<IActionResult> Index(string name, int page = 1)
        {
      
            int limit = 5;
            var agencies = await _context.Agencies.OrderBy(a => a.AgenciesId).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                agencies = await _context.Agencies.Where(a => a.Address.Contains(name)).OrderBy(a => a.AgenciesId).ToPagedListAsync(page, limit);
            }
            return View(agencies);
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
                try
                {
                    _context.Add(agencies);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Add successfully!";
                    return RedirectToAction(nameof(Index));

                }
                catch
                {
                    ViewBag.error = "Error: Unable to add new Agency";
                    return View(agencies);
                }
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
                    TempData["success"] = "Update successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgenciesExists(agencies.AgenciesId))
                    {
                        ViewBag.error = "Error: Agency not found";
                        return View(agencies);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to update Agency";
                        return View(agencies);
                    }
                }
            }
            return View(agencies);
        }

        

        // POST: Admin/Agencies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            if (_context.Buses == null)
            {
                return Problem("Entity set 'AppDbContext.Agencies'  is null.");
            }
            try
            {
                var agencies = await _context.Agencies.FindAsync(id);
                if (agencies != null)
                {
                    _context.Agencies.Remove(agencies);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Delete successfully!";
                }

            }
            catch
            {
                TempData["error"] = "Delete failed!";

            }

            return RedirectToAction(nameof(Index));
        }

        private bool AgenciesExists(int id)
        {
          return (_context.Agencies?.Any(e => e.AgenciesId == id)).GetValueOrDefault();
        }
    }
}
