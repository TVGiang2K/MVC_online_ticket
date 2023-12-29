using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{

	[Area("Admin")]
	public class HomeController : AdminBaseController
	{
		public IActionResult Index()
        {
            return View();
        }
    }
}
