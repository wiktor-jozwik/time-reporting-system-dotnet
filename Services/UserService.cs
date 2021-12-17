using System.Linq;
using System.Collections.Generic;

namespace NtrTrs.Services
{
    public class UserService
    {
        private NtrTrsContext _context { get; set; }
        public UserService(NtrTrsContext context) {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _GetAllUsers();
        }

        public void LogUser(string userName)
        {
            var user = _GetUserByName(userName);
            var loggedUser = _GetLoggedUser();


            if(loggedUser != null)
            {
                loggedUser.LoggedIn = false;
            }
            if(user != null) 
            {
                user.LoggedIn = true;
            }
            _context.SaveChanges();
        }

        public User GetUserByName(string userName)
        {
            return _GetUserByName(userName);
        }

        public User GetLoggedUser()
        {
            return _GetLoggedUser();
        }

        private List<User> _GetAllUsers()
        {
            return _context.Users.ToList();
        }

        private User _GetUserByName(string userName)
        {
            return _context.Users.Where(u => u.Name == userName).FirstOrDefault();
        }

        private User _GetLoggedUser()
        {
            return _context.Users.Where(u => u.LoggedIn == true).FirstOrDefault();
        }
    }
}
