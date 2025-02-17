using System.Globalization;
using System.Reflection.Metadata;
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
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS habitTracker(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Habit TEXT,
                Quantity INTEGER,
                Units   TEXT
            )";
            command.ExecuteNonQuery();
        }
    }
    SeedData();
}

void AddRecord()
{
    string date = GetDate(
        "\nEnter the date (format - dd-mm-yy) or insert 0 to Go Back to Main Menu:\n"
    );

    string habit = GetHabit("\nPlease enter the habit, or enter 0 to go back to Main Menu.");

    int quantity = GetNumber(
        "\nPlease enter the quantity (no decimals or negatives allowed) or enter 0 to go back to Main Menu."
    );

    string unit = GetUnit("Please enter the measurement unit for the quantity");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText =
                $"INSERT INTO habitTracker(date, habit, quantity, units) VALUES('{date}','{habit}', {quantity}, '{unit}')";

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

string GetUnit(string message)
{
    Console.WriteLine(message);
    string unitInput = Console.ReadLine();

    if (unitInput == "0")
        MainMenu();
    return unitInput;
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
                            int id = reader.GetInt32(0);
                            DateTime date = reader.IsDBNull(1)
                                ? DateTime.MinValue
                                : DateTime.ParseExact(
                                    reader.GetString(1),
                                    "dd-MM-yy",
                                    CultureInfo.InvariantCulture
                                );
                            string description = reader.IsDBNull(2)
                                ? string.Empty
                                : reader.GetString(2);
                            int value = reader.GetInt32(3);
                            string unit = reader.GetString(4);

                            records.Add(new HabitRecord(id, date, description, value, unit)); // null values to find and fix
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
    table.AddColumn("Id");
    table.AddColumn("Date");
    table.AddColumn("Habit");
    table.AddColumn("Amount");
    table.AddColumn("Units");

    foreach (var record in records)
    {
        table.AddRow(
            record.Id.ToString(),
            record.Date.ToString("D"),
            record.Habit.ToString(),
            record.Quantity.ToString(),
            record.Unit.ToString()
        );
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

bool IsTableEmpty(string tableName)
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
            long count = (long)command.ExecuteScalar();

            return count == 0;
        }
    }
}

void SeedData()
{
    bool recordsTableEmpty = IsTableEmpty("habitTracker");

    if (!recordsTableEmpty)
        return;

    string[] habitNames =
    {
        "Reading",
        "Running",
        "Chocolate",
        "Drinking Water",
        "Glasses of Wine",
        "Pooping",
    };

    string[] habitUnits = { "Pages", "Meters", "Grams", "Milliliters", "Milliliters", "Poops" };

    string[] dates = GenerateRandomDates(100);
    int[] quantities = GenerateRandomQuantities(100, 0, 2000);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        for (int i = 0; i < 100; i++)
        {
            int habitSelector = GetRandomHabitId();
            var insertSql =
                $"INSERT INTO habitTracker (Date, Habit, Quantity, Units) VALUES ('{dates[i]}', '{habitNames[habitSelector]}', {quantities[i]}, '{habitUnits[habitSelector]}' )";
            var command = new SqliteCommand(insertSql, connection);

            command.ExecuteNonQuery();
        }
    }
}

int[] GenerateRandomQuantities(int count, int min, int max)
{
    Random random = new Random();
    int[] quantities = new int[count];

    for (int i = 0; i < count; i++)
    {
        // max + 1 because the top range is excluded
        quantities[i] = random.Next(min, max + 1);
    }

    return quantities;
}
string[] GenerateRandomDates(int count)
{
    DateTime startDate = new DateTime(1984, 12, 12);
    DateTime endDate = new DateTime(2025, 12, 31);
    TimeSpan range = endDate - startDate;

    string[] randomDateStrings = new string[count];
    Random random = new Random();

    for (int i = 0; i < count; i++)
    {
        int daysToAdd = random.Next(0, (int)range.TotalDays);
        DateTime randomDate = startDate.AddDays(daysToAdd);
        randomDateStrings[i] = randomDate.ToString("dd-MM-yy");
    }

    return randomDateStrings;
}

int GetRandomHabitId()
{
    Random random = new Random();
    return random.Next(0, 6);
}

record HabitRecord(int Id, DateTime Date, string Habit, int Quantity, string Unit);
