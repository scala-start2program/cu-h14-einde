using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Users
{
    public class AccountModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        public AccountModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public User User { get; set; }
        public Availability Availability { get; set; }
        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            if(string.IsNullOrEmpty(Availability.UserId))
            {
                return NotFound();
            }
            int id = int.Parse(Availability.UserId);

            User = _context.User.FirstOrDefault(m => m.Id == id);
            if (User == null)
            {
                return NotFound();
            }
            return Page();

        }
    }
}
