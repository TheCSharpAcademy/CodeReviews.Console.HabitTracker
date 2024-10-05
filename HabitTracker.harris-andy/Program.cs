using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

string connectionString = @"Data Source=HabitTracker.db";

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habits (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Hobby TEXT,
        Units TEXT,
        Quantity INTEGER
        )";

    tableCmd.ExecuteNonQuery();

    using (var command = new SqliteCommand("SELECT COUNT(*) FROM habits;", connection))
    {
        var count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
            PopulateDatabase();
        }
    }
    connection.Close();
}

MainMenu();

void MainMenu()
{
    Console.Clear();
    bool closeApp = false;
    bool withIds = false;
    while (closeApp == false)
    {
        string message =
            "--------------------------------------------------\n" +
            "\n\t\tMAIN MENU\n\n" +
            "\tWhat would you like to do?\n\n" +
            "\tType 0 to Close Application\n" +
            "\tType 1 to View All Records\n" +
            "\tType 2 to Insert Record\n" +
            "\tType 3 to Delete Record\n" +
            "\tType 4 to Update Record\n" +
            "\tType 5 to View A Record Summary\n" +
            "\tType 6 to Delete All Records :(\n" +
            "\tType 7 to Add 100 Rows of Fake Data\n" +
            "--------------------------------------------------\n";

        int inputNumber = -1;
        while (inputNumber < 0)
        {
            inputNumber = validateNumberEntry(message, isMainMenu: true);
            Console.Clear();
        }

        switch (inputNumber)
        {
            case 0:
                Console.WriteLine("\nBye!\n");
                closeApp = true;
                Environment.Exit(0);
                break;
            case 1:
                GetAllRecords(withIds);
                break;
            case 2:
                Insert();
                break;
            case 3:
                Delete();
                break;
            case 4:
                Update();
                break;
            case 5:
                GetRecordSummary();
                break;
            case 6:
                DeleteTableContents();
                break;
            case 7:
                PopulateDatabase();
                break;
            default:
                Console.Clear();
                Console.WriteLine("\nInvalid Command. Give me number!");
                break;
        }
    }
}

void Insert()
{
    Console.Clear();
    string date = GetDateInput();
    Console.Clear();
    string hobby = GetHobby();
    Console.Clear();
    string units = GetUnitsInput();
    Console.Clear();
    int quantity = GetQuantityInput();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("INSERT INTO habits (Date, Hobby, Units, Quantity) VALUES (@date, @hobby, @units, @quantity)", connection))
        {
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@hobby", hobby);
            command.Parameters.AddWithValue("@units", units);
            command.Parameters.AddWithValue("@quantity", quantity);

            command.ExecuteNonQuery();
        }
        connection.Close();
    }
}

