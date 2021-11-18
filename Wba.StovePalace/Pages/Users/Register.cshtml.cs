using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wba.StovePalace.Models;
using Wba.StovePalace.Helpers;
using Microsoft.AspNetCore.Http;

namespace Wba.StovePalace.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public RegisterModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            return Page();
        }

        [BindProperty]
        public User User { get; set; }
        public Availability Availability { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Availability = new Availability(_context, HttpContext);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Password = Hashing.HashPassword(User.Password);
            _context.User.Add(User);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Page();
            }

            string IdCookie = Encoding.EncryptString(User.Id.ToString(), "P@sw00rd");
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddDays(365));
            HttpContext.Response.Cookies.Append("UserId", IdCookie, cookieOptions);
            return RedirectToPage("../Stoves/Index");

        }
    }
}
