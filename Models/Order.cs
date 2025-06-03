namespace tesla.Models
{
    public class Order
    {
        public int id { get; set; }
        public int cart_id { get; set; }
        public int user_id { get; set; }
        public string address { get; set; }
        public decimal totalAmount { get; set; }
        public string date { get; set; }
        public string status { get; set; }
    }
}
