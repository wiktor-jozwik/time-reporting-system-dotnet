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
                    dateTime = EntryService.getRequestedDateTime(dateString);

                } catch (System.FormatException) {
                    return View("BadRequest");
                }
            }
            string userName = FileParser.getLoggedUser();

            string filePath = EntryService.getFileNameFromDate(userName.ToLower(), dateTime);
            try {
                MonthModel monthData = EntryService.getMonthData(filePath);
                monthEntries = monthData.Entries.OrderBy(x => x.Date).ToList();
                ViewData["Frozen"] = monthData.Frozen;
                ViewData["DateTime"] = dateTime;
                ViewData["UserName"] = userName;

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
            string filePath = EntryService.getFileNameFromDate(userName, Date);
            try {
                EntryModel entryModel = EntryService.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
                return View(entryModel);
            } catch (System.IO.FileNotFoundException) {
                return View("BadRequest");
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

                string filePath = EntryService.getFileNameFromDate(userName, entryModel.Date);

                MonthModel monthData = EntryService.getMonthData(filePath);
                bool frozen = monthData.Frozen;
                if(frozen) {
                    return View("BadRequest");
                }

                entryModel.Id = new Random().Next();

                FileParser.writeEntry(entryModel, filePath);


                this.addActivitiesToView();
                ViewData["Frozen"] = frozen;
                ViewData["DateTime"] = entryModel.Date;
                ViewData["UserName"] = userName;

                return View("Index", EntryService.getMonthEntries(filePath));  
            }

            return View(entryModel);
        }


        public IActionResult Edit(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = EntryService.getFileNameFromDate(userName, Date);

            try {
                MonthModel monthData = EntryService.getMonthData(filePath);
                EntryModel entryModel = monthData.Entries.FirstOrDefault(x => x.Id == Id);

                this.addActivitiesToView();
                ViewData["Frozen"] = monthData.Frozen;

                return View(entryModel);
            } catch (System.IO.FileNotFoundException) {
                return View("BadRequest");
            } 
            catch (Exception) {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, [Bind("Id,Date,Code,Subcode,Time,Description")] EntryModel entryModel)
        {
            this.addActivitiesToView();
            ViewData["DateTime"] = entryModel.Date;

            if (ModelState.IsValid)  {  
                string userName = FileParser.getLoggedUser();

                string filePath = EntryService.getFileNameFromDate(userName, entryModel.Date);

                try {
                    List<EntryModel> monthEntries = null;
                    MonthModel monthData = EntryService.getMonthData(filePath);
                    bool frozen = monthData.Frozen;
                    if(frozen) {
                        return View("BadRequest");
                    }
                    monthEntries = monthData.Entries;

                    int index = monthEntries.FindIndex(x => x.Id == Id);
                    monthEntries[index] = entryModel;

                    FileParser.writeMonth(monthData, filePath);

                    ViewData["UserName"] = userName;
                    ViewData["Frozen"] = frozen;
                    return View("Index", monthEntries);
                } catch (System.IO.FileNotFoundException) {
                    return View("BadRequest");
                } catch(Exception) {
                    return View("Error");
                }

            }
            return View(entryModel);
        }

        public IActionResult Delete(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = EntryService.getFileNameFromDate(userName, Date);

            EntryModel entryModel = null;
            try {
                entryModel = EntryService.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
            } catch (System.IO.FileNotFoundException) {
                return View("BadRequest");
            } catch (Exception) {
                return View("Error");
            }
            if (entryModel == null) {
                return View("BadRequest");
            }
            return View(entryModel);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();

            string filePath = EntryService.getFileNameFromDate(userName, Date);

            try {
                List<EntryModel> monthEntries = null;
                MonthModel monthData = EntryService.getMonthData(filePath);
                bool frozen = monthData.Frozen;
                if(frozen) {
                    return View("BadRequest");
                }

                monthEntries = monthData.Entries;
                monthEntries.RemoveAll(x => x.Id == Id);

                FileParser.writeMonth(monthData, filePath);


                ViewData["DateTime"] = Date;
                ViewData["UserName"] = userName;
                ViewData["Frozen"] = frozen;

                return View("Index", monthEntries); 
            } catch (System.IO.FileNotFoundException) {
                return View("BadRequest");
            } 
            catch (Exception) {
                return View("Error");
            }

        }

        private void addActivitiesToView() {
            AcitvityList activityList = FileParser.readJson<AcitvityList>("Data/activity.json");
            List<ActivityModel> activities = activityList.Activities.Where(a => a.Active == true).ToList();
            ViewData["Activities"] = activities;
        }

    }
}
