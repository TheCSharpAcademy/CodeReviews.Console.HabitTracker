using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

public static class Utils
{
    private static readonly Random Gen = new();
    private const int NumOfRecordsToSeed = 100;
    
    public static DateTime GetDateInput()
    {
        bool validDateEntered = false;
        DateTime habitDate = default; 
            
        while (!validDateEntered)
        {
            Console.Write("Type the date when the habit was done in dd/mm/yyyy format or type \"today\" for today's date: ");
            string? date = Console.ReadLine();

            if (date is null || !Regex.IsMatch(date, @"^((0?[1-9]|[12][0-9]|3[01])/(0?[1-9]|1[0-2])/\d{1,4}|today)$"))
            {
                Console.WriteLine("Error: date entered is not valid");
            }
            else
            {
                if (date.Equals("today"))
                {
                    habitDate = DateTime.Today;
                }
                else
                {
                    string[] dateElements = date.Split("/");
                    int day = int.Parse(dateElements[0]);
                    int month = int.Parse(dateElements[1]);
                    int year = int.Parse(dateElements[2]);
                    try
                    {
                        habitDate = new DateTime(year, month, day);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("Error: Number of days entered greater than number of days in the given month");
                    }
                }
                validDateEntered = true;
            }
        }

        return habitDate;
    }

    public static string GetAlphabeticalInput(string type)
    {
        bool validStringEntered = false;
        string? input = "";
        
        while (!validStringEntered)
        {
            Console.Write($"Type the {type} and press Enter: ");
            input = Console.ReadLine();

            if (input is null || !Regex.IsMatch(input, "^[a-zA-Z ]+$"))
            {
                Console.WriteLine($"Name of {type} should contain only alphabetical letters");
            }
            else
            {
                validStringEntered = true;
            }
        }

        return input?.ToLower() ?? string.Empty;
    }

    public static int GetQuantityInput()
    {
        bool validQuantityEntered = false;
        int habitQuantity = 0;
        
        while (!validQuantityEntered)
        {
            Console.Write("Type the quantity of the habit done: ");
            string? quantity = Console.ReadLine();

            if (quantity is null || int.TryParse(quantity, out habitQuantity) == false)
            {
                Console.WriteLine("Quantity of the habit done must be an integer");
            }
            else
            {
                validQuantityEntered = true;
            }
        }

        return habitQuantity;
    }

    public static int GetIdOfRecord(HashSet<int> recordIndexes, string action)
    {
        string? input;
        bool validIndexEntered = false;
        int recordIndex = 0;

        while (!validIndexEntered)
        {
            Console.Write($"\nEnter the id of the record that you want to {action}: ");
            input = Console.ReadLine();

            if (input == null || int.TryParse(input, out recordIndex) == false)
            {
                Console.WriteLine("Error: Invalid Input");
            }
            else if (!recordIndexes.Contains(recordIndex))
            {
                Console.WriteLine("Error: Id entered is not in the list shown.");
            }
            else
            {
                validIndexEntered = true;
            }
        }

        return recordIndex;
    }
    
    public static string GetCorrectPathToStoreDatabase(string dbName)
    {
        string curPath = Directory.GetCurrentDirectory();
        var directoryInfo = Directory.GetParent(curPath);

        for (int i = 0; i < 2; i++)
        {
            if (directoryInfo != null)
            {
                directoryInfo = directoryInfo.Parent;
            }
            else
            {
                break;
            }
        }

        string dbPath = directoryInfo?.FullName + $"/{dbName}";

        return dbPath;
    }
    
    public static SqliteConnection SystemStartUpCheck(string dbPath, string tableName)
    {
        Console.WriteLine("Performing application start up checks...");
        var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        Console.WriteLine("Successfully connected to SQLite database!");

        if (!CheckTableExists(connection, tableName))
        {
            CreateTable(connection, tableName);
            SeedTable(connection);
        }
        else
        {
            Console.WriteLine($"{tableName} table found!");
        }

        return connection;
    }

    private static bool CheckTableExists(SqliteConnection connection, string tableName)
    {
    
        using var command = new SqliteCommand(Queries.CheckTableExistsQuery, connection);
        command.Parameters.AddWithValue("@tableName", tableName);
        using var reader = command.ExecuteReader();

        return reader.HasRows;
    }

    private static void CreateTable(SqliteConnection connection, string tableName)
    {
        using var command = new SqliteCommand(Queries.CreateTableCommand, connection);
        command.Parameters.AddWithValue("@tableName", tableName);

        command.ExecuteNonQuery();
        Console.WriteLine($"{tableName} table successfully created.");
    }

    private static void SeedTable(SqliteConnection connection)
    {
        string[] habits = ["push ups", "run", "sleep"];
        string[] units = ["times", "km", "hours"];
        int[][] ranges =
        {
            [20, 70],
            [2, 10],
            [6, 9]
        };

        for (int i = 0; i < NumOfRecordsToSeed; i++)
        {
            using var command = new SqliteCommand(Queries.InsertRecordCommand, connection);
            
            int randomIndex = Gen.Next(0, 3);
            string randomHabit = habits[randomIndex];
            string randomUnit = units[randomIndex];
            int[] randomRange = ranges[randomIndex];
            var randomDate = RandomDate();
            int randomQuantity = Gen.Next(randomRange[0], randomRange[1]);

            command.Parameters.AddWithValue("@date", randomDate);
            command.Parameters.AddWithValue("@habit", randomHabit);
            command.Parameters.AddWithValue("@unit", randomUnit);
            command.Parameters.AddWithValue("@quantity", randomQuantity);

            command.ExecuteNonQuery();
        }
    }

    private static DateTime RandomDate()
    {
        var start = new DateTime(2024, 5, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(Gen.Next(range));
    }
}