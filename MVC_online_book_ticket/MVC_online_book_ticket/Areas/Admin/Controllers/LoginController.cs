using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.IdentityModel.Tokens;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using MVC_online_book_ticket.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
	[Area("Admin")]
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
        public IActionResult Index(AuthenticationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var checkAccount = _context.Accounts.FirstOrDefault(a => a.Username == model.Username);
                if (checkAccount != null && !_hashPassword.VerifyPassword(model.Password, checkAccount.Password))
                {
                    if (checkAccount.Role == Role.Administrator)
                    {
                        var token = GenerateToken(checkAccount.Username, checkAccount.Role.ToString(), checkAccount.AccountsId.ToString());
                        HttpContext.Session.SetString("LoginToken", token);
						Console.WriteLine($"Login successful. Redirecting to Home/Index in Admin area.");
						return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        var token = GenerateToken(checkAccount.Username, checkAccount.Role.ToString(), checkAccount.AccountsId.ToString());
                        HttpContext.Session.SetString("LoginToken", token);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", " Invalid username or password");
                }

            }
            return View();
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
        public IActionResult ForgetPassword(string phone)
        {
            if (phone == null)
            {
                ModelState.AddModelError("", "Phone is required");
            }
            else
            {
                var checkAccount = _context.Accounts.FirstOrDefault(a => a.Phone == phone);
                if (checkAccount != null)
                {
                    return RedirectToAction("ChangePassword", "Login");
                }
                else
                {
                    ModelState.AddModelError("", " Invalid phone");
                }
            }
            return View();
        }

        public IActionResult ChangePassword()
        {
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
