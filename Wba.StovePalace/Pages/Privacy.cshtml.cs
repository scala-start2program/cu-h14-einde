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
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public PrivacyModel(ILogger<PrivacyModel> logger,
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
