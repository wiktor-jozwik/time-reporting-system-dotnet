using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using System.IO;

using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index() {
            UserList users = FileParser.readJson<UserList>("Data/users.json");

            ViewData["Users"] = users;
            return View();
        }

        public IActionResult Logged(string userName) {
            FileParser.logUser(userName);

            return Redirect("/Home/Index");
        }
    }
}