using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker;

internal class Program
{
    public static string connectionString = @"Data Source=HabitTracker.db";

    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            CreateTables(connection);
            SeedDatabaseIfEmpty(connection);
            GetUserInput();
            connection.Close();
        }
    }

    private static void CreateTables(SqliteConnection connection)
    {
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS HabitTracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    HabitName TEXT,
                    UnitOfMeasurement TEXT
                    );
            CREATE TABLE IF NOT EXISTS Habit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitTrackerId INTEGER,
                    Date TEXT,
                    Description TEXT,
                    Quantity INTEGER,
                    FOREIGN KEY (HabitTrackerId) REFERENCES HabitTracker(Id)
                    );";
        tableCmd.ExecuteNonQuery();
    }

    private static void SeedDatabaseIfEmpty(SqliteConnection connection)
    {
        SeedHabitTracker(connection);
        SeedHabit(connection);
    }

    private static void SeedHabitTracker(SqliteConnection connection)
    {
        if (CountRecords(connection, "HabitTracker") == 0)
        {
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
                @"INSERT INTO HabitTracker (Date, HabitName, UnitOfMeasurement) VALUES 
                ('01-01-23', 'Exercise', 'minutes'),
                ('02-01-23', 'Read', 'minutes'),
                ('03-01-23', 'Meditate', 'sessions'),
                ('03-01-23', 'Drink water', 'cups');";
            insertCmd.ExecuteNonQuery();
            Console.WriteLine("HabitTracker database seeded with initial records.");
        }
    }

    private static void SeedHabit(SqliteConnection connection)
    {
        if (CountRecords(connection, "Habit") == 0)
        {
            var insertHabitCmd = connection.CreateCommand();
            insertHabitCmd.CommandText =
                @"INSERT INTO Habit (HabitTrackerId, Date, Description, Quantity) VALUES 
                (1, '11-04-23', 'Morning workout', 30),
                (1, '11-05-23', 'Evening jog', 45),
                (2, '01-01-23', 'Read 30 pages', 15),
                (4, '01-01-23', 'Coke', 2),
                (4, '01-01-23', 'Cold Beer', 15),
                (4, '01-01-23', 'Water', 15),
                (3, '01-01-23', 'Meditate on the couch', 10);";
            insertHabitCmd.ExecuteNonQuery();
            Console.WriteLine("Habit database seeded with initial records.");
        }
    }

    private static long CountRecords(SqliteConnection connection, string tableName)
    {
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT COUNT(*) FROM {tableName}";
        return (Int64)checkCmd.ExecuteScalar();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {
            DisplayMainMenu();
            string command = Console.ReadLine();
            closeApp = ExecuteCommand(command);
        }
    }

    private static void DisplayMainMenu()
    {
        Console.WriteLine("MAIN MENU");
        Console.WriteLine("\n What would you like to do?");
        Console.WriteLine("\nType 0 to close app.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("Type 5 to Add record to a Habit");
        Console.WriteLine("Type 6 to View all records for a Habit");
        Console.WriteLine("Type 7 to get Progress Report");
        Console.WriteLine("------------------------------------------\n");
    }

    private static bool ExecuteCommand(string command)
    {
        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                return true;
            case "1":
                GetAllRecords();
                break;
            case "2":
                Insert();
                break;
            case "3":
                DeleteAction();
                break;
            case "4":
                Update();
                break;
            case "5":
                AddRecordToHabit();
                break;
            case "6":
                PrintAllHabitsRecord();
                break;
            case "7":
                GetReport();
                break;
            default:
                Console.WriteLine("\n Invalid Command. Please type a number from 0 to 7.\n");
                break;
        }
        return false;
    }

    private static void GetReport()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            "SELECT HabitName, SUM(Quantity), COUNT(Habit.Id) AS HabitCount, UnitOfMeasurement " +
            "FROM HabitTracker JOIN Habit ON HabitTracker.Id = Habit.HabitTrackerId " +
            "GROUP BY HabitName";
            SqliteDataReader reader = tableCmd.ExecuteReader();

            Console.Clear();
            Console.WriteLine("Report: \n");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Habit: {reader.GetString(0)} Total Quantity: {reader.GetInt32(1)} {reader.GetString(3)} Count of Records: {reader.GetInt32(2)}");
                }
            }
            Console.WriteLine("\n\nPress any key to return to main menu.");
            Console.ReadKey();
            GetUserInput();
        }
    }

    private static void PrintAllHabitsRecord()
    {
        Console.Clear();
        Console.WriteLine("\n\n Which habit to view? Provide ID\n\n");
        int id = GetIdInput();
        var habitList = QueryAllHabitRecords(id);
        Console.WriteLine($"Total records for Habit ID {id}: {habitList.Count}");
        PrintAllRecords(habitList);
    }

    private static List<Habit> QueryAllHabitRecords(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            "SELECT * FROM Habit WHERE HabitTrackerId = @id";
            tableCmd.Parameters.AddWithValue("@id", id);

            List<Habit> listWithHabits = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listWithHabits.Add(new Habit
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(2),
                        Description = reader.GetString(3),
                        Quantity = reader.GetInt32(4)
                    });
                }
            }
            return listWithHabits;
        }
    }

    private static void AddRecordToHabit()
    {
        Console.Clear();
        Console.WriteLine("\n\n Which habit to update? Provide ID\n\n");
        int id = GetIdInput();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            "INSERT INTO Habit (HabitTrackerId, Date, Description, Quantity) VALUES (@HabitTrackerId, @Date, @Description, @Quantity)";
            tableCmd.Parameters.AddWithValue("@HabitTrackerId", id);
            tableCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString());
            tableCmd.Parameters.AddWithValue("@Description", GetHabitInput());
            tableCmd.Parameters.AddWithValue("@Quantity", GetQuantityInput());

            tableCmd.ExecuteNonQuery();
            Console.WriteLine("\n\n Record inserted successfully!");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static int GetQuantityInput()
    {
        string input = Console.ReadLine();
        if (Int32.TryParse(input, out int quantity))
        {
            return quantity;
        }
        else
        {
            Console.WriteLine("\n\nInvalid input. Please provide a valid quantity.");
            return GetQuantityInput();
        }
    }

    private static void GetAllRecords()
    {
        PrintAllRecords(QueryAllRecords());
    }

    private static List<HabitTracker> QueryAllRecords()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM HabitTracker";

            List<HabitTracker> listWithHabits = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listWithHabits.Add(new HabitTracker
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(1),
                        HabitName = reader.GetString(2),
                        UnitOfMeasurement = reader.GetString(3)
                    });
                }
            }
            return listWithHabits;
        }
    }

    private static void Insert()
    {
        string date = GetDateInput();
        Console.WriteLine("\n\nPlease insert the unit of measurement. \n\n");
        string unitOfMeasurement = Console.ReadLine();
        string habitName = GetHabitInput();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            "INSERT INTO HabitTracker(date, habitName, unitOfMeasurement) VALUES(@date, @habitName, @unitOfMeasurement)";
            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@habitName", habitName);
            tableCmd.Parameters.AddWithValue("@unitOfMeasurement", unitOfMeasurement);

            tableCmd.ExecuteNonQuery();
            Console.WriteLine("\n\n Record inserted successfully! Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static void DeleteAction()
    {
        Console.Clear();
        foreach (var habit in QueryAllRecords())
        {
            Console.WriteLine($"Id: {habit.Id} Date: {habit.Date} Habit: {habit.HabitName}");
        }
        Console.WriteLine("\n\nProvide the ID number of the record to delete:");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (QueryAllRecords().Any(x => x.Id == id))
            {
                Delete(id);
            }
            else
            {
                Console.WriteLine("\n\nInvalid input. Please provide a valid ID number. Press any key to continue");
                Console.ReadKey();
            }
        }
        GetUserInput();
    }

    private static void Delete(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "DELETE FROM HabitTracker WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@id", id);
            tableCmd.ExecuteNonQuery();
            Console.WriteLine("\n\n Action performed successfully!");
        }
    }

    private static void Update()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            Console.WriteLine("Which habit to update? Provide ID\n\n");
            int id = GetIdInput();
            string habitName = GetHabitInput();
            string date = GetDateInput();
            Console.WriteLine("\n\nPlease insert the unit of measurement. \n\n");
            string unitOfMeasurement = Console.ReadLine();

            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
            "UPDATE HabitTracker SET Date = @date, HabitName = @habitName, UnitOfMeasurement = @unitOfMeasurement WHERE Id = @id";
            tableCmd.Parameters.AddWithValue("@date", date);
            tableCmd.Parameters.AddWithValue("@habitName", habitName);
            tableCmd.Parameters.AddWithValue("@id", id);
            tableCmd.Parameters.AddWithValue("@unitOfMeasurement", unitOfMeasurement);

            tableCmd.ExecuteNonQuery();
            Console.WriteLine("\n\n Record updated successfully!");
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private static string GetHabitInput()
    {
        Console.WriteLine("\n\nPlease insert the habit. Type 0 to return to main menu. \n\n");
        string habitInput = Console.ReadLine();
        if (habitInput == "0") GetUserInput();
        return habitInput;
    }

    private static int GetIdInput()
    {
        List<HabitTracker> listWithHab = QueryAllRecords();
        string id = Console.ReadLine();
        if (Int32.TryParse(id, out int intId) && listWithHab.Any(x => x.Id == intId))
        {
            return intId;
        }
        else
        {
            Console.WriteLine("\n\nInvalid input. Please provide a valid ID number.");
            return GetIdInput();
        }
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu. \n\n");
        string dateInput;
        while (true)
        {
            dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();
            if (DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return dateInput;
            }
            Console.WriteLine("\n\nInvalid date format. Please provide a valid date. Type 0 to return to main menu. \n\n");
        }
    }

    private static void PrintAllRecords(List<HabitTracker> listWithHabits)
    {
        Console.Clear();
        Console.WriteLine("All Records: \n");
        foreach (var habit in listWithHabits)
        {
            Console.WriteLine($"Id: {habit.Id} Date: {habit.Date} Habit: {habit.HabitName}, Unit of measurement: {habit.UnitOfMeasurement}");
        }
        Console.WriteLine("\n\nPress any key to return to main menu.");
        Console.ReadKey();
        GetUserInput();
    }

    private static void PrintAllRecords(List<Habit> listWithHabits)
    {
        Console.Clear();
        Console.WriteLine("All Records: \n");
        foreach (var habit in listWithHabits)
        {
            Console.WriteLine($"Id: {habit.Id} Date: {habit.Date} Habit: {habit.Description} Quantity: {habit.Quantity}");
        }
        Console.WriteLine("\n\nPress any key to return to main menu.");
        Console.ReadKey();
        GetUserInput();
    }
}

