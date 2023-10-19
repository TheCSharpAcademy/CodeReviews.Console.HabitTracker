// Initialize SQLite database to ensure it exists before proceeding
// This will create a new database file if it doesn't already exist.
DatabaseManager.InitializeDatabase();

// Create table in the initialized SQLite database
// This will create the 'habits' table if it doesn't already exist.
DatabaseManager.CreateTable();

ConsoleMessages.DisplayAppInformation();

ConsoleMessages.ShowMainMenu();

int userInput = ConsoleMessages.GetUserInput();

switch (userInput)
    {
            case 1:
                Console.WriteLine("Create new habit");
                break;
            case 2:
                Console.WriteLine("Read habits");
                break;
            case 3:
                Console.WriteLine("Update habit");
                break;
            case 4:
                Console.WriteLine("Delete habit");
                break;
            case 5:
                Console.WriteLine("Close");
                break;
    }

Console.ReadLine();