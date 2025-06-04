using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
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
        private static List<CartItem> cartItems = new List<CartItem>();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(CartItem model)
        {
            if (!ModelState.IsValid)//flipped idk ytf it doesn't proceed when its ModelState.IsValid
            {
                var existingItem = cartItems.FirstOrDefault(c => c.product_id == model.product_id && c.cart_id == model.cart_id);
                if (existingItem != null)
                {
                    // Update quantity in memory
                    existingItem.quantity += model.quantity;

                    // Update quantity in database
                    string updateQuery = $"UPDATE cartitems SET quantity = quantity + {model.quantity} WHERE product_id = {model.product_id} AND cart_id = {model.cart_id}";
                    _helper.execute(updateQuery);
                }
                else
                {
                    // Assign a new ID and date
                    model.id = cartItems.Count > 0 ? cartItems.Max(c => c.id) + 1 : 1;
                    model.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Save to in-memory list
                    cartItems.Add(model);

                    // Non-parameterized insert query (string interpolation)
                    string query = $"INSERT INTO cartitems (product_id, quantity, date, cart_id) " +
                                $"VALUES ({model.product_id}, {model.quantity}, '{model.date}', {model.cart_id})";
                    _helper.execute(query);

                    // Redirect to cart view after successful add
                    // return Content("Item added to cart successfully.");//debugging purpose
                    // OH MY GOD CASCADING PALA... hinde na ako marunong magprogram, taena sori guys T_T 
                    // Two hours ako na stuck doon
                    // Pero walang session pa guys
                    return RedirectToAction("ShowCart");
                }

                return RedirectToAction("ShowCart");
            }
            // return Content("Failed to add item to cart. Please check the input.");//debugging purpose
            // If model is invalid, show the cart with current items and validation errors
            return View("ShowCart", cartItems);
        }
        [HttpGet]
        public IActionResult ShowCart()
        {
            // Example: Fetch products from DB
            var products = new List<Product>();
            var dt = _helper.read("SELECT * FROM products");
            foreach (DataRow row in dt.Rows)
            {
                products.Add(new Product
                {
                    id = Convert.ToInt32(row["id"]),
                    prod_name = row["prod_name"].ToString(),
                    prod_img = row["prod_img"].ToString(),
                    price = Convert.ToDecimal(row["price"])
                });
            }

            // Join cartItems with product info
            var viewModel = cartItems.Select(item =>
            {
                var product = products.FirstOrDefault(p => p.id == item.product_id);
                return new CartItemView
                {
                    product_id = item.product_id,
                    quantity = item.quantity,
                    date = item.date,
                    cart_id = item.cart_id,
                    prod_name = product?.prod_name,
                    prod_img = product?.prod_img,
                    price = (product?.price * item.quantity) ?? 0
                };
            }).ToList();

            return View("ShowCart", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int product_id, int cart_id)
        {
            // Remove from in-memory list
            var item = cartItems.FirstOrDefault(c => c.product_id == product_id && c.cart_id == cart_id);
            if (item != null)
            {
                cartItems.Remove(item);
                // Remove from database
                string deleteQuery = $"DELETE FROM cartitems WHERE product_id = {product_id} AND cart_id = {cart_id}";
                _helper.execute(deleteQuery);
            }
            return RedirectToAction("ShowCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int product_id, int cart_id, int quantity)
        {
            var item = cartItems.FirstOrDefault(c => c.product_id == product_id && c.cart_id == cart_id);
            if (item != null && quantity > 0)
            {
                item.quantity = quantity;
                string updateQuery = $"UPDATE cartitems SET quantity = {quantity} WHERE product_id = {product_id} AND cart_id = {cart_id}";
                _helper.execute(updateQuery);
            }
            return RedirectToAction("ShowCart");
        }
    }
}