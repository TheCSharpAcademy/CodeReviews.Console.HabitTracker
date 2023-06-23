using Microsoft.Data.Sqlite;
using System.Globalization;

class Program {
    /// <summary>
    /// connectionString is the database name we are connecting to.
    /// </summary>
    readonly static string connectionString = @"Data Source=Habit-Tracker.db";

    /// <summary>
    /// The entry point of our program.
    /// </summary>
    static void Main()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS distance_ran (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Distance INTEGER
        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
        GetUserInput();
    }

    /// <summary>
    /// The GetDateInput method is a helper function within the class to get the users date input for inserting and updating
    /// a record found within the database.
    /// </summary>
    /// <returns>Returns a valid date in the format of dd-mm-yy, e.g., 23-Jul-23</returns>
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

    /// <summary>
    /// The GetNumberInput method is a helper function within the class to get the users provided record number for
    /// chosing the record they would like to update or delete. This parses the user input to ensure it is a valid Int32.
    /// </summary>
    /// <param name="message">Takes a string to print to the console, e.g., "Please type the ID you would like to update or delete".</param>
    /// <returns>The integer to be used for the record ID.</returns>
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

    /// <summary>
    /// GetUserInput presents the user with a main menu and awaits a users input.
    /// </summary>
    static void GetUserInput()
    {
        Console.Clear();
        var closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\n Main Menu");
            Console.WriteLine("\nPlease pick between the options below?");
            Console.WriteLine("\n0 - Close Application.");
            Console.WriteLine("1 - View all Records");
            Console.WriteLine("2 - Insert Record");
            Console.WriteLine("3 - Delete Record");
            Console.WriteLine("4 - Update Record");
            Console.WriteLine("-------------------------------------");

            string commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0":
                    Console.WriteLine("\nThank you, goodbye.\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
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
                default:
                    Console.WriteLine("\nInvalid option.. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    /// <summary>
    /// Update gets a single record from the database where the ID is specified by the user via the GetNumberInput method.
    /// </summary>
    internal static void Update()
    {
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type the ID of the record you would like to update. Type 0 to return to main manu.\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM distance_ran WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                Update();
            }

            string date = GetDateInput();

            int distance = GetNumberInput("\n\nPlease insert the distance you ran or other measure of your choice (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE distance_ran SET date = '{date}', distance = {distance} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }


    }

    /// <summary>
    /// Delete removes a single record from the database where the ID is specified by the user via the GetNumberInput method.
    /// </summary>
    private static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type the ID of the record you want to delete or type 0 to go back to Main Menu\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE from distance_ran WHERE Id = '{recordId}'";

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

    /// <summary>
    /// Insert adds a new record based on the users inputs for a date and distance that was ran into the database.
    /// </summary>
    private static void Insert()
    {
        string date = GetDateInput();

        int distance = GetNumberInput("\n\nPlease insert the distance you ran or other measure of your choice (no decimals allowed)\n\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
               $"INSERT INTO distance_ran(date, distance) VALUES('{date}', {distance})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    /// <summary>
    /// GetAllRecords retrieves all the records from the database and places them into a list of DistanceRan objects.
    /// The data that is stored in the list is then looped through to present to the console.
    /// </summary>
    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionString)) {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM distance_ran";
            List<DistanceRan> tableData = new List<DistanceRan>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new DistanceRan
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Distance = reader.GetInt32(2)
                    });
                }
            } else
            {
                Console.WriteLine("No rows founds");
            }

            connection.Close();
            Console.WriteLine("------------------------------------------\n");
            foreach (var data in tableData)
            {
                Console.WriteLine($"{data.Id} - {data.Date.ToString("dd-MMM-yyyy")} - Distance: {data.Distance}");
            }
            Console.WriteLine("------------------------------------------\n");
        }
    }


}


/// <summary>
/// DistanceRan is a class that represents the data that is stored within the database.
/// </summary>
public class DistanceRan
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Distance { get; set; }
}