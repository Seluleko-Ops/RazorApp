using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.Linq;

namespace RazorApp.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly AppDbContext _context;

        public Order Cart { get; set; }
        public decimal Total { get; set; }

        public CheckoutModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var userId = User.Identity?.Name ?? "Guest";

            Cart = _context.Orders
                .FirstOrDefault(o => o.UserID == userId && o.Status == "Cart");

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

                Total = Cart.OrderItems.Sum(i => i.Price * i.Quantity);
            }
        }

        public IActionResult OnPostPay()
        {
            var userId = User.Identity?.Name ?? "Guest";

            var order = _context.Orders
                .FirstOrDefault(o => o.UserID == userId && o.Status == "Cart");

            if (order == null)
                return RedirectToPage("/Cart/Cart");

            return RedirectToPage("/Payment/PaymentPortal", new { orderId = order.OrderID });
        }
    }
}