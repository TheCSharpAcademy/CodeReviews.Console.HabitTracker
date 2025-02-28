using System.Diagnostics.Tracing;

namespace HabitTracker
{
    internal class CliHandler
    {
        private readonly DatabaseManager _db;

        internal CliHandler(DatabaseManager db)
        {
            _db = db;
        }

        // Checks for existence of database and handles CRUD operations
        internal void RunApplication()
        {
            _db.EnsureDatabaseExists();

            Console.WriteLine("Welcome to your habit tracker! Please select an option from the following menu.");

            DisplayMenu();

            int menuOption;

            do
            {
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
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
                default:
                    // This shouldn't be reachable
                    throw new Exception("Error fetching menu selection.");
            }
        }

        // Handles inserting new rows into table
        private void AddRow()
        {
            string title = GetTitle();

            _db.AddHabit(title);
        }

        // Handles updating a row in the table
        private void UpdateRow()
        {
            int taskId = GetId();

            Console.WriteLine("Please select the number for the column you'd like to modify:\n");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Completed\n");

            int column;

            do
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice == 1 || choice == 2)
                    {
                        column = choice;
                        break;
                    }
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

        // Handles deleting rows from table
        private void DeleteRow()
        {
            int taskId = GetId();

            _db.DeleteHabit(taskId.ToString());
        }

        // Helper method for grabbing task ID
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

        // Helper method for providing a title for a field
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

        // Helper method for getting a boolean value from user and converting to a string
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

        // Displays the currently logged habits for the user
        private void PrintHabits()
        {
            Console.WriteLine("Current habits:\n");
            _db.OutputTable();
            Console.WriteLine();
        }

        // Prints the main menu options to the user
        private static void DisplayMenu()
        {
            Console.WriteLine("\n1. View logged habits");
            Console.WriteLine("2. Add a new habit");
            Console.WriteLine("3. Update an existing habit");
            Console.WriteLine("4. Delete an existing habit\n");
        }
    }
}
