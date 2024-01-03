using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using MVC_online_book_ticket.Models.DTOs;

namespace MVC_online_book_ticket.Controllers
{
    public class ReservationsController : EmployeesBaseController
    {
        private readonly AppDbContext _context;
        public ReservationsController(AppDbContext context)
        {
            _context = context;
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
                    ViewBag.VAT = reservation.Vat;
                    ViewBag.TotlPrice = discountedPercentage;
                    ViewBag.Financials = discountPercen;
                    ViewBag.ReservationInfo = reservation;
                    return View(reservation);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReservationsConfirm([Bind("TripsId,CustomersId,AccountsId,Vat,TotalPrice,ReservationDateTime,ActiveReservation")]ReservationsConfirmDTO reservations)
        {
            
            try
            {
                var res = new Reservations()
                {
                    TripsId = reservations.TripsId,
                    CustomersId = reservations.CustomersId,
                    AccountsId = reservations.AccountsId,
                    ActiveReservation = 1,
                    Vat = reservations.Vat,
                    TotalPrice = reservations.TotalPrice,
                    ReservationDateTime = DateTime.Now,
                };

                await _context.Reservations.AddAsync(res);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReservationsDone));
            } catch
            {
                return NotFound();
            }
        }

        public IActionResult ReservationsDone()
        {
            return View();
        }

    }
}
