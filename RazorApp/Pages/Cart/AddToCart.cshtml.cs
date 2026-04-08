using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;

namespace RazorApp.Pages.Cart
{
    public class AddToCartModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddToCartModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int ProductID { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public IActionResult OnPost()
        {
            var product = _context.Product.FirstOrDefault(p => p.ProductID == ProductID);

            if (product == null)
                return RedirectToPage("/Index");

            if (Quantity <= 0 || Quantity > product.Stock)
                return RedirectToPage("/Index");

            var userId = User.Identity?.Name ?? "Guest";

            // 🛒 Get or create cart
            var order = _context.Orders
                .FirstOrDefault(o => o.UserID == userId && o.Status == "Cart");

            if (order == null)
            {
                order = new Models.Order
                {
                    UserID = userId,
                    Status = "Cart",
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }

            // Check if item exists
            var existingItem = _context.OrderItems
                .FirstOrDefault(i => i.OrderID == order.OrderID && i.ProductID == ProductID);

            if (existingItem != null)
            {
                existingItem.Quantity += Quantity;
            }
            else
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = ProductID,
                    Quantity = Quantity,
                    Price = product.Price
                });
            }

            // Reduce stock
            product.Stock -= Quantity;

            _context.SaveChanges();

            return RedirectToPage("/Cart/Cart");
        }
    }
}