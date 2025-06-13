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

                        ProcessHabitCreation(inputManager, dbManager);
                        break;

                    //Edit existing Habit
                    case "E":
                    case "e":

                        ProcessHabitEdit(inputManager, dbManager);
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

        private static void ProcessHabitEdit(InputManager inputManager, DBManager dbManager)
        {
            var habits = dbManager.GetAllExistingHabitEntries();
            int targetHabit = -1;

            Console.WriteLine("Enter the ID number of the habit you wish to edit: ");

            //while()
        }

        private static void ProcessHabitCreation(InputManager inputManager, DBManager dbManager)
        {
            Console.Clear();

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Add a New Habit");
            Console.WriteLine("------------------------------------------------------------");

            DateTime habitDate;
            string habitType, habitUnits;
            int habitQuantity;
            GetHabitInfoFromUser(inputManager, out habitDate, out habitType, out habitQuantity, out habitUnits);

            //Create Habit and Add to DB
            Habit newHabit = new(habitDate, habitType, habitQuantity, habitUnits);
            dbManager.AddHabitToDB(newHabit);

            Console.WriteLine("Habit added! Press enter to continue...");
            Console.ReadLine();
        }

        private static void GetHabitInfoFromUser(InputManager inputManager, out DateTime habitDate, out string habitType, out int habitQuantity, out string habitUnits)
        {
            Console.Write("Enter a date for this habit (MM/DD/YYYY or enter 'T' to use today's date): ");
            habitDate = inputManager.GetValidUserDateTimeInput();
            Console.Write("Enter the name of the habit you want to add:  ");
            habitType = inputManager.GetValidUserStringInput();
            Console.Write("Enter The quantity of this habit:  ");
            habitQuantity = inputManager.GetValidUserIntegerInput();
            Console.Write("Enter the unit of measurement for this habit:  ");
            habitUnits = inputManager.GetValidUserStringInput();
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
                input = inputManager.GetValidUserStringInput();
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