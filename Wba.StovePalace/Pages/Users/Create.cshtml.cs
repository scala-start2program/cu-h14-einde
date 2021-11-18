using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Wba.StovePalace.Data;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        [BindProperty]
        public User User { get; set; }
        public Availability Availability { get; set; }

        public CreateModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                return RedirectToPage("../Stoves/Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Password = Hashing.HashPassword(User.Password);
            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
