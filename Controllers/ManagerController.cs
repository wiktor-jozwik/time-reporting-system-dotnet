using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
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
            List<EntryModel> monthEntriesForAllUsers = new List<EntryModel>();
            string userName = FileParser.getLoggedUser();
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
            // List<MonthWithUserModel> monthsDataWithUser = this.readMonthsDataWithUser();
            // Accepted month
            monthData = monthData.Where(m => m.Frozen == true).ToList();
            monthData = monthData.Where(e => e.Entries.Any(x => x.Activity.Code == Code)).ToList();

            List<UserModel> users = new List<UserModel>();
            // foreach (var month in monthData) {
            //     var user = new UserModel {Name = month.Entries};

            //     if(!users.Any(u => u.Name == user.Name)) {
            //         users.Add(user);
            //     }
            // }

            // ViewData["Users"] = users;
            ViewData["Code"] = Code;
            return View();
        }

        public IActionResult UserEntries(string Code, string UserName) {
            List<ManagerViewModel> managerList = this.getListForManager(Code, UserName);

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
            string filePath = EntrysService.getFileNameFromDate(UserName, Date);

            if (System.IO.File.Exists(filePath)) {
                MonthModel monthData = EntrysService.GetMonthDataForUser(filePath);
                AcceptedEntryModel acceptedModel = new AcceptedEntryModel {Code = Code, Time = accepted.AcceptedTime};
                
                if (monthData.Accepted != null && monthData.Accepted.Count != 0) {
                    var monthAccepted = monthData.Accepted;

                    int index = monthAccepted.FindIndex(x => x.Code == Code);
                    if(index > 0) {
                        monthAccepted[index] = acceptedModel;
                    } else {
                        monthData.Accepted = new List<AcceptedEntryModel>();
                        monthData.Accepted.Add(acceptedModel);
                    }
                } else {
                    monthData.Accepted = new List<AcceptedEntryModel>();
                    monthData.Accepted.Add(acceptedModel);
                }

                FileParser.writeMonth(monthData, filePath);
                } 

            
            ViewData["User"] = UserName;

            List<ManagerViewModel> managerList = this.getListForManager(Code, UserName);

            ViewData["User"] = UserName;
            ViewData["Code"] = Code;
            return View("UserEntries", managerList);
        }
        private bool validateIfUserIsManager(string code, string userName) {
            ActivityList activities = FileParser.readJson<ActivityList>("Data/activity.json");

            return activities.Activities.Where(a => a.Code == code).FirstOrDefault().Manager.ToLower() == userName.ToLower();
        }

        private List<MonthWithUserModel> readMonthsDataWithUser() {
            List<MonthWithUserModel> allMonthsDataWithUser = new List<MonthWithUserModel>();
            string[] filePaths = 
            Directory.GetFiles("Data/entries", "*.json");

            foreach (var filePath in filePaths) {
                string userName = filePath.Split("/")[^1].Split("-")[0];
                MonthWithUserModel monthData = FileParser.readJson<MonthWithUserModel>(filePath);
                monthData.User = userName;
                allMonthsDataWithUser.Add(monthData);
            }

            return allMonthsDataWithUser;
        }

        private List<ManagerViewModel> getListForManager(string code, string userName) {
            List<MonthWithUserModel> monthsDataWithUser = this.readMonthsDataWithUser();
            // Accepted month
            monthsDataWithUser = monthsDataWithUser.Where(m => m.Frozen == true && m.User == userName).ToList();

            List<ManagerViewModel> managerList = new List<ManagerViewModel>();

            foreach(var month in monthsDataWithUser) {
                managerList.AddRange(month.Entries
                    .Where(en => en.Code == code)
                    .GroupBy(en => en.Code)
                    .Select(ml => new ManagerViewModel {
                        TotalTime = ml.Sum(c => c.Time), 
                        Date = ml.Select(a => a.Date).FirstOrDefault()}));

                if (month.Accepted != null) {
                    var accepted = month.Accepted.Where(a => a.Code == code).FirstOrDefault();

                    if(accepted != null ){
                        foreach(var m in managerList) {
                            int acceptedTime = accepted.Time;
                            m.AcceptedTime = acceptedTime;
                        }


                    }
            }
            }

            return managerList;
        }

        private List<MonthModel> readAllMonthsAllUsersData() {
            List<MonthModel> allMonthsData = new List<MonthModel>();
            string[] filePaths = 
            Directory.GetFiles("Data/entries", "*.json");

            foreach (var filePath in filePaths) {
                MonthModel monthData = FileParser.readJson<MonthModel>(filePath);
                allMonthsData.Add(monthData);
            }

            return allMonthsData;
        }

        private List<EntryModel> getEntriesFromAllMonths(List<MonthModel> allMonthsData) {
            List<EntryModel> allEntries = new List<EntryModel>();

            foreach(var month in allMonthsData) {
                allEntries.AddRange(month.Entries);
            }

            return allEntries;
        }

        private ActivityModel getActivityByCode(string code) {
            ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");
            return activityList.Activities.FirstOrDefault(a => a.Code == code);
        }
        private List<ActivityModel> getManagerActivities(string name) {
            ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");
            return activityList.Activities.Where(a => a.Manager.ToLower() == name.ToLower()).ToList();
        }
    }
}