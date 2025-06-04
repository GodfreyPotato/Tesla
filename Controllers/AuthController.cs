using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;

using System.Data;
using tesla.Models;


namespace tesla.Controllers
{
    public class AuthController : Controller
    {
        DatabaseHelper _helper;
        public AuthController()
        {
            _helper = new DatabaseHelper();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                DataTable dt = _helper.read($"select * from users where email = '{login.email}'");

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["password"].ToString() == login.password)
                    {
                        HttpContext.Session.SetString("role",dt.Rows[0]["role"].ToString());
                        return RedirectToAction("ShowProducts", "Product");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Email/Password"); // Password mismatch
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email/Password"); // Email not found
                }
            }

            return View(login); // Show form again with validation errors
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            if (ModelState.IsValid)
            {
                DataTable dt = _helper.read($"select * from users where email = '{register.email}'");

                if (dt.Rows.Count > 0)
                {
                    ModelState.AddModelError("", "Email already exist!");
                }
                else
                {
                   if (_helper.execute($"INSERT INTO users (firstname, middlename, lastname, address, email, password) VALUES ('{register.firstname}','{register.middlename}','{register.lastname}','{register.address}','{register.email}','{register.password}') ") > 0)
                    {

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something went wrong!");
                    }
                }
            }

            return View(register);
        }
    }
}
