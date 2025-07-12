using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Globalization;
using System.Net.Mail;

string connectionString = "Data Source=habit-Tracker.db";

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
                .AddChoices(
                   "Add Habit",
                   "Delete Habit",
                   "Update Habit",
                   "Add Record",
                   "Delete Record",
                   "View Records",
                   "Update Record",
                   "Quit")
                );

        switch (usersChoice)
        {
            case "Add Habit":
                AddHabit();
                break;
            case "Delete Habit":
                DeleteHabit();
                break;
            case "Update Habit":
                UpdateHabit();
                break;
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
bool IsTableEmplty(string tableName)
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
    bool recorsTableEmpty = IsTableEmplty("records");
    bool habitsTableEmpty = IsTableEmplty("habits");

    if (!recorsTableEmpty || !habitsTableEmpty)
        return;

    string[] habitnames = { "Reading", "Running", "Chocolate", "Drinking Water", "Glasses of Wine" };
    string[] habitUnits = { "Pages", "Meters", "Grams", "Mililiters", "Mililiters" };
    string[] dates = GenerateRandomDates(100);
    int[] quantities = GenerateRandomQuantities(100, 0, 2000);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        
        for (int i = 0; i < habitnames.Length; i++)
        {
            var insertSql = $"INSERT INTO habits (Name, MeasurementUnit) VALUES ('{habitnames[i]}', '{habitUnits[i]}');";
            var command = new SqliteCommand(insertSql, connection);

            command.ExecuteNonQuery();
        }

        for (int i = 0; i < 100; i++)
        {
            var insertSql = $"INSERT INTO records (Date, Quantity, HabitId) VALUES ('{dates[i]}', {quantities[i]}, {GetRandomHabitId()});";
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
    DateTime startDate = new DateTime(2023, 1, 1);
    DateTime endDate = new DateTime(2024, 12, 31);
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
    return random.Next(1, 6);
}
void UpdateRecord()
{
    GetRecords();

    var id = GetNumber("Please type the ID of the record you want to update");

    bool updateDate = AnsiConsole.Confirm("Update date?");
    string date = "";
    if (updateDate)
    {
        date = GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");
    }

    bool updateDistance = AnsiConsole.Confirm("Update amount?");
    int distance = 0;
    if (updateDistance)
    {
        distance = GetNumber("\nPlease enter the unit amount (no decimals or negatives allowed) or enter 0 to go back");
    }

    string query;
    if (updateDate && updateDistance)
    {
        query = $"UPDATE records SET DATE = '{date}', Quantity = {distance} WHERE Id = {id}";
    }
    else if (updateDate && !updateDistance)
    {
        query = $"UPDATE records SET DATE = '{date}' WHERE Id = {id}";
    }
    else
    {
        query = $"UPDATE records SET Quantity = '{distance}' WHERE Id = {id}";
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

void DeleteRecord()
{
    GetRecords();

    var id = GetNumber("Please type the ID of the record you want to delete");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = $@"DELETE FROM records WHERE Id = {id}";
            int rowsAffected = command.ExecuteNonQuery();

            if(rowsAffected != 0)
            {
                Console.WriteLine("Record deleted successfully.");
            }
            else
            {
                Console.WriteLine("No record found with that ID was found.");
            }
        }
    }
}
void AddHabit()
{
    var name = AnsiConsole.Ask<string>("Habit's name:");
    while (string.IsNullOrEmpty(name))
    {
        name = AnsiConsole.Ask<string>("Habit's name can't be empty. Try again:");
    }

    var unit = AnsiConsole.Ask<string>("What's the habit's unit of measurement?");
    while (string.IsNullOrEmpty(unit))
    {
        unit = AnsiConsole.Ask<string>("Unit of measurement can't be empty. Try again:");
    }
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"INSERT INTO habits(Name, MeasurementUnit) VALUES('{name}', '{unit}')";

        tableCmd.ExecuteNonQuery();
    }
}

