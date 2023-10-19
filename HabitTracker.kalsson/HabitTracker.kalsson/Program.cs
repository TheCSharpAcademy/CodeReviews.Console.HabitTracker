// Initialize SQLite database to ensure it exists before proceeding
// This will create a new database file if it doesn't already exist.
DatabaseManager.InitializeDatabase();

// Create table in the initialized SQLite database
// This will create the 'habits' table if it doesn't already exist.
DatabaseManager.CreateTable();

ConsoleMessages.AppInformation();


Console.ReadLine();