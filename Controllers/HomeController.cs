using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;

using System.Text.Json;

namespace NtrTrs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string name = "kowalski", string dateString = null)
        {
            DateTime dateTime;
            List<EntryModel> userEntries = null;

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
                UserModel userData = this.fetchDataFromJson(this.getFileNameFromDate(name, dateTime));
                userEntries = userData.entries.FindAll(e => e.date.Date == dateTime.Date);

            } catch (System.IO.FileNotFoundException) {

            } catch (Exception) {
                return View("Error");
            }

            ViewData["UserEntries"] = userEntries;

            return View();
        }

        private UserModel fetchDataFromJson(string fileName) {

            string jsonString = System.IO.File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<UserModel>(jsonString);
        }

        private DateTime getRequestedDateTime(string dateString) {
            DateTime dateTime =  DateTime.ParseExact(dateString, "yyyy-MM-dd", null);
            
            return dateTime;
        }

        private string getFileNameFromDate(string name, DateTime date) {
            ViewData["UserName"] = name;

            return $"Data/entries/{name}-{date.ToString("yyyy-MM")}.json";
        }

    }
}
