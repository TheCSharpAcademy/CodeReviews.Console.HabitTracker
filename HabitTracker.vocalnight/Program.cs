using Microsoft.Data.Sqlite;
using System.Globalization;

Console.WriteLine("Welcome to the habit Tracker\n");
Console.WriteLine("Here you will be able to track habits");
string connectionAdress = @"Data Source=habits.db";


using (var connection = new SqliteConnection(connectionAdress))
{
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText =
        @"CREATE TABLE IF NOT EXISTS drink_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            TimesDone INTEGER
            )";

    command.ExecuteNonQuery();

    connection.Close();
}

GetMainInput();

void DeleteAHabit()
{

    Console.WriteLine("Type the name of the habit you want to delete");
    string habit = Console.ReadLine();

    using (var connection = new SqliteConnection("Data Source=habits.db"))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @$"DROP TABLE {habit}";

        command.ExecuteNonQuery();
        Console.WriteLine("Command Executed");
        connection.Close();
    }

}

void InsertNewHabit()
{
    Console.WriteLine("Type the name of the habit you want to create");
    string habit = Console.ReadLine();

    using (var connection = new SqliteConnection(connectionAdress))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @$"CREATE TABLE IF NOT EXISTS {habit} (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            TimesDone INTEGER
            )";

        command.ExecuteNonQuery();

        connection.Close();
    }
}

void ListHabits()
{
    using (var connection = new SqliteConnection(connectionAdress))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @"
            SELECT name FROM sqlite_master WHERE type='table' AND name != 'sqlite_sequence'";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine("----------------");

                Console.WriteLine(
                    @$"Name: {reader.GetString(0)}");
            }
        }
        connection.Close();
    }
}

void ViewHabitEntries()
{

    Console.WriteLine("Type the name of the habit you want to check");
    string habit = Console.ReadLine();


    using (var connection = new SqliteConnection(connectionAdress))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT * FROM {habit}";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var date = reader.GetString(1);
                var timesDone = reader.GetString(2);
                Console.WriteLine("----------------");

                Console.WriteLine(
                    @$"Id: {reader.GetString(0)}
Times done: {timesDone}
Date done: {date}");

            }
        }
        connection.Close();
    }
}


void InsertHabitEntry()
{

    Console.WriteLine("What habit you want to edit?");
    string habit = Console.ReadLine();

    Console.WriteLine("Insert the number of times");
    string numberOfTimes = Console.ReadLine();

    string date = GetDateInput();


    using (var connection = new SqliteConnection(connectionAdress))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            $"INSERT INTO {habit} (TimesDone, Date) VALUES ({numberOfTimes}, '{date}')";

        command.ExecuteNonQuery();
        Console.WriteLine("Command Executed");
        connection.Close();
    }
}

void UpdateHabitEntry()
{

    Console.WriteLine("What habit you want to edit?");
    string habit = Console.ReadLine();

    Console.WriteLine("Whats the id of the entry you want to edit?");
    string id = Console.ReadLine();

    Console.WriteLine("Insert the number of times");
    string numberOfTimes = Console.ReadLine();

    Console.WriteLine("Do you want to edit the date? y/n");
    string sql = GenerateUpdateSql(Console.ReadLine(), habit, id, numberOfTimes);

    using (var connection = new SqliteConnection("Data Source=habits.db"))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            sql;

        command.ExecuteNonQuery();
        Console.Clear();
        Console.WriteLine("Command Executed");
        connection.Close();
    }

}

string GenerateUpdateSql( string response, string habit, string id, string numberOfTimes )
{
    if (response == "y")
    {

        string date = GetDateInput();

        return @$"
            UPDATE {habit}
            SET timesDone = {numberOfTimes},
            Date = '{date}'
            WHERE id = {id}";

    }
    else
    {
        return @$"
            UPDATE {habit}
            SET timesDone = {numberOfTimes}
            WHERE id = {id}";
    }
}

void DeleteHabitEntry()
{

    Console.WriteLine("What habit you want to edit?");
    string habit = Console.ReadLine();

    Console.WriteLine("Whats the id of the entry you want to delete?");
    string id = Console.ReadLine();

    using (var connection = new SqliteConnection(connectionAdress))
    {
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
            @$"
            DELETE from {habit} WHERE id = {id}
        ";
        command.ExecuteNonQuery();
        Console.Clear();
        Console.WriteLine("Command Executed");
        connection.Close();
    }

}

string GetDateInput()
{
    Console.WriteLine("\nPlease insert the date in the format dd-mm-yyyy. Type 0 to return to the main menu");

    string dateInput = Console.ReadLine();

    if (dateInput == "0")
    {
        GetMainInput();
    }

    while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\nInvalid date. Type 0 to return to the main menu or try again:\n");
        dateInput = Console.ReadLine();
    }

    return dateInput;
}

void GetMainInput()
{

    Console.Clear();
    bool isRunning = true;
    while (isRunning)
    {

        Console.WriteLine($@"What would you like to do:
    1 - Insert a new habit
    2 - Insert a new ocurrence of a habit
    3 - Update an existing habit
    4 - Delete an entry in a habit
    5 - Delete a habit
    6 - View entries in a habit
    7 - View all habits
    0 - Exit the program");

        string op = Console.ReadLine();

        switch (op)
        {
            case "1":
                try
                {
                    InsertNewHabit();
                }
                catch (Exception)
                {
                    DealWithError();
                }
                break;
            case "2":
                try
                {
                    InsertHabitEntry();
                }
                catch (Exception)
                {
                    DealWithError();
                }
                break;
            case "3":
                try
                {
                    UpdateHabitEntry();
                }
                catch (Exception e)
                {
                    DealWithError();
                }
                break;
            case "4":
                try
                {
                    DeleteHabitEntry();
                }
                catch (Exception)
                {
                    DealWithError();
                }
                break;
            case "5":
                try
                {
                    DeleteAHabit();
                }
                catch (Exception)
                {
                    DealWithError();
                }
                break;
            case "6":
                try
                {
                    ViewHabitEntries();
                }
                catch (Exception)
                {
                    DealWithError();
                }
                break;
            case "7":
                try
                {
                    ListHabits();
                }
                catch (Exception)
                {

                    DealWithError();
                }
                break;
            case "0":
                Console.WriteLine("\nGoodbye");
                isRunning = false;
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }

        Console.WriteLine("-------------------------------------------\n");
    }
}

void DealWithError()
{
    Console.WriteLine("Something Went wrong! Check what you typed, you might have typed the name of the habit incorrectly!");
}