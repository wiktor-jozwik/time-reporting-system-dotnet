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
            ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");
            List<ActivityModel> activities = activityList.Activities.Where(a => (a.Active == true && a.Manager.ToLower() == userName.ToLower())).ToList();
            ViewData["Activities"] = activities;

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


            allEntries = allEntries.Where(e => e.Code == Code).ToList();

            ViewData["Budget"] = this.getProjectBudgetByCode(Code);

            return View(allEntries);
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

    private int getProjectBudgetByCode(string code) {
        ActivityList activityList = FileParser.readJson<ActivityList>("Data/activity.json");
        return activityList.Activities.FirstOrDefault(a => a.Code == code).Budget;
    }
    }
}