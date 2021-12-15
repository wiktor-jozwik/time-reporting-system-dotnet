using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using NtrTrs.Services;

using System.Linq;

namespace NtrTrs.Controllers
{
    public class EntryController : Controller
    {
        private readonly UserService _userService;
        private readonly MonthEntryService _monthEntryService;
        private readonly EntryService _entryService;
        private readonly ActivityService _activityService;

        public EntryController(
            UserService userService, 
            MonthEntryService monthEntryService, 
            ActivityService activityService,
            EntryService entryService
            )
        {
            _userService = userService;
            _monthEntryService = monthEntryService;
            _activityService = activityService;
            _entryService = entryService;
        }
        public IActionResult Index(string dateString = null)
        {
            DateTime dateTime;
            List<Entry> monthEntries = null;

            if (dateString == null) {
                dateTime = DateTime.Now;
            } else {
                try {
                    dateTime = EntrysService.getRequestedDateTime(dateString);

                } catch (System.FormatException) {
                    return View("BadRequest");
                }
            }

            User loggedUser = _userService.GetLoggedUser();
            string userName = "";
            if (loggedUser != null) {
                userName = loggedUser.Name;
            }

            ViewData["DateTime"] = dateTime;
            ViewData["UserName"] = userName;

            try {
                MonthEntry monthData = _monthEntryService.GetMonthData(dateTime);

                if (monthData != null)
                {
                    monthEntries = monthData.Entries.OrderBy(x => x.Date).ToList();
                    ViewData["Frozen"] = monthData.Frozen;
                }

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
            // User user = _userService.GetLoggedUser();
            string filePath = EntrysService.getFileNameFromDate(userName, Date);
            try {

                EntryModel entryModel = EntrysService.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
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
        public IActionResult Create([Bind("Date,Code,Subcode,Time,Description")] Entry entry)
        {
            this.addActivitiesToView();

            if (ModelState.IsValid)  {

                User loggedUser = _userService.GetLoggedUser();
                if(loggedUser != null)
                {
                    ViewData["UserName"] = loggedUser.Name;
                }
                ViewData["DateTime"] = entry.Date;
                // string userName = FileParser.getLoggedUser();

                // string filePath = EntrysService.getFileNameFromDate(userName, entryModel.Date);

                MonthEntry monthData = _monthEntryService.GetMonthData(entry.Date);

                if (monthData != null)
                {
                    bool frozen = monthData.Frozen;

                    if (frozen)
                    {
                        return View("BadRequest");
                    }
                }

                _entryService.CreateEntry(entry);

                // if (System.IO.File.Exists(filePath)) {
                //     MonthModel monthData = EntrysService.getMonthData(filePath);
                //     bool frozen = monthData.Frozen;

                //     if(frozen) {
                //         return View("BadRequest");
                //     }
                // } 
                // entryModel.Id = new Random().Next();

                // FileParser.writeEntry(entryModel, filePath);


                return View("Index", _monthEntryService.GetMonthData(entry.Date));  
            }

            return View(entry);
        }


        public IActionResult Edit(DateTime Date, int Id)
        {
            string userName = FileParser.getLoggedUser();
            string filePath = EntrysService.getFileNameFromDate(userName, Date);

            try {
                MonthModel monthData = EntrysService.getMonthData(filePath);
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

                string filePath = EntrysService.getFileNameFromDate(userName, entryModel.Date);

                try {
                    List<EntryModel> monthEntries = null;
                    MonthModel monthData = EntrysService.getMonthData(filePath);
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
            this.addActivitiesToView();

            string userName = FileParser.getLoggedUser();
            string filePath = EntrysService.getFileNameFromDate(userName, Date);

            EntryModel entryModel = null;
            try {
                entryModel = EntrysService.getMonthEntries(filePath).FirstOrDefault(x => x.Id == Id);
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

            string filePath = EntrysService.getFileNameFromDate(userName, Date);

            try {
                List<EntryModel> monthEntries = null;
                MonthModel monthData = EntrysService.getMonthData(filePath);
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
            List<Activity> activities = _activityService.GetActiveActivities();
            // ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");
            // List<ActivityModel> activities = activityList.Activities.Where(a => a.Active == true).ToList();
            ViewData["Activities"] = activities;
        }

    }
}
