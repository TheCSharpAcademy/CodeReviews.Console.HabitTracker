using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.TomDonegan
{
    internal static class HabitCreationHandler
    {
        internal static void CreateNewHabit(string habit)
        {
            Console.Clear();

            Helpers.DisplayHeader($"Current Habit: {habit}");

            Helpers.DisplayHeader("New Habit Creation");

            ArrayList habitList = HabitDataHandler.ListDatabaseTables();

            Console.WriteLine("Would you like to create a new habit? (y/n)");
            string createhabitAnswer = Console.ReadLine();

            switch (createhabitAnswer.ToLower())
            {
                case "y":
                    GetCreationData();
                    break;
                case "n":
                    break;
            }
        }

        internal static void GetCreationData()
        {
            bool habitConfirmed = false;

            Console.WriteLine("Please enter a habit name:");
            string habitName = Console.ReadLine();

            while (!habitConfirmed)
            {
                Console.WriteLine($"Please confirm habit name (y/n): {habitName}");
                string confirmation = Console.ReadLine();

                switch (confirmation.ToLower())
                {
                    case "y":
                        Console.WriteLine("Please enter a unit of measuement you would like to use for this habit?");
                        string uom = Console.ReadLine();
                        DatabaseAccess.DatabaseCreation(habitName, uom);
                        habitConfirmed = true;
                        break;
                    case "n":
                        continue;
                }
            }
        }
    }
}
