using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{


    public class AdminBaseController : Controller, IActionFilter
	{
		[Authorize(Roles = "Administrator")]
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			string jwtToken = context.HttpContext.Session.GetString("LoginToken");
			if (string.IsNullOrEmpty(jwtToken))
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary(new { controller = "Login", Action = "Index" })
				);
			}
			base.OnActionExecuting(context);
		}
	}
}
