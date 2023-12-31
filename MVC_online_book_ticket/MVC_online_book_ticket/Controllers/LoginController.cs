using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using MVC_online_book_ticket.Models.DTOs;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MVC_online_book_ticket.Controllers
{
	public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HashPassword _hashPassword;
        public LoginController(AppDbContext context, HashPassword hashPassword, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _hashPassword = hashPassword;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("Username,Password")] AuthenticationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
			}
			else
			{
                var checkAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == model.Username);
                if (checkAccount != null && _hashPassword.VerifyPassword(model.Password, checkAccount.Password) && checkAccount.Role == Role.Employees)
                {
                    var token = GenerateToken(checkAccount.Username, checkAccount.Role.ToString(), checkAccount.AccountsId.ToString());
                    HttpContext.Session.SetString("EmployeesLoginToken", token);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
					ViewBag.error = "Error: Invalid username or password";
					return View(model);
				}

            }
        }


        private string GenerateToken(string username, string role, string id)
        {

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.NameIdentifier, username),
                        new Claim(ClaimTypes.Role, role),
                        new Claim("AccountsId", id),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword([Bind("Email")] ForgetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
				return View(model);
            }
			else
            {
			
				var checkAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == model.Email && a.Role != Role.Administrator);
                if (checkAccount != null)
                {
					return RedirectToAction("ChangePassword", new { id = checkAccount.AccountsId });
				}
                else
                {
					ViewBag.error = "Error: Your Email is not in the system";
					return View(model);
				}
            }
        }

        public async Task<IActionResult> ChangePassword(int? id)
        {
			if (id == null || _context.Accounts == null)
			{
				return NotFound();
			}
			var accounts = await _context.Accounts.FindAsync(id);
			if (accounts == null)
			{
				return NotFound();
			}
			ViewData["Id"] = id;
			return View();
        }

		[HttpPost]
		public async Task<IActionResult> ChangePassword(int id, [Bind("AccountsId,NewPassword,ConfirmPassword")] ChangePasswordDTO model)
		{

			if (id != model.AccountsId)
			{
				ViewBag.error = "Error: Account not found";
				return View();
			}
			

			if (ModelState.IsValid)
			{
				try
				{
					var acc = await _context.Accounts.FindAsync(id);

					if (acc == null)
					{
						ViewBag.error = "Error: Account not found";
						ViewData["Id"] = id;
						return View();
					}

					if (!string.Equals(model.NewPassword, model.ConfirmPassword, StringComparison.Ordinal))
                    {
						ViewBag.error = "Error: passwords do not match";
						ViewData["Id"] = id;
						return View();
					}

					acc.Password = _hashPassword.SetPassword(model.ConfirmPassword);

					_context.Update(acc);
					await _context.SaveChangesAsync();
					TempData["success"] = "Change Password successfully!";
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException)
				{
					ViewBag.error = "Change Password failed!";
					return RedirectToAction(nameof(Index));
				}
			}
			ViewData["Id"] = id;
			return View();
		}



		[HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoginToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
