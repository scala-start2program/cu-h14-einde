using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Users
{
    public class LoginModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public LoginModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public Availability Availability { get; set; }

        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            return Page();
        }
        public IActionResult OnPost(string email, string password)
        {
            Availability = new Availability(_context, HttpContext);
            Email = email;
            User user = _context.User.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                if (!Hashing.ValidatePassword(password, user.Password))
                {
                    return RedirectToPage("./Login");
                }
                string IdCookie = Encoding.EncryptString(user.Id.ToString(), "P@sw00rd");
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(7));
                HttpContext.Response.Cookies.Append("UserId", IdCookie, cookieOptions);
                return RedirectToPage("../Stoves/Index");
            }
        }
    }
}
