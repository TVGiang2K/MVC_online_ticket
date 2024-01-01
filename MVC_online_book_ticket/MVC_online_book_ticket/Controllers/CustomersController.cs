using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using System.Drawing;
using X.PagedList;

namespace MVC_online_book_ticket.Controllers
{
	public class CustomersController : EmployeesBaseController
    {
		private readonly AppDbContext _context;

		public CustomersController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string name, int page = 1)
		{
			int limit = 5;
			IPagedList<Customers> customers = customers = await _context.Customers.Where(c => c.Name.Contains(name) || c.Phone.Contains(name)).OrderBy(b => b.CustomersId).ToPagedListAsync(page, limit);
			return View(customers);
		}

		public ActionResult Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CustomersId,Name,Phone,Birthday,Age,Gender")] Customers customer)
		{
			if (ModelState.IsValid)
			{
				try
				{
					_context.Add(customer);
					await _context.SaveChangesAsync();
					TempData["success"] = "Add successfully!";
					return RedirectToAction(nameof(Index));

				}
				catch
				{
					ViewBag.error = "Error: Unable to add new Customer";
					return View(customer);
				}
			}
			return View(customer);
		}


		public ActionResult Search()
		{
			return View();
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

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Buses == null)
			{
				return NotFound();
			}

			var customer = await _context.Customers.FindAsync(id);
			if (customer == null)
			{
				return NotFound();
			}
			return View(customer);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CustomersId,Name,Phone,Birthday,Age,Gender")] Customers customer)
		{
			if (id != customer.CustomersId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(customer);
					await _context.SaveChangesAsync();
					TempData["success"] = "Update successfully!";
					return RedirectToAction(nameof(Index));

				}
				catch
				{
					if (!CustomersExists(customer.CustomersId))
					{
						ViewBag.error = "Error: Customer not found";
						return View(customer);
					}
					else
					{
						ViewBag.error = "Error: Unable to update Customer";
						return View(customer);
					}
					
				}
			}
			return View(customer);
		}

		private bool CustomersExists(int id)
		{
			return (_context.Customers?.Any(e => e.CustomersId == id)).GetValueOrDefault();
		}
	}
}
