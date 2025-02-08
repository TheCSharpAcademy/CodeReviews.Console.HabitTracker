using habbitTracker.fatihskalemci.Models;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

namespace habbitTracker.fatihskalemci;

internal class DataBaseConnection
{
    private string connectionString = "Data Source=habbit-Tracker.db";

    public void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var commandTable = connection.CreateCommand();

            commandTable.CommandText =
                @"CREATE TABLE IF NOT EXISTS water_tracker (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
            commandTable.ExecuteNonQuery();

            connection.Close();
        }

    }

    internal List<Habbit> GetEntries()
    {
        List<Habbit> tableData = new();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @"SELECT * FROM water_tracker";

            using (var insertCommand = new SqliteCommand(commandString, connection))
            {
                using (SqliteDataReader reader = insertCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Habbit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("tr-TR")),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
            }
        }
        return tableData;
    }

    internal void AddEntry()
    {
        string dateInput = UserInterface.getDateInput();
        if (dateInput == "0")
        {
            return;
        }
        int integerInput = UserInterface.getIntegerInput("Please enter the quantity");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @"INSERT INTO water_tracker (Date, Quantity)
                VALUES (@entryDate, @entryQuantity)";

            using (var insertCommand = new SqliteCommand(commandString, connection))
            {
                insertCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = dateInput;
                insertCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = integerInput;

                insertCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    internal void UpdateEntry()
    {
        List<Habbit> tableData = GetEntries();

        if (!tableData.Any())
        {
            Console.WriteLine("No entry to update.");
            Console.ReadKey();
            return;
        }

        var entryToUpdate = AnsiConsole.Prompt(
            new SelectionPrompt<Habbit>()
            .Title("Select entry to update")
            .UseConverter(e => $"{e.Id} | {e.Date.ToString("dd-MM-yy")} | {e.Quantity} ")
            .AddChoices(tableData));

        string dateInput = UserInterface.getDateInput();
        if (dateInput == "0")
        {
            return;
        }
        int integerInput = UserInterface.getIntegerInput("Please enter the quantity");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @"UPDATE water_tracker
                                SET Date = @entryDate, Quantity = @entryQuantity
                                WHERE Id = @entryID";

            using (var updateCommand = new SqliteCommand(commandString, connection))
            {
                updateCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = dateInput;
                updateCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = integerInput;
                updateCommand.Parameters.Add("@entryID", SqliteType.Integer).Value = entryToUpdate.Id;
                updateCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    internal void DeleteEntry()
    {
        List<Habbit> tableData = GetEntries();

        if (!tableData.Any())
        {
            Console.WriteLine("No entry to delete.");
            Console.ReadKey();
            return;
        }

        var entryToDelelte = AnsiConsole.Prompt(
            new SelectionPrompt<Habbit>()
            .Title("Select entry to delete")
            .UseConverter(e => $"{e.Id} | {e.Date.ToString("dd-MM-yy")} | {e.Quantity} ")
            .AddChoices(tableData));

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string commandString = @"DELETE FROM water_tracker WHERE Id = @entryID";

            using (var deleteCommand = new SqliteCommand(commandString, connection))
            {
                deleteCommand.Parameters.Add("@entryID", SqliteType.Integer).Value = entryToDelelte.Id;
                deleteCommand.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    internal void ShowEtries()
    {
        List<Habbit> tableData = GetEntries();

        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Date[/]");
        table.AddColumn("[yellow]Quantity[/]");

        foreach (var item in tableData)
        {
            table.AddRow(
                item.Id.ToString(),
                item.Date.ToString("dd-MM-yy"),
                item.Quantity.ToString()
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }
}
