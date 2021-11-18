using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wba.StovePalace.Helpers;

namespace Wba.StovePalace.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public IndexModel(ILogger<IndexModel> logger,
            Wba.StovePalace.Data.StoveContext context)
        {
            _logger = logger;
            _context = context;
        }
        public Availability Availability { get; set; }

        public void OnGet()
        {
            Availability = new Availability(_context, HttpContext);
        }
    }
}
