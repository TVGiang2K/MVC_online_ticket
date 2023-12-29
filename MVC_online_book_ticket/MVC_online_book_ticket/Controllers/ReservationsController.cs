using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC_online_book_ticket.Controllers
{
	public class ReservationsController : Controller
	{
		// GET: ReservationsController
		public ActionResult Index()
		{
			return View();
		}

		public IActionResult ReservationsDone()
		{
			return View();
		}
		
	}
}
