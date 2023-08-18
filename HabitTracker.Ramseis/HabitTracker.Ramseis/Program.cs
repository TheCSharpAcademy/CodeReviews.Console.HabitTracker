using Microsoft.Data.Sqlite;
using System.Globalization;

const string CONNECTIONSTRING = @"Data Source=habit-Tracker.db";

using (SqliteConnection connection = new SqliteConnection(CONNECTIONSTRING))
{
    connection.Open();
    SqliteCommand tableCmd = connection.CreateCommand();
    tableCmd.CommandText =
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";
    tableCmd.ExecuteNonQuery();
    connection.Close();
}

GetUserInput();
static void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (!closeApp)
    {
        Console.Write(
            "\n\nMain Menu - Water Drinking Database\n" +
            "─────────────────────────────────────────────────────\n" +
            "What would you like to do?\n" +
            "   0 - Close application\n" +
            "   1 - View ALL records\n" +
            "   2 - Insert record\n" +
            "   3 - Delete record\n" +
            "   4 - Update record\n" +
            "─────────────────────────────────────────────────────\n" +
            ": ");
        int.TryParse(Console.ReadLine().Trim(), out int command);
        if (command == 0)
        {
            Console.Write("\n\nGoodbye...\n");
            closeApp = true;
            Environment.Exit(0);
        }
        else if (command == 1)
        {
            GetAllRecords();
            Console.WriteLine(
                "\n\n─────────────────────────────────────────────────────\n" +
                "Press any key to return to the main menu...");
            Console.ReadKey();
            Console.Clear();
        }
        else if (command == 2)
        {
            InsertRecord();
            Console.Clear();
        }
        else if (command == 3)
        {
            DeleteRecord();
            Console.Clear();
        }
        else if (command == 4)
        {
            UpdateRecord();
            Console.Clear();
        }
        else
        {
            Console.Clear();
            Console.Write(
                "─────────────────────────────────────────────────────\n" +
                "Invalid command. Please select from the listed options!\n");
        }

    }
}
static void InsertRecord()
{
    string date = GetRecordDateInput();
    int quantity = GetRecordNumberInput("Enter integer quantity of item.", true);

    SqlWrite($"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})");
}
static void GetAllRecords()
{
    List<DrinkingWater> data = SqlRead("SELECT * FROM drinking_water ");
    Console.Clear();
    Console.Write(
        "\n\n─────────────────────────────────────────────────────\n" +
        "ID\tDate (d-m-y)\tQuantity\n");
    foreach (DrinkingWater dw in data)
    {
        Console.Write($"{dw.Id}\t{dw.Date.ToString("dd-MM-yyyy")}\t{dw.Quantity}\n");
    }
}
static void DeleteRecord()
{
    GetAllRecords();
    int recordId = GetRecordNumberInput("Enter the ID of the record to delete.", false);
    SqlWrite($"DELETE from drinking_water WHERE Id = '{recordId}'");
}
static void UpdateRecord()
{
    GetAllRecords();
    int recordId = GetRecordNumberInput("Enter the ID of the record to modify.", false);

    using (SqliteConnection connection = new SqliteConnection(CONNECTIONSTRING))
    {
        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 from drinking_water WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.Write($"\nRecord with ID {recordId} not found.\n");
            connection.Close();
            Console.ReadKey();
        }
        else
        {
            string date = GetRecordDateInput();
            int quantity = GetRecordNumberInput("Enter integer quantity of item.", true);
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
static int GetRecordNumberInput(string message, bool clear)
{
    if (clear) Console.Clear();
    Console.Write(
        $"\n\n{message}\n" +
        "Enter 0 to return to main menu.\n" +
        "─────────────────────────────────────────────────────\n" +
        ": ");
    int.TryParse(Console.ReadLine().Trim(), out int integerInput);
    if (integerInput == 0)
    {
        GetUserInput();
    }
    return integerInput;
}
static string GetRecordDateInput()
{
    Console.Clear();
    Console.Write(
        "\n\nPlease enter the date: (dd-mm-yy).\n" +
        "Enter 0 to return to main menu.\n" +
        "─────────────────────────────────────────────────────\n" +
        ": ");

    string dateInput = Console.ReadLine().Trim().ToLower();
    if (dateInput == "0")
    {
        GetUserInput();
    }
    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.Write("\nInvalid Date. (Format: dd-mm-yy).\n");
        dateInput = Console.ReadLine().Trim().ToLower();
    }

    return dateInput;
}
static void SqlWrite(string command)
{
    using (SqliteConnection connection = new SqliteConnection(CONNECTIONSTRING))
    {
        connection.Open();
        SqliteCommand tableCmd = connection.CreateCommand();
        tableCmd.CommandText = command;
        int rowCount = tableCmd.ExecuteNonQuery();
        if (rowCount == 0)
        {
            Console.Write("\nNo records changed. Did you input valid data?\n");
            Console.ReadKey();
        }
        connection.Close();
    }
}
static List<DrinkingWater> SqlRead(string command)
{
    List<DrinkingWater> tableData = new();
    using (SqliteConnection connection = new SqliteConnection(CONNECTIONSTRING))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = command;
        SqliteDataReader reader = tableCmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        connection.Close();
    }
    return tableData;
}
class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}