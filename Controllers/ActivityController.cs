using Microsoft.AspNetCore.Mvc;
using NtrTrs.Models;


namespace NtrTrs.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index() {            
            AcitvityList activities = FileParser.readJson<AcitvityList>("Data/activity.json");

            return View(activities.Activities);
        }
    }
}