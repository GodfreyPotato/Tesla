using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using tesla.Models;

namespace tesla.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    DatabaseHelper helper;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        helper = new DatabaseHelper();
    }

    public IActionResult Index()
    {
        DataTable pr = helper.read($"select * from products where prod_img != '' limit 6");
        List<object> products = new List<object>();
        foreach (DataRow dr in pr.Rows)
        {
            products.Add(new {
                id = dr["id"].ToString(),
                prod_name = dr["prod_name"].ToString(),
                prod_description = dr["prod_description"].ToString(),
                price = dr["price"].ToString(),
                prod_img = dr["prod_img"].ToString(),
            });
        }
        ViewBag.Products = products; 
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
