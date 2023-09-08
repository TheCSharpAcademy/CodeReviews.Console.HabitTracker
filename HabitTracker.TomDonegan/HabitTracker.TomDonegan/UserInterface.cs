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

                Helpers.DisplayHeader("Welcome to your Habit Tracker");
                Helpers.DisplayHeader($"Current Habit: {currentHabit}");

                Console.WriteLine("What would like to do today?\n");
                Console.WriteLine("1 - View all habit data.");
                Console.WriteLine("2 - Add new habit entry.");
                Console.WriteLine("3 - Update an existing entry.");
                Console.WriteLine("4 - Delete an entry.");
                Console.WriteLine("5 - Create a new habit.");
                Console.WriteLine("6 - Switch habit.");
                Console.WriteLine("7 - Delete a habit.");
                Console.WriteLine("8 - View date range habit data.");
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
                        HabitDataHandler.ModifyEntry("update", currentHabit);
                        break;
                    case "4":
                        HabitDataHandler.ModifyEntry("delete", currentHabit);
                        break;
                    case "5":
                        HabitManagementHandler.AddRemoveHabit(currentHabit, "create");
                        break;
                    case "6":
                        currentHabit = HabitDataHandler.SwitchHabit();
                        break;
                    case "7":
                        HabitManagementHandler.AddRemoveHabit(currentHabit, "delete");
                        break;
                    case "8":
                        HabitDataHandler.ViewHabitDataOverPeriod(currentHabit);
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
