namespace RazorApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Cart";
    }
}
