namespace HabitHandler
{
    using StatLibrary;
    using DatabaseHandler;
    using HabitLibrary;

    public sealed class HabitHandler
    {
        static readonly Database db = new("HabitsDB");
        static readonly List<Habit> habitList = db.InitializeDatabase();

        public static void CreateHabit()
        {
            Console.Clear();
            Console.WriteLine("Enter a name for your habit: ");
            Habit habit = new()
            {
                Name = Console.ReadLine()
            };

            try
            {
                db.Create(habit.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Looks like there was an error creating your habit: {ex}");

                DisplayContinue();
                return;
            }

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

            habit.Stat = new Stat(statName, statResult);

            try
            {
                db.Insert(habit.Name, habit.Stat);
                habitList.Add(habit);
            }
            catch(Exception ex)
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

            if (!db.TableExists(habitName))
            {
                Console.WriteLine("Unable to locate that habit, please try again.");
                DisplayContinue();
                return;
            }

            Habit habit = new();

            foreach (var item in habitList)
            {
                if (item.Name == habitName)
                {
                    habit = item;
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine($"{habit.Name} - Currently at {habit.Stat.value} {habit.Stat.name}\n");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("n: Change the habit name");
            Console.WriteLine("s: Change the stat name");
            Console.WriteLine("v: Change the stat value");
            Console.WriteLine("0: Return to main menu");

            string input = Console.ReadLine();
            switch (input)
            {
                case "n":
                    ChangeHabitName(habit.Name);
                    break;
                case "v":
                    ChangeStatValue(habit.Name, habit.Stat.name);
                    break;
                case "s":
                    ChangeStatName(habit.Name, habit.Stat.name);
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
                db.Read(habitName);
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

        private static void DisplayHabits()
        {
            Console.WriteLine("Current list of habits:");
            foreach (Habit habit in habitList)
            {
                Console.WriteLine($"\t* {habit.Name}");
            }
            Console.Write("\n");
        }

        private static void PruneHabitList(string habitName)
        {
            habitList.RemoveAll(habit => habit.Name == habitName);
        }

        private static void UpdateHabitName(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                return;
            }

            foreach (Habit habit in habitList)
            {
                if (habit.Name == oldName)
                {
                    habit.Name = newName;
                }
            }
        }

        private static void UpdateStatName(string oldName, string newName)
        {
            if (string.IsNullOrEmpty(oldName) || string.IsNullOrEmpty(newName))
            {
                return;
            }

            foreach (Habit habit in habitList)
            {
                if (habit.Stat.name == oldName)
                {
                    habit.Stat.name = newName;
                }
            }
        }

        private static void UpdateStatValue(string statName, int newValue)
        {
            if (string.IsNullOrEmpty(statName))
            {
                return;
            }

            foreach (Habit habit in habitList)
            {
                if (habit.Stat.name == statName)
                {
                    habit.Stat.value = newValue;
                }
            }
        }

        private static void ChangeHabitName(string oldName)
        {
            Console.WriteLine("Enter the new name of the habit:");
            string newName = Console.ReadLine();

            try
            {
                db.UpdateHabitName(oldName, newName);
                Console.WriteLine("Habit name updated successfully!");
                UpdateHabitName(oldName, newName);
                DisplayContinue();
            }
            catch
            {
                Console.WriteLine("Looks like an error occurred trying to change names, please try again!");
                DisplayContinue();
                return;
            }
        }

        private static void ChangeStatName(string tableName, string oldName)
        {
            Console.WriteLine("Enter the new name of the stat:");
            string newName = Console.ReadLine();

            try
            {
                db.UpdateStatName(tableName, oldName, newName);
                Console.WriteLine("Stat name updated successfully!");
                UpdateStatName(oldName, newName);
                DisplayContinue();
            }
            catch
            {
                Console.WriteLine("Looks like an error occurred trying to change names, please try again!");
                DisplayContinue();
                return;
            }
        }

        private static void ChangeStatValue(string tableName, string statName)
        {
            Console.WriteLine("Enter the new numerical value of the stat:");
            string newValue = Console.ReadLine();
            int newStatValue = 0;
            while (!int.TryParse(newValue, out newStatValue))
            {
                Console.WriteLine("Please enter a numerical value.");
                newValue = Console.ReadLine();
            }    

            try
            {
                db.UpdateStatValue(tableName, statName, newStatValue);
                Console.WriteLine("Stat value updated successfully!");
                UpdateStatValue(statName, newStatValue);
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