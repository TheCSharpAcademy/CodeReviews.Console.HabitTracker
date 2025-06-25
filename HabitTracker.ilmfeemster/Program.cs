using System.Data;
using Microsoft.Data.Sqlite;

class Program
{
    static string dbPath = "habitdatabase.db";
    static void Main()
    {
        // Create or open database file
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
                Name TEXT NOT NULL UNIQUE,
                Unit TEXT NOT NULL
                )";
                cmd.ExecuteNonQuery();
            }

            // Create Occurrence table if needed
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Occurrence (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Amount REAL NOT NULL,
                OccurrenceDate TEXT NOT NULL,
                HabitId INTEGER,
                FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                    ON DELETE SET NULL
                )";
                cmd.ExecuteNonQuery();
            }

            // Create Run Habit Column
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                INSERT OR IGNORE INTO Habit (Name, Unit)
                VALUES ('Run', 'Mile(s)')
                ";
                cmd.ExecuteNonQuery();
            }
        }

        // User interface
        bool menuActive = true;
        bool habitMenuActive = false;

        System.Console.WriteLine("\n\n\nRun Tracker");
        System.Console.WriteLine("Select an option:\n");

        while (menuActive)
        {
            System.Console.WriteLine("1: View Runs");
            System.Console.WriteLine("0: Exit Program");
            System.Console.Write("\nYour selection: ");

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
                ShowRuns();

                System.Console.WriteLine("\nSelect an option:\n");
                System.Console.WriteLine("1: Add Run");
                System.Console.WriteLine("2: Update Run");
                System.Console.WriteLine("3: Delete Run");
                System.Console.WriteLine("0: Previous Menu");
                System.Console.Write("\nYour Selection: ");

                string? habitInput = System.Console.ReadLine();
                input = habitInput != null ? habitInput.ToLower() : string.Empty;

                switch (input)
                {
                    case "0":
                        habitMenuActive = false;
                        break;

                    case "1":
                        AddRunPrompt();
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

    public static void AddRunPrompt()
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

        InsertRun(runEntry);
    }

    // Function to add input to DB
    public static void InsertRun(RunEntry currentRun)
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                INSERT INTO Occurrence (Amount, OccurrenceDate, HabitId)
                VALUES (@miles, @date, (SELECT Id FROM Habit WHERE Name LIKE 'Run' LIMIT 1))
                ";

                cmd.Parameters.AddWithValue("@miles", currentRun.Miles);
                cmd.Parameters.AddWithValue("@date", currentRun.Date.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void ShowRuns()
    {
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                SELECT Id, Amount, OccurrenceDate
                FROM Occurrence
                ";

                using (var reader = cmd.ExecuteReader())
                {
                    System.Console.WriteLine("\nRun History:");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        double miles = reader.GetDouble(1);
                        string date = reader.GetString(2);

                        System.Console.WriteLine($"ID: {id} Miles: {miles} Date: {date}");
                    }
                }
            }
        }
    }
}