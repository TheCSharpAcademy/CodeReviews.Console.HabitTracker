using habitTracker.fatihskalemci.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

namespace habitTracker.fatihskalemci;

internal class DataBaseConnection
{
    private readonly string connectionString = "Data Source=habit-Tracker.db";

    public void CreateHabitEntriesTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var commandTable = connection.CreateCommand();
            commandTable.CommandText =
                @"CREATE TABLE IF NOT EXISTS habit_entries (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Habit TEXT,
            Date TEXT,
            Quantity INTEGER,
            Unit TEXT
            )";
            commandTable.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal List<Habit> GetHabits()
    {
        List<Habit> habits = new();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @"SELECT DISTINCT Habit, Unit FROM habit_entries";
            using (var selectCommand = new SqliteCommand(commandString, connection))
            {
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habits.Add(
                        new Habit
                        {
                            HabitName = reader.GetString(0),
                            Unit = reader.GetString(1)
                        });
                    }
                }
            }
            connection.Close();
        }
        return habits;
    }

    internal List<HabitEntry> GetEntries(Habit habit)
    {
        List<HabitEntry> entries = [];

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @$"SELECT * FROM habit_entries
                                    WHERE Habit = @entryHabitName";
            using (var selectCommand = new SqliteCommand(commandString, connection))
            {
                selectCommand.Parameters.Add("@entryHabitName", SqliteType.Text).Value = habit.HabitName;
                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(
                        new HabitEntry
                        {
                            Id = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", new CultureInfo("tr-TR")),
                            Quantity = reader.GetInt32(3),
                            Unit = reader.GetString(4)
                        });
                    }
                }
            }
            connection.Close();
        }
        return entries;
    }

    internal void AddEntry(HabitEntry habitEntry)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @$"INSERT INTO habit_entries (Habit, Date, Quantity, Unit)
                VALUES (@habitName, @entryDate, @entryQuantity, @unit)";
            using (var insertCommand = new SqliteCommand(commandString, connection))
            {
                insertCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = habitEntry.Date.ToString("yyyy-MM-dd");
                insertCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = habitEntry.Quantity;
                insertCommand.Parameters.Add("@habitName", SqliteType.Text).Value = habitEntry.HabitName;
                insertCommand.Parameters.Add("@unit", SqliteType.Text).Value = habitEntry.Unit;
                insertCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    internal void UpdateEntry(Habit habit)
    {
        List<HabitEntry> habitEntries = GetEntries(habit);

        if (habitEntries.Count == 0)
        {
            Console.WriteLine("No entry to update.");
            Console.ReadKey();
            return;
        }

        HabitEntry habitEntry = AnsiConsole.Prompt(
            new SelectionPrompt<HabitEntry>()
            .Title("Select entry to update")
            .UseConverter(e => $"{e.Id} | {e.HabitName} | {e.Date:yyyy-MM-dd} | {e.Quantity} | {e.Unit}")
            .AddChoices(habitEntries));

        habitEntry.Date = UserInterface.GetDateInput();
        habitEntry.Quantity = UserInterface.GetIntegerInput("Please enter the quantity");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @$"UPDATE habit_entries
                                SET Date = @entryDate, Quantity = @entryQuantity
                                WHERE Id = @entryID";
            using (var updateCommand = new SqliteCommand(commandString, connection))
            {
                updateCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = habitEntry.Date.ToString("yyyy-MM-dd");
                updateCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = habitEntry.Quantity;
                updateCommand.Parameters.Add("@entryID", SqliteType.Integer).Value = habitEntry.Id;
                updateCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    internal void DeleteEntry(Habit habit)
    {
        List<HabitEntry> habitEntries = GetEntries(habit);

        if (habitEntries.Count == 0)
        {
            Console.WriteLine("No entry to delete.");
            Console.ReadKey();
            return;
        }

        var entryToDelelte = AnsiConsole.Prompt(
            new SelectionPrompt<HabitEntry>()
            .Title("Select entry to delete")
            .UseConverter(e => $"{e.Id} | {e.HabitName} | {e.Date:yyyy-MM-dd} | {e.Quantity} | {e.Unit}")
            .AddChoices(habitEntries));

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @$"DELETE FROM habit_entries WHERE Id = @entryID";
            using (var deleteCommand = new SqliteCommand(commandString, connection))
            {
                deleteCommand.Parameters.Add("@entryID", SqliteType.Integer).Value = entryToDelelte.Id;
                deleteCommand.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    internal void ShowEtries(Habit habit)
    {
        List<HabitEntry> habitEntries = GetEntries(habit);

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.Title($"{habit.HabitName}");

        table.AddColumn("[yellow]Date[/]");
        table.AddColumn("[yellow]Quantity[/]");
        table.AddColumn("[yellow]Unit[/]");

        foreach (var item in habitEntries)
        {
            table.AddRow(
                item.Date.ToString("yyyy-MM-dd"),
                item.Quantity.ToString(),
                item.Unit
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal void MakeReport(Habit habit)
    {
        int total = GetTotal(habit);
        int lastYearTotal = GetTotal(habit, true);

        (int maxQuantity, DateTime maxDate) = GetMostOrLeastDoneDay(habit, "most");
        (int minQuantity, DateTime minDate) = GetMostOrLeastDoneDay(habit, "least");

        var table = new Table();

        table.Border = TableBorder.Simple;

        table.AddColumn("Report Type");
        table.AddColumn("Report");

        table.AddRow($"Total {habit.HabitName}", $"{total} {habit.Unit}");
        table.AddRow($"Last year total {habit.HabitName}", $"{lastYearTotal} {habit.Unit}");
        table.AddRow($"Most {habit.HabitName}", $"{maxQuantity} {habit.Unit} on {maxDate.ToString("yyyy-MM-dd")}");
        table.AddRow($"Least {habit.HabitName}", $"{minQuantity} {habit.Unit} on {minDate.ToString("yyyy-MM-dd")}");

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    internal (int Quantity, DateTime Date) GetMostOrLeastDoneDay(Habit habit, string type)
    {
        int quantity = 0;
        DateTime date = DateTime.Now;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString;
            if (type.ToLower() == "least")
            {
                commandString = @"SELECT MIN(Quantity), Date
                                FROM habit_entries
                                WHERE Habit = @habitName";
            }
            else
            {
                commandString = @"SELECT MAX(Quantity), Date
                                FROM habit_entries
                                WHERE Habit = @habitName";
            }

            using (var selectCommand = new SqliteCommand(commandString, connection))
            {
                selectCommand.Parameters.Add("@habitName", SqliteType.Text).Value = habit.HabitName;

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        quantity = reader.GetInt32(0);
                        date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", new CultureInfo("tr-TR"));
                    }
                }
            }
            connection.Close();
        }
        return (quantity, date);
    }

    internal int GetTotal(Habit habit, bool lastYear = false)
    {
        int sum = 0;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString;
            if (lastYear)
            {
                commandString = @"SELECT SUM(Quantity)
                                FROM habit_entries
                                WHERE Habit = @habitName AND Date > @date";
            }
            else
            {
                commandString = @"SELECT SUM(Quantity)
                                FROM habit_entries
                                WHERE Habit = @habitName";
            }

            using (var selectCommand = new SqliteCommand(commandString, connection))
            {
                selectCommand.Parameters.Add("@habitName", SqliteType.Text).Value = habit.HabitName;
                selectCommand.Parameters.Add("@date", SqliteType.Text).Value = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sum = reader.GetInt32(0);
                    }
                }
            }
            connection.Close();
        }
        return sum;
    }
}
