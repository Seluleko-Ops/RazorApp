using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.Linq;

namespace RazorApp.Pages.Cart
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int OrderItemID { get; set; }

        public IActionResult OnPost()
        {
            var item = _context.OrderItems.FirstOrDefault(i => i.OrderItemID == OrderItemID);

            if (item != null)
            {
                // Restore stock since item is removed
                var product = _context.Product.FirstOrDefault(p => p.ProductID == item.ProductID);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                }

                _context.OrderItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToPage("/Cart/Cart");
        }
    }
}