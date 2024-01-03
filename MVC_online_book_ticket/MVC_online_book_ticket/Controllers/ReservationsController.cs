using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;

namespace MVC_online_book_ticket.Controllers
{
	public class ReservationsController : Controller
	{
		private readonly AppDbContext _context;
		public ReservationsController(AppDbContext context)
		{
			_context = context;
		}
		// GET: ReservationsController
		public async Task<IActionResult> Index(int id)
		{
			if (id == null || _context.Trips == null)
			{
				return NotFound();
			}
			var trip = await _context.Trips.FindAsync(id);
			if (trip == null)
				NotFound();

			return View(trip);
		}
		public async Task<IActionResult> AddQuantityGuest(string name, int id)
		{
			var customers = await _context.Customers.Where(c => c.Name.Contains(name) || c.Phone.Contains(name)).OrderBy(b => b.CustomersId).ToListAsync();
			ViewBag.idTrip = id;
			return View(customers);
		}

		public async Task<IActionResult> ReservationsConfirm(int? idCus, int id)
		{
			ViewBag.IdTrip = id;
			if (idCus.HasValue)
			{
				var cus = await _context.Customers.FirstOrDefaultAsync(c => c.CustomersId == idCus);
				var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripsId == id);

				var reservation = new Reservations
                {
                    Customers = cus,
                    Trips = trip,
					Vat = 10,
                    ReservationDateTime = DateTime.Now,
					AccountsId="nv001",
                    // Các giá trị khác có thể cần được thiết lập tùy thuộc vào logic của bạn
                };
				var discountPercen = await _context.Financials.Where(x => x.FinancialFrom <= reservation.Customers.Age && x.FinancialTo > reservation.Customers.Age && x.Category == Category.Discount).Select(x => x.Percentage).FirstOrDefaultAsync();

                if (reservation != null)
				{
					double discountedPercentage = 0;
					if (discountPercen != null)
					{
						var discountAmount = (discountPercen / 100) * reservation.Trips.BasePrice;
						var discountedAmount = reservation.Trips.BasePrice - discountAmount;
						discountedPercentage = discountedAmount + (reservation.Vat / 100 * discountedAmount);
					}
					else
					{
						discountedPercentage = reservation.Trips.BasePrice + (reservation.Vat / 100 * reservation.Trips.BasePrice);
					}
					ViewBag.CustomerName = reservation.Customers.Name;
					ViewBag.CustomerPhone = reservation.Customers.Phone;
					ViewBag.CustomerEmail = reservation.Customers.Email;
					ViewBag.TripDepartureLocation = reservation.Trips.DepartureLocation;
					ViewBag.TripDestinationLocation = reservation.Trips.DestinationLocation;
					ViewBag.TripDepartureDateTime = reservation.Trips.DepartureDateTime;
					ViewBag.TripDestinationDateTime = reservation.Trips.DestinationDateTime;
					ViewBag.VAT = reservation.Vat;
					ViewBag.TotlPrice = discountedPercentage;
					ViewBag.Financials = discountPercen;
					return View(reservation);
				}
			}
			return NotFound();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ReservationsConfirm(Reservations reservations)
		{
            await _context.Reservations.AddAsync(reservations);
			await _context.SaveChangesAsync();

            return  RedirectToAction(nameof(ReservationsDone)); ;
		}

		public IActionResult ReservationsDone()
		{
			return View();
		}

	}
}
