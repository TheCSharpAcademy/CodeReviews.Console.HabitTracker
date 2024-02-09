using HabitTracker.nwdorian.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.nwdorian;

internal class DbMethods
{
    static string connectionString = @"Data Source=habit-Tracker.db";

    internal static void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS exercising (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Minutes INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS meditation (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Minutes INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS push_ups (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS studying (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Minutes INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static void SeedDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            var habitNames = GetAllHabits();

            connection.Open();

            var seedCmd = connection.CreateCommand();

            foreach (var name in habitNames)
            {
                if (!RecordsExist(name))
                {
                    var habits = DataGenerator.GenerateHabitData();

                    foreach (var h in habits)
                    {
                        seedCmd.CommandText = $"INSERT INTO {name} (date, {GetHabitValue(name)}) VALUES ('{h.Date.ToString("yy-MM-dd")}', {h.Value})";
                        seedCmd.ExecuteNonQuery();
                    }
                }
            }
            connection.Close();
        }
    }

    internal static void CreateHabit(string habitName, string measurement)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {habitName} (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                {measurement} INTEGER
                )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        Console.WriteLine($"New habit {habitName} was successfully created! Press any key to continue...");
        Console.ReadKey();
    }

    internal static List<string> GetAllHabits()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"SELECT name FROM sqlite_schema WHERE type='table' AND name NOT LIKE 'sqlite_%'";

            List<string> allHabits = new List<string>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    allHabits.Add(reader.GetString(0));
                }
            }
            else
            {
                Console.WriteLine("No habits found.");
            }
            connection.Close();

            return allHabits;
        }
    }

    internal static void GetAllRecords(string habitName)
    {
        List<Habit> habitData = new List<Habit>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM {habitName}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habitData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "yy-MM-dd", new CultureInfo("en-US")),
                            Value = reader.GetInt32(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine($"No rows found in {habitName}.");
            }

            connection.Close();

            if (habitData.Any())
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Clear();
                Console.WriteLine($"Records for {habitName}");
                Console.WriteLine("------------------------------");

                foreach (var h in habitData)
                {
                    Console.WriteLine($"{h.Id} - {h.Date.ToString("dd-MMM-yyyy")} - {GetHabitValue(habitName)}: {h.Value}");
                }

                Console.WriteLine("------------------------------\n");
            }
        }
    }

    internal static string GetHabitValue(string habitName)
    {
        string valueName = string.Empty;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT name FROM PRAGMA_TABLE_INFO('{habitName}')";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    valueName = reader.GetString(0);
                }
            }
        }
        return valueName;
    }

    internal static bool RecordsExist(string habitName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    internal static void Insert(string habitName)
    {
        string date = Helpers.GetDateInput();

        int quantity = Helpers.GetNumberInput("Please insert a number for measurement (no decimals allowed): ");

        string valueName = GetHabitValue(habitName);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $@"INSERT INTO {habitName} (date, {valueName}) VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static void Delete(string habitName)
    {
        Console.Clear();
        GetAllRecords(habitName);

        if (RecordsExist(habitName))
        {
            int recordId = Helpers.GetNumberInput("Please type the Id of the record you want to delete:");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM {habitName} WHERE Id = {recordId}";

                int rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"\nRecord with Id {recordId} was deleted. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    internal static void Update(string habitName)
    {
        Console.Clear();
        GetAllRecords(habitName);

        if (RecordsExist(habitName))
        {
            int recordId = Helpers.GetNumberInput("Please type the Id of the record you want to update:");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. Press any key to continue...");
                    Console.ReadKey();
                    connection.Close();
                }
                else
                {
                    string date = Helpers.GetDateInput();

                    int quantity = Helpers.GetNumberInput("Please insert a number for measurement (no decimals allowed): ");

                    string valueName = GetHabitValue(habitName);

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"UPDATE {habitName} SET date = '{date}', {valueName} = {quantity} WHERE Id = {recordId}";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();

                    Console.WriteLine($"\nRecord with Id {recordId} was updated. Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    internal static void GetWeeklyRecords(string habitName)
    {
        List<Habit> habitData = new List<Habit>();

        var date = DateTime.Now.AddDays(-7);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM {habitName} WHERE Date >= '{date.ToString("yy-MM-dd")}' ORDER BY date";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habitData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "yy-MM-dd", new CultureInfo("en-US")),
                            Value = reader.GetInt32(2)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine($"No rows found in {habitName}.");
            }

            connection.Close();

            if (habitData.Any())
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Clear();
                Console.WriteLine($"Records for {habitName}");
                Console.WriteLine("------------------------------");

                foreach (var h in habitData)
                {
                    Console.WriteLine($"{h.Id} - {h.Date.ToString("dd-MMM-yyyy")} - {GetHabitValue(habitName)}: {h.Value}");
                }

                Console.WriteLine("------------------------------\n");
            }
        }
    }
}

