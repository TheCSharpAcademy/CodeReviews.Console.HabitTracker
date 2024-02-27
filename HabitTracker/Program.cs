using HabitDatabaseLibrary;

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
            db.ReadData(connectionString);
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
        }

        static void DeleteCoffeeLog()
        {
            string? userInput;
            string formattedData;

            Console.Clear();
            Console.WriteLine("Example input (Feb 12)");
            Console.Write("Enter the Log Date to be removed: ");
            userInput = Console.ReadLine();
            if (userInput != null)
            {
                if (userInput.Length <= 6)
                {
                    userInput = userInput.Trim();
                    formattedData = FormatInput(userInput);
                    db.DeleteData(connectionString, formattedData);

                    Console.WriteLine("Operation Performed.");
                    Console.WriteLine("Press any key to return to menu.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine("Press any key to return to menu.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid Input");
                Console.WriteLine("Press any key to return to menu.");
                Console.ReadKey();
            }
        }

        static void CreateUpdateCoffeeLog()
        {
            int consumedCups;
            string? userInput;

            Console.Clear();
            Console.Write("Please enter cups of coffee consumed: ");
            userInput = Console.ReadLine();
            if (userInput != null)
            {
                if(int.TryParse(userInput, out consumedCups))
                {
                    string month = DateTime.Now.ToString("MMM");
                    string day = DateTime.Now.ToString("dd");
                    string date = $"{month} {day}";
                    date = date.Trim();
                    
                    bool found = db.ReadData(connectionString, check: date, ifExists: true);
                    if (found)
                    {
                        int currentCount = db.GetCurrentCount(connectionString, date);
                        Thread.Sleep(100);
                        db.UpdateData(connectionString, date, consumedCups, currentCount);
                    }
                    else
                    {
                        db.InsertData(connectionString, date, consumedCups);
                    }

                }
                else
                {
                    Console.WriteLine("Invalid input. Press any key to return to menu.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Press any key to return to menu");
                Console.ReadKey();
            }
        }

        static string FormatInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input is null or empty.");
            char[] inputChar = input.ToCharArray();
            inputChar[0] = char.ToUpper(inputChar[0]);
            return new string(inputChar);
        }
    }
}