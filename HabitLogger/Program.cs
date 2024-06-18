using Microsoft.Data.Sqlite;

CreateDB();
PrintMainMenu();
InputLoop();

static void PrintMainMenu()
{
    Console.WriteLine("Welcome to my water-drinking tracker!");
    Console.WriteLine("0 to exit");
    Console.WriteLine("I to insert a record");
    Console.WriteLine("U to update a record");
    Console.WriteLine("D to delete a record");
    Console.WriteLine("V to view all records");
}

static void CreateDB()
{
    var connectionString = @"Data Source=habit-tracker.db";

    using var connection = new SqliteConnection(connectionString);
    connection.Open();
    var query = connection.CreateCommand();
    query.CommandText =
        @"CREATE TABLE IF NOT EXISTS drinking_water (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT,
                quantity INTEGER
            )";
    query.ExecuteNonQuery();
    connection.Close();
}

static void InputLoop()
{
    while (true)
    {
        var input = Console.ReadLine() ?? "";

        switch (input.ToLower())
        {
            case "0":
                Environment.Exit(0);
                break;
            case "i":
                PrintInsertMenu();
                break;
            case "u":
                PrintUpdateMenu();
                break;
            case "d":
                PrintDeleteMenu();
                break;
            case "v":
                PrintAllDocuments();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            default:
                Console.WriteLine("Input is not recognized. Please try again.");
                continue;
        }

        Console.Clear();
        PrintMainMenu();
    }
}

static void PrintInsertMenu()
{
    Console.Clear();
    var date = GetDateInput();
    if (date == "quit") return;

    var num = GetIntInput();
    if (num == -1) return;

    Console.WriteLine("Creating entry...");
    var connectionString = @"Data Source=habit-tracker.db";

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var query = connection.CreateCommand();
    query.CommandText = $"INSERT INTO drinking_water (date, quantity) VALUES('{date}', '{num}')";
    query.ExecuteNonQuery(); // this tells the database to not return anything

    connection.Close();

    Console.WriteLine("Record created successfully! Press any key to continue...");
    Console.ReadKey();
}

static void PrintUpdateMenu()
{
    PrintAllDocuments();

    while (true)
    {
        Console.WriteLine("Please input the document ID you would like to change, or type 'quit' to exit.");
        var input = Console.ReadLine() ?? "";

        if (input == "quit") return;

        var connectionString = @"Data Source=habit-tracker.db";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $"SELECT * FROM drinking_water WHERE ID = {input}";
        var results = query.ExecuteReader();

        if (!results.HasRows)
        {
            Console.WriteLine("ID not found.");
            continue;
        }

        int id = 0;
        string date = "";
        int quantity = 0;

        while (results.Read())
        {
            id = results.GetInt32(0);
            date = results.GetString(1);
            quantity = results.GetInt32(2);
        }

        results.Close();

        Console.Clear();
        Console.WriteLine("You are updating this record:");
        Console.WriteLine($"{id}). Date: {date} Quantity: {quantity}");
        Console.WriteLine("Press Y if you would like to keep the date the same, N if you want to change it, or 0 to go back.");

        bool changeDate = false;
        ConsoleKeyInfo readKey;
        bool quit = false;

        while (true)
        {
            readKey = Console.ReadKey();

            if (readKey.Key.ToString().ToLower() == "y") changeDate = false;
            else if (readKey.Key.ToString().ToLower() == "n") changeDate = true;
            else if (readKey.Key.ToString() == "D0") quit = true;
            else continue;

            break;
        }

        if (quit)
        {
            PrintAllDocuments();
            continue;
        }

        Console.WriteLine("\n");

        if (changeDate) date = GetDateInput(); 

        if (changeDate && date == "quit") return;

        quantity = GetIntInput();

        if (quantity == -1) return;

        query.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE id = {id}";
        query.ExecuteNonQuery();

        connection.Close();

        Console.WriteLine("Record updated successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        break;
    }
}

static void PrintDeleteMenu()
{
    PrintAllDocuments();

    while (true)
    {
        Console.WriteLine("Please input the document ID you would like to delete, or type 'quit' to exit.");
        var input = Console.ReadLine() ?? "";

        if (input == "quit") return;

        var connectionString = @"Data Source=habit-tracker.db";

        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var query = connection.CreateCommand();
        query.CommandText = $"SELECT * FROM drinking_water WHERE ID = {input}";
        var results = query.ExecuteReader();

        if (!results.HasRows)
        {
            Console.WriteLine("ID not found.");
            continue;
        }

        while (results.Read())
        {
            Console.Clear();
            Console.WriteLine("You are deleting this record:");
            Console.WriteLine($"{results.GetInt32(0)}). Date: {results.GetString(1)} Quantity: {results.GetInt32(2)}");
            Console.WriteLine("Press Y to confirm, or any other key to go back.");
        }

        var readKey = Console.ReadKey();

        if (readKey.Key.ToString().ToLower() != "y")
        {
            PrintAllDocuments();
            continue;
        };

        results.Close();

        query.CommandText = $"DELETE FROM drinking_water WHERE ID = {input}";
        query.ExecuteNonQuery();

        Console.WriteLine("\nRecord deleted successfully! Press any key to go back...");
        Console.ReadKey();
        break;

    }
}

static void PrintAllDocuments()
{
    Console.Clear();
    var connectionString = @"Data Source=habit-tracker.db";

    using var connection = new SqliteConnection(connectionString);
    connection.Open();

    var query = connection.CreateCommand();
    query.CommandText = "SELECT * FROM drinking_water";
    var results = query.ExecuteReader();

    List<DrinkWater> list = [];

    if (results.HasRows)
    {
        while (results.Read())
        {
            list.Add(new DrinkWater(
                results.GetInt32(0),
                results.GetString(1),
                results.GetInt32(2)
                ));
        }
    }
    else
    {
        Console.WriteLine("No results found.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        return;
    }

    connection.Close();
    Console.WriteLine("Results:");
    
    foreach (var drink in list)
    {
        Console.WriteLine($"{drink.ID}). Date: {drink.Date} Quantity: {drink.Quantity}");
    }
}

static string GetDateInput()
{
    Console.WriteLine("Please input a valid date in the MM/DD/YYYY format. You can also type 'quit' to return to the main menu.");
    string input;

    while (true)
    {
        input = Console.ReadLine() ?? "quit";
        input = input.Replace("-", "/");
        var inputArray = input.Split('/');

        if (input == "quit") return input;
        else if (input.Replace("/", "").Replace("-", "").Any(c => !char.IsNumber(c))
            || inputArray.Length != 3
            || inputArray[0].Length > 2
            || int.Parse(inputArray[0]) > 12
            || inputArray[1].Length > 2
            || int.Parse(inputArray[1]) > 31
            || inputArray[2].Length != 4)
        {
            Console.WriteLine("Invalid input. Please try again.");
            continue;
        }
        break;
    }

    return input;
}

static int GetIntInput()
{
    Console.WriteLine("Please input the number of drinks you had. You can also type 'quit' to return to the main menu.");

    while (true)
    {
        var input = Console.ReadLine() ?? "quit";

        if (input == "quit") return -1;
        else if (input.All(c => char.IsNumber(c))) return int.Parse(input);
        else
        {
            Console.WriteLine("Invalid input. Try again");
            continue;
        }
    }
}

public class DrinkWater
{
    public int ID;
    public string Date;
    public int Quantity;

    public DrinkWater(int id, string date, int quantity)
    {
        ID = id;
        Date = date;
        Quantity = quantity;
    }
}