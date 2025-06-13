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

                    //Delete existing Habit
                    case "D":
                    case "d":
                        ProcessHabitDelete(inputManager, dbManager);
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

        private static void ProcessHabitDelete(InputManager inputManager, DBManager dbManager)
        {
            //Finding and Validating Habit to Delete
            var habits = dbManager.GetAllExistingHabitEntries();

            if (habits.Count <= 0)
            {
                Console.WriteLine("No habits to delete!");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Enter the ID number of the habit you wish to delete: ");
            while (true)
            {
                int habitId = inputManager.GetValidUserIntegerInput();

                if (!dbManager.ConfirmHabitExists(habitId))
                {
                    Console.Write("Invalid ID. Please choose an ID from the habits listed above: ");
                }
                else
                {
                    dbManager.DeleteExistingHabit(habitId);
                    Console.WriteLine("Habit deleted. Press enter to continue...");
                    Console.ReadLine();
                    break;
                }
            }
        }

        private static void ProcessHabitEdit(InputManager inputManager, DBManager dbManager)
        {
            //Finding and Validating Habit to Edit
            var habits = dbManager.GetAllExistingHabitEntries();
            Habit? habit;

            if (habits.Count <= 0)
            {
                Console.WriteLine("No habits to edit!");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                return;
            }

            habit = FetchValidHabit(inputManager, dbManager);

            //Editing the Habit
            EditExistingHabit(inputManager, dbManager, habit);
        }

        private static Habit? FetchValidHabit(InputManager inputManager, DBManager dbManager)
        {
            Habit? habit;
            Console.WriteLine("Enter the ID number of the habit you wish to edit: ");
            while (true)
            {
                int habitId = inputManager.GetValidUserIntegerInput();

                if (!dbManager.GetExistingHabitByID(habitId, out habit))
                {
                    Console.Write("Invalid ID. Please choose an ID from the habits listed above: ");
                }
                else
                {
                    break;
                }
            }

            return habit;
        }

        private static void EditExistingHabit(InputManager inputManager, DBManager dbManager, Habit? habit)
        {
            Console.Clear();

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Current Habit Details: " + habit);
            Console.WriteLine("------------------------------------------------------------");

            DateTime habitDate;
            string habitType, habitUnits;
            int habitQuantity;
            GetHabitInfoFromUser(inputManager, out habitDate, out habitType, out habitQuantity, out habitUnits);

            habit.Date = habitDate;
            habit.HabitType = habitType;
            habit.Quantity = habitQuantity;
            habit.UnitOfMeasurement = habitUnits;

            dbManager.UpdateExistingHabit(habit);
            Console.Write("Entry updated! Press enter to continue...");
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
            Console.WriteLine("\tD - Delete an Existing Habit");
            Console.WriteLine("\tQ - Quit");
            Console.WriteLine("------------------------------------------------------------");
            Console.Write("Please Make A Selection: ");

            string input;
            while (true)
            {
                input = inputManager.GetValidUserStringInput();
                var validChoices = new List<string> { "A", "E", "D", "Q"};

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