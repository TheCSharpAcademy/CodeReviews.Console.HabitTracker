using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = @"Data Source=habit-Tracker.db";
List<string> dbNames = new List<string>();
string dbName;

MainMenu(connectionString, dbNames);

string FormatTableName(string tableName)
{
    string formattedTableName = tableName;

    if (formattedTableName.IndexOf("_") != -1)
    {
        formattedTableName = formattedTableName.Replace("_", " ").ToLower();

    }

    char firstLetter = char.ToUpper(formattedTableName[0]);

    formattedTableName = firstLetter + formattedTableName.Substring(1);

    return formattedTableName;
}

void GetUserInput()
{
    Console.Clear();

    bool closeApp = false;

    while (!closeApp)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to go back to the main menu.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Update Record.");
        Console.WriteLine("Type 4 to Delete Record.");
        Console.WriteLine("Type 5 to View Past Year's Stats");
        Console.WriteLine("------------------------------------------------");

        string commandInput = Console.ReadLine().Trim();

        switch (commandInput)
        {
            case "0":
                closeApp = true;
                break;
            case "1":
                GetAllRecords();
                break;
            case "2":
                Insert();
                break;
            case "3":
                Update();
                break;
            case "4":
                Delete();
                break;
            case "5":
                GetPastYearsStats();
                break;
            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                break;
        }

    }
}

void Insert()
{
    string date = GetDateInput();

    int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var colCmd = connection.CreateCommand();
        colCmd.CommandText = $"SELECT * FROM {dbName}";

        var reader = colCmd.ExecuteReader();

        var quantityColumnName = reader.GetName(2);

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $@"INSERT INTO {dbName} (date, {quantityColumnName}) VALUES ('{date}', {quantity})";

        tableCmd.ExecuteNonQuery();
        connection.Close();
    }
}

void GetAllRecords()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var colCmd = connection.CreateCommand();
        colCmd.CommandText = $"SELECT * FROM {dbName}";

        var colReader = colCmd.ExecuteReader();

        var quantityColumnName = colReader.GetName(2);

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"SELECT * FROM {dbName}";

        List<Habit> tableData = new();

        // tableCmd is executed, and returns a Reader object that can read the data retrieved
        SqliteDataReader reader = tableCmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new Habit
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

        Console.WriteLine("----------------------------------------\n");

        foreach (var dw in tableData)
        {

            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - {quantityColumnName}: {dw.Quantity}");

        }

        Console.WriteLine("----------------------------------------\n");

    }
}

void Update()
{

    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var colCmd = connection.CreateCommand();
        colCmd.CommandText = $"SELECT * FROM {dbName}";

        var reader = colCmd.ExecuteReader();

        var quantityColumnName = reader.GetName(2);

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {dbName} WHERE Id = {recordId})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
            connection.Close();
            Update();
        }

        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE {dbName} SET date = '{date}', {quantityColumnName} = {quantity} WHERE Id = {recordId}";

        tableCmd.ExecuteNonQuery();

        connection.Close();

    }
}

void Delete()
{
    // Console.Clear();
    GetAllRecords();

    var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"DELETE from {dbName} WHERE Id = '{recordId}'";

        int rowCount = tableCmd.ExecuteNonQuery();

        if (rowCount == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
            Delete();
        }

    }

    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);

    string numberInput = Console.ReadLine();

    if (numberInput == "0") GetUserInput();

    while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
    {
        Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        numberInput = Console.ReadLine();
    }

    int finalInput = Convert.ToInt32(numberInput);

    return finalInput;
}

string GetDateInput()
{
    Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");

    string dateInput = Console.ReadLine();

    if (dateInput == "0") GetUserInput();

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

void MainMenu(string connectionString, List<string> dbNames)
{
    bool closeApp = false;

    while (!closeApp)
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var dbCmd = connection.CreateCommand();
            dbCmd.CommandText = @"SELECT name FROM sqlite_schema WHERE type='table' AND name NOT LIKE 'sqlite%'";

            SqliteDataReader reader = dbCmd.ExecuteReader();

            var index = 0;

            Console.WriteLine("Habits:\n\n");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var tableName = reader.GetString(0);

                    if (!dbNames.Contains(tableName))
                    {
                        dbNames.Add(tableName);
                    }

                    var formattedTableName = FormatTableName(tableName);
                    Console.WriteLine($"{++index}: {formattedTableName}");
                }
            }

            connection.Close();
        }

        Console.WriteLine("\n---------------------------------------------------------------------------------------------------------");

        Console.WriteLine("\nChoose habit to track (enter corresponding number), or press 0 to create new habit. Enter -1 to close the program.");

        Console.WriteLine("\n---------------------------------------------------------------------------------------------------------\n\n");

        var userChoice = Console.ReadLine().Trim();

        while (!int.TryParse(userChoice, out _))
        {
            Console.WriteLine("\n\nInvalid input. Please try again.\n\n");

            userChoice = Console.ReadLine().Trim();

        }

        if (userChoice == "-1")
        {
            Console.WriteLine("Goodbye!");
            closeApp = true;
            Environment.Exit(0);
        }

        else if (userChoice == "0")
        {
            CreateNewHabit();
        }

        else
        {
            try
            {
                var dbNameIndex = int.Parse(userChoice) - 1;

                dbName = dbNames[dbNameIndex];

                GetUserInput();
            }

            catch (Exception ex)
            {
                Console.WriteLine("\n\nInvalid input.\n\n");
            }
        }

    }

    
}

void CreateNewHabit()
{
    Console.Clear();
    Console.WriteLine("\n\nPlease enter the habit you wish to track (ex. 'working out')\n\n");

    var habit = Console.ReadLine().Trim().ToLower().Replace(" ", "_");

    Console.WriteLine("How is your habit measured? (ex. 'hours', 'cups', 'calories')");
    var measurement = Console.ReadLine();
    measurement = char.ToUpper(measurement[0]) + measurement.Substring(1);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, {measurement} INTEGER)";

        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

}
void GetPastYearsStats()
{
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var colNameCmd = connection.CreateCommand();
        colNameCmd.CommandText = $"SELECT * FROM {dbName}";
        var reader = colNameCmd.ExecuteReader();
        Console.WriteLine($"{reader.GetName(2)}");

        var amtCmd = connection.CreateCommand();
        amtCmd.CommandText = $"SELECT SUM({reader.GetName(2)}) FROM {dbName}";
        var totalAmt = amtCmd.ExecuteScalar();

        var timesCmd = connection.CreateCommand();
        timesCmd.CommandText = $"SELECT COUNT(*) FROM {dbName}";
        var numTimes = timesCmd.ExecuteScalar();

        Console.WriteLine($"\n\nNumber of Times:\t\tTotal ({reader.GetName(2)}):");
        Console.WriteLine($"{numTimes}\t\t\t\t{totalAmt}");

        connection.Close();

    }
}

public class Habit
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}