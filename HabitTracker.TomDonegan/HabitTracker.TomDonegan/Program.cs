using System;

namespace HabitTracker.TomDonegan
{
    class Program
    {
       static void Main()
        {
            while (true)
            {
                HabitTrackerApp.Run();
                DatabaseAccess.DatabaseCreation("", "");
                UserInterface.MainMenu();
            }
        }        
    }
}
