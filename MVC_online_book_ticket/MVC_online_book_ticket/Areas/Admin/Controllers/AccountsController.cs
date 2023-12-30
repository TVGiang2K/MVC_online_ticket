using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_online_book_ticket.Common;
using MVC_online_book_ticket.Data;
using MVC_online_book_ticket.Models;

namespace MVC_online_book_ticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HashPassword _hashPassword;

        public AccountsController(AppDbContext context, HashPassword hashPassword)
        {
            _context = context;
            _hashPassword = hashPassword;
        }

        public async Task<IActionResult> Index()
        {
              return _context.Accounts != null ? 
                          View(await _context.Accounts.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Accounts'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountsId == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountsId,Username,Password,Name,Age,Phone,Gender,Avatar,Qualification,Position,Role,Create_date,Update_date,Delete_date,Status")] Accounts accounts)
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
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            accounts.Avatar = FileName; 
                        }
                    }

                    if(accounts.Avatar == null)
                    {
                        ViewBag.errorA = "Avatar is required";
                        return View(accounts);
                    }
                    accounts.Password = _hashPassword.SetPassword(accounts.Password);
                    _context.Add(accounts);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Accounts", new { area = "Admin" });
                }
                catch
                {
                    ViewBag.error = "Error: Unable to add new Account";
                    return View(accounts);
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
        public async Task<IActionResult> Edit(int id, [Bind("AccountsId,Username,Password,Name,Age,Phone,Gender,Avatar,Qualification,Position,Role,Create_date,Update_date,Delete_date,Status")] Accounts accounts)
        {
            if (id != accounts.AccountsId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0)
                    {
                        var file = files[0];
                        var FileName = GenerateUniqueFileName(file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            accounts.Avatar = FileName;
                        }
                    }
                    else
                    {
                        var acc = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountsId == id);
                        accounts.Avatar = acc.Avatar;
                    }

                    _context.Update(accounts);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Accounts", new { area = "Admin" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.AccountsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(accounts);
        }

        

        public async Task<IActionResult> Delete(int id)
        {

            
            var accounts = await _context.Accounts.FindAsync(id);

            if (accounts != null)
            {
                _context.Accounts.Remove(accounts);
            }


            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Delete successfully!";
            }
            catch
            {
                TempData["error"] = "Delete failed!";
            }
            return RedirectToAction("Index", "Accounts", new { area = "Admin" });
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
