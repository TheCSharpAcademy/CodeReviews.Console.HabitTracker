using System;

namespace HabitTracker.TomDonegan
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            while (true)
            {
                Database.DatabaseCheck();
                UserInterface.MainMenu();
            }
        }        
    }
}
