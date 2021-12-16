using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NtrTrs.Services
{
    public class ActivityService
    {
        private NtrTrsContext _context { get; set; }
        public ActivityService(NtrTrsContext context) {
            _context = context;
        }

        public List<Activity> GetAllActivities()
        {
            return _GetAllActivities();
        }

        public List<Activity> GetActiveActivities()
        {
            return _GetAllActivities().Where(a => a.Active == true).ToList();
        }

        public Activity GetActivityByCode(string Code)
        {
            return _GetActivityByCode(Code);
        }

        public bool CheckCodeUniqueness(string code)
        {
            var activities = _GetAllActivities();

            return !activities.Any(a => a.Code == code);
        }

        public void CreateActivity(Activity activity)
        {
            _context.Activties.Add(activity);
            _context.SaveChanges();
        }

        private List<Activity> _GetAllActivities()
        {
            return _context.Activties
                    .Include(a => a.Manager)
                    .Include(a => a.Subactivities)
                    .ToList();
        }

        private Activity _GetActivityByCode(string code)
        {
            return _context.Activties.Where(a => a.Code == code).FirstOrDefault();
        }
    }
}