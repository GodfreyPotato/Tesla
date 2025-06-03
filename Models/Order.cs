namespace tesla.Models
{
    public class Order
    {
        public int id { get; set; }
        public int cart_id { get; set; }
        public int user_id { get; set; }
        public string address { get; set; }
        public decimal totalAmount { get; set; }
        public string date { get; set; } //changed the dataType to string, pero sa db naka DateTime. Baka need kasi ifilter order through date
        public string status { get; set; }
    }
}
