using System.Globalization;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace habit_tracker;

class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Type TEXT NOT NULL UNIQUE,
                        Unit TEXT NOT NULL
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        Habits_Id INTEGER NOT NULL,
                        FOREIGN KEY (Habits_Id) REFERENCES Habits(Id)
                        )";

            tableCmd.ExecuteNonQuery();

            tableCmd.CommandText = $"SELECT COUNT(*) FROM Records";
            int count = Convert.ToInt32(tableCmd.ExecuteScalar());
            
            if(count < 10)
            {
                GenerateRandomData();
            }
            connection.Close();
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to Create a new habit to track.");
            Console.WriteLine("Type 6 to View records for specefic Habit.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllHabits();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                case "5":
                    CreateNewTable();
                    break;
                case "6":
                    ViewHabitRecords();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }

    }

    private static void GenerateRandomData()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            Random random = new();

            for (int i = 0; i < 100; i++)
            {
                tableCmd.CommandText =
$"INSERT INTO Records(Date, Quantity, Habits_Id) VALUES('{DateTime.TryParseExact($"{random.Next(1,32)}-{random.Next(1,13)-random.Next(1924, 2025)}", "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _)}', {random.Next(1, 100)}, {random.Next(1,4)})";

                tableCmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    private static void ViewHabitRecords()
    {
        Console.Clear();
        Console.WriteLine("Current Habits");
        GetAllHabits();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            int habitId = GetNumberInput("Enter Id to show habit's records or enter 0 to go back to main menu");
            tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE Id = '{habitId}'";
            int count = Convert.ToInt32(tableCmd.ExecuteScalar());

            while (count == 0)
            {
                Console.WriteLine("The Id/Habit does not exist in the current table");

                habitId = GetNumberInput("Please enter the ID of the habit that you want to insert");

                tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE Id = '{habitId}'";
                count = Convert.ToInt32(tableCmd.ExecuteScalar());

            }

            tableCmd.CommandText =
                $"SELECT * FROM Records WHERE Habits_Id = {habitId}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id - {reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)}");
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
        }
    }

    private static void CreateNewTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            // Display Habits Table
            GetAllHabits();

            connection.Open();
            var tableCmd = connection.CreateCommand();

            string habit = GetHabitInput("Please enter the habit you want to create or type 0 to go back to the Main Menu\n\n");

            tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE LOWER(Type) = '{habit.ToLower()}'";
            int count = Convert.ToInt32(tableCmd.ExecuteScalar());

            while (count > 0)
            {
                habit = GetHabitInput("The habit already exists. Enter a new one\n\n");
                tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE LOWER(Type) = '{habit.ToLower()}'";
                count = Convert.ToInt32(tableCmd.ExecuteScalar());
            }

            string unit = GetUnitInput($"Please type the unit you want to use for {habit} or type 0 to go back to the Main Menu\n\n");

            tableCmd.CommandText = $"INSERT INTO Habits(Type, Unit) VALUES('{habit}', '{unit}')";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    private static string GetHabitInput(string message)
    {
        Console.WriteLine(message);

        string userInput = Console.ReadLine();

        if (userInput == "0") GetUserInput();

        while (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine("Please type a valid habit name");
            userInput = Console.ReadLine();
        }

        return userInput;

    }

    private static string GetUnitInput(string message)
    {
        Console.WriteLine(message);

        string userInput = Console.ReadLine();

        if (userInput == "0") GetUserInput();

        while (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine("Please type a valid unit name");
            userInput = Console.ReadLine();
        }

        return userInput;
    }

    private static void GetAllHabits()
    {
        Console.Clear();
        Console.WriteLine("Current Table");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM Habits";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id - {reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)}");
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
        }
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        Console.WriteLine("Current Records");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM Records";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id - {reader.GetInt32(0)} | {reader.GetString(1)} | {reader.GetString(2)}");
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            connection.Close();
        }
    }

    private static void Insert()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {

            GetAllHabits();

            connection.Open();
            var tableCmd = connection.CreateCommand();

            int habitId = GetNumberInput("Please enter the ID of the habit that you want to insert or enter 0 to go back to main menu");

            tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE Id = '{habitId}'";
            int count = Convert.ToInt32(tableCmd.ExecuteScalar());
            
            while (count == 0)
            {
                Console.WriteLine("The Id/Habit does not exist in the current table");

                habitId = GetNumberInput("Please enter the ID of the habit that you want to insert");

                tableCmd.CommandText = $"SELECT COUNT(*) FROM Habits WHERE Id = '{habitId}'";
                count = Convert.ToInt32(tableCmd.ExecuteScalar());

            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of choice (no decimals allowed)\n\n");

            tableCmd.CommandText =
$"INSERT INTO Records(Date, Quantity, Habits_Id) VALUES('{date}', {quantity}, {habitId})";

            tableCmd.ExecuteNonQuery();


            connection.Close();
        }
    }

    private static void Delete()
    {
        //TO CHANGE
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE from Records WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Delete();
            }

        }

        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

        GetUserInput();
    }

    internal static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Records WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                Update();
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number quantity(no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE Records SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }


    }

    internal static string GetDateInput()
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

    internal static int GetNumberInput(string message)
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
}

