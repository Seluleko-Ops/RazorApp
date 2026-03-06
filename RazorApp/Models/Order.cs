namespace RazorApp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserID { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
