namespace tesla.Models
{
    public class OrderDetail
    {
        public int id { get; set; }
        public string date { get; set; }
        public decimal totalAmount { get; set; }
        public List<OrderItem> items { get; set; }
    }

    public class OrderItem
    {
        public string productName { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal subtotal => quantity * price;
    }
}
