using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;


namespace NtrTrs.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index() {            
            ActivityList activities = FileParser.readJson<ActivityList>("Data/activity.json");

            return View(activities.Activities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string sub, [Bind("Id,Code,Budget")] ActivityModel activityModel)
        {
            if (!ModelState.IsValid) {
                return View();
            }

            if (this.checkCodeUniqueness(activityModel.Code))  {  

            string userName = FileParser.getLoggedUser();
            if(sub != null) {
                var subactivitiesFromString = sub.Split(new [] { "," }, StringSplitOptions.None);

                List<Subactivity> subactivities = new List<Subactivity>{}; 

                foreach(var x in subactivitiesFromString) {
                    Subactivity subactivity = new Subactivity {Code = x}; 
                    subactivities.Add(subactivity);
                }
                activityModel.Subactivities = subactivities;
            }


            activityModel.Manager = userName;
            activityModel.Active = true;
            activityModel.Id = new Random().Next();

            FileParser.appendActivity(activityModel);
            }

            ActivityList activities = FileParser.readJson<ActivityList>("Data/activity.json");

            return View("~/Views/Activity/Index.cshtml", activities.Activities);
        }

        private bool checkCodeUniqueness(string code) {
            ActivityList activities = FileParser.readJson<ActivityList>("Data/activity.json");

            return !activities.Activities.Any(act => act.Code == code);
        }
    }
}