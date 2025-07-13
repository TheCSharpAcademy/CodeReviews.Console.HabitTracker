using System.Globalization;

namespace HabitTracker
{
    internal class Helpers
    {
        private static DataAccess _dataAccess;

        internal static void GetUserMenu(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;

            //Console.Clear();

            bool closeApp = false;
            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("\n=== HABIT TRACKER - MAIN MENU ===");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("Type 0 to close the application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Records.");
                Console.WriteLine("Type 3 to Delete Records.");
                Console.WriteLine("Type 4 to Update Records.");
                Console.WriteLine("Type 5 to Manage Habits");
                Console.WriteLine("==================================\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nSee you!");
                        closeApp = true;
                        break;
                    case "1":
                        dataAccess.SelectAllRecords();
                        break;
                    case "2":
                        dataAccess.InsertRecord();
                        break;
                    case "3":
                        dataAccess.DeleteRecord();
                        break;
                    case "4":
                        dataAccess.UpdateRecord();
                        break;
                    case "5":
                        ShowHabitsMenu(dataAccess);
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        private static void ShowHabitsMenu(DataAccess dataAccess)
        {
            bool backToMainMenu = false;
            while (!backToMainMenu)
            {
                Console.Clear();
                Console.WriteLine("\n=== HABITS MANAGEMENT ===");
                Console.WriteLine("What would you like to to?");
                Console.WriteLine("Type 0 to Return to Main Menu.");
                Console.WriteLine("Type 1 to View All Habits.");
                Console.WriteLine("Type 2 to Add a New Habit.");
                Console.WriteLine("Type 3 to Delete Your Habit.");
                Console.WriteLine("Type 4 to Get Habit Statistics.");
                Console.WriteLine("============================\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        backToMainMenu = true;
                        break;
                    case "1":
                        dataAccess.ShowAllHabits();
                        break;
                    case "2":
                        dataAccess.AddHabit();
                        break;
                    case "3":
                        dataAccess.DeleteHabit();
                        break;
                    case "4":
                        dataAccess.GetHabitStatistics();
                        break;
                }
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd.mm.yy). Type 0 to return to main menu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserMenu(_dataAccess);

            while (!DateTime.TryParseExact(dateInput, "dd.mm.yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalide date. (Format: dd.mm.yy). Type 0 to return to Main Menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string userInput = Console.ReadLine();

            if (userInput == "0") GetUserMenu(_dataAccess);

            while (!Int32.TryParse(userInput, out _) || Convert.ToInt32(userInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                userInput = Console.ReadLine();
            }

            int checkedInput = Convert.ToInt32(userInput);
            return checkedInput;
        }
    }
}
