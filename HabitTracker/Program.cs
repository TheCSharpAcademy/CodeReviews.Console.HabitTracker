using HabitDatabaseLibrary;
using ConsoleTables;
using System.Reflection.PortableExecutable;
using System.CodeDom.Compiler;
using System.ComponentModel.Design;

namespace HabitTrackerProgram
{
    class Program
    {

        static Database db = new Database();
        const string connectionString = "Data Source=database.db;Version=3;new=True;";

        static void Main(string[] args)
        {
            bool exit = false;
            string? userInput;
            int optionSelected;

            db.CreateDatabase(connectionString);

            do
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Logs"); // Read
                Console.WriteLine("Type 2 to Delete a Specific Logged Date"); // Delete
                Console.WriteLine("Type 3 to Log an instance of coffee consumed"); // Create + Update
                Console.WriteLine("------------------------------------------");
                Console.Write("Option: ");
                userInput = Console.ReadLine();
                if (userInput == null || userInput == "")
                {
                    Console.WriteLine("Error: Menu option cannot be empty.");
                    continue;
                }
                else if (!int.TryParse(userInput, out optionSelected))
                {
                    Console.WriteLine("Error: Menu option must be an intergal number.");
                    continue;
                }
                else
                {
                    switch (optionSelected)
                    {
                        case 0: // Exit
                            break;
                        case 1: // View Data
                            ViewCoffeeConsumed();
                            break;
                        case 2: // Delete Specific Logged Dare
                            DeleteCoffeeLog();
                            break;
                        case 3: // Create Update a log
                            CreateUpdateCoffeeLog();
                            break;
                        default:
                            Console.WriteLine("Invalid Menu Option.");
                            break;
                    }
                }
            } while (!exit);
        }

        static void ViewCoffeeConsumed()
        {
            var results = db.ReadData(connectionString);
            if (results != null)
            {
                var table = new ConsoleTable("Date", "Cups of coffee Consumed.");
                while (results.Read())
                {
                    table.AddRow(results["Date"], results["Count"]);
                }
                table.Write();
            }
            else
            {
                Console.WriteLine("No results found. Table may not exist.");
            }
        }

        static void DeleteCoffeeLog()
        {
            // Delete
        }

        static void CreateUpdateCoffeeLog()
        {
            // Create Update
        }
    }
}