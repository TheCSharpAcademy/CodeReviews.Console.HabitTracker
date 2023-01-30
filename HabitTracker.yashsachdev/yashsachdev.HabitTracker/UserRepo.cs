using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yashsachdev.HabitTracker
{
    public class UserRepo
    {
        public User Retrieve(int userId)
        {

            User user = new User();
            user.Name = "";
            user.Email = string.Empty;
            user.Password = string.Empty;
            return user;
        }
        public bool Save(User user)
        {
                return true;
        }
    }
   
}
