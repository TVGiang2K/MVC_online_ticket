﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
	
	public class FinancialController : AdminBaseController
	{
        public IActionResult Index()
        {
            return View();
        }
    }
}
