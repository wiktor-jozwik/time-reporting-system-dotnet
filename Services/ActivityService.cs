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

        public List<Activity> GetManagerActivities(User manager)
        {
            return _GetManagerActivities(manager);
        }

        public void CloseProject(Activity activity)
        {
            activity.Active = false;
            _context.Update(activity);
            _context.SaveChanges();
        }

        public bool CheckCodeUniqueness(string code)
        {
            var activities = _GetAllActivities();

            return !activities.Any(a => a.Code == code);
        }

        public bool ValidateIfUserIsManager(string code, User user)
        {
            Activity activity = _GetActivityByCode(code);
            return activity.Manager == user;
        }

        public void CreateActivity(Activity activity)
        {
            _context.Activities.Add(activity);
            _context.SaveChanges();
        }

        private List<Activity> _GetAllActivities()
        {
            return _context.Activities
                    .Include(a => a.Manager)
                    .Include(a => a.Subactivities)
                    .ToList();
        }

        private Activity _GetActivityByCode(string code)
        {
            return _context.Activities.Where(a => a.Code == code).FirstOrDefault();
        }

        private List<Activity> _GetManagerActivities(User manager)
        {
            return _context.Activities
                    .Where(a => a.Manager == manager)
                    .ToList();
        }
    }
}