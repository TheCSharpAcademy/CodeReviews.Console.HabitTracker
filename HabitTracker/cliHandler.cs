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
                    _db.OutputTable();
                    Console.WriteLine("Printed values will go here...");
                    break;
                case 2:
                    AddRow();
                    break;
                case 3:
                    // TODO: Write method for UPDATE statement
                    // TODO: User will need to provide habit ID
                    Console.WriteLine("Update a value...");
                    break;
                case 4:
                    // TODO: Write method for DELETE statement
                    // TODO: User will need to provide habit ID
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
                Console.WriteLine("Please enter the name of your new habit (max length: 50 char):");
                title = Console.ReadLine()?.Trim() ?? "";
                
                if (title.Length > 50)
                {
                    Console.WriteLine("Habit length is too long.");
                }
            } while (string.IsNullOrEmpty(title) || title.Length > 50);

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
