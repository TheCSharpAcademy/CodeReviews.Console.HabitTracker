using System.Globalization;
using Microsoft.Data.Sqlite;
using Spectre.Console;

string connectionString = @"Data Source=habit-logger.db";

CreateDatabase();

MainMenu();

void MainMenu()
{
    var isMenuRunning = true;

    while (isMenuRunning)
    {
        var usersChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do?")
                .AddChoices("Add Record", "Delete Record", "View Records", "Update Record", "Quit")
        );

        switch (usersChoice)
        {
            case "Add Record":
                AddRecord();
                break;
            case "Delete Record":
                DeleteRecord();
                break;
            case "View Records":
                GetRecords();
                break;
            case "Update Record":
                UpdateRecord();
                break;
            case "Quit":
                Console.WriteLine("Goodbye");
                isMenuRunning = false;
                break;
            default:
                Console.WriteLine("Invalid choice. Please choose one of the above");
                break;
        }
    }
}
void CreateDatabase()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habitTracker(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Habit TEXT,
                    Quantity INTEGER
                )";
                command.ExecuteNonQuery();
            }
        }
    }
}

void AddRecord()
{
    string date = GetDate(
        "\nEnter the date (format - dd-mm-yy) or insert 0 to Go Back to Main Menu:\n"
    );

    string habit = GetHabit("\nPlease enter the habit, or enter 0 to go back to Main Menu.");

    int quantity = GetNumber(
        "\nPlease enter the quanity (no decimals or negatives allowed) or enter 0 to go back to Main Menu."
    );

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText =
                $"INSERT INTO habitTracker(date, habit, quantity) VALUES('{date}','{habit}', {quantity})";

            command.ExecuteNonQuery();
        }
    }
}

string GetDate(string message)
{
    Console.WriteLine(message);
    string dateInput = Console.ReadLine();

    if (dateInput == "0")
        MainMenu();

    while (
        !DateTime.TryParseExact(
            dateInput,
            "dd-MM-yy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        )
    )
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). PLease try again\n\n");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

int GetNumber(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();

    if (numberInput == "0")
        MainMenu();

    int output = 0;
    while (!int.TryParse(numberInput, out output) || output < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again. \n\n");
        numberInput = Console.ReadLine();
    }

    return output;
}

string GetHabit(string message)
{
    Console.WriteLine(message);
    string habitInput = Console.ReadLine();

    if (habitInput == "0")
        MainMenu();
    return habitInput;
}

void GetRecords()
{
    List<HabitRecord> records = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM habitTracker";

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                        try
                        {
                            records.Add(
                                new HabitRecord(
                                    reader.GetInt32(0),
                                    DateTime.ParseExact(
                                        reader.GetString(1),
                                        "dd-MM-yy",
                                        CultureInfo.InvariantCulture
                                    ),
                                    reader.GetString(2),
                                    reader.GetInt32(3)
                                )
                            );
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(
                                $"Error parsing date: {ex.Message}. Skipping this record."
                            );
                        }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
            }
        }
    }

    ViewRecords(records);
}

void ViewRecords(List<HabitRecord> records)
{
    var table = new Table();
    table.AddColumn("ID");
    table.AddColumn("Date");
    table.AddColumn("Habit");
    table.AddColumn("Amount");

    foreach (var record in records)
    {
        table.AddRow(
            record.Id.ToString(),
            record.Date.ToString(),
            record.Habit.ToString(),
            record.Quantity.ToString()
        ); //Changed to Quantity from meters
    }

    AnsiConsole.Write(table);
}

void DeleteRecord()
{
    GetRecords();

    var id = GetNumber("Please type the id of the record you want to delete.");

    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();

            command.CommandText = @$"DELETE FROM habitTracker WHERE Id = {id}";
            command.ExecuteNonQuery();
        }
    }
}

void UpdateRecord()
{
    GetRecords();
    var id = GetNumber("Please type the id of the record you want to delete.");

    string date = "";
    string query = "";
    bool updateDate = AnsiConsole.Confirm("Update date?");
    if (updateDate)
    {
        date = GetDate(
            "\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n"
        );
    }

    string habit = "";
    bool updateHabit = AnsiConsole.Confirm("Update Habit?");
    if (updateHabit)
    {
        habit = GetHabit("\nEnter the Habit or insert 0 to Go Back to Main Menu:\n");
    }

    int quantity = 0;
    bool updateQuantity = AnsiConsole.Confirm("Update quantity?");
    if (updateQuantity)
    {
        quantity = GetNumber(
            "\nPlease enter the number of times the Habit has been done or the amount of meters (no decimals or negatives allowed) or enter 0 to go back to Main Menu."
        );
    }

    if (updateDate && updateHabit && updateQuantity)
    {
        query =
            $"UPDATE habitTracker SET Date = '{date}', Habit = '{habit}', Quantity = '{quantity}' WHERE Id = {id}";
    }
    else if (updateDate && updateHabit && !updateQuantity)
    {
        query = $"UPDATE habitTracker  SET Date = '{date}', Habit = '{habit}', WHERE Id = {id}";
    }
    else if (updateDate && !updateHabit && updateQuantity)
    {
        query =
            $"UPDATE habitTracker  SET Date = '{date}', Quantity = '{quantity}' WHERE Id = {id}";
    }
    else if (updateDate && !updateHabit && !updateQuantity)
    {
        query = $"UPDATE habitTracker  SET Date = '{date}' WHERE Id = {id}";
    }
    else if (!updateDate && !updateHabit && updateQuantity)
    {
        query = $"UPDATE habitTracker  SET Quantity = '{quantity}' WHERE Id = {id}";
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;

            command.ExecuteNonQuery();
        }
    }
}

record HabitRecord(int Id, DateTime Date, string Habit, int Quantity);