string GetDateInput()
{
    string? date = null;
    while (!DateTime.TryParseExact(date, format: "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("Enter date in format dd-mm-yyyy. Press 0 to return to Main Menu");
        date = Console.ReadLine();
        if (int.TryParse(date, out int number))
        {
            if (number == 0) MainMenu();
        }
    }
    return date;
}

string GetUnitsInput()
{
    string units = "";

    while (string.IsNullOrWhiteSpace(units))
    {
        Console.WriteLine("Enter whatever unit of measure you'd like to use. Or press 0 to return to Main Menu");
        string? temp = Console.ReadLine();
        if (temp != null)
        {
            units = temp.Trim().ToLower();
        }
        if (units == "0") MainMenu();
    }
    return units;
}

int GetQuantityInput()
{
    int quantity = -1;
    while (quantity < 0)
    {
        string message = "Enter amount of activity done/consumed/lost/whatever (greater than 0). Or press 0 to return to Main Menu";
        quantity = validateNumberEntry(message);
    }
    return quantity;
}

string GetHobby()
{
    string? hobby = "";
    while (string.IsNullOrWhiteSpace(hobby))
    {
        Console.WriteLine("Enter name of activity (keep it short). Or press 0 to return to Main Menu");
        string? temp = Console.ReadLine();
        hobby = temp?.Trim().ToLower();
    }
    if (hobby == "0") MainMenu();
    return hobby;
}

void GetAllRecords(bool withIds)
{
    Console.Clear();
    string sortMessage = "Sort records by\n1. ID\n2. Date";
    int sortOption = -1;
    while (sortOption < 0 || sortOption > 2)
    {
        sortOption = validateNumberEntry(sortMessage);
    }

    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        List<HobbyRecord> hobbiesSortedID = new List<HobbyRecord>();

        connection.Open();
        using (SqliteCommand command = new SqliteCommand("SELECT * FROM habits", connection))
        {
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    hobbiesSortedID.Add(new HobbyRecord
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), format: "dd-MM-yyyy", new CultureInfo("en-US")),
                        Hobby = reader.GetString(2),
                        Units = reader.GetString(3),
                        Quantity = reader.GetInt32(4)
                    });
                }
            }
        }
        connection.Close();
        Console.WriteLine("--------------------------------------------------\n");
        Console.WriteLine("Here's all the fun stuff you did!\n");
        if (hobbiesSortedID.Count == 0)
        {
            Console.WriteLine("No records found. Do stuff!");
        }
        var hobbiesSortedDate = hobbiesSortedID.OrderBy(record => record.Date).ToList();
        foreach (HobbyRecord record in sortOption == 1 ? hobbiesSortedID : hobbiesSortedDate)
        {
            string outputByDate = $"{record.Date.ToString("dd-MMM-yyyy"),-13} {record.Hobby,-14} {record.Units,-5}: {record.Quantity,-5}";
            string outputByID = $"{record.Id,-3}: {record.Date.ToString("dd-MMM-yyyy"),-13} {record.Hobby,-14} {record.Units,-5}: {record.Quantity,-5}";
            if (sortOption == 1) Console.WriteLine(outputByID);
            if (sortOption == 2) Console.WriteLine(outputByDate);
        }
        Console.WriteLine("--------------------------------------------------\n");
    }
}

void GetRecordSummary()
{
    (string searchTerm, string searchTermCategory) = GetSearchTerm();
    Console.Clear();

    using (var connection = new SqliteConnection(connectionString))
    {
        SqliteCommand chosenCommand;
        var commandYear = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE SUBSTR(Date, 7, 4) = @year GROUP BY Year, Hobby;", connection);
        var commandHobby = new SqliteCommand("SELECT Hobby, Units, COUNT(*) AS TotalCount, SUM(Quantity) AS TotalUnits FROM habits WHERE Hobby = @hobby GROUP BY Hobby, Units;", connection);
        var commandUnits = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE Units = @units GROUP BY Hobby, Units;", connection);

        if (searchTermCategory == "year")
        {
            chosenCommand = commandYear;
            string year = searchTerm;
            chosenCommand.Parameters.AddWithValue("@year", year);
        }
        else if (searchTermCategory == "hobby")
        {
            chosenCommand = commandHobby;
            string hobby = searchTerm;
            chosenCommand.Parameters.AddWithValue("@hobby", hobby);
        }
        else
        {
            chosenCommand = commandUnits;
            string units = searchTerm;
            chosenCommand.Parameters.AddWithValue("@units", units);
        }

        connection.Open();
        using (chosenCommand)
        {
            using (var reader = chosenCommand.ExecuteReader())
            {
                if (searchTermCategory == "hobby")
                {
                    if (reader.Read())
                    {
                        string activity = reader.GetString(0);
                        string units = reader.GetString(1);
                        int count = reader.GetInt32(2);
                        int quantity = reader.GetInt32(3);
                        string howManyTimes = count == 1 ? "time" : "times";

                        Console.WriteLine("--------------------------------------------------\n");
                        Console.WriteLine($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1)} done {count} {howManyTimes} for {quantity} {units}. Nice!\n");
                        Console.WriteLine("--------------------------------------------------\n");
                    }
                }
                else
                {
                    int totalQuantity = 0;
                    List<string> actionRecord = new();
                    string howManyActivities = actionRecord.Count == 1 ? "activity" : "activities";

                    while (reader.Read())
                    {
                        int year = reader.GetInt32(0);
                        string activity = reader.GetString(1);
                        string units = reader.GetString(2);
                        int quantity = reader.GetInt32(3);
                        totalQuantity += quantity;
                        actionRecord.Add($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1),-12}: {quantity} {units}");
                    }

                    string yearOutput = $"Completed {actionRecord.Count} {howManyActivities} in {searchTerm}:\n";
                    string unitsOutput = $"{totalQuantity} {searchTerm} completed across {actionRecord.Count} {howManyActivities}:\n";

                    Console.WriteLine("\n");
                    Console.WriteLine("--------------------------------------------------\n");
                    if (searchTermCategory == "year") Console.WriteLine(yearOutput);
                    if (searchTermCategory == "units") Console.WriteLine(unitsOutput);
                    foreach (string item in actionRecord)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("--------------------------------------------------\n");
                }
            }
        }
        connection.Close();
    }
}

