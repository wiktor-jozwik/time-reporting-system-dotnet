using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;

using System.Linq;

namespace NtrTrs.Controllers
{
    public class EntryController : Controller
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

            string filePath = this.getFileNameFromDate(userName.ToLower(), dateTime);
            try {
                monthEntries = this.getMonthEntries(filePath);
                monthEntries = monthEntries.OrderBy(x => x.Date).ToList();

            } catch (System.IO.FileNotFoundException) {
                monthEntries = null;

            } catch (Exception) {
                return View("Error");
            }

            return View(monthEntries);
        }

        public IActionResult Details(int Id, DateTime Date)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = getFileNameFromDate(userName, Date);
            try {
                EntryModel entryModel = this.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
                return View(entryModel);
            } catch (Exception) {
                return View("Error");
            }
        }
        
        public IActionResult Create()
            {
                this.addActivitiesToView();

                return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Date,Code,Subcode,Time,Description")] EntryModel entryModel)
        {
            this.addActivitiesToView();

            if (ModelState.IsValid)  {  
                string userName = FileParser.getLoggedUser();

                string filePath = getFileNameFromDate(userName, entryModel.Date);

                entryModel.Id = new Random().Next();

                FileParser.writeEntry(entryModel, filePath);

                // ViewBag.ResponseStatus = "SUCCESS";

                // ViewBag.SuccessResponse = $"Successfully submitted {entryModel.Time} minutes to {entryModel.Code} project on {entryModel.Date.ToString("yyyy-MM")}";
                return View();  
            }
                // ViewBag.ResponseStatus = "ERROR";

                // ViewBag.SuccessResponse = $"Successfully submitted {entryModel.Time} minutes to {entryModel.Code} project on {entryModel.Date.ToString("yyyy-MM")}";

            return View(entryModel);
        }


        public IActionResult Edit(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = getFileNameFromDate(userName, Date);

            try {
                EntryModel entryModel = this.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);

                this.addActivitiesToView();

                return View(entryModel);
            } catch (Exception) {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, [Bind("Id,Date,Code,Subcode,Time,Description")] EntryModel entryModel)
        {
            if (ModelState.IsValid)  {  
                string userName = FileParser.getLoggedUser();

                string filePath = getFileNameFromDate(userName, entryModel.Date);

                try {
                    List<EntryModel> monthEntries = null;
                    MonthModel monthData = this.getMonthData(filePath);
                    monthEntries = monthData.Entries;

                    int index = monthEntries.FindIndex(x => x.Id == Id);
                    monthEntries[index] = entryModel;

                    FileParser.writeMonth(monthData, filePath);

                    ViewData["DateTime"] = entryModel.Date;
                    ViewData["UserName"] = userName;
                    this.addActivitiesToView();
                    return View("~/Views/Entry/Index.cshtml", monthEntries);
                } catch(Exception) {
                    return View("Error");
                }

            } else {
                return View("Error");
            }
        }

        public IActionResult Delete(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = getFileNameFromDate(userName, Date);

            try {
                EntryModel entryModel = this.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
                return View(entryModel);
            } catch (Exception) {
                return View("Error");
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();

            string filePath = getFileNameFromDate(userName, Date);

            try {
                List<EntryModel> monthEntries = null;
                MonthModel monthData = this.getMonthData(filePath);
                monthEntries = monthData.Entries;
                monthEntries.RemoveAll(x => x.Id == Id);

                FileParser.writeMonth(monthData, filePath);


                ViewData["DateTime"] = Date;
                ViewData["UserName"] = userName;
                return View("~/Views/Entry/Index.cshtml", monthEntries); 
            } catch (Exception) {
                return View("Error");
            }

        }


        private DateTime getRequestedDateTime(string dateString) {
            DateTime dateTime =  DateTime.ParseExact(dateString, "yyyy-MM", null);
            
            return dateTime;
        }

        private string getFileNameFromDate(string name, DateTime date) {
            return $"Data/entries/{name}-{date.ToString("yyyy-MM")}.json";
        }

        private MonthModel getMonthData(string filePath) {
            return FileParser.readJson<MonthModel>(filePath);
        }
        private List<EntryModel> getMonthEntries(string filePath) {
            MonthModel monthData = FileParser.readJson<MonthModel>(filePath);
            return monthData.Entries;
        }

        private void addActivitiesToView() {
            AcitvityList activities = FileParser.readJson<AcitvityList>("Data/activity.json");
            ViewData["Activities"] = activities;
        }
    }
}
