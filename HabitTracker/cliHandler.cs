using System.Diagnostics.Tracing;

namespace HabitTracker
{
    internal class CliHandler
    {
        private readonly DatabaseManager _db;
        private bool _runApp = true;

        internal CliHandler(DatabaseManager db)
        {
            _db = db;
        }

        internal void RunApplication()
        {
            _db.EnsureDatabaseExists();

            Console.WriteLine("Welcome to your habit tracker!");

            while (_runApp)
            {
                Console.WriteLine("Please select an option from the following menu.");

                DisplayMenu();

                int menuOption;

                do
                {
                    if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
                    {
                        menuOption = choice;
                        break;
                    }

                    Console.WriteLine("Invalid input. Please enter an integer from the above menu.");
                } while (true);

                Console.WriteLine("");

                switch (menuOption)
                {
                    case 1:
                        PrintHabits();
                        break;
                    case 2:
                        AddRow();
                        break;
                    case 3:
                        PrintHabits();
                        UpdateRow();
                        break;
                    case 4:
                        PrintHabits();
                        DeleteRow();
                        break;
                    case 5:
                        _runApp = false;
                        break;
                    default:
                        // This shouldn't be reachable
                        throw new Exception("Error fetching menu selection.");
                }

            }
        }

        private void AddRow()
        {
            string title = GetTitle();

            _db.AddHabit(title);
        }

        private void UpdateRow()
        {
            Console.WriteLine("Please enter the ID of the habit you'd like to modify.\n");

            int taskId = GetId();

            Console.WriteLine("Please select the number for the column you'd like to modify:\n");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Completed\n");

            int column;

            do
            {
                if (int.TryParse(Console.ReadLine(), out int choice) && (choice is 1 or 2))
                {
                    column = choice;
                    break;
                }

                Console.WriteLine("Invalid input. Please enter either '1' or '2'.");
            } while (true);

            if (column == 1)
            {
                string title = GetTitle();
                _db.UpdateHabit(taskId.ToString(), "title", title);
            }
            else if (column == 2)
            {
                string completionStatus = GetBool();
                _db.UpdateHabit(taskId.ToString(), "completed", completionStatus);
            }
            else
            {
                Console.WriteLine("Invalid selection. Returning to menu.");
            }
        }

        private void DeleteRow()
        {
            Console.WriteLine("Please enter the ID of the habit you'd like to delete.\n");

            int taskId = GetId();

            _db.DeleteHabit(taskId.ToString());
        }

        private static int GetId()
        {
            int taskId;

            do
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    taskId = choice;
                    break;
                }

                Console.WriteLine("Invalid input. Please enter an integer.");
            } while (true);

            return taskId;
        }

        private static string GetTitle()
        {
            string title;

            do
            {
                Console.WriteLine("Please enter the name of your new habit (max length: 50 char):");
                title = Console.ReadLine()?.Trim() ?? "";

                if (title.Length > 50)
                {
                    Console.WriteLine("Habit length is too long.");
                }
            } while (string.IsNullOrEmpty(title) || title.Length > 50);

            return title;
        }

        private static string GetBool()
        {
            string result;

            do
            {
                Console.WriteLine("Please enter either 'true' or 'false':");
                result = Console.ReadLine()?.Trim() ?? "";

                if (result == "true" || result == "false")
                    break;
            } while (true);

            return result;
        }

        private void PrintHabits()
        {
            Console.WriteLine("Current habits:\n");
            _db.OutputTable();
            Console.WriteLine();
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\n1. View logged habits");
            Console.WriteLine("2. Add a new habit");
            Console.WriteLine("3. Update an existing habit");
            Console.WriteLine("4. Delete an existing habit");
            Console.WriteLine("5. Quit the app\n");
        }
    }
}
