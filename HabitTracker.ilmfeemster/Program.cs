using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
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
                Ammount REAL NOT NULL,
                OccurenceDate TEXT NOT NULL,
                HabitId INTEGER,
                FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                    ON DELETE SET NULL
                )";
                cmd.ExecuteNonQuery();
            }
        }

        // User interface
        bool menuActive = true;
        bool habitMenuActive = false;

        System.Console.WriteLine("Welcome to the Run Tracker");
        System.Console.WriteLine("Select an option:\n");

        while (menuActive)
        {
            System.Console.WriteLine("1: View Runs");
            System.Console.WriteLine("0: Exit Program");
            System.Console.Write("Your selection: ");

            string? input = System.Console.ReadLine();
            if (input == null)
            {
                input = string.Empty;
            }

            switch (input)
            {
                case "0":
                    menuActive = false;
                    System.Console.WriteLine("Goodbye");
                    break;

                case "1":
                    habitMenuActive = true;
                    break;
            }

            while (habitMenuActive)
            {
                System.Console.WriteLine("\nSelect an option:\n");
                System.Console.WriteLine("1: Add Run");
                System.Console.WriteLine("2: Update Run");
                System.Console.WriteLine("3: Delete Run");
                System.Console.WriteLine("0: Previous Menu");

                string? habitInput = System.Console.ReadLine();
                input = habitInput != null ? habitInput.ToLower() : string.Empty;

                switch (input)
                {
                    case "0":
                        habitMenuActive = false;
                        break;

                    case "1":
                        RunEntry newRun = AddRunPrompt();
                        break;
                }
            }

            // Menu header presented after any input
            System.Console.WriteLine("\nSelect an option: \n");
        }
    }

    // Define a class to represent a run

    public class RunEntry
    {
        public double Miles { get; set; }

        public DateTime Date { get; set; }
    }

    // Function to collect input from user

    public static RunEntry AddRunPrompt()
    {
        double miles;
        DateTime runDate;

        System.Console.WriteLine("\n How many miles did you run?");
        string milesInput = System.Console.ReadLine();
        while (!double.TryParse(milesInput, out miles))
        {
            System.Console.WriteLine("Enter a valid number");
            milesInput = System.Console.ReadLine();
        }

        System.Console.WriteLine("What date did you run? (YYYY-MM-DD)");
        string runDateInput = System.Console.ReadLine();
        while (!DateTime.TryParse(runDateInput, out runDate))
        {
            System.Console.WriteLine("Enter a valid date (YYYY-MM-DD)");
            runDateInput = System.Console.ReadLine();
        }

        RunEntry runEntry = new RunEntry { Miles = miles, Date = runDate };

        System.Console.WriteLine(runEntry);
        return runEntry;
    }
}