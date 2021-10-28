using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;

using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index() {
            UserList users = JsonParserSingleton.readJson<UserList>("Data/users.json");

            ViewData["Users"] = users;
            return View();
        }
    }
}