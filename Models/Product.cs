using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tesla.Models
{
    public class Product
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string prod_name { get; set; }

        [Required]
        public string prod_description { get; set; }

        [Required]
        public decimal price { get; set; }
       
        public string? prod_img { get; set; }

        [Required]
        public int cat_id { get; set; }

        [NotMapped] 
        public IFormFile? ImageFile { get; set; }
    }
}