Tuple<string, string> GetSearchTerm()
{
    Console.Clear();
    string searchTermCategory = "";
    string searchTerm = "";
    List<string> searchOptions = new List<string>();
    string message = "Choose a summary by:\n1. Year\n2. Hobby\n3. Units (e.g. how many miles or hours)";

    int number = -1;
    while (number < 1 || number > 3)
    {
        Console.Clear();
        number = validateNumberEntry(message);
    }

    switch (number)
    {
        case 1:
            searchTermCategory = "year";
            break;
        case 2:
            searchTermCategory = "hobby";
            break;
        case 3:
            searchTermCategory = "units";
            break;
        default:
            Console.WriteLine("Invalid answer. Need number between 1-3:");
            break;
    }


    using (var connection = new SqliteConnection(connectionString))
    {
        var commandYear = new SqliteCommand("SELECT DISTINCT SUBSTR(Date, 7, 4) AS Year FROM habits;", connection);
        var commandHobby = new SqliteCommand("SELECT DISTINCT Hobby FROM habits;", connection);
        var commandUnits = new SqliteCommand("SELECT DISTINCT Units FROM habits;", connection);

        SqliteCommand chosenCommand = (searchTermCategory == "year") ? commandYear
            : (searchTermCategory == "hobby") ? commandHobby
            : commandUnits;

        connection.Open();

        using (chosenCommand)
        {
            using (var reader = chosenCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    searchOptions.Add(reader.GetString(0));
                }
            };
        }
        connection.Close();
        Console.Clear();
        searchOptions.Sort();

        if (searchOptions.Count == 0)
        {
            Console.WriteLine("No records found");
        }
        else
        {
            string messageSearchTerm = $"Enter the number for which {searchTermCategory} you'd like a summary. Or press 0 to return to Main Menu";
            int tempNumber = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("Available Units:");
                for (int i = 0; i < searchOptions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {searchOptions[i]}");
                }
                tempNumber = validateNumberEntry(messageSearchTerm);
            } while (tempNumber < 0 || tempNumber > searchOptions.Count);
            searchTerm = searchOptions[tempNumber - 1];
        }
    }
    return Tuple.Create(searchTerm, searchTermCategory);
}

void Delete()
{
    Console.Clear();
    bool withIds = true;
    GetAllRecords(withIds);
    string message = "Enter the ID number for the record you'd like to delete. Or enter 0 to return to Main Menu.";
    int recordID = validateNumberEntry(message);

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("DELETE FROM habits WHERE ID = @id", connection))
        {
            command.Parameters.AddWithValue("@id", recordID);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }
    Console.WriteLine($"Record ID {recordID} has been deleted.");
}

