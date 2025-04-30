using Microsoft.Data.Sqlite;
using Spectre.Console;
using Golvi1124.HabitLogger.src.Database;
using Golvi1124.HabitLogger.src.Helpers;
using Golvi1124.HabitLogger.src.Models;


namespace Golvi1124.HabitLogger.src.Operations;
public class SearchOperations
{
    private readonly string _connectionString;

    public SearchOperations()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }

    HelperMethods helper = new();
    public void ShowChart()
    {
        List<(string HabitName, int LogCount)> habitChartData = new();

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand command = connection.CreateCommand())
        {
            connection.Open();

            // Query for total quantity
            command.CommandText = @"
            SELECT habits.Name, COUNT(records.Id) AS LogCount
            FROM records
            INNER JOIN habits ON records.HabitId = habits.Id
            GROUP BY habits.Id";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    habitChartData.Add((reader.GetString(0), reader.GetInt32(1)));
                }
            }
        }

        Console.Clear();

        if (habitChartData.Count > 0)
        {
            AnsiConsole.Write(new BarChart()
                .Width(60)
                .Label("[bold yellow]Habit Chart by times done[/]\n")
                .AddItems(habitChartData, (habit) => new BarChartItem(
                    habit.HabitName, habit.LogCount, Color.Green)));

        }
        else
        {
            Console.WriteLine("No data available to display chart.");
        }
    }

    public void ShowTopHabits()
    {
        List<(string HabitName, int TotalQuantity)> topByQuantity = new();
        List<(string HabitName, int LogCount)> topByCount = new();

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand command = connection.CreateCommand())
        {
            connection.Open();

            // Query for top 3 habits by total quantity
            command.CommandText = @"
            SELECT habits.Name, SUM(records.Quantity) AS TotalQuantity
            FROM records
            INNER JOIN habits ON records.HabitId = habits.Id
            GROUP BY habits.Id
            ORDER BY TotalQuantity DESC
            LIMIT 3";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    topByQuantity.Add((reader.GetString(0), reader.GetInt32(1)));
                }
            }

            // Query for top 3 habits by log count
            command.CommandText = @"
            SELECT habits.Name, COUNT(records.Id) AS LogCount
            FROM records
            INNER JOIN habits ON records.HabitId = habits.Id
            GROUP BY habits.Id
            ORDER BY LogCount DESC
            LIMIT 3";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    topByCount.Add((reader.GetString(0), reader.GetInt32(1)));
                }
            }
        }
        Console.Clear();
        // Display top 3 by total quantity
        if (topByQuantity.Count > 0)
        {
            var quantityTable = new Table();
            quantityTable.AddColumn("Rank");
            quantityTable.AddColumn("Habit");
            quantityTable.AddColumn("Total Quantity");

            for (int i = 0; i < topByQuantity.Count; i++)
            {
                quantityTable.AddRow((i + 1).ToString(), topByQuantity[i].HabitName, topByQuantity[i].TotalQuantity.ToString());
            }

            AnsiConsole.Write(new Markup("[bold yellow]Top 3 Habits by Total Quantity[/]\n"));
            AnsiConsole.Write(quantityTable);
        }
        else
        {
            Console.WriteLine("No data available to display top habits by quantity.");
        }

        // Display top 3 by log count
        if (topByCount.Count > 0)
        {
            var countTable = new Table();
            countTable.AddColumn("Rank");
            countTable.AddColumn("Habit");
            countTable.AddColumn("Log Count");

            for (int i = 0; i < topByCount.Count; i++)
            {
                countTable.AddRow((i + 1).ToString(), topByCount[i].HabitName, topByCount[i].LogCount.ToString());
            }

            AnsiConsole.Write(new Markup("\n[bold yellow]Top 3 Habits by Log Count[/]\n"));
            AnsiConsole.Write(countTable);
        }
        else
        {
            Console.WriteLine("No data available to display top habits by log count.");
        }
    }

    public void ShowAverage()
    {
        List<(string HabitName, int MinQuantity, double AverageQuantity, int MaxQuantity, string Measurement)> averageQuantities = new();

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand command = connection.CreateCommand())
        {
            connection.Open();

            // Query for average quantity
            command.CommandText = @"
            SELECT habits.Name, AVG(records.Quantity) AS AverageQuantity, Min(records.Quantity) AS MinQuantity, Max(records.Quantity) AS MaxQuantity, habits.MeasurementUnit
            FROM records
            INNER JOIN habits ON records.HabitId = habits.Id
            GROUP BY habits.Id";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Corrected column order
                    averageQuantities.Add((reader.GetString(0), reader.GetInt32(2), reader.GetDouble(1), reader.GetInt32(3), reader.GetString(4)));
                }
            }
        }

        Console.Clear();
        // Display average quantity
        if (averageQuantities.Count > 0)
        {
            var averageQuantityTable = new Table();
            averageQuantityTable.AddColumn("Habit");
            averageQuantityTable.AddColumn("Min Quantity");
            averageQuantityTable.AddColumn("Average Quantity");
            averageQuantityTable.AddColumn("Max Quantity");
            averageQuantityTable.AddColumn("Measurement Unit");

            for (int i = 0; i < averageQuantities.Count; i++)
            {
                averageQuantityTable.AddRow(
                    averageQuantities[i].HabitName,
                    averageQuantities[i].MinQuantity.ToString(),
                    averageQuantities[i].AverageQuantity.ToString("F1"), // Format average to 1 decimal place
                    averageQuantities[i].MaxQuantity.ToString(),
                    averageQuantities[i].Measurement.ToString() // Measurement unit
                );
            }

            AnsiConsole.Write(new Markup("[bold yellow]Average Habit Quantity[/]\n"));
            AnsiConsole.Write(averageQuantityTable);
        }
        else
        {
            Console.WriteLine("No data available to display average quantities.");
        }
    }

    public void ShowSpecificHabit()
    {
        var isSpecificHabitMenuRunning = true;

        while (isSpecificHabitMenuRunning)
        {

            // GetHabits() returns void, so we need to modify it to return a list of habits.
            // Assuming GetHabits() is updated to return a List<Habit>, we can use it here.

            List<Habit> habits = helper.GetHabits(); // Fetch the list of habits from the database.

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found. Returning to the previous menu.");
                isSpecificHabitMenuRunning = false;
                return;
            }

            var habitChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a Habit:")
                    .AddChoices(habits.Select(h => h.Name).Concat(new[] { "Back" })) // Adds "Back" option
    );
            if (habitChoice == "Back")
            {
                isSpecificHabitMenuRunning = false;
                return;
            }

            // Additional logic for handling the selected habit can be added here.
            List<(string Date, int Quantity, string MeasurementUnit)> specificHabitData = new();

            using (SqliteConnection connection = new(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                // Query for specific habit data
                command.CommandText = @"
                SELECT records.Date, records.Quantity, habits.MeasurementUnit
                FROM records
                INNER JOIN habits ON records.HabitId = habits.Id
                WHERE habits.Name = @HabitName ";

                command.Parameters.AddWithValue("@HabitName", habitChoice); // Use the selected habit name
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        specificHabitData.Add((reader.GetString(0), reader.GetInt32(1), reader.GetString(2)));
                    }
                }
            }

            Console.Clear();
            if (specificHabitData.Count > 0)
            {


                var specificHabitTable = new Table();
                specificHabitTable.AddColumn("Date");
                specificHabitTable.AddColumn("Quantity");
                specificHabitTable.AddColumn("Measurement Unit");

                foreach (var data in specificHabitData)
                {
                    specificHabitTable.AddRow(data.Date, data.Quantity.ToString(), data.MeasurementUnit);
                }
                AnsiConsole.Write(new Markup($"[bold yellow]Entries for {habitChoice}[/]\n"));
                AnsiConsole.Write(specificHabitTable);
            }
            else
            {
                Console.WriteLine($"No entries found for the habit: {habitChoice}.");
            }
        }
    }

    public void ShowSpecificMonth()
    {
        var isSpecificMonthMenuRunning = true;

        while (isSpecificMonthMenuRunning)
        {
            // Step 1: Fetch available years
            List<string> years = new();
            using (SqliteConnection connection = new(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                SELECT DISTINCT substr(Date, 7, 2) AS Year
                FROM records
                ORDER BY Year ASC";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        years.Add("20" + reader.GetString(0)); // Convert 'yy' to 'yyyy'
                    }
                }
            }

            if (years.Count == 0)
            {
                Console.WriteLine("No records found in the database.");
                return;
            }

            // Step 2: Prompt user to select a year
            var selectedYear = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a Year:")
                    .AddChoices(years.Concat(new[] { "Back" })) // Add "Back" option
            );

            if (selectedYear == "Back")
            {
                isSpecificMonthMenuRunning = false;
                return;
            }

            // Step 3: Fetch monthly entry counts for the selected year
            List<(string Month, int EntryCount)> months = new();
            using (SqliteConnection connection = new(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                SELECT substr(Date, 4, 2) AS Month, COUNT(*) AS EntryCount
                FROM records
                WHERE substr(Date, 7, 2) = @Year
                GROUP BY Month
                ORDER BY Month ASC";

                command.Parameters.AddWithValue("@Year", selectedYear.Substring(2, 2)); // Extract 'yy' from 'yyyy'

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    // Initialize all months with 0 entries
                    for (int i = 1; i <= 12; i++)
                    {
                        months.Add((i.ToString("D2"), 0));
                    }

                    // Update entry counts for months with data
                    while (reader.Read())
                    {
                        string month = reader.GetString(0);
                        int entryCount = reader.GetInt32(1);
                        int index = months.FindIndex(m => m.Month == month);
                        if (index != -1)
                        {
                            months[index] = (month, entryCount);
                        }
                    }
                }
            }

            // Step 4: Prompt user to select a month
            var selectedMonth = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Choose a Month for {selectedYear}:")
                    .AddChoices(months.Select(m => $"{m.Month} (Entries: {m.EntryCount})").Concat(new[] { "Back" }))
            );

            if (selectedMonth == "Back")
            {
                continue;
            }

            // Extract the selected month (e.g., "01" from "01 (Entries: 0)")
            string monthNumber = selectedMonth.Substring(0, 2);

            // Step 5: Fetch and display entries for the selected year and month
            List<(string Date, string HabitName, int Quantity, string MeasurementUnit)> entries = new();
            using (SqliteConnection connection = new(_connectionString))
            using (SqliteCommand command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                SELECT records.Date, habits.Name, records.Quantity, habits.MeasurementUnit
                FROM records
                INNER JOIN habits ON records.HabitId = habits.Id
                WHERE substr(records.Date, 7, 2) = @Year AND substr(records.Date, 4, 2) = @Month
                ORDER BY records.Date ASC";

                command.Parameters.AddWithValue("@Year", selectedYear.Substring(2, 2)); // Extract 'yy' from 'yyyy'
                command.Parameters.AddWithValue("@Month", monthNumber);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add((
                            reader.GetString(0), // Date
                            reader.GetString(1), // HabitName
                            reader.GetInt32(2),  // Quantity
                            reader.GetString(3)  // MeasurementUnit
                        ));
                    }
                }
            }

            Console.Clear();
            if (entries.Count > 0)
            {
                var table = new Table();
                table.AddColumn("Date");
                table.AddColumn("Habit");
                table.AddColumn("Quantity");
                table.AddColumn("Measurement Unit");

                foreach (var entry in entries)
                {
                    table.AddRow(entry.Date, entry.HabitName, entry.Quantity.ToString(), entry.MeasurementUnit);
                }

                AnsiConsole.Write(new Markup($"[bold yellow]Entries for {selectedYear}-{monthNumber}[/]\n"));
                AnsiConsole.Write(table);
            }
            else
            {
                Console.WriteLine($"No entries found for {selectedYear}-{monthNumber}.");
            }
        }
    }
}
