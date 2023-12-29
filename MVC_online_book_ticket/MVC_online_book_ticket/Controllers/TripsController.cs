using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC_online_book_ticket.Controllers
{
	public class TripsController : Controller
	{
		// GET: TripsController
		public ActionResult Index()
		{
			return View();
		}
	}
}
