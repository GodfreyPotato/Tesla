using System.ComponentModel.DataAnnotations;

namespace tesla.Models
{
    public class Register
    {
        [Required]
        public string firstname { get; set; }
        [Required]
        public string middlename { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
