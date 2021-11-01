using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;

using System.Linq;

namespace NtrTrs.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string dateString = null)
        {
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
            string userName = FileParser.getLoggedUser();

            ViewData["DateTime"] = dateTime;
            ViewData["UserName"] = userName;

            try {
                MonthModel monthData = FileParser.readJson<MonthModel>(this.getFileNameFromDate(userName.ToLower(), dateTime));
                monthEntries = monthData.Entries;
                monthEntries = monthEntries.OrderBy(x => x.Date).ToList();

                // monthEntries = monthData.entries.FindAll(e => e.date.Date == dateTime.Date);

            } catch (System.IO.FileNotFoundException) {
                monthEntries = null;

            } catch (Exception) {
                return View("Error");
            }

            return View(monthEntries);
        }
        
        public IActionResult Create()
            {
                return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Date,Code,Subcode,Time,Description")] EntryModel entryModel)
        {
            if (ModelState.IsValid)  {  
                string userName = FileParser.getLoggedUser();

                string filePath = getFileNameFromDate(userName, entryModel.Date);

                entryModel.Id = new Random().Next();

                FileParser.writeJson<MonthModel>(entryModel, filePath);

                return View();  
            }
            ViewBag.Error = string.Format("FAILLLL");

            return View(entryModel);
        }



        private DateTime getRequestedDateTime(string dateString) {
            DateTime dateTime =  DateTime.ParseExact(dateString, "yyyy-MM", null);
            
            return dateTime;
        }

        private string getFileNameFromDate(string name, DateTime date) {
            return $"Data/entries/{name}-{date.ToString("yyyy-MM")}.json";
        }

    }
}
