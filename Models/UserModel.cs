
using System.Collections.Generic;

namespace NtrTrs.Models
{

    public class UserList 
    {
        public List<UserModel> users { get; set;}
    }
    public class UserModel
    {
        public string name { get; set; }
    }
}
