using System.Text.RegularExpressions;

namespace HabitTracker.GoldRino456
{
    class HabitTracker
    {
        static void Main()
        {
            //Init Managers
            DBManager dbManager = DBManager.Instance;
            InputManager inputManager = InputManager.Instance;

            //Init Database
            dbManager.CreateTableIfDoesNotExist();

            bool isAppRunning = true;

            while(isAppRunning)
            {
                Console.Clear();
                
                //Fetch and Display Existing Habits
                var habits = dbManager.GetAllExistingHabitEntries();
                DisplayHabits(habits);

                string selection = ProcessHabitTrackerMenu(inputManager);

                switch(selection)
                {
                    //Add a Habit
                    case "a":
                    case "A":
                        
                        break;

                    //Edit existing Habit
                    case "E":
                    case "e":
                        
                        break;

                    //Quit the app
                    case "Q":
                    case "q":

                        Console.WriteLine("Closing...");
                        isAppRunning = false;
                        break;
                }
            }
        }

        private static string ProcessHabitTrackerMenu(InputManager inputManager)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Main Menu: ");
            Console.WriteLine("\tA - Add a New Habit");
            Console.WriteLine("\tE - Edit an Existing Habit");
            Console.WriteLine("\tQ - Quit");
            Console.WriteLine("------------------------------------------------------------");
            Console.Write("Please Make A Selection: ");

            string input;
            while (true)
            {
                input = inputManager.GetValidUserInput();
                var validChoices = new List<string> { "A", "E", "Q"};

                if (!validChoices.Contains(input, StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Invalid selection. Please select an option from the menu above.");
                    continue;
                }
                else
                {
                    break;
                }
            }

            return input;
        }

        private static void DisplayHabits(List<Habit> habits)
        {
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Habit Entries");
            Console.WriteLine("------------------------------------------------------------");

            if (habits.Count > 0)
            {
                foreach (var habit in habits)
                {
                    Console.WriteLine($"\t{habit}");
                }
            }
            else
            {
                Console.WriteLine("\tNo habits yet to display! Add one below!");
            }
            
        }
    }
}