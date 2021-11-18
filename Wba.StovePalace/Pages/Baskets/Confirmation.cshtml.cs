using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Helpers;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Baskets
{
    public class ConfirmationModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public ConfirmationModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }
        public IList<Basket> Baskets { get; set; }
        public Availability Availability { get; set; }

        public ActionResult OnGet()
        {
            int userId;
            Availability = new Availability(_context, HttpContext);
            if (string.IsNullOrEmpty(Availability.UserId))
            {
                return RedirectToPage("../Users/Login");
            }
            else
            {
                userId = int.Parse(Availability.UserId);
            }

            IQueryable<Basket> query = _context.Basket
                .Include(b => b.Stove)
                .Include(b => b.Stove.Brand)
                .Include(f => f.Stove.Fuel)
                .Include(b => b.User);
            query = query.Where(b => b.UserId.Equals(userId));
            Baskets = query.ToList();
            if (Baskets == null)
            {
                return NotFound();
            }
            return Page();
        }

        public void OnPost()
        {
            int userId;
            Availability = new Availability(_context, HttpContext);
            if (string.IsNullOrEmpty(Availability.UserId))
            {
               Response.Redirect("../Users/Login");
            }
            userId = int.Parse(Availability.UserId);

            Order order = new Order();
            order.UserId = userId;
            order.DateTimeStamp = DateTime.Now;

            _context.Order.Add(order);
            _context.SaveChanges();

            int orderId = order.Id;

            IQueryable<Basket> query = _context.Basket
                .Include(b => b.Stove)
                .Include(b => b.User);
            query = query.Where(b => b.UserId.Equals(userId));
            Baskets = query.ToList();

            OrderDetail orderDetail;
            foreach (Basket basket in Baskets)
            {
                orderDetail = new OrderDetail();
                orderDetail.OrderId = orderId;
                orderDetail.StoveId = basket.StoveId;
                orderDetail.Count = basket.Count;
                orderDetail.SalesPrice = basket.Stove.SalesPrice;

                _context.OrderDetail.Add(orderDetail);
                _context.SaveChanges();
            }

            foreach(Basket basket in Baskets)
            {
                _context.Basket.Remove(basket);
                _context.SaveChanges();
            }
            Response.Redirect("../Stoves/Index");

        }
    }
}
