using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.Linq;

namespace RazorApp.Pages.Cart
{
    public class CartModel : PageModel
    {
        private readonly AppDbContext _context;

        public Order Cart { get; set; }

        public CartModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var userId = User.Identity?.Name ?? "Guest";

            Cart = _context.Orders
                .Where(o => o.UserID == userId && o.Status == "Cart")
                .FirstOrDefault();

            if (Cart != null)
            {
                Cart.OrderItems = _context.OrderItems
                    .Where(i => i.OrderID == Cart.OrderID)
                    .ToList();

                foreach (var item in Cart.OrderItems)
                {
                    item.Product = _context.Product
                        .FirstOrDefault(p => p.ProductID == item.ProductID);
                }
            }
        }
    }
}