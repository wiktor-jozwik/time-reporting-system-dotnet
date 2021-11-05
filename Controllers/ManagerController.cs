using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;
using System.Linq;
using System.IO;

namespace NtrTrs.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index() {
            string userName = FileParser.getLoggedUser();
            ViewData["Activities"] = this.getManagerActivities(userName);

            return View();
        }
        public IActionResult Entries(string Code) {
            List<EntryModel> monthEntriesForAllUsers = new List<EntryModel>();
            string userName = FileParser.getLoggedUser();
            bool valid = this.validateIfUserIsManager(Code, userName);

            if(!valid) {
                return View("BadRequest");
            }

            List<MonthModel> allMonthsData = this.readAllMonthsAllUsersData();
            List<EntryModel> allEntries = this.getEntriesFromAllMonths(allMonthsData);


            allEntries = allEntries.Where(e => e.Code == Code).OrderBy(e => e.Date).ToList();

            ActivityModel activity = this.getActivityByCode(Code);
            ViewData["Budget"] = activity.Budget;
            ViewData["Active"] = activity.Active;

            return View(allEntries);
        }

        public IActionResult CloseProject(string Code) {
            string userName = FileParser.getLoggedUser();
            ActivityModel activity = this.getActivityByCode(Code);

            bool active = activity.Active;
                if(!active) {
                    return View("BadRequest");
                }

            activity.Active = false;
            ViewData["Active"] = activity.Active;

            try {
                ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");

                int index = activityList.Activities.FindIndex(x => x.Code == Code);
                activityList.Activities[index] = activity;

                FileParser.writeActivity(activityList);
            } catch (Exception) {
                return View("Error");
            }
            ViewData["Activities"] = this.getManagerActivities(userName);

            return View("Index");
        }
        private bool validateIfUserIsManager(string code, string userName) {
            ActivityList activities = FileParser.readJson<ActivityList>("Data/activity.json");

            return activities.Activities.Where(a => a.Code == code).FirstOrDefault().Manager.ToLower() == userName.ToLower();
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