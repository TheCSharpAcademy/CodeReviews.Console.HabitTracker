using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;

string connectionString = "Data Source=habbit-Tracker.db";

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

MainMenu();

void MainMenu()
{
    bool exit = false;
    while (!exit)
    {
        Console.Clear();

        List<string> menuChoices = ["Add", "Update", "Delete", "ShowEntries", "Exit"];

        var menuSelection = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Please select the action you want to perform")
            .AddChoices(menuChoices));

        switch (menuSelection)
        {
            case "Add":
                AddEntry();
                break;
            case "Update":
                UpdateEntry();
                break;
            case "Delete":
                DeleteEntry();
                break;
            case "ShowEntries":
                ShowEtries();
                break;
            case "Exit":
                exit = true;
                break;
        }
    }
}

void DeleteEntry()
{
    List<DrinkigWater> tableData = GetEntries();

    if (!tableData.Any())
    {
        Console.WriteLine("No entry to delete.");
        Console.ReadKey();
        return;
    }

    var entryToDelelte = AnsiConsole.Prompt(
        new SelectionPrompt<DrinkigWater>()
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

void UpdateEntry()
{
    List<DrinkigWater> tableData = GetEntries();

    if (!tableData.Any())
    {
        Console.WriteLine("No entry to update.");
        Console.ReadKey();
        return;
    }

    var entryToUpdate = AnsiConsole.Prompt(
        new SelectionPrompt<DrinkigWater>()
        .Title("Select entry to delete")
        .UseConverter(e => $"{e.Id} | {e.Date.ToString("dd-MM-yy")} | {e.Quantity} ")
        .AddChoices(tableData));

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        string commandString = @"UPDATE water_tracker
                                SET Date = @entryDate, Quantity = @entryQuantity
                                WHERE Id = @entryID";

        using (var updateCommand = new SqliteCommand(commandString, connection))
        {
            updateCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = getDateInput();
            updateCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = getIntegerInput("Please enter the quantity");
            updateCommand.Parameters.Add("@entryID", SqliteType.Integer).Value = entryToUpdate.Id;
            updateCommand.ExecuteNonQuery();
        }

        connection.Close();
    }
}

List<DrinkigWater> GetEntries()
{
    List<DrinkigWater> tableData = new();

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
                    new DrinkigWater
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

void ShowEtries()
{
    List<DrinkigWater> tableData = GetEntries();

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
void AddEntry()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        string commandString = @"INSERT INTO water_tracker (Date, Quantity)
                VALUES (@entryDate, @entryQuantity)";

        using (var insertCommand = new SqliteCommand(commandString, connection))
        {
            insertCommand.Parameters.Add("@entryDate", SqliteType.Text).Value = getDateInput();
            insertCommand.Parameters.Add("@entryQuantity", SqliteType.Integer).Value = getIntegerInput("Please enter the quantity");

            insertCommand.ExecuteNonQuery();
        }

        connection.Close();
    }
}

int getIntegerInput(string message)
{
    Console.WriteLine(message);
    string? userInput = Console.ReadLine();
    int integerInput;

    while (!Int32.TryParse(userInput, out integerInput))
    {
        Console.WriteLine("Please enter a valid integer");
        userInput = Console.ReadLine();
    }
    return integerInput;
}

string getDateInput()
{
    Console.WriteLine("Please enter the date of the entry as dd-mm-yy(0 to return main menu, Press enter for today");
    string? userInput = Console.ReadLine();
    if (userInput == "")
    {
        userInput = DateTime.Now.ToString("dd-MM-yy");
    }
    else if (userInput == "0")
    {
        MainMenu();
    }
    else
    {
        while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("tr-TR"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("Please check your date format (dd-mm-yy)");
            userInput = Console.ReadLine();
        }
    }

    return userInput;
}

public class DrinkigWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}