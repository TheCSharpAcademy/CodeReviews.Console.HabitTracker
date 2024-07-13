using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

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
         HabitDate TEXT NOT NULL,
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
        Console.WriteLine("Let's log a habit! Enter the date (MM-DD-YY).");
        string habitDate = Console.ReadLine();

        if (string.IsNullOrEmpty(habitDate) || !Regex.IsMatch(habitDate, @"^\d{2}-\d{2}-\d{2}$") || !IsValidDate(habitDate))
        {
            Console.WriteLine("Invalid input. Please enter a valid date in MM-DD-YY format.");
            return;
        }

        Console.WriteLine("What is the unit for this habit (positive number)?");
        string unitInput = Console.ReadLine();

        if (string.IsNullOrEmpty(unitInput) || !int.TryParse(unitInput, out int unit) || unit <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive number for the unit.");
            return;
        }

        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Habits (HabitDate, Unit) VALUES (@habitDate, @unit);";
        cmd.Parameters.AddWithValue("@habitDate", habitDate);
        cmd.Parameters.AddWithValue("@unit", unit);
        cmd.ExecuteNonQuery();

        Console.WriteLine($"Habit '{habitDate}' with unit '{unit}' created.");
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
        Console.WriteLine("List of currently existing Logs:");
        Console.WriteLine("----------------------------------------");
        TableList();
        Console.WriteLine("\nIf you would like to change this list, please press any key to return to the main menu!");
    }
    static void UpdateMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("What habit would you want to update");
        Console.WriteLine("List of currently existing habits:");
        Console.WriteLine("-----------------------------------------");
        TableList();
        Console.WriteLine("\nWhat entry would you like to Update? Enter the ID number.");

        string habitIdInput = Console.ReadLine();
        if (!int.TryParse(habitIdInput, out int habitId))
        {
            Console.WriteLine("Invalid input. Please enter a valid ID.");
            Console.ReadKey();
            UpdateMenu();
            return;
        }

        if (HabitExists(habitId))
        {
            UpdateHabit(habitId);
        }
        else
        {
            Console.WriteLine("This habit does not exist. Press a key to return.");
            Console.ReadKey();
        }

        Console.WriteLine("Press any key to return to the main menu!");
        Console.ReadKey();
        Menu();
    }

    static bool IsValidDate(string date)
    {
        try
        {
            DateTime.ParseExact(date, "MM-dd-yy", null);
            return true;
        }
        catch
        {
            return false;
        }
    }

    static bool HabitExists(int habitId)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT 1 FROM Habits WHERE Id = @habitId LIMIT 1;";
        cmd.Parameters.AddWithValue("@habitId", habitId);

        using (var reader = cmd.ExecuteReader())
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
                Console.WriteLine($"ID: {reader["Id"]} || Habit Date: {reader["habitDate"]} || Quantity: {reader["Unit"]}");
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


    static void InsertQuantity(string habitDate, string habitQuantity)
    {
        var tableCmd = connection.CreateCommand();

        string createTableQuery = $@"
                INSERT INTO {habitDate} (Quantity)
                VALUES (@{habitQuantity});";

        tableCmd.CommandText = createTableQuery;
        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit quantity ({habitQuantity})");
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

    static void UpdateHabit(int habitId)
    {
        Console.WriteLine($"What would you like to update? You may type either 'HabitDate' or 'Unit'");
        string updateChoice = Console.ReadLine();

        if (updateChoice.Equals("HabitDate", StringComparison.OrdinalIgnoreCase))
        {
            UpdateHabitDate(habitId);
        }
        else if (updateChoice.Equals("Unit", StringComparison.OrdinalIgnoreCase))
        {
            UpdateUnit(habitId);
        }
        else
        {
            Console.WriteLine("Invalid choice. Please type either 'HabitDate' or 'Unit'.");
            Console.ReadKey();
        }
    }



    static void UpdateHabitDate(int habitId)
    {
        Console.WriteLine("Please enter the new date (MM-DD-YY):");
        string newHabitDate = Console.ReadLine();

        if (string.IsNullOrEmpty(newHabitDate) || !Regex.IsMatch(newHabitDate, @"^\d{2}-\d{2}-\d{2}$") || !IsValidDate(newHabitDate))
        {
            Console.WriteLine("Invalid input. Please enter a valid date in MM-DD-YY format.");
            return;
        }

        var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE Habits SET HabitDate = @newHabitDate WHERE Id = @habitId;";
        cmd.Parameters.AddWithValue("@newHabitDate", newHabitDate);
        cmd.Parameters.AddWithValue("@habitId", habitId);
        cmd.ExecuteNonQuery();

        Console.WriteLine($"Habit date updated to '{newHabitDate}'.");
    }

    static void UpdateUnit(int habitId)
    {
        Console.WriteLine("Please enter the new unit (positive number):");
        string newUnitInput = Console.ReadLine();

        if (string.IsNullOrEmpty(newUnitInput) || !int.TryParse(newUnitInput, out int newUnit) || newUnit <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive number for the unit.");
            return;
        }

        var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE Habits SET Unit = @newUnit WHERE Id = @habitId;";
        cmd.Parameters.AddWithValue("@newUnit", newUnit);
        cmd.Parameters.AddWithValue("@habitId", habitId);
        cmd.ExecuteNonQuery();

        Console.WriteLine($"Habit unit updated to '{newUnit}'.");
    }

    static void Clock()
    {
        Console.WriteLine($"Date and Time: {DateTime.Now.ToString("MM/dd/yyyy h:mm tt")}");
    }

}