void Update()
{
    // Console.Clear();
    bool withIds = true;
    bool recordExists = false;
    GetAllRecords(withIds);
    string message = "Enter the ID number for the record you'd like to update. Or enter 0 to return to Main Menu.";
    int recordID = validateNumberEntry(message);
    if (recordID == 0) MainMenu();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("SELECT * FROM habits WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@id", recordID);
            recordExists = Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        connection.Close();
    }
    if (!recordExists)
    {
        Console.Clear();
        Console.WriteLine("No record found with that ID. Try again.");
        Thread.Sleep(2000);
        Update();
    }

    Console.Clear();
    Console.WriteLine("Enter the updated record:");
    string date = GetDateInput();
    string hobby = GetHobby();
    string units = GetUnitsInput();
    int quantity = GetQuantityInput();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("UPDATE habits SET date = @date, hobby = @hobby, units = @units, quantity = @quantity WHERE Id = @id", connection))
        {
            command.Parameters.AddWithValue("@id", recordID);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@hobby", hobby);
            command.Parameters.AddWithValue("@units", units);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.ExecuteNonQuery();
        }
        connection.Close();
    }
    Console.WriteLine($"Record ID {recordID} has been updated.");
}

void PopulateDatabase()
{
    Random random = new Random();
    Dictionary<string, string> fakeHobbies = new()
    {
        {"walking", "miles"},
        {"hiking", "miles"},
        {"drinking", "beers"},
        {"sleeping", "hours"},
        {"gaming", "hours"},
        {"programming", "hours"}
    };
    string[] fakeActivities = ["walking", "hiking", "drinking", "sleeping", "gaming", "programming"];

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        for (int randomRecord = 0; randomRecord < 100; randomRecord++)
        {
            int randomIndex = random.Next(fakeActivities.Length);
            string date = GetRandomDate();
            string hobby = fakeActivities[randomIndex];
            string units = fakeHobbies[hobby];
            int quantity = random.Next(10);

            using (var command = new SqliteCommand("INSERT INTO habits (Date, Hobby, Units, Quantity) VALUES (@date, @hobby, @units, @quantity)", connection))
            {
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@hobby", hobby);
                command.Parameters.AddWithValue("@units", units);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.ExecuteNonQuery();
            }
        }
        connection.Close();
    }
}

string GetRandomDate()
{
    Random random = new Random();
    string day = Convert.ToString(random.Next(1, 31));
    string month = Convert.ToString(random.Next(1, 13));
    string year = Convert.ToString(random.Next(2023, 2025));
    return $"{day.PadLeft(2, '0')}-{month.PadLeft(2, '0')}-{year}";
}

int validateNumberEntry(string message, bool isMainMenu = false)
{
    string? numberInput = null;
    int finalInput = -1;
    while (finalInput < 0)
    {
        Console.WriteLine(message);
        numberInput = Console.ReadLine();
        if (numberInput == "0")
        {
            if (isMainMenu)
            {
                return 0;
            }
            else
            {
                MainMenu();
                return 1;
            }
        }
        if (!int.TryParse(numberInput, out finalInput))
        {
            return -1;
        }
    }
    return finalInput;
}

void DeleteTableContents()
{
    Console.Clear();
    string? verify = null;
    string[] prompts = {
        "Are you SURE you want to delete all this data? y/n",
        "Are you REALLY SUPER SURE? y/n",
        "Last chance to turn back! Are you ABSOLUTELY TOTALLY POSITIVELY SURE? y/n"
    };

    foreach (string prompt in prompts)
    {
        Console.WriteLine(prompt);
        verify = Console.ReadLine()?.ToLower();
        if (verify != "y")
        {
            return;
        }
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("DELETE FROM habits;", connection))
        {
            command.ExecuteNonQuery();
        }
        connection.Close();
    }

    Console.WriteLine("\n\n");
    Console.WriteLine("Whelp. It's all gone. Now what...?");
    Thread.Sleep(2000);
    MainMenu();
}
class HobbyRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Hobby { get; set; }
    public string? Units { get; set; }
    public int Quantity { get; set; }
}