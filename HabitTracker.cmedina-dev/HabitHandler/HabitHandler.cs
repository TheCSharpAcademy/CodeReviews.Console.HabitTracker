namespace HabitHandler
{
    using StatLibrary;
    using DatabaseHandler;
    using HabitLibrary;

    public sealed class HabitHandler
    {
        static readonly Database db = new("HabitsDB");
        static readonly HashSet<string> habitList = db.InitializeDatabase();

        public static void CreateHabitMenu()
        {
            Console.Clear();
            if (IsNewHabit())
            {
                CreateHabit();
            }
            else
            {
                AddRecord();
            }
        }
        private static void AddRecord()
        {
            DisplayHabits();
            Console.WriteLine("Enter the name of the habit you want to add an entry for:");

            string habitName = Console.ReadLine();
            string habit = habitList.FirstOrDefault(existingName => existingName == habitName)!;

            while (habit == null)
            {
                Console.Clear();
                DisplayHabits();
                Console.WriteLine("Please enter one of the above habits: ");
                habitName = Console.ReadLine();
                habit = habitList.FirstOrDefault(existingName => existingName == habitName)!;
            }

            string statName = db.GetStatName(habit);
            Console.WriteLine($"Tracking {habit} via {statName}");
            Console.WriteLine("Enter an amount for this entry: ");
            string amount = Console.ReadLine();
            int value = 0;

            while (!int.TryParse(amount, out value))
            {
                Console.Clear();
                Console.WriteLine("Please enter a valid number:");
                amount = Console.ReadLine();
            }

            Habit newEntry = new()
            {
                Name = habitName,
                Stat = new Stat(statName, value)
            };

            try
            {
                db.Create(newEntry.Name, newEntry.Stat);
            }
            catch
            {
                Console.WriteLine("There was an error adding the record.");
                DisplayContinue();
                return;
            }

            Console.WriteLine("Entry successfully logged!");
            DisplayContinue();
        }
        private static void CreateHabit()
        {
            Console.WriteLine("Enter a name for your habit: ");
            string habitName = Console.ReadLine();

            Console.WriteLine("Next, enter the stat you'd like to track: ");
            string statName = Console.ReadLine();

            Console.WriteLine("Finally, enter the starting value for this stat: ");
            string statValue = Console.ReadLine();
            int statResult;

            while (!int.TryParse(statValue, out statResult))
            {
                Console.WriteLine("Please enter a whole number.");
                statValue = Console.ReadLine();
            }

            Habit habit = new()
            {
                Name = habitName,
                Stat = new Stat(statName, statResult)
            };

            try
            {
                db.Create(habit.Name, habit.Stat);
                habitList.Add(habitName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Looks like there was an error entering your stat: {ex}");

                DisplayContinue();
                return;
            }

            Console.WriteLine("Habit successfully created!");

            DisplayContinue();
        }
        public static void UpdateHabit()
        {
            Console.Clear();
            DisplayHabits();

            Console.WriteLine("Enter the name of the habit to update:");
            string habitName = Console.ReadLine();
            string habit = habitList.FirstOrDefault(existingName => existingName == habitName)!;

            while (habit == null)
            {
                Console.Clear();
                DisplayHabits();
                Console.WriteLine("Please enter one of the above habits: ");
                habitName = Console.ReadLine();
                habit = habitList.FirstOrDefault(existingName => existingName == habitName)!;
            }

            string statName = db.GetStatName(habit);

            Console.Clear();
            Console.WriteLine($"{habit} tracking {statName}\n");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("n: Change the habit name");
            Console.WriteLine("s: Change the stat name");
            Console.WriteLine("0: Return to main menu");

            string input = Console.ReadLine();
            switch (input)
            {
                case "n":
                    ChangeHabitName(habit);
                    break;
                case "s":
                    ChangeStatName(habit);
                    break;
                case "0":
                    break;
                default:
                    break;
            }
        }
        public static void DeleteHabitDB()
        {
            Console.Clear();
            DisplayHabits();

            Console.WriteLine("Enter the name of the habit you want to delete: ");
            string habitName = Console.ReadLine();

            if (!db.TableExists(habitName))
            {
                Console.WriteLine("Unable to locate that habit, please try again.");
                DisplayContinue();
                return;
            }

            db.Drop(habitName);
            PruneHabitList(habitName);
            Console.WriteLine("Habit successfully deleted!");
            DisplayContinue();
        }
        public static void ViewHabit()
        {
            Console.Clear();
            DisplayHabits();

            Console.WriteLine("Enter the name of the habit you want to view: ");
            string habitName = Console.ReadLine();

            try
            {
                //db.Read(habitName);
            }
            catch
            {
                Console.WriteLine("Unable to locate that habit, please try again.");
            }

            DisplayContinue();
        }
        private static void DisplayContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();
        }
        private static bool IsNewHabit()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1 - Create new habit");
            Console.WriteLine("2 - Add entry to existing habit");

            string input = Console.ReadLine();
            while (input != "1" && input != "2")
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1 - Create new habit");
                Console.WriteLine("2 - Add entry to existing habit");
                input = Console.ReadLine();
            }

            return input == "1";
        }
        private static void DisplayHabits()
        {
            Console.WriteLine("Current list of habits:");
            foreach (string habit in habitList)
            {
                Console.WriteLine($"\t* {habit}");
            }
            Console.Write("\n");
        }
        private static void PruneHabitList(string habitName)
        {
            habitList.RemoveWhere(habit => habit == habitName);
        }
        private static void ChangeHabitName(string habit)
        {
            Console.WriteLine("Enter the new name of the habit:");
            string newName = Console.ReadLine();

            string originalHabit = habit;

            try
            {
                if (habitList.Add(newName))
                {
                    PruneHabitList(originalHabit);
                    db.UpdateHabitName(originalHabit, newName);
                    Console.WriteLine("Habit name updated successfully!");
                }
            }
            catch
            {
                Console.WriteLine("Looks like an error occurred trying to change names, please try again!");
                habitList.Add(originalHabit); // Restores the original habit
            }

            DisplayContinue();
        }
        private static void ChangeStatName(string habit)
        {
            Console.WriteLine("Enter the new name of the stat:");
            string newName = Console.ReadLine();

            try
            {
                //db.UpdateStatName(tableName, oldName, newName);
                Console.WriteLine("Stat name updated successfully!");
                //UpdateStatName(oldName, newName);
                DisplayContinue();
            }
            catch
            {
                Console.WriteLine("Looks like an error occurred trying to change names, please try again!");
                DisplayContinue();
                return;
            }
        }
    }
}