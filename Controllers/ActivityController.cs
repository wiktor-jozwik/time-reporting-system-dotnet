using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NtrTrs.Services;


namespace NtrTrs.Controllers
{
    public class ActivityController : Controller
    {
        private readonly UserService _userService;
        private readonly ActivityService _activityService;        

        public ActivityController(UserService userService, ActivityService activityService)
        {
            _userService = userService;
            _activityService = activityService;
        }
        public IActionResult Index() {            
            List<Activity> activities = _activityService.GetAllActivities();

            return View(activities);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string sub, [Bind("Code,Budget")] Activity activity)
        {
            if (!ModelState.IsValid) {
                return View();
            }

            if (_activityService.CheckCodeUniqueness(activity.Code))  {  

            User loggedUser = _userService.GetLoggedUser();

            if (loggedUser != null) {
                activity.Manager = loggedUser;
            }

            if(sub != null) {
                var subactivitiesFromString = sub.Split(new [] { "," }, StringSplitOptions.None);

                List<Subactivity> subactivities = new List<Subactivity>{}; 

                foreach(var x in subactivitiesFromString) {
                    Subactivity subactivity = new Subactivity {Code = x}; 
                    subactivities.Add(subactivity);
                }
                activity.Subactivities = subactivities;
            }


            _activityService.CreateActivity(activity);
            }

            return RedirectToAction("Index");
        }
    }
}