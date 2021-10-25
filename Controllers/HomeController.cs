using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NtrTrs.Models;

using System;
using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string name = "kowalski", string date = null)
        {
            DateTime dateTime;

            if (date != null)
            {
                try {
                    dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", null);

                } catch (System.FormatException) {
                    return View("Error");
                }
            } else {
                dateTime = DateTime.Now;
            }

            string dateString = dateTime.ToString("yyyy-MM");

            UserModel user;
            string fileName = $"Data/entries/{name}-{dateString}.json";


            try {
                ViewData["Date"] = dateTime.ToShortDateString();
                ViewData["UserName"] = name;

                string jsonString = System.IO.File.ReadAllText(fileName);
                user = JsonSerializer.Deserialize<UserModel>(jsonString);
            } catch (System.IO.FileNotFoundException) { 
                return View("FileNotFound");
            }

            ViewData["User"] = user;

            return View();
        }

    }
}
