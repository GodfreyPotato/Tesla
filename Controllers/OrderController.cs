using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;

namespace tesla.Controllers
{
    public class OrderController : Controller
    {
        DatabaseHelper _helper;
        public OrderController() {
            _helper = new DatabaseHelper();
        }


        public IActionResult Index()
        {   

            return View();
        }


    }
}
