using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using NtrTrs.Services;
using System.IO;

using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index() {
            List<User> users = _userService.GetAllUsers();

            ViewData["Users"] = users;
            return View();
        }

        public IActionResult Logged(string userName) {
            // FileParser.logUser(userName);

            _userService.LogUser(userName);

            return Redirect("/Entry/Index");
        }
    }
}