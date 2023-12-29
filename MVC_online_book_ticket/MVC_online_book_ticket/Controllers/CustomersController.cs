using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC_online_book_ticket.Controllers
{
	public class CustomersController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		public ActionResult Search()
		{
			return View();
		}

	}
}
