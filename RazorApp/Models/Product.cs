using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorApp.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string? ProductName { get; set; }
        public int Stock { get; set; }
        [Required(ErrorMessage = "Product price is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public List<OrderItem>? OrderItems { get; set; }
    }
}
