using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;

using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string userName, string dateString = null)
        {
            System.Console.WriteLine(userName);
            ViewData["UserName"] = userName;

            DateTime dateTime;
            List<EntryModel> monthEntries = null;

            if (dateString == null) {
                dateTime = DateTime.Now;
            } else {
                try {
                    dateTime = this.getRequestedDateTime(dateString);

                } catch (System.FormatException) {
                    return View("Error");
                }
            }
            ViewData["DateTime"] = dateTime;

            try {
                MonthModel monthData = JsonParserSingleton.readJson<MonthModel>(this.getFileNameFromDate(userName, dateTime));
                monthEntries = monthData.entries.FindAll(e => e.date.Date == dateTime.Date);

            } catch (System.IO.FileNotFoundException) {

            } catch (Exception) {
                return View("Error");
            }

            ViewData["monthEntries"] = monthEntries;

            return View();
        }
        private DateTime getRequestedDateTime(string dateString) {
            DateTime dateTime =  DateTime.ParseExact(dateString, "yyyy-MM-dd", null);
            
            return dateTime;
        }

        private string getFileNameFromDate(string name, DateTime date) {
            return $"Data/entries/{name}-{date.ToString("yyyy-MM")}.json";
        }

    }
}
