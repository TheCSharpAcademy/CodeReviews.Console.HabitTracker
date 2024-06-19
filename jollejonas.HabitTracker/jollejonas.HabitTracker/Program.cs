using Microsoft.Data.Sqlite;
using System.Globalization;

string connectionString = "Data Source=habit-tracker.db;";
List <string> habits = new List<string>();
using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();

    using (SqliteCommand createTableCmd = new SqliteCommand())
    {
        createTableCmd.Connection = connection;
        createTableCmd.CommandText = $"CREATE TABLE IF NOT EXISTS drinkedwater(" +
            $"id INTEGER PRIMARY KEY AUTOINCREMENT," +
            $"date TEXT, " +
            $"quantity INTEGER, " +
            $"unit TEXT DEFAULT 'glass(es)')";
    createTableCmd.ExecuteNonQuery();
    }

    using (SqliteCommand getHabitsCmd = new SqliteCommand())
    {

       getHabitsCmd.Connection = connection;
        getHabitsCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
        using (var reader = getHabitsCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                if (reader.GetString(0) != "sqlite_sequence")
                {
                    habits.Add(reader.GetString(0));
                }
            }
        }
    }

    using (SqliteCommand insertRecordsCmd = new SqliteCommand())
    {
        insertRecordsCmd.Connection = connection;
        insertRecordsCmd.CommandText = $"SELECT COUNT(*) FROM drinkedwater";
        int count = Convert.ToInt32(insertRecordsCmd.ExecuteScalar());
        if (count == 0)
        {
            insertRecordsCmd.CommandText = $"INSERT INTO drinkedwater (date, quantity) VALUES " +
                $"('2024-01-01', 3), " +
                $"('2024-01-02', 2), " +
                $"('2024-01-03', 1)";
            insertRecordsCmd.ExecuteNonQuery();
        }
    }

        connection.Close();
}

while(true)
{
    Console.WriteLine("Welcome to your Habit Tracker.");
    Console.WriteLine("Here are your options: ");
    Console.WriteLine("1.\t Create new habit.");
    Console.WriteLine("2.\t Show all registered habits");
    Console.WriteLine("3.\t View all records.");
    Console.WriteLine("4.\t Insert record.");
    Console.WriteLine("5.\t Delete record.");
    Console.WriteLine("6.\t Update record.");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("7.\t Accumulated quantity year to date");
    Console.WriteLine("8.\t Accumulated quantity month to date");
    Console.WriteLine("9.\t Accumulated quantity week to date");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("0.\t Close application");
    Console.Write("What would you like to do? ");

    string? option = Console.ReadLine();

    switch (option)
    {
        case "0":
            Console.WriteLine("Goodbye!");
            return;
        case "1":
            CreateNewHabit();
            break;
        case "2":
            ViewAllHabits();
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            break;
        case "3":
            ViewAllRecords();
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            break;
        case "4":
            InsertRecord();
            break;
        case "5":
            DeleteRecord();
            break;
        case "6":
            UpdateRecord();
            break;
        case "7":
            SummarizedQuantity("year");
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            break;
        case "8":
            SummarizedQuantity("month");
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            break;
        case "9":
            SummarizedQuantity("day");
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            break;
        default:
            while (option != "0" && option != "1" && option != "2" && option != "3" && option != "4")
            {
                Console.WriteLine("Invalid option.");
                Console.Write("What would you like to do? ");
                option = Console.ReadLine();
            }
            break;
    }
    Console.Clear();
}

void ViewAllHabits()
{
    Console.WriteLine("Registered habits: ");
    foreach (var habit in habits)
    {
        int index = habits.IndexOf(habit);
        Console.WriteLine($"{index}. {habit}");
    }
}
void ViewAllRecords()
{
    string habitName = SelectHabit();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = $"SELECT * FROM [{habitName}]";
        if (selectCmd.ExecuteScalar() == null)
        {
            Console.WriteLine("No records found. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)}\tDate: {reader.GetString(1)}\tQuantity: {reader.GetInt32(2)}{reader.GetString(3)}");
            }
        }
    }
}

