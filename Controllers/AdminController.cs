using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
using System.Globalization;
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

        public IActionResult OrderList() {
            return View(getOrders());
        }
        public List<Order> getOrders() {
            List<Order> orders = new List<Order>();

            DataTable dt = _helper.read("SELECT * FROM orders");

            foreach (DataRow dr in dt.Rows) {
                orders.Add(new Order
                {
                    id = int.Parse(dr["id"].ToString()),
                    cart_id = int.Parse(dr["cart_id"].ToString()),
                    user_id = int.Parse(dr["user_id"].ToString()),
                    address = dr["address"].ToString(),
                    totalAmount = decimal.Parse(dr["totalAmount"].ToString()),
                    date = DateTime.Parse(dr["date"].ToString()).ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture), //Bale sa adding or ng order dapat naayus na or smth
                    //ToModify na lang to
                    status = dr["status"].ToString()
                });
            }
            return orders;
        }
    }
}
