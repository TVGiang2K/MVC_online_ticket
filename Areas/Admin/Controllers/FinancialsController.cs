using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using X.PagedList;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FinancialsController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public FinancialsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Financials
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;
            var financials = await _context.Financials.OrderBy(f => f.FinancialsId).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                financials = await _context.Financials.Where(f => f.Title.Contains(name)).OrderBy(f => f.Title).ToPagedListAsync(page, limit);
            }
            return View(financials);
        }

        // GET: Admin/Financials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Financials == null)
            {
                return NotFound();
            }

            var financial = await _context.Financials
                .FirstOrDefaultAsync(f => f.FinancialsId == id);
            if (financial == null)
            {
                return NotFound();
            }

            return View(financial);
        }

        // GET: Admin/Financials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Financials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FinancialsId,Title,FinancialFrom,FinancialTo,Percentage,Category")] Financials financial)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(financial);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Add successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ViewBag.error = "Error: Unable to add new Financial";
                    return View(financial);
                }
            }
            return View(financial);
        }

        // GET: Admin/Financials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Financials == null)
            {
                return NotFound();
            }

            var financial = await _context.Financials.FindAsync(id);
            if (financial == null)
            {
                return NotFound();
            }
            return View(financial);
        }

        // POST: Admin/Financials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FinancialsId,Title,FinancialFrom,FinancialTo,Percentage,Category")] Financials financial)
        {
            if (id != financial.FinancialsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(financial);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Update successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinancialExists(financial.FinancialsId))
                    {
                        ViewBag.error = "Error: Financial not found";
                        return View(financial);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to update Financial";
                        return View(financial);
                    }
                }
            }
            return View(financial);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Financials == null)
            {
                return Problem("Entity set 'AppDbContext.Financials'  is null.");
            }

            try
            {
                var financial = await _context.Financials.FindAsync(id);
                if (financial != null)
                {
                    _context.Financials.Remove(financial);
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

        private bool FinancialExists(int id)
        {
          return (_context.Financials?.Any(e => e.FinancialsId == id)).GetValueOrDefault();
        }
    }
}
