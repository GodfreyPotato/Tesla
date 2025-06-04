using Microsoft.AspNetCore.Mvc;

namespace tesla.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult FAQ()
        {
            return View();
        }
    }
}
