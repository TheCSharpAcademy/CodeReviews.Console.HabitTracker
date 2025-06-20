using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        bool menuActive = true;

        System.Console.WriteLine("Welcome to the Run Tracker");
        System.Console.WriteLine("Select an option:\n");

        while (menuActive)
        {
            System.Console.WriteLine("1: Add Run");
            System.Console.WriteLine("0: Exit Program");
            System.Console.Write("Your selection: ");

            string input = System.Console.ReadLine();

            switch (input)
            {
                case "0":
                    menuActive = false;
                    System.Console.WriteLine("Goodbye");
                    break;
            }

            // Menu header presented after any input
            System.Console.WriteLine("\nSelect an option: \n");
        }

        // Create or open database file
        string dbPath = "habitdatabase.db";
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();

            //Enable foregin keys requirement
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();

            }

            // Create Habit table if needed
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habit (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Unit TEXT NOT NULL
                )";
                cmd.ExecuteNonQuery();
            }

            // Create Occurence table if needed
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Occurrence (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Ammount INTEGER NOT NULL,
                OccurenceDate TEXT NOT NULL,
                HabitId INTEGER,
                FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                    ON DELETE SET NULL
                )";
                cmd.ExecuteNonQuery();
            }
        }
    }
}