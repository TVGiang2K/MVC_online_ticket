using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MVC_online_book_ticket.Controllers
{
	public class EmployeesBaseController : Controller, IActionFilter
	{
		[Authorize(Roles = "Employees")]
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			string jwtToken = context.HttpContext.Session.GetString("EmployeesLoginToken");
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
