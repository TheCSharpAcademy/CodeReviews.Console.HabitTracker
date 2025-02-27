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

            switch (menuOption)
            {
                case 1:
                    // TODO: Write method for getting input from user for a READ statement
                    Console.WriteLine("Printed values will go here...");
                    break;
                case 2:
                    // TODO: Write method for INPUT statement
                    AddRow();
                    break;
                case 3:
                    // TODO: Write method for UPDATE statement
                    Console.WriteLine("Update a value...");
                    break;
                case 4:
                    // TODO: Write method for DELETE statement
                    Console.WriteLine("Delete a value...");
                    break;
                default:
                    // This shouldn't be reachable
                    throw new Exception("Error fetching menu selection.");
            }
        }

        // Handles inserting new rows into table
        private void AddRow()
        {
            string title;

            do
            {
                Console.WriteLine("Please enter the name of your new habit:");
                title = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrEmpty(title));

            _db.AddHabit(title);
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\n1. View logged habits");
            Console.WriteLine("2. Add a new habit");
            Console.WriteLine("3. Update an existing habit");
            Console.WriteLine("4. Delete an existing habit\n");
        }
    }
}
