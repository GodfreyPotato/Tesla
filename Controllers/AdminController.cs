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

        public IActionResult OrderList(string? status) {
            var orders = getOrders();

            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(order => order.status == status).ToList();
            }
            return View(orders);
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
                    date = DateTime.Parse(dr["date"].ToString()).ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture),
                    status = dr["status"].ToString()
                });
            }
            return orders;
        }


        public IActionResult OrderDetails(int id)
        {
            return View(getOrderDetails(id));
        }

        public OrderDetail getOrderDetails(int orderId) {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.items = new List<OrderItem>();

            DataTable dt = _helper.read(@"
            SELECT 
                orders.id AS 'OrderId',
                orders.date AS 'OrderDate',
                orders.totalAmount,
                products.prod_name AS 'ProductName',
                cartitems.quantity,
                products.price,
                (products.price * cartitems.quantity) AS 'Subtotal'
            FROM orders
            JOIN cartitems ON orders.cart_id = cartitems.cart_id
            JOIN products ON cartitems.product_id = products.id
            WHERE orders.id = " + orderId);

            foreach(DataRow row in dt.Rows)
            {
                orderDetail.id = Convert.ToInt32(row["OrderId"]);
                orderDetail.date = Convert.ToDateTime(row["OrderDate"]).ToString("MMMM dd, yyyy");
                orderDetail.totalAmount = Convert.ToDecimal(row["totalAmount"]);

                orderDetail.items.Add(new OrderItem
                {
                    productName = row["ProductName"].ToString(),
                    quantity = Convert.ToInt32(row["quantity"]),
                    price = Convert.ToDecimal(row["price"]),
                });
            }
            return orderDetail;
        }
    }
}
