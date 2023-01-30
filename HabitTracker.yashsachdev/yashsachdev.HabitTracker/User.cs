using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yashsachdev.HabitTracker
{
    
    public class User
    {
        public User()
        {

        }
        public User(int User_Id)
        {
        }
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

       /// <summary>
       /// Validate the properties of User class.
       /// </summary>
       /// <returns></returns>
       public bool validate()
        {
            var isValid = true;
            if(string.IsNullOrEmpty(Name)) isValid = false; 
            if(string.IsNullOrWhiteSpace(Password)) isValid = false;
            if(string.IsNullOrWhiteSpace(Email))isValid= false;
            return isValid; 
        }
    }
}
