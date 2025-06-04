using System.Data;
using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using tesla.Models;

namespace tesla.Controllers
{
    public class CategoryController : Controller
    {
        DatabaseHelper helper;
        public CategoryController()
        {
            helper = new DatabaseHelper();
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    helper.execute($"insert into categories(cat_name, cat_description) values ('{category.cat_name}', '{category.cat_description}')");

                }
                catch (Exception e)
                {
                    return Content($"Error {e.Message}");
                }
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult ShowCategory()
        {
            var categories = new List<Category>();
            var dt = helper.read("SELECT * FROM categories");
            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new Category
                {
                    id = Convert.ToInt32(row["id"]),
                    cat_name = row["cat_name"].ToString(),
                    cat_description = row["cat_description"].ToString()
                });
            }
            return View(categories);
        }
        [HttpGet]
        public IActionResult ByCategory(int categoryId)
        {
            var products = new List<Product>();
            var dt = helper.read($"SELECT * FROM products WHERE category_id = {categoryId}");
            foreach (DataRow row in dt.Rows)
            {
                products.Add(new Product
                {
                    id = Convert.ToInt32(row["id"]),
                    prod_name = row["prod_name"].ToString(),
                    prod_img = row["prod_img"].ToString(),
                    price = Convert.ToDecimal(row["price"]),
                });
            }
            ViewBag.CategoryId = categoryId;
            return View(products);
        }
    }
}