void CreateNewHabit()
{
    Console.WriteLine("Already tracked habits: ");
    ViewAllHabits();
    Console.Write("Insert habit name: ");
    string? habit = Console.ReadLine();
    Console.Write($"Insert you unit of measure:");
    string? unit = Console.ReadLine();
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        using (SqliteCommand createTableCmd = new SqliteCommand())
        {
            createTableCmd.Connection = connection;
            createTableCmd.CommandText = $"CREATE TABLE IF NOT EXISTS [{habit}](" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"date TEXT," +
                $"unit TEXT DEFAULT '{unit}'," +
                $"quantity INTEGER)";
            createTableCmd.ExecuteNonQuery();
        }

        connection.Close();
    }
}
void InsertRecord()
{
    string habitName = SelectHabit();
    Console.Write("Insert date(dd-mm-yyyy): ");
    string? date = Console.ReadLine();

    // Check if valid date not later than today's date
    DateTime parsedDate;
    while (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) || parsedDate > DateTime.Today)
    {
        Console.WriteLine("Invalid date. Please insert a valid date not later than today's date (dd-mm-yyyy).");
        Console.Write("Insert date(dd-mm-yyyy): ");
        date = Console.ReadLine();
    }

    string formattedDate = parsedDate.ToString("yyyy-MM-dd");
    int quantity = 0;
    Console.Write("Insert glasses of water drinked: ");
    while (!int.TryParse(Console.ReadLine(), out quantity))
    {
        Console.WriteLine("Invalid input. Please insert a number.");
        Console.Write("Insert quantity: ");
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();

        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = $"INSERT INTO {habitName} (date, quantity) VALUES ('{formattedDate}', {quantity})";
        insertCmd.ExecuteNonQuery();
    }
    habits.Add(habitName);
    Console.WriteLine("Record inserted successfully.");
    Console.Write("Press any key to continue.");
    Console.ReadKey();
}

void DeleteRecord()
{
    string habitName = SelectHabit();
    Console.WriteLine("List of records: ");
    ViewAllRecords();
    Console.Write("Insert id of record you want to delete: ");
    int id = 0;
    while (!int.TryParse(Console.ReadLine(), out id))
    {
        Console.WriteLine("Invalid input. Please insert a number.");
        Console.Write("Insert id of record you want to delete: ");
    }

    using(var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = $"DELETE FROM {habitName} WHERE id = {id}";
        if (deleteCmd.ExecuteNonQuery() == 0)
        {
            Console.WriteLine("Record not found.");
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            return;
        }
        deleteCmd.ExecuteNonQuery();
    }
    Console.WriteLine("Record deleted successfully.");
    Console.Write("Press any key to continue.");
    Console.ReadKey();
}

void UpdateRecord()
{
    string habitName = SelectHabit();
    Console.WriteLine("List of records: ");
    ViewAllRecords();
    Console.Write("Insert id of record you want to update: ");
    int id = 0;
    while (!int.TryParse(Console.ReadLine(), out id))
    {
        Console.WriteLine("Invalid input. Please insert a number.");
        Console.Write("Insert id of record you want to update: ");
    }

    Console.Write("Insert date(dd-mm-yyyy): ");
    string? date = Console.ReadLine();

    DateTime parsedDate;
    while (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) || parsedDate > DateTime.Today)
    {
        Console.WriteLine("Invalid date. Please insert a valid date not later than today's date (dd-mm-yyyy).");
        Console.Write("Insert date(dd-mm-yyyy): ");
        date = Console.ReadLine();
    }

    string formattedDate = parsedDate.ToString("yyyy-MM-dd");
    Console.Write("Insert new quantity: ");
    int quantity = 0;
    while (!int.TryParse(Console.ReadLine(), out quantity))
    {
        Console.WriteLine("Invalid input. Please insert a number.");
        Console.Write("Insert new quantity: ");
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var updateCmd = connection.CreateCommand();
        updateCmd.CommandText = $"UPDATE {habitName} SET date = '{formattedDate}', quantity = {quantity} WHERE id = {id}";
        if (updateCmd.ExecuteNonQuery() == 0)
        {
            Console.WriteLine("Record not found.");
            Console.Write("Press any key to continue.");
            Console.ReadKey();
            return;
        }
        updateCmd.ExecuteNonQuery();
    }
    Console.WriteLine("Record updated successfully.");
    Console.Write("Press any key to continue.");
    Console.ReadKey();
}

string SelectHabit()
{
    ViewAllHabits();
    Console.Write("Select the ID for the habit you want to see records for: ");
    int habitId = 0;
    while (!int.TryParse(Console.ReadLine(), out habitId))
    {
        Console.WriteLine("Invalid input. Please insert a number.");
        Console.Write("Select the ID for the habit you want to see records for: ");
    }
    // Retrieve the habit name based on the selected ID
    string habitName = habits[habitId];

    while (habitName == null)
    {
        Console.WriteLine("Invalid habit ID. Press any key to continue.");
        Console.ReadKey();
        SelectHabit();
    }

    return habitName;
}

void SummarizedQuantity(string interval)
{
    string habitName = SelectHabit();
    int intervalDays = 0;
    switch(interval)
    {
        case "year":
            intervalDays = -365;
            break;
        case "month":
            intervalDays = -30;
            break;
        case "week":
            intervalDays = -7;
            break;
    }
    string today = DateTime.Today.ToString("yyyy-MM-dd");
    string intervalDate = DateTime.Today.AddDays(intervalDays).ToString("yyyy-MM-dd");
    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = $"SELECT COALESCE(SUM(quantity), 0) FROM {habitName} WHERE date BETWEEN '{intervalDate}' AND '{today}'";
        if (selectCmd.ExecuteScalar() == null)
        {
            Console.WriteLine("No records found. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"Summarized quantity {interval} to date: {reader.GetInt32(0)}");
            }
        }
    }
}