using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
using tesla.Models;

namespace tesla.Controllers
{
    public class AdminController : Controller
    {
        DatabaseHelper _helper;
        public AdminController()
        {
            _helper = new DatabaseHelper();
        }
        public IActionResult ProductList()
        {
            ViewBag.img = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/productImages");
            return View(getProducts());
        }

        private List<Product> getProducts()
        {
            List<Product> products = new List<Product>();

            DataTable dt = _helper.read("Select * from products;");

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(new Product
                {
                    id = int.Parse(dr["id"].ToString()),
                    prod_name = dr["prod_name"].ToString(),
                    prod_description = dr["prod_description"].ToString(),
                    prod_img = dr["prod_img"].ToString(),
                    price = decimal.Parse(dr["price"].ToString()),
                    cat_id = int.Parse(dr["cat_id"].ToString())
                });
            }

            return products;
        }
    }
}
