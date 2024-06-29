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

    public static void InsertMenu()
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
        else
        {
            Console.WriteLine($"You will be logging {habitName}. Press any key to confirm.");
            Console.ReadKey(true);


            Console.WriteLine("What will be your quantity? (ex. Amount Drank)");
            string habitQuantity = Console.ReadLine();

            if (string.IsNullOrEmpty(habitQuantity))
            {
                Console.WriteLine("Invalid input. Please enter a valid quantity.");
                return;
            }

            Console.WriteLine($"You will be logging {habitName} with quantity {habitQuantity}. Press any key to confirm.");
            Console.ReadKey(true);
            Console.WriteLine($"Creating {habitName} habit...");
            InsertHabit(habitName, habitQuantity);
        }



    }
    public static void DeleteMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("What habit would you want to delete?");
        Console.WriteLine("List of currently existing tables:");
        Console.WriteLine("-----------------------------------------");
        TableList();
        Console.WriteLine("What would you like to delete? All tables are case-sensitive!\nEnter the name below:");
        string habitName = Console.ReadLine();

        if (TableExists(habitName))
        {
            DeleteHabit(habitName);
        } 
        else
        {
            Console.WriteLine("Invalid name, press any key to try again.");
            Console.ReadKey();
            DeleteMenu();
        }

        Console.WriteLine("Press any key to return to the main menu!");
        Console.ReadKey();
        Menu();

    }
    public static void ViewMenu()
    {

        Console.Clear();
        Clock();
        Console.WriteLine("List of currently existing tables:");
        Console.WriteLine("----------------------------------------");
        TableList();
        Console.WriteLine("\nIf you would like to change this list, please press any key to return to the main menu!");
    }
    private static void UpdateMenu()
    {
        Console.Clear();
        Clock();
        Console.WriteLine("What habit would you want to update");
        Console.WriteLine("List of currently existing tables:");
        Console.WriteLine("-----------------------------------------");
        TableList();
        Console.WriteLine("What habit would you like to Update? All tables are case-sensitive!\nEnter the name below:");
        string habitName = Console.ReadLine();
        if (TableExists(habitName))
        {
            UpdateHabit(habitName);
        } 
        else
        {
            Console.WriteLine("This does not exist. Press a key to return.");
            Console.ReadKey();
        }

        Console.WriteLine("Press any key to return to the main menu!");
        Console.ReadKey();
        Menu();
    }


    private static bool TableExists(string tableName)
    {
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
        tableCmd.Parameters.AddWithValue("@tableName", tableName);

        using (var reader = tableCmd.ExecuteReader())
        {
            return reader.Read();
        }
    }
    private static void TableList()
    {

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name != 'sqlite_sequence';";

        using (var reader = tableCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string tableName = reader.GetString(0);
                Console.Write($"-{tableName}\t");
                

                var quantityCmd = connection.CreateCommand();
                quantityCmd.CommandText = $"SELECT Quantity FROM \"{tableName}\";";

                try
                {
                    using (var quantityReader = quantityCmd.ExecuteReader())
                    {
                        while (quantityReader.Read())
                        {
                            int quantity = quantityReader.GetInt32(0);

                            Console.Write($"\t\tQuantity: {quantity}\n");
                            Console.WriteLine("----------------------------------------");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading Quantity from {tableName}: {ex.Message}");
                }
            }
        }
    }

    public static void InsertHabit(string habitName, string habitQuantity)
        {
            var tableCmd = connection.CreateCommand();

            string createTableQuery = $@"
                CREATE TABLE IF NOT EXISTS ""{habitName}"" (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Quantity INTEGER NOT NULL
                );";

            tableCmd.CommandText = createTableQuery;
            tableCmd.ExecuteNonQuery();

            string insertQuantityQuery = $@"
                INSERT INTO ""{habitName}"" (Quantity)
                VALUES (@habitQuantity);";

        tableCmd.CommandText = insertQuantityQuery;
        tableCmd.Parameters.AddWithValue("@habitQuantity", habitQuantity);
        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit table '{habitName}' created with initial quantity {habitQuantity}.");
    }
    public static void InsertQuantity(string habitName, string habitQuantity)
    {
        var tableCmd = connection.CreateCommand();

        string createTableQuery = $@"
                INSERT INTO {habitName} (Quantity)
                VALUES (@{habitQuantity});";

        tableCmd.CommandText = createTableQuery;
        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit quantity ({habitQuantity})");
    }


    public static void DeleteHabit(string habitName)
        {
            if (string.IsNullOrEmpty(habitName)) { 
            Console.WriteLine("Invalid input. Please enter a valid name.");
            }

            else
            {
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"DROP TABLE IF EXISTS ""{habitName}"";";
                tableCmd.ExecuteNonQuery();

                Console.WriteLine($"Habit '{habitName}' deleted.");
            }
        }
    private static void UpdateHabit(string habitName)
    {
        if (string.IsNullOrEmpty(habitName))
        {
            Console.WriteLine("Invalid input. Please enter a valid name.");
        }
        else
        {
            Console.WriteLine($"What would you like to update? You may type either 'Name' or 'Quantity'");
            string habitChoice = Console.ReadLine();

            if (habitChoice.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                UpdateName(habitName);
            }
            else if (habitChoice.Equals("Quantity", StringComparison.OrdinalIgnoreCase))
            {
                UpdateQuantity(habitName);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please type either 'Name' or 'Quantity'.");
                Console.ReadKey();
            }
        }
    }



    private static void UpdateName(string habitName)
    {
        Console.WriteLine("What would you like to rename it to?");
        string newTable = Console.ReadLine();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = $@"ALTER TABLE ""{habitName}"" RENAME TO ""{newTable}"";";
        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit is now called '{newTable}'");
    }

    private static void UpdateQuantity(string habitName)
    {
        Console.WriteLine("Please enter new quantity amount.");
        string newQuantityStr = Console.ReadLine();

        if (!int.TryParse(newQuantityStr, out int newQuantity))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer for quantity.");
            return;
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $@"UPDATE ""{habitName}"" SET Quantity = @newQuantity WHERE Id = (SELECT Id FROM ""{habitName}"" LIMIT 1);";
        tableCmd.Parameters.AddWithValue("@newQuantity", newQuantity);

        tableCmd.ExecuteNonQuery();

        Console.WriteLine($"Habit quantity is now '{newQuantity}'");
    }

    private static void Clock()
    {
        Console.WriteLine($"Date and Time: {DateTime.Now.ToString("MM/dd/yyyy h:mm tt")}");
    }

    }