using System.Data;
using Microsoft.Data.Sqlite;

class Program
{
    static string dbPath = "habitdatabase.db";
    static void Main()
    {
        // DB INIT or OPEN
        using (var connection = new SqliteConnection($"Data Source={dbPath}"))
        {
            connection.Open();


            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "PRAGMA foreign_keys = ON;";
                cmd.ExecuteNonQuery();

            }


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

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                INSERT OR IGNORE INTO Habit (Name, Unit)
                VALUES ('Run', 'Mile(s)')
                ";
                cmd.ExecuteNonQuery();
            }
        }

        // CONSOLE MENU
        bool menuActive = true;
        bool habitMenuActive = false;
        bool updateMenuActive = false;
        bool deleteMenuActive = false;

        System.Console.WriteLine("\n\n\nRun Tracker");
        System.Console.WriteLine("Select an option:\n");

        while (menuActive)
        {
            System.Console.WriteLine("1: View Runs");
            System.Console.WriteLine("0: Exit Program");
            System.Console.Write("\nYour selection: ");

            string? input = System.Console.ReadLine();
            input ??= string.Empty;

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
                    case "2":
                        updateMenuActive = true;
                        break;
                    case "3":
                        break;
                }

                while (updateMenuActive)
                {
                    UpdateRunPrompt();
                    updateMenuActive = false;
                }
            }
            System.Console.WriteLine("\nSelect an option: \n");
        }
    }
    public class RunEntry
    {
        public double Miles { get; set; }

        public DateTime Date { get; set; }
    }

    // INTERFACE FUNCTIONS

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

    public static void UpdateRunPrompt()
    {
        System.Console.WriteLine("\nSelect a \"Run ID\" to update:\n");
        ShowRuns();
        System.Console.WriteLine("\nCancel with 0");
        System.Console.Write("\nYour Selection: ");
        string updateInput = System.Console.ReadLine();
        ValidId(updateInput);
    }

    // DB FUNCTIONS
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

    // Input Handling Functions
    public static void ValidId(string id)
    {
        int validId;
        while (!int.TryParse(id, out validId))
        {
            System.Console.WriteLine("\nEnter a valid number:");
            id = System.Console.ReadLine();
        }

        if (validId == 0)
        {
            return;
        }
        else
        {
            System.Console.WriteLine($"You selected: {validId}");
            return;
        }
    }
}