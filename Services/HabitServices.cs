using Habit_Logger.Data;
using Habit_Logger.UI;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;
using static Habit_Logger.Data.Data;

namespace Habit_Logger.Services
{
    internal class HabitServices
    {

        internal static void InsertHabit()
        {
            string name = AnsiConsole.Ask<string>("Enter the name of the habit:");
            while (string.IsNullOrWhiteSpace(name))
            {
                name = AnsiConsole.Ask<string>("Habit name cannot be empty. Enter the name of the habit:");
            }

            string measurementUnit = AnsiConsole.Ask<string>("Enter the measurement unit for the habit (e.g., 'pushups', 'minutes', etc.):");

            while (string.IsNullOrWhiteSpace(measurementUnit))
            {
                measurementUnit = AnsiConsole.Ask<string>("Measurement unit cannot be empty. Enter the measurement unit for the habit:");
            }

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText =
                        $"INSERT INTO habits(Name, MeasurementUnit) VALUES ('{name}', '{measurementUnit}')";

                    tableCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        internal static void GetHabits()
        {
            List<Habit> habits = new();

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM habits";

                using (SqliteDataReader reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            try
                            {
                                habits.Add(
                                new Habit(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2)
                                    )
                                );
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error getting record: {ex.Message}. ");
                            }
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
            }

            ViewAllHabits(habits);
        }

        internal static void ViewAllHabits(List<Data.Data.Habit> habits)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Measurement Unit");
            foreach (var habit in habits)
            {
                table.AddRow(habit.Id.ToString(), habit.Name, habit.UnitOfMeasurement);
            }
            AnsiConsole.Write(table);
        }

        internal static void DeleteHabit()
        {
            GetHabits();

            var id = GetNumberInput("Please type the id of the habit you want to delete.");

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText =
                            @$"DELETE FROM habits WHERE Id = {id}";

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        internal static void UpdateHabit()
        {
            GetHabits();

            var id = GetNumberInput("Please type the id of the habit you want to update.");

            string name = "";
            bool updateName = AnsiConsole.Confirm("Update name?");
            if (updateName)
            {
                name = AnsiConsole.Ask<string>("Habit's new name:");
                while (string.IsNullOrEmpty(name))
                {
                    name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
                }
            }

            string unit = "";
            bool updateUnit = AnsiConsole.Confirm("Update Unit of Measurement?");
            if (updateUnit)
            {
                unit = AnsiConsole.Ask<string>("Habit's Unit of Measurement:");
                while (string.IsNullOrEmpty(unit))
                {
                    unit = AnsiConsole.Ask<string>("Habit's unit can't be empty. Try again:");
                }
            }

            string query;
            if (updateName && updateUnit)
            {
                query = $"UPDATE habits SET Name = '{name}', MeasurementUnit = '{unit}' WHERE Id = {id}";
            }
            else if (updateName && !updateUnit)
            {
                query = $"UPDATE habits SET Name = '{name}' WHERE Id = {id}";
            }
            else
            {
                query = $"UPDATE habits SET MeasurementUnit = '{unit}' WHERE Id = {id}";
            }

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = query;

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static void InsertProgress()
        {
            string date = GetDateInput("Enter the date. (Format: mm-dd-yyyy). Type 0 to return to the main menu.");

            GetHabits();

            var habitId = GetNumberInput("Enter the ID of the habit for which you want to add a record. Type 0 to return to the main menu.");
            int quantity = GetNumberInput("Enter quantity. Type 0 to return to the main menu.");

            Console.Clear();

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText =
                        $"INSERT INTO progress(date, quantity, habitId) VALUES ('{date}', {quantity}, {habitId})";

                    tableCommand.ExecuteNonQuery();

                }

                connection.Close();
            }
        }

        internal static void GetProgress()
        {
            List<ProgressWithHabit> records = new();

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText = @"
                    SELECT progress.Id, progress.Date, progress.Quantity, progress.HabitId, habits.Name AS HabitName, habits.MeasurementUnit
                    FROM progress
                    INNER JOIN habits ON progress.HabitId = habits.Id";

                    using (SqliteDataReader reader = tableCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    records.Add(new ProgressWithHabit(
                                    reader.GetInt32(0),
                                    DateTime.ParseExact(reader.GetString(1), "MM-dd-yyyy", CultureInfo.InvariantCulture),
                                    reader.GetInt32(2),
                                    reader.GetString(4),
                                    reader.GetString(5)));
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine($"Error parsing record: {ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No progress found.");
                        }
                    }
                }
            }

            ViewAllProgress(records);
        }

        internal static void ViewAllProgress(List<Data.Data.ProgressWithHabit> progress)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Date");
            table.AddColumn("Quantity");
            table.AddColumn("Habit Name");

            foreach (var record in progress)
            {
                table.AddRow(record.Id.ToString(), record.Date.ToString("D"), $"{record.Quantity} {record.MeasurementUnit}", record.HabitName.ToString());
            }

            AnsiConsole.Write(table);
        }

        internal static void DeleteProgress()
        {
            GetProgress();

            var id = GetNumberInput("Enter the ID of the record you want to delete. Type 0 to return to the main menu.");

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText =
                        $"DELETE FROM progress WHERE Id = {id}";

                    int rowsAffected = tableCommand.ExecuteNonQuery();
                    if (rowsAffected != 0)
                    {
                        Console.WriteLine($"Record with ID {id} deleted successfully.");
                    }
                }

                connection.Close();
            }
        }

        internal static void UpdateProgress()
        {
            GetProgress();

            var id = GetNumberInput("Enter the ID of the record you want to update. Type 0 to return to the main menu.");

            string date = "";
            bool updateDate = AnsiConsole.Confirm("Do you want to update the date?");
            if (updateDate)
            {
                date = GetDateInput("Enter the new date. (Format: mm-dd-yyyy). Type 0 to return to the main menu.");
            }

            int quantity = 0;
            bool updateQuantity = AnsiConsole.Confirm("Do you want to update the quantity?");
            if (updateQuantity)
            {
                quantity = GetNumberInput("Enter the new quantity. Type 0 to return to the main menu.");
            }

            string query;
            if (updateDate && updateQuantity)
            {
                query = $"UPDATE progress SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";
            }
            else if (updateDate && !updateQuantity)
            {
                query = $"UPDATE progress SET Date = '{date}' WHERE Id = {id}";
            }
            else
            {
                query = $"UPDATE progress SET Quantity = {quantity} WHERE Id = {id}";
            }

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText = query;

                    tableCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        internal static string GetDateInput(string message)
        {
            Console.WriteLine(message);
            string dateInput = Console.ReadLine();

            if (dateInput == "0") Menu.MainMenu();

            while (!DateTime.TryParseExact(dateInput, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date format. Please enter the date in mm-dd-yyyy format.");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput == "0") Menu.MainMenu();

            int output = 0;
            while (!int.TryParse(numberInput, out output) || output < 0)
            {
                Console.WriteLine("Invalid number. Try again");
                numberInput = Console.ReadLine();
            }

            return output;
        }
    }
}
