using System.ComponentModel.DataAnnotations;

namespace RazorApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public List<OrderItem>? OrderItems { get; set; }
    }
}
