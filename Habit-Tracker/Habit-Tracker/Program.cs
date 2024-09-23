using Microsoft.Data.Sqlite;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;

internal class Program
{
    static string habit = "";
    static string unit = "";
    static string connectionString = "Data Source = habit_Tracker.db";
    internal static void Main(string[] args)
    {
        CreateHabit();
        GetUserInput();
    }

    private static void GetUserInput()
    {
        while (true)
        {
            Console.WriteLine("\n\nMain Menu.");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Exit.");
            Console.WriteLine("\tType 1 to View records.");
            Console.WriteLine("\tType 2 to Insert record.");
            Console.WriteLine("\tType 3 to Delete record.");
            Console.WriteLine("\tType 4 to Update record.");
            Console.WriteLine("\tType 5 to Report the whole year record.");
            Console.WriteLine("\tType 6 to Change habit and/or unit of measurement.");

            string command = Console.ReadLine();

            switch(command)
            {
                case "0":
                    Console.WriteLine("GOOD BYE!!!");
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    Report();
                    break;
                case "6":
                    CreateHabit();
                    break;
                default:
                    Console.WriteLine("\n\nInvalid input. Please type from 0 to 6.");
                    break;
            }
        }
    }

    private static void Report()
    {
        Console.WriteLine("\n\nWhich year record you wan to track?\n\n");
        string year = GetDateInput();
        string finalYear = year.Substring(year.Length - 2, 2);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var reportCmd = connection.CreateCommand();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE date LIKE '%{finalYear}')";

            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if(checkQuery == 0)
            {
                Console.WriteLine($"\n\nThere are no record in the year 20{finalYear}");
                connection.Close();
                Report();
            }

            reportCmd.CommandText = $@"SELECT SUM(quantity), AVG(quantity), COUNT(ID)
                                        FROM {habit} WHERE date LIKE '%{finalYear}'";

            SqliteDataReader reader = reportCmd.ExecuteReader();

            while (reader.Read())
            {
                int totalYear = reader.GetInt32(2);
                int average = reader.GetInt32(1);
                int sum = reader.GetInt32(0);

                Console.Write($"\nTotal number of times {habit} has been done in 20{finalYear}:\t{totalYear} times\n");
                Console.Write($"Average number of {unit} of {habit} that has been done in 20{finalYear}:\t{average} {unit}\n");
                Console.WriteLine($"Total number of {unit} of {habit} that has been done in 20{finalYear}:\t{sum} {unit}");
            }
            connection.Close();
        }
    }

    private static void UpdateRecord()
    {
        Console.Clear();
        GetAllRecords();
        var recordID = GetNumberInput("\n\nPlease type the ID of the record you would like to updata or type 0 to return to main menu.\n\n");

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habit} WHERE ID = {recordID})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if(checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordID} does not exists.\n");
                connection.Close();
                UpdateRecord();
            }

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert the new quantity, or type 0 to return to main menu\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {habit} SET date = '{date}', quantity = {quantity} WHERE ID = {recordID}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private static void DeleteRecord()
    {
        Console.Clear();
        GetAllRecords();
        var recordID = GetNumberInput("\n\nPlease type ID of the record you want to delete or type 0 to return back to main menu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE FROM {habit} WHERE ID = {recordID}";

            int rowCount = tableCmd.ExecuteNonQuery();

            if(rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with ID {recordID} does not exist.\n\n");
                DeleteRecord();
            }
            connection.Close();
        }
        Console.WriteLine($"\n\nRecord wiht ID {recordID} was deleted.\n\n");
        GetUserInput();
    }

    private static void InsertRecord()
    {
        Console.Clear();

        string date = GetDateInput();
        int quantity = GetNumberInput($"\n\nPlease type the number of {unit}, or type 0 to return to main menu.\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"INSERT INTO {habit} (Date , Quantity) VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numInput = Console.ReadLine();

        if (numInput == "0") GetUserInput();
        while(!Int32.TryParse(numInput, out _) || Convert.ToInt32(numInput) < 0)
        {
            Console.WriteLine("\n\nInvalid input number. Try again.\n\n");
            numInput = Console.ReadLine();
        }

        return Convert.ToInt32(numInput);
    }

    private static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert date: (Format: dd-mm-yy).Type 0 to return back to main menu.\n");
        string dateInput = Console.ReadLine();

        if (dateInput == "0") GetUserInput();

        while(!DateTime.TryParseExact(dateInput,"dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None,out _))
        {
            Console.WriteLine("\n\nInvalid date.(Format: dd-mm-yy).Type 0 to  return to main menu.");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }

    private static void CreateHabit()
    {
        Console.WriteLine("\nWhat habit you would like to track?");
        habit = Console.ReadLine();

        while(string.IsNullOrEmpty(habit) || habit.Any(x => !char.IsLetter(x)))
        {
            Console.WriteLine("\nInvalid input. Please type text only.");
            habit = Console.ReadLine();
        }

        Console.WriteLine($"\n\nWhat unit of measurement you want to use for {habit}?");

        unit = Console.ReadLine();
        while(string.IsNullOrEmpty (unit) || unit.Any(x => !char.IsLetter(x)))
        {
            Console.WriteLine("\n\nInvalid input. Please type only text.");
            unit = Console.ReadLine();
        }

        CreateTable();
    }

    private static void CreateTable()
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {habit}(
                                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        Console.WriteLine("ID\tDate\t\tQuantity");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT *FROM {habit}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    int ID = reader.GetInt32(0);
                    DateTime Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US"));
                    int Quantity = reader.GetInt32(2);

                    Console.WriteLine($"\n{ID}\t{Date.Date.ToString("dd-MM-yy")}\t{Quantity} {unit}\n");
                }
            }
            else
            {
                Console.WriteLine("\n-------No rows found!---------");
            }
            connection.Close();
        }
    }
}
