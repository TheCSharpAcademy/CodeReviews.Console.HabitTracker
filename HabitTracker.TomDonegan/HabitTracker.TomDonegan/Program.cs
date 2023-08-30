using System;

namespace HabitTracker.TomDonegan
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Database.DatabaseCreation();
            UserInterface.MainMenu();
        }

        
    }
}
