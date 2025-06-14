﻿using Microsoft.AspNetCore.Mvc;
using practiceQuiz.DataAccess;
using System.Data;
using tesla.Models;

namespace tesla.Controllers
{
    public class ProfileController : Controller
    {
        DatabaseHelper _helper;
        public ProfileController()
        {
            _helper = new DatabaseHelper();
        }
        [HttpGet]
        public IActionResult ShowProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("id")))
            {
                return RedirectToAction("Login", "Auth");
            }
            DataTable user = _helper.read($"select * from users where id = {HttpContext.Session.GetString("id")}");
            var userModel = new User
            {
                firstname = user.Rows[0]["firstname"].ToString(),
                middlename= user.Rows[0]["middlename"].ToString(),
                lastname = user.Rows[0]["lastname"].ToString(),
                address = user.Rows[0]["address"].ToString(),
                email = user.Rows[0]["email"].ToString(),
                password = user.Rows[0]["password"].ToString(),
            };
            
            return View(userModel);
        }

        [HttpPost]
        public IActionResult ShowProfile(User user)
        {
            _helper.execute($"update users set firstname = '{user.firstname}', middlename = '{user.middlename}', " +
                $"lastname = '{user.lastname}', address = '{user.address}', email ='{user.email}', password = '{user.password}' where id = {HttpContext.Session.GetString("id")} ");
            return View(user);
        }
    }
}
