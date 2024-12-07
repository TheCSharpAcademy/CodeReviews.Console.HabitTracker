using System.Globalization;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Alvind0.HabitTracker;

internal class Database
{
    private const string ConnectionString = @"Data Source=habit-Tracker.db";
    internal record WalkingRecord(int Id, DateTime Date, int Quantity, string Unit);
    internal record HabitTypes(string Habit);

    internal static void CreateDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(ConnectionString))
        {
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();

                // TODO: Seed data automatically on DB creation
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS walkingHabit(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER,
                    Unit TEXT
                    )";
                tableCmd.ExecuteNonQuery();
            }
        }
    }

    internal static void AddRecord()
    {
        var date = GetDate("\nEnter the date (format - dd-MM-yy) or insert 0 to go back to Main Menu:\n");
        if (date == null)
        {
            Console.Clear();
            return;
        }

        var quantity = GetNumber("\nEnter the number of meters walked(positive integers only) or enter 0 to go back to Main Menu:\n");
        if (quantity == -1)
        {
            Console.Clear();
            return;
        }

        Console.Clear();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO walkingHabit(date, quantity, unit) VALUES(@date, @quantity, \'Km\')";

            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);
            tableCmd.ExecuteNonQuery();
        }
        Console.Clear();
        Console.WriteLine("Successful.");
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        int id = 0;
        while (true)
        {
            GetRecords();
            id = GetNumber("Please type the id of the record you want to delete or insert 0 to Go Back to Main Menu:\n");
            if (id == -1)
            {
                Console.Clear();
                return;
            }
            if (CheckIfIdExists(id)) break;
            else
            {
                Console.Clear();
                Console.WriteLine("Id does not exist.");
            }
        }

        using (var connection = new SqliteConnection(ConnectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = @$"DELETE FROM walkingHabit WHERE Id = @id";
                tableCmd.Parameters.AddWithValue("@id", id);
                tableCmd.ExecuteNonQuery();
            }
        }
        Console.Clear();
        Console.WriteLine("Successful.");
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        int id = 0;
        while (true)
        {
            GetRecords();
            id = GetNumber("Please type the id of the record you want to update or insert 0 to Go Back to Main Menu:\n");
            if (id == -1)
            {
                Console.Clear();
                return;
            }
            if (CheckIfIdExists(id)) break;
            else
            {
                Console.Clear();
                Console.WriteLine("Id does not exist.");
            }
        }

        string date = "";
        bool updateDate = AnsiConsole.Confirm("Update date?");
        if (updateDate)
        {
            date = GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
            if (date == null) return;
        }

        int quantity = 0;
        bool updateQuantity = AnsiConsole.Confirm("Update distance?");
        if (updateQuantity)
        {
            quantity = GetNumber("\nPlease enter number of meters walked (no decimals or negatives allowed) or enter 0 to go back to Main Menu.");
        }

        string query;
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            if (updateDate && updateQuantity)
            {
                query = $"UPDATE walkingHabit SET Date = @date, Quantity = @quantity WHERE Id = @id";
            }
            else if (updateDate && !updateQuantity)
            {
                query = $"UPDATE walkingHabit SET Date = @date WHERE Id = @id";
            }
            else
            {
                query = $"UPDATE walkingHabit SET Quantity = '@quantity' WHERE Id = @id";
            }

            tableCmd.CommandText = query;
            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);
            tableCmd.Parameters.AddWithValue("@id", id);
            tableCmd.ExecuteNonQuery();
        }

        Console.Clear();
        Console.WriteLine("Successful.");
    }

    internal static void ViewRecords(List<WalkingRecord> records)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("Id").Centered());
        table.AddColumn(new TableColumn("Date"));
        table.AddColumn(new TableColumn("Amount"));

        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.Date.ToShortDateString(), record.Quantity.ToString("N0") + record.Unit);
        }
        AnsiConsole.Write(table);

    }

    internal static void AddHabit()
    {
        string habitName, unitOfMeasurement;

        while (true)
        {
            GetHabits();
            habitName = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the habit or input 0 to go back to main menu"));
            if (habitName == "0") return;

            if (CheckIfHabitExists(habitName))
            {
                Console.Clear();
                Console.WriteLine("Habit already exists.");
                continue;
            }

            unitOfMeasurement = AnsiConsole.Ask<string>("Enter the unit of measurement or input 0 to go back to main menu\n");
            if (unitOfMeasurement == "0") return;
            else break;
        }

        using (SqliteConnection connection = new(ConnectionString))
        {
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $@"
CREATE TABLE IF NOT EXISTS {habitName}(
id INTEGER PRIMARY KEY AUTOINCREMENT,
Date TEXT,
Quantity INTEGER,
{unitOfMeasurement} TEXT
)";

                tableCmd.ExecuteNonQuery();
            }
        }
    }

    internal static void DeleteHabit()
    {
        string habitName;

        while (true)
        {
            GetHabits();
            habitName = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the habit or input 0 to go back to main menu"));
            if (habitName == "0") return;

            if (!CheckIfHabitExists(habitName))
            {
                Console.Clear();
                Console.WriteLine("Habit does not exist");
                continue;
            }
            break;
        }

        using (SqliteConnection connection = new(ConnectionString))
        {
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"DROP TABLE IF EXISTS {habitName}";
                tableCmd.ExecuteNonQuery();
            }
        }

        Console.Clear();
        Console.WriteLine("Successful.");
    }

    internal static bool CheckIfIdExists(int id)
    {
        using (SqliteConnection connection = new(ConnectionString))
        {
            connection.Open();
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = $"SELECT count(1) FROM walkingHabit WHERE Id = @id";
                tableCmd.Parameters.AddWithValue("@id", id);
                var result = (long)tableCmd.ExecuteScalar();
                return result == 1 ? true : false;
            }

        }
    }

    internal static bool CheckIfHabitExists(string table)
    {
        using (SqliteConnection connection = new(ConnectionString))
        {
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = \"{table}\"";
                return Convert.ToInt64(tableCmd.ExecuteScalar()) == 1 ? true : false;
            }
        }
    }

    // TODO: Implement
    internal static void ViewHabits(List<HabitTypes> habits)
    {
        var table = new Table();
        table.Border = TableBorder.Horizontal;
        table.AddColumn(new TableColumn("Habits"));

        foreach (var habit in habits)
        {
            table.AddRow(habit.Habit);
        }
        AnsiConsole.Write(table);

    }
    // TODO: Implement
    internal static void GetHabits()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            List<HabitTypes> habitTypes = new();
            connection.Open();
            using (SqliteCommand tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type = 'table' AND NOT name = \'sqlite_sequence\'";

                using (SqliteDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitTypes.Add(
                                new HabitTypes(
                                    reader.GetString(0)
                                ));
                        }
                    }
                    else Console.WriteLine("No rows found.");
                }
            }

            Console.Clear();
            ViewHabits(habitTypes);
        }
    }
    internal static void GetRecords(bool isFromMenu = false)
    {
        List<WalkingRecord> records = new List<WalkingRecord>();

        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM walkingHabit";

            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            records.Add(
                                new WalkingRecord(
                                    reader.GetInt32(0),
                                    DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                    reader.GetInt32(2),
                                    reader.GetString(3)
                                ));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error parsing data: {ex.Message}. Skipping this record.");
                        }
                    }
                }
                else Console.WriteLine("No rows found.");
            }
        }
        if (isFromMenu) Console.Clear();
        ViewRecords(records);
    }

    internal static string GetDate(string message)
    {
        Console.WriteLine(message);

        string? dateInput = Console.ReadLine();

        if (dateInput == "0") return null;

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Console.Clear();
            Console.WriteLine("Invalid date (format dd-MM-yy). Please try again.\n");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static int GetNumber(string message)
    {
        Console.WriteLine(message);

        string? numberInput = Console.ReadLine();
        int output;

        if (numberInput == "0") return -1;

        while (!int.TryParse(numberInput, out output) || Convert.ToInt32(numberInput) <= 0)
        {
            Console.Clear();
            Console.WriteLine("Invalid number (positive integers only). Please try again.\n");
            numberInput = Console.ReadLine();
        }
        return output;
    }


}
