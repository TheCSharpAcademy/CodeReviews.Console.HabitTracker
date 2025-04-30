using System.Globalization;
using Microsoft.Data.Sqlite;
using Golvi1124.HabitLogger.src.Database;
using Golvi1124.HabitLogger.src.Models;
using Spectre.Console;

namespace Golvi1124.HabitLogger.src.Helpers;

public class HelperMethods
{
    private readonly string _connectionString;

    public HelperMethods()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }

    public List<RecordWithHabit> GetRecords()
    {
        var records = new List<RecordWithHabit>();

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand getCmd = connection.CreateCommand())
        {
            connection.Open();
            getCmd.CommandText = @"
    SELECT records.Id, records.Date, habits.Name AS HabitName, records.Quantity, habits.MeasurementUnit
    FROM records
    INNER JOIN habits ON records.HabitId = habits.Id";

            using (SqliteDataReader reader = getCmd.ExecuteReader()) //(SqliteDataReader reader = command.ExecuteReader())
            {
                /*checking if the reader has rows and if it does we will read the data in a loop. 
                For each row found in the table, we will add a new WalkingRecord to the list.*/

                if (reader.HasRows) // check if there are any records in the table
                {
                    while (reader.Read())
                    {
                        /*The code around the reading operation is wrapped by a try-catch block 
                        to prevent the app from crashing in case the operation against the database goes wrong.*/
                        try
                        {
                            // Parse the date string explicitly
                            string dateString = reader.GetString(1); // Read the date as a string
                            DateTime date = DateTime.ParseExact(dateString, "dd-MM-yy", CultureInfo.InvariantCulture);


                            // Update the RecordWithHabit instantiation to use the constructor with parameters
                            records.Add(new RecordWithHabit(
                                reader.GetInt32(0), // Id
                                date, // Parsed Date
                                reader.GetString(2), // HabitName
                                reader.GetInt32(3), // Quantity
                                reader.GetString(4)  // MeasurementUnit
                             ));
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine($"Error reading record: {ex.Message}. Skipping this record.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
        }
        return records;
    }


    public List<Habit> GetHabits() // Update the GetHabits method to return a List<Habit> instead of void.
    {
        List<Habit> habits = new();

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand getCmd = connection.CreateCommand())
        {
            connection.Open();
            getCmd.CommandText = "SELECT * FROM habits";

            using (SqliteDataReader reader = getCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            habits.Add(
                                new Habit(
                                    reader.GetInt32(0), // Id
                                    reader.GetString(1), // Name
                                    reader.GetString(2) // MeasurementUnit
                                )
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading record: {ex.Message}. Skipping this record.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
        }
        // Return the list of habits.
        return habits;
    }


    public List<int> GenerateRandomQuantities(int count, int min, int max)
    {
        Random random = new();
        List<int> quantities = new();

        for (int i = 0; i < count; i++)
        {
            // max + 1 because the top range is excluded 
            quantities.Add(random.Next(min, max + 1));
        }
        return quantities;
    }

    public List<string> GenerateRandomDates(int count)
    {
        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = DateTime.Now; // current date
        TimeSpan timeSpan = endDate - startDate;

        List<string> randomDateStrings = new(count);
        Random random = new();

        for (int i = 0; i < count; i++)
        {
            int daysToAdd = random.Next(0, (int)timeSpan.TotalDays);
            DateTime randomDate = startDate.AddDays(daysToAdd);
            randomDateStrings.Add(randomDate.ToString("dd-MM-yy")); // format the date to dd-mm-yy
        }
        return randomDateStrings;
    }

    public int GetRandomHabitId()
    {
        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand command = connection.CreateCommand())
        {
            connection.Open();
            command.CommandText = "SELECT Id FROM habits";

            var reader = command.ExecuteReader();
            List<int> ids = new();
            while (reader.Read())
            {
                // Check if the value is not null before unboxing
                if (!reader.IsDBNull(0)) // Ensure the column is not null
                {
                    ids.Add(reader.GetInt32(0));
                }
            }

            if (ids.Count == 0)
            {
                throw new Exception("No habits found in the database.");
            }

            Random random = new();
            return ids[random.Next(ids.Count)];
        }
    }

    public string GetDate(string message) //will use it display message to user about wanted input
    {
        Console.WriteLine(message);
        string? dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            Program.MainMenu(); // Call MainMenu from Program.cs
            return string.Empty; // Return an empty string or handle as needed
        }


        // returns input only if parsing successful. if incorrect, stays in the loop
        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Please try again!\n\n");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    public int GetNumber(string message)
    {
        Console.WriteLine(message);
        string? numberInput = Console.ReadLine();

        if (numberInput == "0") Program.MainMenu();
        int output = 0;

        while (!int.TryParse(numberInput, out output) || output < 0)
        {
            Console.WriteLine("\n\nInvalid number. Please try again!\n\n");
            numberInput = Console.ReadLine();
        }
        return output;
    }

    public void DisplayHabitsTable(List<Habit> habits)
    {
        if (habits.Count == 0)
        {
            Console.WriteLine("No habits found. Please add habits first.");
            return;
        }

        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Unit of Measurement");

        foreach (var habit in habits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name, habit.UnitOfMeasurement);
        }

        AnsiConsole.Write(table);
    }
}
