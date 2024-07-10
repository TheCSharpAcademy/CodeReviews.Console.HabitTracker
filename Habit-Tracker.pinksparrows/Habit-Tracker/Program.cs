using System;
using System.Data.SQLite;

public class Program
{
    static SQLiteConnection connection;

    static void Main(string[] args)
    {
        InitializeDatabase();
        Menu();
    }
    static void InitializeDatabase()
    {
        string connectionString = "Data Source=HabitTracker.db;Version=3;";

        connection = new SQLiteConnection(connectionString);
        connection.Open();

        string createHabitsTable = @"
        CREATE TABLE IF NOT EXISTS Habits (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            HabitName TEXT NOT NULL,
            Unit TEXT NOT NULL
        );";
        var cmd = new SQLiteCommand(createHabitsTable, connection);
        cmd.ExecuteNonQuery();

        Console.WriteLine("Database initialized.");
    }

    static void Menu()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.WriteLine("Welcome to your Habit Logger!\nPlease choose from one of the options below with the corresponding number.");
        Clock();
        Console.WriteLine("------------------------------------\n");
        Console.WriteLine("Insert -> 1\n");
        Console.WriteLine("Delete -> 2\n");
        Console.WriteLine("Update -> 3\n");
        Console.WriteLine("View -> 4\n");
        Console.WriteLine("Exit -> 0\n");
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Please provide your input below:");
        GetInput();
    }

    static void GetInput()
    {
        string inputString = Console.ReadLine();
        int input;

        if (!int.TryParse(inputString, out input))
        {
            Console.WriteLine("Invalid input. Press any key to try again.");
            Console.ReadKey();
            Menu();
            return;
        }

        switch (input)
        {
            case 1:
                InsertMenu();
                Console.ReadKey();
                Console.Clear();
                Menu();
                break;
            case 2:
                DeleteMenu();
                Console.ReadKey();
                Console.Clear();
                Menu();
                break;
            case 3:
                UpdateMenu();
                Console.ReadKey();
                Console.Clear();
                Menu();
                break;
            case 4:
                ViewMenu();
                Console.ReadKey();
                Console.Clear();
                Menu();
                break;
            case 0:
                Console.WriteLine("You are exiting.");
                Environment.Exit(0);
                break;
            default:
                if (input > 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Clear();
                    Console.WriteLine("Input must be between 0 and 4.");
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                    Menu();
                }
                break;
        }
    }

    static void InsertMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("What habit would you want to create?");
        string habitName = Console.ReadLine();

        if (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Invalid input. Please enter a valid name.");
            return;
        }

        Console.WriteLine("What is the unit for this habit (e.g., minutes, times, liters)?");
        string unit = Console.ReadLine();

        if (string.IsNullOrEmpty(unit))
        {
            Console.WriteLine("Invalid input. Please enter a valid unit.");
            return;
        }

        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Habits (HabitName, Unit) VALUES (@habitName, @unit);";
        cmd.Parameters.AddWithValue("@habitName", habitName);
        cmd.Parameters.AddWithValue("@unit", unit);
        cmd.ExecuteNonQuery();

        Console.WriteLine($"Habit '{habitName}' with unit '{unit}' created.");
    }

    static void UpdateMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("List of currently existing habits:");
        Console.WriteLine("-----------------------------------------");
        TableList();
        Console.WriteLine("Enter the ID of the habit you want to update:");

        string habitIdInput = Console.ReadLine();
        if (!int.TryParse(habitIdInput, out int habitId))
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
            Console.ReadKey();
            UpdateMenu();
            return;
        }

        Console.WriteLine("What would you like to update? Enter 'Name' or 'Unit':");
        string updateField = Console.ReadLine();

        if (updateField.Equals("Name", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the new name:");
            string newName = Console.ReadLine();
            UpdateHabitName(habitId, newName);
        }
        else if (updateField.Equals("Unit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Enter the new unit:");
            string newUnit = Console.ReadLine();
            UpdateHabitUnit(habitId, newUnit);
        }
        else
        {
            Console.WriteLine("Invalid choice. Please type either 'Name' or 'Unit'.");
            Console.ReadKey();
            UpdateMenu();
        }

        Console.WriteLine("Press any key to return to the main menu!");
        Console.ReadKey();
        Menu();
    }

    static void DeleteMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("List of currently existing habits:");
        Console.WriteLine("-----------------------------------------");
        TableList();
        Console.WriteLine("Enter the habit ID to delete:");

        string habitIdInput = Console.ReadLine();
        if (!int.TryParse(habitIdInput, out int habitId))
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
            Console.ReadKey();
            DeleteMenu();
            return;
        }

        DeleteHabit(habitId);

        Console.WriteLine("Press any key to return to the main menu!");
        Console.ReadKey();
        Menu();
    }

    static void ViewMenu()
    {

        Console.Clear();
        Clock();
        Console.WriteLine("List of currently existing habits:");
        Console.WriteLine("----------------------------------------");
        TableList();
        Console.WriteLine("\nIf you would like to change this list, please press any key to return to the main menu!");
    }


    static bool TableExists(string tableName)
    {
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;";
        tableCmd.Parameters.AddWithValue("@tableName", tableName);

        using (var reader = tableCmd.ExecuteReader())
        {
            return reader.Read();
        }
    }

    static void TableList()
    {
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM Habits;";

        using (var reader = tableCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Console.WriteLine($"\nID: {reader["Id"]}\tName: {reader["HabitName"]}\tUnit: {reader["Unit"]}\n");
            }
        }
    }

    static void ReorderHabitIds(int deletedId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
        UPDATE Habits
        SET Id = Id - 1
        WHERE Id > @deletedId;
    ";
        cmd.Parameters.AddWithValue("@deletedId", deletedId);
        cmd.ExecuteNonQuery();

        cmd.CommandText = "UPDATE sqlite_sequence SET seq = (SELECT MAX(Id) FROM Habits) WHERE name = 'Habits';";
        cmd.ExecuteNonQuery();
    }


    static void InsertQuantity(string habitName, int habitQuantity)
    {
        var tableCmd = connection.CreateCommand();

        string createTableQuery = $@"
            INSERT INTO {habitName} (Quantity)
            VALUES (@habitQuantity);";

        tableCmd.CommandText = createTableQuery;
        tableCmd.Parameters.AddWithValue("@habitQuantity", habitQuantity);
        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit quantity ({habitQuantity}) inserted.");
    }



    static void DeleteHabit(int habitId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Habits WHERE Id = @habitId;";
        cmd.Parameters.AddWithValue("@habitId", habitId);
        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected > 0)
        {
            Console.WriteLine($"Habit with ID '{habitId}' deleted.");
            ReorderHabitIds(habitId);
        }
        else
        {
            Console.WriteLine($"Habit with ID '{habitId}' not found.");
        }
    }



