using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;
using X.PagedList;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : AdminBaseController
    {
        private readonly AppDbContext _context;
        private readonly HashPassword _hashPassword;

        public AccountsController(AppDbContext context, HashPassword hashPassword)
        {
            _context = context;
            _hashPassword = hashPassword;
        }

        public async Task<IActionResult> Index(string name, int page = 1)
        {
            int limit = 5;
            var accounts = await _context.Accounts.OrderBy(a => a.AccountsId).ToPagedListAsync(page, limit);
            if (!String.IsNullOrEmpty(name))
            {
                accounts = await _context.Accounts.Where(a=> a.Name.Contains(name)).OrderBy(a => a.AccountsId).ToPagedListAsync(page, limit);
            }
            return View(accounts);
        }

        public async Task<IActionResult> Details(int? id, int page = 1)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountsId == id);

            int limit = 5;
            var reservations = await _context.Reservations.Where(r => r.AccountsId == account.AccountsId).OrderBy(a => a.ReservationDateTime).ToPagedListAsync(page, limit);
            if (account == null)
            {
                return NotFound();
            }

            ViewBag.account = account;

            return View(reservations);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountsId,Username,Password,Name,Age,Birthday,Phone,Email,Gender,Avatar,Qualification,Position,Role")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = GenerateUniqueFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "accounts", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            accounts.Avatar = FileName; 
                        }
                    }

                    var existingBus = await _context.Accounts.FirstOrDefaultAsync(b => b.Username == accounts.Username);

                    if (existingBus != null)
                    {
                        ViewBag.errN = "Error: Accounts with the Username already exists.";
                        return View(accounts);
                    }


                    accounts.Password = _hashPassword.SetPassword(accounts.Password);
                    accounts.Role = Role.Employees;
                    _context.Add(accounts);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Add successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch 
                {
                    if (accounts.Avatar == null)
                    {
                        ViewBag.errA = "Avatar is required";
                        return View(accounts);
                    }
                    else if (accounts.Password == null)
                    {
                        ViewBag.errP = "Password is required";
                        return View(accounts);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to add new Account";
                        return View(accounts);
                    }
                }
            }
            return View(accounts);
        }

        public async Task<IActionResult> Edit(int? id)
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
            return View(accounts);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("AccountsId,Username,Password,Name,Age,Birthday,Phone,Email,Gender,Avatar,Qualification,Position,Role")] Accounts accounts)
        {
            var acc = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountsId == id);

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = GenerateUniqueFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "accounts", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            accounts.Avatar = FileName;
                        }
                    }
                    else
                    {
                        accounts.Avatar = acc.Avatar;
                    }


                    if (!string.IsNullOrEmpty(accounts.Password)) {
                        accounts.Password = _hashPassword.SetPassword(accounts.Password);
                    }
                    else
                    {
                        accounts.Password = acc.Password;
                    }

                    accounts.Role = Role.Employees;

                    _context.Entry(acc).CurrentValues.SetValues(accounts);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Update successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.AccountsId))
                    {
                        ViewBag.error = "Error: Account not found";
                        return View(accounts);
                    }
                    else
                    {
                        ViewBag.error = "Error: Unable to update a Account";
                        return View(accounts);
                    }
                }
            }
            return View(accounts);
        }

        

        public async Task<IActionResult> Delete(int id)
        {
            var accounts = await _context.Accounts
                                .Where(a => a.AccountsId == id && a.Role != Role.Administrator)
                                .FirstOrDefaultAsync();
            try
            {
                if (accounts != null)
                {
                    _context.Accounts.Remove(accounts);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Delete successfully!";
                }
                else
                {
                    TempData["error"] = "Don't Delete Accounts Admin failed!";
                }
            }
            catch
            {
                TempData["error"] = "Delete failed!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AccountsExists(int id)
        {
          return (_context.Accounts?.Any(e => e.AccountsId == id)).GetValueOrDefault();
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 8);

            var uniqueFileName = $"{timeStamp}_{randomString}{Path.GetExtension(fileName)}";

            return uniqueFileName;
        }
    }
}
