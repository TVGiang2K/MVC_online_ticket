using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using X.PagedList;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CustomersController : AdminBaseController
	{
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string name, int page = 1)
        {
           
                int limit = 5;
                var customers = await _context.Customers.OrderBy(c => c.CustomersId).ToPagedListAsync(page, limit);
                if (!String.IsNullOrEmpty(name))
                {
                    customers = await _context.Customers.Where(c => c.Name.Contains(name)).OrderBy(c => c.CustomersId).ToPagedListAsync(page, limit);
                }
                return View(customers);
        }

        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomersId == id);

            int limit = 5;
            var reservations = await _context.Reservations.Where(r => r.CustomersId == customer.CustomersId).OrderBy(a => a.ReservationDateTime).ToPagedListAsync(page, limit);
            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.customer = customer;

            return View(reservations);
        }
		public async Task<IActionResult> Delete(int id)
		{
			if (_context.Customers == null)
			{
				return Problem("Entity set 'AppDbContext.Customers'  is null.");
			}
			try
			{
				var customer = await _context.Customers.FindAsync(id);
				if (customer != null)
				{
					_context.Customers.Remove(customer);
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
	}
}
