using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.ViewModels;
using NtrTrs.Services;
using System.Linq;
using System.IO;

namespace NtrTrs.Controllers
{
    public class ManagerController : Controller
    {
        private readonly UserService _userService;
        private readonly MonthEntryService _monthEntryService;
        private readonly ActivityService _activityService;
        private readonly EntryService _entryService;    

        public ManagerController(
                        UserService userService, 
                        ActivityService activityService, 
                        MonthEntryService monthEntryService,
                        EntryService entryService)
        {
            _userService = userService;
            _monthEntryService = monthEntryService;
            _activityService = activityService;
            _entryService = entryService;
        }
        public IActionResult Index() {
            User loggedUser = _userService.GetLoggedUser();
            List<Activity> activities = _activityService.GetManagerActivities(loggedUser);

            return View(activities);
        }
        public IActionResult Entries(string Code) {
            User loggedUser = _userService.GetLoggedUser();

            bool valid = _activityService.ValidateIfUserIsManager(Code, loggedUser);

            if(!valid) {
                return View("BadRequest");
            }

            List<MonthEntry> allMonthsData = _monthEntryService.GetAllMonthsData();
            List<Entry> allEntries = _entryService.GetEntriesFromAllMonths(allMonthsData);

            Activity activity = _activityService.GetActivityByCode(Code);


            allEntries = allEntries.Where(e => e.Activity == activity).OrderBy(e => e.Date).ToList();

            int acceptedTimeForProject = 0;
            foreach(var m in allMonthsData) {
                if (m.Accepted != null){
                    var acc = m.Accepted.Where(e => e.Activity == activity).FirstOrDefault();
                    if (acc != null) {
                        acceptedTimeForProject += acc.Time;
                    }
                }
            }
            int budgetLeft = activity.Budget - acceptedTimeForProject;

            ViewData["Budget"] = budgetLeft;
            ViewData["Active"] = activity.Active;

            return View(allEntries);
        }

        public IActionResult CloseProject(string Code) {
            Activity activity = _activityService.GetActivityByCode(Code);

            bool active = activity.Active;
                if(!active) {
                    string cause = "Activity is already closed.";
                    ViewData["Cause"] = cause;
                    return View("BadRequest");
                }

            activity.Active = false;
            ViewData["Active"] = activity.Active;

            _activityService.CloseProject(activity);

            User loggedUser = _userService.GetLoggedUser();
            List<Activity> activities = _activityService.GetManagerActivities(loggedUser);

            ViewData["Activities"] = activities;

            return View("Index");
        }

        public IActionResult SelectUser(string Code) {
            List<MonthEntry> monthData = _monthEntryService.GetAllMonthsData();

            // Accepted month
            monthData = monthData.Where(m => m.Frozen == true).ToList();
            monthData = monthData.Where(e => e.Entries.Any(x => x.Activity.Code == Code)).ToList();

            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var month in monthData) {
                foreach (var entry in month.Entries)
                {
                    var user = new UserViewModel {Name = entry.User.Name};
                    if(!users.Any(u => u.Name == user.Name)) {
                        users.Add(user);
                    }
                }
            }

            ViewData["Code"] = Code;
            return View(users);
        }

        public IActionResult UserEntries(string Code, string UserName) {
            List<ManagerViewModel> managerList = _monthEntryService.GetListForManager(Code, UserName);

            ViewData["User"] = UserName;
            ViewData["Code"] = Code;

            return View(managerList);

        }

        public IActionResult Accept(string UserName, string Code, DateTime Date)
            {
                ViewData["User"] = UserName;
                ViewData["Date"] = Date;
                ViewData["Code"] = Code;
                return View();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Accept(string UserName, string Code, DateTime Date, [Bind("AcceptedTime")] ManagerViewModel accepted)
        {
            Activity activity = _activityService.GetActivityByCode(Code);

            if (activity == null)
            {
                string cause = "Activity has not beed found.";
                ViewData["Cause"] = cause;
                return View("BadRequest");
            }

            User user = _userService.GetUserByName(UserName);
            _monthEntryService.CreateAcceptedEntry(activity, Date, user, accepted.AcceptedTime);

            
            ViewData["User"] = UserName;

            List<ManagerViewModel> managerList = _monthEntryService.GetListForManager(Code, UserName);

            ViewData["User"] = UserName;
            ViewData["Code"] = Code;
            return View("UserEntries", managerList);
        }
    }
}