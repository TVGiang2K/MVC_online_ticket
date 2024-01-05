using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using MVC_online_book_ticket.Models.DTOs;
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
            if (customer == null)
            {
                return NotFound();
            }
            var reservations = await _context.Reservations.Where(r => r.CustomersId == customer.CustomersId).OrderBy(a => a.ReservationDateTime).ToPagedListAsync(page, limit);

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

        public async Task<IActionResult> ManagerBooking(int? id, int? idRes)
        {
            if (id.HasValue && idRes.HasValue)
            {
                var cus = await _context.Customers.FirstOrDefaultAsync(c => c.CustomersId == id);
                var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationsId == idRes && r.CustomersId == id);

                if (reservation != null)
                {
                    var trip = await _context.Trips.FirstOrDefaultAsync(t => t.TripsId == reservation.TripsId);
                    var discountPercen = await _context.Financials.Where(x => x.FinancialFrom <= reservation.Customers.Age && x.FinancialTo > reservation.Customers.Age && x.Category == Category.Discount).Select(x => x.Percentage).FirstOrDefaultAsync();

                    double discountedPercentage = 0;
                    if (discountPercen != null)
                    {
                        var discountAmount = (discountPercen / 100) * trip.BasePrice;
                        var discountedAmount = trip.BasePrice - discountAmount;
                        discountedPercentage = discountedAmount + (reservation.Vat / 100 * discountedAmount);
                    }
                    else
                    {
                        discountedPercentage = trip.BasePrice + (reservation.Vat / 100 * trip.BasePrice);
                    }
                    var discountRefund = await _context.Financials.Where(x => x.Category == Category.Reimbursement).ToListAsync();
                    var departureDateTime = _context.Trips.Where(x => x.TripsId == reservation.TripsId).Select(x => x.DepartureDateTime).FirstOrDefault();
                    // xxử lí lấy ra số tiền refund
                    ViewBag.idRes = idRes;
                    ViewBag.discountPercen = discountPercen;
                    ViewBag.discountRefund = discountRefund;
                    return View(reservation);
                }
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagerBooking(ReservationsCancleDTO reservation)
        {
            var res = await _context.Reservations.FirstOrDefaultAsync(x => x.ReservationsId == reservation.ReservationsId);
            var departureDateTime = _context.Trips.Where(x => x.TripsId == reservation.TripsId).Select(x => x.DepartureDateTime).FirstOrDefault();

            if (reservation == null || res == null)
            {
                return NotFound();
            }
            try
            {
                    res.TripsId = reservation.TripsId;
                    res.CustomersId = reservation.CustomersId;
                    res.AccountsId = reservation.AccountsId;
                    res.Vat = 10;
                    res.ActiveReservation = 0;
                    res.CancellationDateTime = DateTime.Now;
                    res.TotalPrice = reservation.TotalPrice;
                res.RefundAmount = CalculateTicketCancellationPercentage(reservation.TotalPrice, departureDateTime, DateTime.Now);
                res.ReservationDateTime = DateTime.Now;
             

                _context.Reservations.Update(res);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ReservationsDone));
            }
            catch
            {
                return NotFound();
            }
        }
        public IActionResult ReservationsDone()
        {
            return View();
        }
        private double CalculateTicketCancellationPercentage(double totalPrice, DateTime departureDateTime, DateTime cancellationDateTime)
        {
            var days = (departureDateTime.Date - cancellationDateTime.Date).TotalDays;
            var dayLimit = _context.Financials.Where(x => x.FinancialFrom == days && x.FinancialTo > days && x.Category == Category.Reimbursement || x.FinancialFrom == days && x.FinancialTo == 0 && x.Category == Category.Reimbursement).Select(x => x.Percentage).FirstOrDefault();

            return (totalPrice - (totalPrice * dayLimit / 100));

        }
        private bool CustomersExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomersId == id)).GetValueOrDefault();
        }
    }
}
