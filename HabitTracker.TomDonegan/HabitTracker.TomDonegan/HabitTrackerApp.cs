using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.TomDonegan
{
    public static class HabitTrackerApp
    {
        public static void Run()
        {
            DatabaseAccess.DatabaseCreation("", "");
            UserInterface.MainMenu();            
        }
    }
}
