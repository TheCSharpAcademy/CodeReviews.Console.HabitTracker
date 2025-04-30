using Microsoft.Data.Sqlite;
using Spectre.Console;
using Golvi1124.HabitLogger.src.Database;
using Golvi1124.HabitLogger.src.Helpers;
using Golvi1124.HabitLogger.src.Models;


namespace Golvi1124.HabitLogger.src.Operations;
public class HabitOperations
{
    HelperMethods helper = new();
    private readonly string _connectionString;

    public HabitOperations()
    {
        _connectionString = DatabaseConfig.ConnectionString;
    }

    public void ViewHabits(List<Habit> habits) // for visualizing the habits in a table
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[bold yellow]Habits Table[/]"); // Display table name in bold yellow

        var table = new Table(); // Spectre Console table 
        table.AddColumn("Id"); // table Class from Spectre Console
        table.AddColumn("Name");
        table.AddColumn("Unit of Measurement");

        foreach (var habit in habits)
        {
            table.AddRow(habit.Id.ToString(), habit.Name.ToString(), habit.UnitOfMeasurement.ToString());
        }
        AnsiConsole.Write(table); // write the table to the console
    }

    public void AddHabit()
    {
        Console.Clear();
        var name = AnsiConsole.Ask<string>("What's the habit's name?");
        while (string.IsNullOrWhiteSpace(name)) //added check for whitespace 
        {
            name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
        }

        var unit = AnsiConsole.Ask<string>("What's the habit's unit of measurement?");
        while (string.IsNullOrEmpty(unit))
        {
            unit = AnsiConsole.Ask<string>("Unit of measurement can't be empty. Try again:");
        }
        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand insertCmd = connection.CreateCommand())
        {
            connection.Open();
            insertCmd.CommandText = $"INSERT INTO habits (Name, MeasurementUnit) VALUES ('{name}', '{unit}')";
            insertCmd.ExecuteNonQuery();
        }

        Console.Clear();
        Console.WriteLine("Habit added successfully.");
    }

    public void UpdateHabit()
    {
        Console.Clear();
        var habits = helper.GetHabits();
        helper.DisplayHabitsTable(habits); // Reuse the new method

        helper.GetRecords();
        var id = helper.GetNumber("Please type the id of the habit you want to update.");

        string name = "";
        bool updateName = AnsiConsole.Confirm("Do you want to update the name?");
        if (updateName)
        {
            name = AnsiConsole.Ask<string>("Habit's new name:");
            while (string.IsNullOrWhiteSpace(name))
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

        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand updateCmd = connection.CreateCommand())
        {
            connection.Open();
            updateCmd.CommandText = query;
            updateCmd.ExecuteNonQuery();
        }
        Console.Clear();
        Console.WriteLine("Habit updated successfully.");
    }

    public void DeleteHabit()
    {
        Console.Clear();
        var habits = helper.GetHabits();
        helper.DisplayHabitsTable(habits); // Reuse the new method

        helper.GetRecords();

        var id = helper.GetNumber("Please type the id of the habit you want to delete.");
        using (SqliteConnection connection = new(_connectionString))
        using (SqliteCommand deleteCmd = connection.CreateCommand())
        {
            connection.Open();
            deleteCmd.CommandText = $"DELETE FROM habits WHERE Id = {id}";
            int rowsAffected = deleteCmd.ExecuteNonQuery();
            if (rowsAffected != 0)
            {
                Console.WriteLine("Habit deleted successfully.");
            }
            else
            {
                Console.WriteLine("Habit not found.");
            }
        }
        Console.Clear();
        Console.WriteLine("Habit deleted successfully.");
    }
}
