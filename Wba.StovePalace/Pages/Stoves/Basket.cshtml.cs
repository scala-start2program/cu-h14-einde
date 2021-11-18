using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;
using Microsoft.EntityFrameworkCore;

namespace Wba.StovePalace.Pages.Stoves
{
    public class BasketModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        public Availability Availability { get; set; }
        public Stove Stove { get; set; }

        [BindProperty]
        public Basket Basket { get; set; }


        public BasketModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (id == null)
            {
                return NotFound();
            }
            Stove = _context.Stove
                .Include(s => s.Brand)
                .Include(s => s.Fuel)
                .FirstOrDefault(m => m.Id == id);
            if (Stove == null)
            {
                return NotFound();
            }
            return Page();
        }
        public IActionResult OnPost(int? id)
        {
            Availability = new Availability(_context, HttpContext);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(id == null)
            {
                return Page();
            }
            Basket.StoveId = (int) id;
            Basket.UserId = int.Parse(Availability.UserId);


            _context.Basket.Add(Basket);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
