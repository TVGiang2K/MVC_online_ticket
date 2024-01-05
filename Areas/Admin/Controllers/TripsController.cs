using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using X.PagedList;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TripsController : AdminBaseController
    {
        private readonly AppDbContext _context;

        public TripsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Trips
        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;
            var trips = await _context.Trips.Include(t => t.Buses).OrderBy(a => a.TripsId).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                trips = await _context.Trips.Where(t => t.DepartureLocation == name || t.DestinationLocation == name).Include(t => t.Buses).OrderBy(a => a.TripsId).ToPagedListAsync(page, limit);
            }
            return View(trips);
        }

        // GET: Admin/Trips/Details/5
        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trip = await _context.Trips
                .Include(t => t.Buses)
                .FirstOrDefaultAsync(m => m.TripsId == id);

            int limit = 5;
            var reservations = await _context.Reservations.Where(r => r.TripsId == trip.TripsId).OrderBy(a => a.ReservationDateTime).ToPagedListAsync(page, limit);
            if (trip == null)
            {
                return NotFound();
            }

            ViewBag.trip = trip;
            return View(reservations);
        }

        // GET: Admin/Trips/Create
        public IActionResult Create()
        {
            ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber");
            return View();
        }

        // POST: Admin/Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TripsId,BusesId,DepartureLocation,DestinationLocation,Image,DepartureDateTime,DestinationDateTime,Distance,RouteTrip,BasePrice")] Trips trips)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = GenerateUniqueFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "trips", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            trips.Image = FileName;
                        }
                    }

                    _context.Add(trips);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Add successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    if (trips.Image == null)
                    {
                        ViewBag.errI = "Image is required";
                        ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
                        return View(trips);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to add new Account";
                        ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
                        return View(trips);
                    }
                }
            }
            ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
            return View(trips);
        }

        // GET: Admin/Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trips == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips.FindAsync(id);
            if (trips == null)
            {
                return NotFound();
            }
            ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
            return View(trips);
        }

        // POST: Admin/Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripsId,BusesId,DepartureLocation,DestinationLocation,Image,DepartureDateTime,DestinationDateTime,Distance,RouteTrip,BasePrice")] Trips trips)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(m => m.TripsId == id);

            if (id != trips.TripsId)
            {
                ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
                ViewBag.error = "Error: Unable to update new Trip";
                return View(trips);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = GenerateUniqueFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "trips", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            trips.Image = FileName;
                        }
                    }
                    else
                    {
                        trips.Image = trip.Image;
                    }

                    _context.Entry(trip).CurrentValues.SetValues(trips);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Update successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripsExists(trips.TripsId))
                    {
                        ViewBag.error = "Error: Trip not found";
                        ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
                        return View(trips);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to update new Trip";
                        ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
                        return View(trips);
                    }
                }
            }
            ViewData["BusesId"] = new SelectList(_context.Buses, "BusesId", "BusNumber", trips.BusesId);
            return View(trips);
        }


       
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Trips == null)
            {
                return Problem("Entity set 'AppDbContext.Trips'  is null.");
            }
            try
            {
                var trips = await _context.Trips.FindAsync(id);
                if (trips != null)
                {
                    _context.Trips.Remove(trips);
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

        private bool TripsExists(int id)
        {
          return (_context.Trips?.Any(e => e.TripsId == id)).GetValueOrDefault();
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 8);

            var uniqueFileName = $"{timeStamp}_{randomString}{Path.GetExtension(fileName)}";

            return uniqueFileName;
        }
    }
}
