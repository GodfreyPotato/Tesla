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

                }catch (Exception e)
                {
                    return Content($"Error {e.Message}");
                }
            }
            return View(category);
        }
    }
}
