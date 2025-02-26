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
