using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
using System.Text.Json;
using tesla.Models;

namespace Tesla.Controllers
{
    public class CartController : Controller
    {
        DatabaseHelper _helper;
        public CartController()
        {
            _helper = new DatabaseHelper();
        }

        [HttpPost]
        public IActionResult AddToCart(int product_id)
        {

            //not logged in
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("role")))
            {
                DataTable dt = _helper.read("select * from cart where user_id IS null");

                if (dt.Rows.Count == 0)
                {
                    _helper.execute("insert into cart (user_id) values (null)");

                }
                DataTable dtble = _helper.read("select * from cart where user_id IS null");
                string id = dtble.Rows[0]["id"].ToString();

                _helper.execute($"insert into cartItems (product_id, quantity, date, cart_id) values ({product_id}, 1, '{DateTime.Today:yyyy-MM-dd}', {id}) ON DUPLICATE KEY UPDATE quantity = quantity + 1");
            }
            else if(HttpContext.Session.GetString("role")=="customer")
            {
                DataTable dt = _helper.read($"select * from cart where user_id = {HttpContext.Session.GetString("id")}");

                if (dt.Rows.Count == 0)
                {
                    _helper.execute($"insert into cart (user_id) values ({HttpContext.Session.GetString("id")})");

                }
                else
                {
                    _helper.execute($"insert into cartItems (product_id, quantity, date, cart_id) values ({product_id}, 1, '{DateTime.Today:yyyy-MM-dd}', {dt.Rows[0]["id"].ToString()}) ON DUPLICATE KEY UPDATE quantity = quantity + 1");
                }

            }
            else
            {
                return Unauthorized();
            }

                return RedirectToAction("ShowCart");
        }

        public IActionResult ShowCart()
        {
            if (HttpContext.Session.GetString("role") == "customer")
            {
                string user_id = HttpContext.Session.GetString("id");
               DataTable query = _helper.read($"select *, cartItems.id as id from cart join cartItems on cart.id = cartItems.cart_id join products on cartItems.product_id = products.id where cart.user_id = {user_id}");
                List<CartItemView> cartItems = new List<CartItemView>();

                foreach(DataRow dr in query.Rows)
                {
                    cartItems.Add(new CartItemView {
                        id = int.Parse(dr["id"].ToString()),
                        cart_id = int.Parse(dr["cart_id"].ToString()),
                        date = dr["date"].ToString(),
                        quantity = int.Parse(dr["quantity"].ToString()),
                        product_id = int.Parse(dr["product_id"].ToString()),
                        prod_img = dr["prod_img"].ToString(),
                        prod_name = dr["prod_name"].ToString(),
                        price = decimal.Parse(dr["price"].ToString())

                        
                    });
                }
                return View(cartItems);
            }
            else
             {
                string user_id = HttpContext.Session.GetString("id");
                DataTable query = _helper.read($"select *, cartItems.id as id from cart join cartItems on cart.id = cartItems.cart_id join products on cartItems.product_id = products.id where cart.user_id IS null");
                List<CartItemView> cartItems = new List<CartItemView>();

                foreach (DataRow dr in query.Rows)
                {
                    cartItems.Add(new CartItemView
                    { id = int.Parse(dr["id"].ToString()),
                        cart_id = int.Parse(dr["cart_id"].ToString()),
                        date = dr["date"].ToString(),
                        quantity = int.Parse(dr["quantity"].ToString()),
                        product_id = int.Parse(dr["product_id"].ToString()),
                        prod_img = dr["prod_img"].ToString(),
                        prod_name = dr["prod_name"].ToString(),
                        price = decimal.Parse(dr["price"].ToString())


                    });
                }
                return View(cartItems);
            }
               
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int quantity, int id)
        {
            _helper.execute($"update cartItems set quantity = {quantity} where id = {id}");
            return RedirectToAction("ShowCart");
        }

    }
}