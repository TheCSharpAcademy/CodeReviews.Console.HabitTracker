using System.Globalization;
using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=HabitTrackerDB.db";
string tableName = "drinking_water";
string habitUnit = "Quantity";

CreateTable(connectionString, tableName, habitUnit);

GetUserInput();

void GetUserInput()
{
    Clear();
    bool quit = false;
    while (!quit)
    {
        WriteLine($"MAIN MENU");
        WriteLine($"Active Habit Type: {tableName}\n");
        WriteLine($"1: View All Records.");
        WriteLine($"2: Insert Record.");
        WriteLine($"3: Delete Record.");
        WriteLine($"4: Update Record.");
        WriteLine($"5: DELETE ALL RECORDS.");
        WriteLine($"6: Change Active Habit Type.");
        WriteLine($"0: Quit!");
        WriteLine(new string('-', 40) + "\n");

        Write("Selection: ");
        string selection = ReadLine()!;

        switch (selection)
        {
            case "0":
                WriteLine($"See you next time!");
                quit = true;
                Environment.Exit(0);
                break;
            case "1":
                GetAllRecords();
                break;
            case "2":
                InsertRecord();
                break;
            case "3":
                DeleteRecord();
                break;
            case "4":
                UpdateRecord();
                break;
            case "5":
                DeleteAllRecords();
                break;
            case "6":
                ChangeWorkingHabit();
                break;
            default:
                WriteLine($"Invalid selection. Please use a number from 0-4");
                break;
        }
    }
}

#region CRUD

void DeleteAllRecords()
{
    Clear();
    WriteLine($"ARE YOU SURE? DELETING ALL ROWS FROM {tableName.ToUpper()}! (Y/N). ");
    string input = ReadLine()!;

    if (input.ToLower() == "y")
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"DELETE FROM {tableName}";
        tableCmd.ExecuteNonQuery();

        Clear();
        WriteLine($"All rows deleted!\n");
    }
    else
    {
        Clear();
        WriteLine($"Aborting.\n");
    }
}

void GetAllRecords()
{
    Clear();
    List<Habit> tableData = [];

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();
    tableCmd.CommandText = $"SELECT * FROM {tableName}";
    SqliteDataReader reader = tableCmd.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            tableData.Add(new Habit
            {
                Id = reader.GetInt32(0),
                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                Measure = reader.GetInt32(2),
            });
        }
    }
    else
    {
        WriteLine("No rows found");
    }

    WriteLine(new string('-', 40));
    foreach (Habit habit in tableData)
    {
        WriteLine($"{habit.Id} - {habit.Date:dd-MMM-yyyy} - {habitUnit}: {habit.Measure}");
    }
    WriteLine(new string('-', 40));
}

void InsertRecord()
{
    Clear();
    WriteLine($"INSERT RECORD");

    string date = GetDateInput();

    int measure = GetNumberInput($"\nPlease type number of {habitUnit} (no decimals allowed).");

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        $"INSERT INTO {tableName}(date, {habitUnit}) VALUES('{date}', {measure})";

    tableCmd.ExecuteNonQuery();

    Clear();
}

void DeleteRecord(bool showRecords = true)
{
    if (showRecords)
    {
        Clear();
        GetAllRecords();
    }

    int recordId = GetNumberInput("\nType the ID of the record you want to delete.");

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        $"DELETE FROM {tableName} WHERE Id = '{recordId}'";

    int rowCount = tableCmd.ExecuteNonQuery();

    if (rowCount == 0)
    {
        WriteLine($"\nRecord with ID {recordId} doesn't exist.");
        DeleteRecord(false);

        return;
    }

    Clear();
    WriteLine($"Record with ID {recordId} was deleted.\n");
}

void UpdateRecord(bool showRecords = true)
{
    if (showRecords)
    {
        Clear();
        GetAllRecords();
    }

    int recordId = GetNumberInput("\nType the ID of the record you want to update.");

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    using var checkCmd = connection.CreateCommand();
    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {recordId})";
    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

    if (checkQuery == 0)
    {
        WriteLine($"\nRecord with ID {recordId} doesn't exist.");
        connection.Close();
        UpdateRecord(false);
    }

    string date = GetDateInput();
    int quantity = GetNumberInput($"\nPlease type number of {habitUnit} (no decimals allowed).");

    using var tableCmd = connection.CreateCommand();
    tableCmd.CommandText =
        $"UPDATE {tableName} SET date = '{date}', {habitUnit} = {quantity} WHERE Id = '{recordId}'";
    tableCmd.ExecuteNonQuery();

    Clear();
    WriteLine($"Record with ID {recordId} updated.\n");
}

#endregion

#region GET INPUTS

int GetNumberInput(string message)
{
    WriteLine(message + " 'X' to go back.");
    Write("Number: ");

    string? numberInput = ReadLine();
    int integerInput;

    while (!int.TryParse(numberInput, out integerInput) || integerInput < 0)
    {
        WriteLine($"Invalid number. Try again.");
        Write("Number: ");
        numberInput = ReadLine();
        if (numberInput != null && numberInput.ToLower() == "x")
        {
            GetUserInput();
        }
    }

    return integerInput;
}