void DeleteHabit()
{
    GetHabits();

    var id = GetNumber("Please type the id of the habit you want to delete.");

    using (var connection = new SqliteConnection(connectionString))
    {
        using (var command = connection.CreateCommand())
        {
            connection.Open();

            command.CommandText =
                    @$"DELETE FROM habits WHERE Id = {id}";
            command.ExecuteNonQuery();
        }
    }
}

void UpdateHabit()
{
    GetHabits();
    var id = GetNumber("Please type the id of the habit you want to update.");

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
        query = $"UPDATE habits SET Quantity = '{unit}' WHERE Id = {id}";
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = query;

        tableCmd.ExecuteNonQuery();
    }
}

void GetHabits()
{
    List<Habit> habits = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM Habits";

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

    ViewHabits(habits);
}

void ViewHabits(List<Habit> habits)
{
    var table = new Table();

    table.AddColumn("Id");
    table.AddColumn("Name");
    table.AddColumn("Unit of Measurement");

    foreach (var habit in habits)
    {
        table.AddRow(habit.Id.ToString(), habit.Name, habit.UnitOfMeasurement);
    }

    AnsiConsole.Write(table);
}

void GetRecords()
{
    List<RecordWithHabit> records = new();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"
    SELECT records.Id, records.Date, records.Quantity, records.HabitId, habits.Name AS HabitName, habits.MeasurementUnit
    FROM records
    INNER JOIN habits ON records.HabitId = habits.Id";

        using (SqliteDataReader reader = tableCmd.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                    try
                    {
                        records.Add(
                        new RecordWithHabit(
                            reader.GetInt32(0),
                            DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", CultureInfo.InvariantCulture),
                            reader.GetInt32(2),
                            reader.GetString(4),
                            reader.GetString(5)
                            )
                        );
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Error parsing date: {ex.Message}. Skipping this record.");
                    }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
        }
    }

    ViewRecords(records);
}

void ViewRecords(List<RecordWithHabit> records)
{
    var table = new Table();
    table.AddColumn("Id");
    table.AddColumn("Date");
    table.AddColumn("Amount");
    table.AddColumn("Habit");

    foreach (var record in records)
    {
        table.AddRow(record.Id.ToString(), record.Date.ToString("D"), $"{record.Quantity} {record.MeasurementUnit}", record.HabitName.ToString());
    }

    AnsiConsole.Write(table);
}

void AddRecord()
{
    string date = GetDate("\nEnter the date (format : dd-mm-yy) or insert 0 to Go Back to Main Menu:\n");

    GetHabits();
    int habitId = GetNumber("\nPlease enter the id of the habit for this record");

    int quantity = GetNumber("\nPlease enter quantity (no decimals or negatives allowed) or enter 0 to go back to Main Menu.");

    Console.Clear();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"INSERT INTO records(date, quantity, habitId) VALUES('{date}', {quantity}, {habitId})";

        tableCmd.ExecuteNonQuery();
    }
}

int GetNumber(string message)
{
    Console.WriteLine(message);
    string numberInput = Console.ReadLine();

    if (numberInput == "0") MainMenu();
    int output = 0;

    while (!int.TryParse(numberInput, out output) || output < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again. \n\n");
        numberInput = Console.ReadLine();
    }
    return output;
}

string GetDate(string message)
{
    Console.WriteLine(message);
    string dateInput = Console.ReadLine();

    if (dateInput == "0") MainMenu();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Please try again\n\n");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

void CreateDatabase()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        using (var tableCmd = connection.CreateCommand())
        {
            connection.Open();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER,
                    HabitId INTEGER,
                    FOREIGN KEY (HabitId) REFERENCES habits(Id) ON DELETE CASCADE
                )";
            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    MeasurementUnit Text
                )";
            tableCmd.ExecuteNonQuery();
        }
    }
    SeedData();
}
record Habit(int Id, string Name, string UnitOfMeasurement);
record RecordWithHabit(int Id, DateTime Date, int Quantity, string HabitName, string MeasurementUnit);