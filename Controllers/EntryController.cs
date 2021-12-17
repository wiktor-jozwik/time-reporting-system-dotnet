using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.ViewModels;
using NtrTrs.Services;

using System.Linq;

namespace NtrTrs.Controllers
{
    public class EntryController : Controller
    {
        string ACCEPTED_CAUSE = "Month is already accepted. Try again!";
        string ACTIVITIY_CLOSED_CAUSE = "Activity is already closed. Try again!";

        string CODE_PRESENT_CAUSE = "Code of project has to be provided.";

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
                    dateTime = _entryService.GetRequestedDateTime(dateString);

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
                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(dateTime, loggedUser);

                if (monthData != null)
                {
                    monthEntries = monthData.Entries.OrderBy(x => x.Date).ToList();;
                    // monthEntries = monthData.Entries.OrderBy(x => x.Date).ToList();
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
            Entry entry = _entryService.GetEntryById(Id);

            if (entry != null)
            {
                return View(entry);
            } 
            else 
            {
                return View("BadRequest");
            }
        }
        
        public IActionResult Create()
            {
                this.addActivitiesToView();

                return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string Code, [Bind("Date,Subcode,Time,Description")] Entry entry)
        {
            this.addActivitiesToView();

            if (ModelState.IsValid)  {

                User loggedUser = _userService.GetLoggedUser();
                if(loggedUser != null)
                {
                    entry.User = loggedUser;
                    ViewData["UserName"] = loggedUser.Name;
                }
                ViewData["DateTime"] = entry.Date;

                Activity activity = _activityService.GetActivityByCode(Code);
                if (activity != null)
                {
                    if (activity.Active == false)
                    {
                        ViewData["Cause"] = ACTIVITIY_CLOSED_CAUSE;
                        return View("BadRequest");
                    }
                }
                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(entry.Date, loggedUser);

                if (monthData != null)
                {
                    bool frozen = monthData.Frozen;

                    if (frozen)
                    {
                        ViewData["Cause"] = ACCEPTED_CAUSE;
                        return View("BadRequest");
                    }
                } 
                else 
                {
                    monthData = new MonthEntry();
                    monthData.Date = entry.Date;
                    monthData.User = loggedUser;
                    _monthEntryService.CreateMonthEntry(monthData);
                }

                entry.MonthEntry = monthData;

                if (activity != null)
                {
                    entry.Activity = activity;
                }

                _entryService.CreateEntry(entry);

                return View("Index", _monthEntryService.GetMonthDataForUser(entry.Date, loggedUser).Entries.OrderBy(x => x.Date).ToList());  
            }

            return View(entry);
        }


        public IActionResult Edit(DateTime Date, int Id)
        {
            try {
                User loggedUser = _userService.GetLoggedUser();
                Entry entry = _entryService.GetEntryById(Id);

                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(entry.Date, loggedUser);

                this.addActivitiesToView();
                ViewData["Frozen"] = monthData.Frozen;

                return View(entry);
            } 
            catch (Exception) {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, string Code, [Bind("Date,Subcode,Time,Description")] Entry entry)
        {
            this.addActivitiesToView();
            ViewData["DateTime"] = entry.Date;

            if (Code == null)
            {
                ViewData["Cause"] = CODE_PRESENT_CAUSE;
                return View("BadRequest");
            }

            if (ModelState.IsValid)  {
                Activity activity = _activityService.GetActivityByCode(Code);
                if (activity != null)
                {
                    if (activity.Active == false)
                    {
                        ViewData["Cause"] = ACTIVITIY_CLOSED_CAUSE;
                        return View("BadRequest");
                    }
                }
                User loggedUser = _userService.GetLoggedUser();

                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(entry.Date, loggedUser);

                if (monthData != null)
                {
                    bool frozen = monthData.Frozen;
                    if(frozen) {
                        ViewData["Cause"] = ACCEPTED_CAUSE;
                        return View("BadRequest");
                    }
                    else
                    {
                        _entryService.EditEntry(Id, Code, entry.Date, entry.Subcode, entry.Time, entry.Description);
                    }
                    string userName = "";
                    User user = _userService.GetLoggedUser();
                    
                    if (user != null)
                    {
                        userName = user.Name;
                    }

                    ViewData["UserName"] = userName;
                    ViewData["Frozen"] = frozen;
                    return View("Index", _monthEntryService.GetMonthDataForUser(entry.Date, loggedUser).Entries.OrderBy(x => x.Date).ToList());
                }
            }
            return View(entry);
        }

        public IActionResult Delete(DateTime Date, int Id)
        {
            this.addActivitiesToView();

            try {
                User loggedUser = _userService.GetLoggedUser();

                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(Date, loggedUser);
                Entry entry = _entryService.GetEntryById(Id);

                this.addActivitiesToView();
                ViewData["Frozen"] = monthData.Frozen;

                return View(entry);
            } 
            catch (Exception) {
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(DateTime Date, int Id)
        {
            this.addActivitiesToView();
            ViewData["DateTime"] = Date;

            if (ModelState.IsValid)  {
                Entry entry = _entryService.GetEntryById(Id);
                if (entry != null) 
                {
                    if (entry.Activity != null)
                    {
                        if (entry.Activity.Active == false)
                        {
                        ViewData["Cause"] = ACTIVITIY_CLOSED_CAUSE;
                        return View("BadRequest");
                        }
                    }
                }
                User loggedUser = _userService.GetLoggedUser();
                
                MonthEntry monthData = _monthEntryService.GetMonthDataForUser(Date, loggedUser);

                if (monthData != null)
                {
                    bool frozen = monthData.Frozen;
                    if(frozen) {
                        ViewData["Cause"] = ACCEPTED_CAUSE;
                        return View("BadRequest");
                    }
                    else
                    {
                        _entryService.DeleteEntry(Id);
                    }
                    string userName = "";
                    User user = _userService.GetLoggedUser();
                    
                    if (user != null)
                    {
                        userName = user.Name;
                    }

                    ViewData["UserName"] = userName;
                    ViewData["Frozen"] = frozen;
                    return View("Index", _monthEntryService.GetMonthDataForUser(Date, loggedUser).Entries.OrderBy(x => x.Date).ToList());
                }
            }
            return View("BadRequest");
        }
        private void addActivitiesToView() {
            List<Activity> activities = _activityService.GetActiveActivities();
            ViewData["Activities"] = activities;
        }

    }
}
