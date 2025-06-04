namespace tesla.Models
{
    public class CartItemView
    {
        //Cart items
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string date { get; set; }
        public int cart_id { get; set; }

        //Product details
        public string prod_name { get; set; }
        public string? prod_img { get; set; }
        public decimal price { get; set; }
    }    
}
