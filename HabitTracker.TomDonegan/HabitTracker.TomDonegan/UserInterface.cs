using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace HabitTracker.TomDonegan
{
    internal class UserInterface
    {
        internal static async Task MainMenu()
        {
            bool runningTracker = false;
            string currentHabit = "drinking_water";

            while (!runningTracker)
            {
                Console.Clear();

                Helpers.DisplayHeader($"Current Habit: {currentHabit}");
                Helpers.DisplayHeader("Welcome to your Habit Tracker");

                Console.WriteLine("What would like to do today?\n");
                Console.WriteLine("1 - View all habit data.");
                Console.WriteLine("2 - Add new habit entry.");
                Console.WriteLine("3 - Update an existing entry.");
                Console.WriteLine("4 - Delete an entry.");
                Console.WriteLine("5 - Create a new habit.");
                Console.WriteLine("6 - Switch habit.");
                Console.WriteLine("0 - Exit Habit Tracker");

                string menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        HabitDataHandler.ViewAllHabitData(currentHabit);
                        break;
                    case "2":
                        HabitDataHandler.InsertHabitData(currentHabit);
                        break;
                    case "3":
                        //UpdateEntry();
                        HabitDataHandler.ModifyEntry("update", currentHabit);
                        break;
                    case "4":
                        //DeleteEntry();
                        HabitDataHandler.ModifyEntry("delete", currentHabit);
                        break;
                    case "5":
                        HabitCreationHandler.CreateNewHabit(currentHabit);
                        break;
                    case "6":
                        currentHabit = SwitchHabit();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static string SwitchHabit()
        {
            ArrayList habitList = DatabaseAccess.GetTableList();

            foreach (var habit in habitList)
            {
                Console.WriteLine(habit);
            }

            Console.WriteLine("Please select a habit by typing its name.");
            string selectedHabit = Console.ReadLine();

            while (!habitList.Contains(selectedHabit))
            {
                Console.WriteLine($"{selectedHabit} does not exist, please ensure the name is typed correctly.");
                selectedHabit = Console.ReadLine();
            }

            return selectedHabit;
        }
    }
}
