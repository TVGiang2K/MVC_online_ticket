using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    public class BusesController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public BusesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Buses
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;
            var buses = await _context.Buses.OrderBy(b => b.BusesId).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                buses = await _context.Buses.Where(b => b.BusNumber.Contains(name)).OrderBy(b => b.BusesId).ToPagedListAsync(page, limit);
            }
            return View(buses);
        }

        // GET: Admin/Buses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Buses == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses
                .FirstOrDefaultAsync(m => m.BusesId == id);
            if (bus == null)

            {
                return NotFound();
            }

            return View(bus);
        }

        // GET: Admin/Buses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Buses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusesId,BusNumber,BusTypes,LicensePlate,SeatCapacity")] Buses buses)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingBus = await _context.Buses.FirstOrDefaultAsync(b => b.BusNumber == buses.BusNumber);

                    if (existingBus != null)
                    {
                        ViewBag.errN = "Error: Bus with the same Bus Number already exists.";
                        return View(buses);
                    }

                    _context.Add(buses);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Add successfully!";
                    return RedirectToAction(nameof(Index));

                }catch{
                    ViewBag.error = "Error: Unable to add new Bus";
                    return View(buses);
                }
            }
            return View(buses);
        }

        // GET: Admin/Buses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Buses == null)
            {
                return NotFound();
            }

            var buses = await _context.Buses.FindAsync(id);
            if (buses == null)
            {
                return NotFound();
            }
            return View(buses);
        }

        // POST: Admin/Buses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusesId,BusNumber,BusTypes,LicensePlate,SeatCapacity")] Buses buses)
        {
            if (id != buses.BusesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buses);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Update successfully!";
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusesExists(buses.BusesId))
                    {
                        ViewBag.error = "Error: Bus not found";
                        return View(buses);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to update Bus";
                        return View(buses);
                    }
                }
            }
            return View(buses);
        }


   
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Buses == null)
            {
                return Problem("Entity set 'AppDbContext.Buses'  is null.");
            }
            try
            { 
                var buses = await _context.Buses.FindAsync(id);
                if (buses != null)
                {
                    _context.Buses.Remove(buses);
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

        private bool BusesExists(int id)
        {
          return (_context.Buses?.Any(e => e.BusesId == id)).GetValueOrDefault();
        }
    }
}
