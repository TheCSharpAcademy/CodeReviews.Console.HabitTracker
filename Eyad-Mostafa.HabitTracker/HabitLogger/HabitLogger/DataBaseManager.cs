using System.Data.SQLite;
using System.Globalization;

namespace HabitLogger;

internal class DataBaseManager
{
    static readonly string _connectionString = @"Data Source=habit-tracker.db";

    public static void CreateDataBase()
    {

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();
            DatabaseSeeder.SeedData(connection);
            connection.Close();
        }
    }

    public static void InsertRecord()
    {
        Console.Clear();
        var date = GetDateInput();

        if (date == "0") return;

        var quantity = GetNumberInput("Please Enter Number of water glasses");
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"INSERT INTO drinking_water(Date, Quantity)
                    VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void DeleteRecord()
    {
        Console.Clear();
        ViewRecords();
        var id = GetNumberInput("Please Choose Id to Delete its values");

        if (id == 0) return;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"DELETE from drinking_water
                    WHERE Id = {id}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void UpdateRecord()
    {
        Console.Clear();
        ViewRecords();
        var id = GetNumberInput("Please Choose Id to update its values");

        if (id == 0) return;

        var date = GetDateInput();

        if (date == "0") return;

        var quantity = GetNumberInput("Please Enter Number of water glasses");

        if (quantity == 0) return;

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"UPDATE drinking_water
	                SET Date = '{date}', Quantity = {quantity}
                    WHERE Id = {id}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void ViewRecords()
    {
        Console.Clear();
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM drinking_water";

            using (SQLiteDataReader reader = tableCmd.ExecuteReader())
            {
                Console.WriteLine("ID  | Date       | Quantity");
                Console.WriteLine("----------------------------");

                if (!reader.Read())
                {
                    Console.WriteLine("There's no records");
                }
                else
                {
                    do
                    {
                        int id = reader.GetInt32(0);
                        string date = reader.GetString(1);
                        int quantity = reader.GetInt32(2);

                        Console.WriteLine($"{id,-3} | {date} | {quantity,5}");
                    }
                    while (reader.Read());
                }
            }

            connection.Close();
        }
        Console.WriteLine("----------------------------------------\n\nPress any key to Continue...");
        Console.ReadKey();
    }

    public static void QuantityInSpecificDay()
    {
        Console.Clear();
        string dateToCheck = GetDateInput();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"SELECT Id, Date, COALESCE(SUM(Quantity), 0) as Quantity
                                  FROM drinking_water
                                  WHERE Date = '{dateToCheck}'";

            using (SQLiteDataReader reader = tableCmd.ExecuteReader())
            {
                if (!reader.Read() || reader.IsDBNull(0))
                {
                    Console.WriteLine("There's no records for this day");
                }
                else
                {
                    Console.WriteLine("ID  | Date       | Quantity");
                    Console.WriteLine("----------------------------");
                    int id = reader.GetInt32(0);
                    string date = reader.GetString(1);
                    int quantity = reader.GetInt32(2);

                    Console.WriteLine($"{id}   | {date} |    {quantity}");
                }
            }
            connection.Close();
        }
        Console.WriteLine("----------------------------------------\n\nPress any key to Continue...");
        Console.ReadKey();
    }

    static string GetDateInput()
    {
        Console.WriteLine("Please Enter the date (format: dd-mm-yyyy). Type 0 to return to main menu.\n");

        string? dateInput = Console.ReadLine()?.Trim();

        if (dateInput == "0") return "0";

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\nInvalid date format, Please try again or type 0 to return to main menu");
            dateInput = Console.ReadLine()?.Trim();

            if (dateInput == "0") return "0";
        }

        return dateInput;
    }

    static int GetNumberInput(string message)
    {
        Console.WriteLine($"{message} Type 0 to return to main menu.");

        string? value = Console.ReadLine()?.Trim();

        if (value == "0") return 0;

        while (String.IsNullOrEmpty(value)|| !int.TryParse(value, out _))
        {
            Console.WriteLine("Please Enter a valid numeric value. Or type 0 to return to main menu");
            value = Console.ReadLine()?.Trim();

            if (value == "0") return 0;
        }
        return int.Parse(value);
    }
}
