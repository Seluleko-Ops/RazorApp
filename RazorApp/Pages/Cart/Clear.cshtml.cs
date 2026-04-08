using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.Linq;

namespace RazorApp.Pages.Cart
{
    public class ClearModel : PageModel
    {
        private readonly AppDbContext _context;

        public ClearModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnPost()
        {
            var userId = User.Identity?.Name ?? "Guest";

            var order = _context.Orders.FirstOrDefault(o => o.UserID == userId && o.Status == "Cart");
            if (order != null)
            {
                var items = _context.OrderItems.Where(i => i.OrderID == order.OrderID).ToList();

                // Restore stock for all items
                foreach (var item in items)
                {
                    var product = _context.Product.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                    }
                }

                _context.OrderItems.RemoveRange(items);
                _context.SaveChanges();
            }

            return RedirectToPage("/Cart/Index");
        }
    }
}