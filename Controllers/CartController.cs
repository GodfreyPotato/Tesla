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
        //private static List<CartItem> cartItems = new List<CartItem>();
        //private int GetOrCreateCartId()
        //{
        //    // If user is logged in, use their user_id (adjust as needed)
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        // Example: parse user_id from claims or session
        //        // Replace with your actual logic to get user_id
        //        return int.Parse(User.Identity.Name);
        //    }
        //    // For guests, use a cookie to store a session cart ID
        //    if (Request.Cookies.ContainsKey("GuestCartId"))
        //    {
        //        return int.Parse(Request.Cookies["GuestCartId"]);
        //    }
        //    else
        //    {
        //        // Generate a new random cart ID for the guest
        //        var guestCartId = new Random().Next(100000, 999999);
        //        Response.Cookies.Append("GuestCartId", guestCartId.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
        //        {
        //            Expires = DateTimeOffset.UtcNow.AddDays(7)
        //        });
        //        return guestCartId;
        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddToCart(CartItem model)
        //{
        //    model.cart_id = GetOrCreateCartId(); // Set cart_id based on user or guest session
        //    string checkCartQuery = $"SELECT COUNT(*) FROM cart WHERE id = {model.cart_id}";
        //    var cartExists = Convert.ToInt32(_helper.scalar(checkCartQuery)) > 0;
        //    if (!cartExists)
        //    {
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            // Insert cart for logged-in user
        //            string insertCartQuery = $"INSERT INTO cart (id, user_id) VALUES ({model.cart_id}, {model.cart_id})";
        //            _helper.execute(insertCartQuery);
        //        }
        //        else
        //        {
        //            // Insert cart for guest (no user_id, but with sessionId)
        //            string insertCartQuery = $"INSERT INTO cart (id, sessionId) VALUES ({model.cart_id}, '{Request.Cookies["GuestCartId"]}')";
        //            _helper.execute(insertCartQuery);
        //        }
        //    }

        //    if (!ModelState.IsValid)//flipped idk ytf it doesn't proceed when its ModelState.IsValid
        //    {
        //        var existingItem = cartItems.FirstOrDefault(c => c.product_id == model.product_id && c.cart_id == model.cart_id);
        //        if (existingItem != null)
        //        {
        //            // Update quantity in memory
        //            existingItem.quantity += model.quantity;

        //            // Update quantity in database
        //            string updateQuery = $"UPDATE cartitems SET quantity = quantity + {model.quantity} WHERE product_id = {model.product_id} AND cart_id = {model.cart_id}";
        //            _helper.execute(updateQuery);
        //        }
        //        else
        //        {
        //            // Assign a new ID and date
        //            model.id = cartItems.Count > 0 ? cartItems.Max(c => c.id) + 1 : 1;
        //            model.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //            // Save to in-memory list
        //            cartItems.Add(model);

        //            // Non-parameterized insert query (string interpolation)
        //            string query = $"INSERT INTO cartitems (product_id, quantity, date, cart_id) " +
        //                        $"VALUES ({model.product_id}, {model.quantity}, '{model.date}', {model.cart_id})";
        //            _helper.execute(query);

        //            // Redirect to cart view after successful add
        //            // return Content("Item added to cart successfully.");//debugging purpose
        //            // OH MY GOD CASCADING PALA... hinde na ako marunong magprogram, taena sori guys T_T 
        //            // Two hours ako na stuck doon
        //            // Pero walang session pa guys
        //            //return RedirectToAction("ShowCart");
        //        }

        //        return RedirectToAction("ShowCart");
        //    }
        //    // return Content("Failed to add item to cart. Please check the input.");//debugging purpose
        //    // If model is invalid, show the cart with current items and validation errors
        //    return View("ShowCart", cartItems);
        //}
        //[HttpGet]
        //public IActionResult ShowCart()
        //{
        //    // Example: Fetch products from DB
        //    var products = new List<Product>();
        //    var dt = _helper.read("SELECT * FROM products");
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        products.Add(new Product
        //        {
        //            id = Convert.ToInt32(row["id"]),
        //            prod_name = row["prod_name"].ToString(),
        //            prod_img = row["prod_img"].ToString(),
        //            price = Convert.ToDecimal(row["price"])
        //        });
        //    }

        //    // Join cartItems with product info
        //    var viewModel = cartItems.Select(item =>
        //    {
        //        var product = products.FirstOrDefault(p => p.id == item.product_id);
        //        return new CartItemView
        //        {
        //            product_id = item.product_id,
        //            quantity = item.quantity,
        //            date = item.date,
        //            cart_id = item.cart_id,
        //            prod_name = product?.prod_name,
        //            prod_img = product?.prod_img,
        //            price = (product?.price * item.quantity) ?? 0
        //        };
        //    }).ToList();

        //    return View("ShowCart", viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult RemoveFromCart(int product_id, int cart_id)
        //{
        //    // Remove from in-memory list
        //    var item = cartItems.FirstOrDefault(c => c.product_id == product_id && c.cart_id == cart_id);
        //    if (item != null)
        //    {
        //        cartItems.Remove(item);
        //        // Remove from database
        //        string deleteQuery = $"DELETE FROM cartitems WHERE product_id = {product_id} AND cart_id = {cart_id}";
        //        _helper.execute(deleteQuery);
        //    }
        //    return RedirectToAction("ShowCart");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult UpdateQuantity(int product_id, int cart_id, int quantity)
        //{
        //    var item = cartItems.FirstOrDefault(c => c.product_id == product_id && c.cart_id == cart_id);
        //    if (item != null && quantity > 0)
        //    {
        //        item.quantity = quantity;
        //        string updateQuery = $"UPDATE cartitems SET quantity = {quantity} WHERE product_id = {product_id} AND cart_id = {cart_id}";
        //        _helper.execute(updateQuery);
        //    }
        //    return RedirectToAction("ShowCart");
        //}
        [HttpPost]
        public IActionResult AddToCart(int product_id)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionCart")))
            {
                var product = new List<CartItem>();

                product.Add(new CartItem {
                    product_id = product_id,
                    quantity = 1,
                    date = DateTime.Today.ToString(),
                    cart_id = 677167
                });
                HttpContext.Session.SetString("sessionCart", JsonSerializer.Serialize(product));
              
            }
            else
            {
                bool same = false;
                List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(HttpContext.Session.GetString("sessionCart"));

                foreach (CartItem ci in cartItems)
                {
                    if (ci.product_id == product_id)
                    {
                        ci.quantity++;
                        same = true;
                       
                        break;
                    }
                }

                if (same == false)
                {
                    cartItems.Add(new CartItem
                    {
                        product_id = product_id,
                        quantity = 1,
                        date = DateTime.Today.ToString(),
                        cart_id = 677167
                    });
                }
                HttpContext.Session.SetString("sessionCart", JsonSerializer.Serialize(cartItems));
              

            }

            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("role")))
            {
                //not logged in


            }
            else
            {

            }

            return Content("Added to Cart");
        }

    }
}