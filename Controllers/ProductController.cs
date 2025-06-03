using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
using tesla.Models;

namespace tesla.Controllers
{
    public class ProductController : Controller
    {
        DatabaseHelper _helper;
        public ProductController()
        {
            _helper = new DatabaseHelper();
        }

        private List<Category> getCategories()
        {
            List<Category> categories = new List<Category>();

            DataTable dt = _helper.read("Select * from categories;");

            foreach (DataRow dr in dt.Rows)
            {
                categories.Add(new Category
                {
                    id = int.Parse(dr["id"].ToString()),
                    cat_name = dr["cat_name"].ToString(),
                    cat_description = dr["cat_description"].ToString()
                });
            }

            return categories;
        }

        [HttpGet]
        public IActionResult AddProduct()
        {


            ViewBag.Categories = getCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/productImages", uniqueFileName);

                    //Try natin implement yung anywhere makakupload ng image kung may time pa
                    //Baka lang magtaka si Sir di tayo add image from any path
                    //Di kasi nagpapakita kung di galing productImages
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    model.prod_img = uniqueFileName; // Save filename in DB
                }


                string query = $"INSERT INTO products (prod_name, prod_description, price, prod_img, cat_id) VALUES " +
                               $"('{model.prod_name}', '{model.prod_description}', {model.price}, '{model.prod_img}', {model.cat_id})";
                _helper.execute(query);

                return RedirectToAction("ProductList", "Admin");
            }


            ViewBag.Categories = getCategories();
            return View(model);
        }
        [HttpGet]
        public IActionResult ShowProducts()
        {
            List<Product> products = new List<Product>();
            DataTable dt = _helper.read("SELECT * FROM products");

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(new Product
                {
                    id = Convert.ToInt32(dr["id"]),
                    prod_name = dr["prod_name"].ToString(),
                    prod_description = dr["prod_description"].ToString(),
                    price = Convert.ToDecimal(dr["price"]),
                    prod_img = dr["prod_img"].ToString(),
                    cat_id = Convert.ToInt32(dr["cat_id"])
                });
            }
            //Will do Cart Tomorrow
            ViewBag.Categories = getCategories();
            return View("~/Views/Home/home.cshtml", products); //VSCode navigation
        }
    }
}
