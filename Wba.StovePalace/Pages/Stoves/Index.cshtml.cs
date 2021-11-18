using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Stoves
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        private readonly int ItemsPerPage = 3;

        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Stove> Stoves { get;set; }
        public List<SelectListItem> AvailableBrands { get; set; }
        public List<SelectListItem> AvailableFuels { get; set; }
        public int? SelectedBrandId { get; set; }
        public int? SelectedFuelId { get; set; }
        public Pagination Pagination { get; private set; }
        public Availability Availability { get; set; }


        public void OnGet(int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext);
            PopulateCollections(null, null, pageIndex);
        }
        private void PopulateCollections(int? brandFilter, int? fuelFilter, int? pageIndex)
        {

            IQueryable<Stove> query = _context.Stove
                .Include(b => b.Brand)
                .Include(f => f.Fuel)
                .OrderBy(f => f.Brand.BrandName);
            if (brandFilter != null && fuelFilter == null)
            {
                query = query.Where(b => b.BrandId.Equals(brandFilter));
            }
            if (brandFilter == null && fuelFilter != null)
            {
                query = query.Where(b => b.FuelId.Equals(fuelFilter));
            }
            if (brandFilter != null && fuelFilter != null)
            {
                query = query.Where(b => b.BrandId.Equals(brandFilter) && b.FuelId.Equals(fuelFilter));
            }

            Pagination = new Pagination(query, pageIndex, ItemsPerPage);

            if (pageIndex > Pagination.LastPageIndex)
            {
                Pagination.PageIndex = 0;
                Pagination.FirstObjectIndex = 0;
            }

            Stoves = query
                        .Skip(Pagination.FirstObjectIndex)
                        .Take(ItemsPerPage)
                        .ToList();
            



            AvailableBrands = _context.Brand.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.BrandName
                }).ToList();
            AvailableBrands = AvailableBrands.OrderBy(b => b.Text).ToList();
            AvailableFuels = _context.Fuel.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FuelName
                }).ToList();
            AvailableFuels = AvailableFuels.OrderBy(b => b.Text).ToList();

            AvailableBrands.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle merken ---"
            });
            AvailableFuels.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle brandstoffen ---"
            });
        }
        public void OnPost(int? brandFilter, int? fuelFilter, int? pageIndex)
        {
            Availability = new Availability(_context, HttpContext);
            SelectedBrandId = brandFilter;
            SelectedFuelId = fuelFilter;
            PopulateCollections(brandFilter, fuelFilter, pageIndex);
        }


    }
}
