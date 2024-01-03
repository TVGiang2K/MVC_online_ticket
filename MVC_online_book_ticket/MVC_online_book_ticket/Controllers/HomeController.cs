﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using System.Diagnostics;
using X.PagedList;

namespace MVC_online_book_ticket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _context;

		public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
			_context = context;
        }

		public IActionResult Index(string departure, string destination, DateTime? departureDate, DateTime? returnDate, int page = 1)
		{
			var limit = 5;
			IEnumerable<Trips> trips;
			if (string.IsNullOrEmpty(departure) && string.IsNullOrEmpty(destination) && !departureDate.HasValue && !returnDate.HasValue)
			{
				// Không có tìm kiếm, hiển thị toàn bộ dữ liệu chuyến xe
				trips = _context.Trips.OrderBy(x => x.TripsId).ToPagedList(page, limit);
			}
			else
			{
				// Có tìm kiếm, sử dụng phân trang
				trips = _context.Trips
					.Where(trip =>
						(string.IsNullOrEmpty(departure) || trip.DepartureLocation.Contains(departure)) &&
						(string.IsNullOrEmpty(destination) || trip.DestinationLocation.Contains(destination)) &&
						(!departureDate.HasValue || trip.DepartureDateTime >= departureDate) &&
						(!returnDate.HasValue || (trip.DestinationDateTime == null || trip.DestinationDateTime <= returnDate)))
					.OrderBy(x => x.TripsId).ToPagedList(page, limit);
			}

			// Truyền dữ liệu đến view
			ViewBag.Departure = departure;
			ViewBag.Destination = destination;
			ViewBag.DepartureDate = departureDate;
			ViewBag.ReturnDate = returnDate;

			return View(trips);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}