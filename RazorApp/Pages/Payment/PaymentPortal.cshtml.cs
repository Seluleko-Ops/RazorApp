using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorApp.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace RazorApp.Pages.Payment
{
    public class PaymentPortalModel : PageModel
    {
        private readonly AppDbContext _context;

        public int OrderId { get; set; }
        public decimal Total { get; set; }

        public PaymentPortalModel(AppDbContext context)
        {
            _context = context;
        }

        // Display the payment portal
        public IActionResult OnGet(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null) return RedirectToPage("/Cart/Cart");

            OrderId = order.OrderID;
            Total = order.OrderItems?.Sum(i => i.Price * i.Quantity) ?? 0;
            return Page();
        }

        // Handle form submission
        public IActionResult OnPost(int orderId, string BankAccount, string PaymentStatus)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
            if (order == null) return RedirectToPage("/Cart/Cart");

            // Validate bank account: must be 5-9 digits
            bool isValidBankAccount = Regex.IsMatch(BankAccount ?? "", @"^\d{5,9}$");

            if (!isValidBankAccount || PaymentStatus == "Fail")
            {
                // Payment failed
                return RedirectToPage("/Cart/Cancel");
            }

            // Payment successful
            order.Status = "Paid";

            // Reduce stock for each item
            var items = _context.OrderItems.Where(i => i.OrderID == orderId).ToList();
            foreach (var item in items)
            {
                var product = _context.Product.FirstOrDefault(p => p.ProductID == item.ProductID);
                if (product != null)
                {
                    product.Stock -= item.Quantity;
                }
            }

            _context.SaveChanges();

            // Redirect to success page
            return RedirectToPage("/Cart/Success");
        }
    }
}