string GetDateInput()
{
    WriteLine($"\nPlease type the date (Format: DD-MM-YY). 0 to go back.");
    Write($"Date: ");
    string dateInput = ReadLine()!;


    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _) && dateInput != "0")
    {
        WriteLine($"Invalid date format (dd-mm-yy). Type 0 to return to main menu.");
        Write("Date: ");
        dateInput = ReadLine()!;
    }
    if (dateInput == "0")
    {
        GetUserInput();
    }

    return dateInput;
}

#endregion

#region CREATE/CHANGE HABIT

void CreateTable(string connectionString, string tableName = "drinking_water", string unit = "Quantity")
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();

    tableCmd.CommandText =
        @$"CREATE TABLE IF NOT EXISTS {tableName} (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            {unit} INTEGER
            )";
    tableCmd.ExecuteNonQuery();

    tableCmd.CommandText = $"SELECT * FROM {tableName}";
    var reader = tableCmd.ExecuteReader();

    if (!reader.HasRows)
    {
        try
        {
            GenerateSeed(tableName, unit);
        }
        catch (Exception e)
        {
            WriteLine($"Seed failed: {e.Message}");
        }
    }
}

void GenerateSeed(string table, string unit)
{
    WriteLine($"Seeding database..");

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();

    Random rand = new();

    for (int i = 0; i < 100; i++)
    {
        string month = rand.Next(1, 13).ToString().PadLeft(2, '0');
        string day = rand.Next(1, 29).ToString().PadLeft(2, '0');
        string year = rand.Next(1, 99).ToString().PadLeft(2, '0');
        string date = $"{day}-{month}-{year}";
        int measure = rand.Next(1, 100);


        tableCmd.CommandText =
            $"INSERT INTO {table}(date, {unit}) VALUES('{date}', {measure})";

        tableCmd.ExecuteNonQuery();
    }

    WriteLine($"Seeding complete.");
}

void ChangeWorkingHabit()
{
    Clear();
    WriteLine($"0. Create New Habit");

    List<string[]> tableList = GetExistingTables();
    for (int i = 0; i < tableList.Count; i++)
    {
        if (tableList[i][0].Equals(tableName))
        {
            WriteLine($"{i + 1}. {tableList[i][0]} (Active)");
        }
        else
        {
            WriteLine($"{i + 1}. {tableList[i][0]}");
        }
    }

    WriteLine($"\nType corresponding number to change Habit. 0 to create new Habit. 'X' to go back.");
    Write("Selection: ");
    string selection = ReadLine()!;

    bool isInt = int.TryParse(selection, out int y);
    if (isInt && y <= tableList.Count && y > 0)
    {
        tableName = tableList[y - 1][0];
        habitUnit = tableList[y - 1][1];
        Clear();
    }
    else if (isInt && y == 0)
    {
        CreateNewHabit();
    }
    else if (selection.ToLower() == "x")
    {
        Clear();
    }
    else
    {
        Clear();
        WriteLine($"Invalid input.\n");
    }
}

List<string[]> GetExistingTables()
{
    List<string[]> tables = [];

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();
    using var tableCmd2 = connection.CreateCommand();
    tableCmd.CommandText =
        @"SELECT *
            FROM sqlite_master
            WHERE type = 'table'
            AND name NOT LIKE 'sqlite_%'";
    SqliteDataReader reader = tableCmd.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            string[] tableArray = new string[2];
            tableArray[0] = reader.GetString(1);
            tableArray[1] = GetHabitUnit(tableArray[0]);
            tables.Add(tableArray);
        }
    }
    else
    {
        WriteLine("No rows found");
    }

    return tables;
}

string GetHabitUnit(string table)
{
    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    using var tableCmd = connection.CreateCommand();
    tableCmd.CommandText =
        @$"SELECT * FROM {table}";
    SqliteDataReader reader = tableCmd.ExecuteReader();

    return reader.GetName(2);
}

void CreateNewHabit()
{
    Clear();
    WriteLine($"CREATE NEW HABIT\n");
    WriteLine($"Type name of new Habir (one word): ");
    string habitName = ReadLine()!;

    if (habitName != null && !habitName.Contains(' '))
    {
        WriteLine($"Type unit of measure to use (one word): ");
        string measure = ReadLine()!;

        if (measure != null && !measure.Contains(' '))
            try
            {
                CreateTable(connectionString, habitName, measure);
                Clear();
                WriteLine($"Habit created successfully.\n");
                return;
            }
            catch (Exception e)
            {
                Clear();
                WriteLine($"Habit creation failed: {e.Message}\n");
                throw;
            }
    }

    Clear();
    WriteLine($"Invalid input.\n");
}

#endregion

internal class Habit
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Measure { get; set; }
}