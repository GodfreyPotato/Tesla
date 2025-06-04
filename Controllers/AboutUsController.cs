using Microsoft.AspNetCore.Mvc;

namespace tesla.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
