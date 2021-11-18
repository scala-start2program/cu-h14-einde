using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Models;
using Wba.StovePalace.Helpers;

namespace Wba.StovePalace.Pages.Fuels
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Fuel> Fuels { get;set; }
        public Pagination Pagination { get; private set; }
        public Availability Availability { get; set; }

        public void OnGet(int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext);
            if (!Availability.IsAdmin)
            {
                Response.Redirect("../Stoves/Index");
            }

            IQueryable<Fuel> query = _context.Fuel
                .OrderBy(f => f.FuelName);
            Pagination = new Pagination(query, pageIndex, 4);

            if (pageIndex > Pagination.LastPageIndex)
            {
                Pagination.PageIndex = 0;
                Pagination.FirstObjectIndex = 0;
            }

            Fuels = query
                    .Skip(Pagination.FirstObjectIndex)
                    .Take(4).ToList();

        }
    }
}
