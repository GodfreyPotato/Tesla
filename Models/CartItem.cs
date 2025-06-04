using System.ComponentModel.DataAnnotations;
namespace tesla.Models
{
    public class CartItem
    {
        public int id { get; set; }
        public int product_id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int quantity { get; set; }//quanitity
        public string date { get; set; }
        public int cart_id { get; set; }

    }
}