static void UpdateHabitName(int habitId, string newName)
{
    var cmd = connection.CreateCommand();
    cmd.CommandText = "UPDATE Habits SET HabitName = @newName WHERE Id = @habitId;";
    cmd.Parameters.AddWithValue("@newName", newName);
    cmd.Parameters.AddWithValue("@habitId", habitId);
    int rowsAffected = cmd.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine($"Habit ID '{habitId}' name updated to '{newName}'.");
    }
    else
    {
        Console.WriteLine($"Habit ID '{habitId}' not found.");
    }
}

static void UpdateHabitUnit(int habitId, string newUnit)
{
    var cmd = connection.CreateCommand();
    cmd.CommandText = "UPDATE Habits SET Unit = @newUnit WHERE Id = @habitId;";
    cmd.Parameters.AddWithValue("@newUnit", newUnit);
    cmd.Parameters.AddWithValue("@habitId", habitId);
    int rowsAffected = cmd.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        Console.WriteLine($"Habit ID '{habitId}' unit updated to '{newUnit}'.");
    }
    else
    {
        Console.WriteLine($"Habit ID '{habitId}' not found.");
    }
}

static void Clock()
    {
        Console.WriteLine($"Date and Time: {DateTime.Now.ToString("MM/dd/yyyy h:mm tt")}");
    }

}