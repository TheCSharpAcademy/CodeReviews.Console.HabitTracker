using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Globalization;
namespace habit_tracker;

class Program
{
    static string connectionString = @"Data Source=habit-tracker-db.db";

    static void Main(string[] args)
    {


        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            // there is no DATE in Sqlite
            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS push_ups (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();

        bool closeApp = false;
        string? userInput;

        while (!closeApp)
        {

            System.Console.WriteLine("-------------------------");
            System.Console.WriteLine("|\tMAIN MENU\t|");
            System.Console.WriteLine("-------------------------");
            System.Console.WriteLine("Choose one of the menu optins (type 'x' for exit): \n");
            System.Console.WriteLine("1 - View all recorded data");
            System.Console.WriteLine("2 - Add a new entry");
            System.Console.WriteLine("3 - Delete an entry");
            System.Console.WriteLine("4 - Update an entry");
            System.Console.WriteLine();

            System.Console.Write("Your choice: ");
            userInput = Console.ReadLine();

            if (userInput == null || userInput.Trim().Length == 0)
            {
                System.Console.WriteLine("Not a valid entry. Try again.");
                continue;
            }

            switch (userInput)
            {
                case "x":
                    System.Console.WriteLine("App has been terminated.");
                    closeApp = true;
                    break;
                case "1":
                    GetAllHabits();
                    break;
                case "2":
                    AddHabit();
                    break;
                case "3":
                    DeleteHabit();
                    break;
                case "4":
                    UpdateHabit();
                    break;
                default:
                    System.Console.Write("\nNot a valid choice. Please type a number from 1 - 4 (or 'x' to exit). \n\n");
                    break;
            }
        }
    }


    private static void GetAllHabits()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM push_ups";

            List<PushUp> tableData = new List<PushUp>();


            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new PushUp
                        {
                            // tell the reader what type of data to expect
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        });
                }
            }
            else
            {
                System.Console.WriteLine("No data found (Empy database)");
            }

            connection.Close();

            System.Console.WriteLine("Id\tDate\tQuantity\n");
            System.Console.WriteLine("_________________________");
            foreach (var pu in tableData)
            {
                System.Console.WriteLine($"{pu.Id} | {pu.Date.ToString("dd-MM-yyyy")} | {pu.Quantity}\t|");
            }
            System.Console.WriteLine("-------------------------\n");
        }
    }

    private static void AddHabit()
    {
        // since the input has a date, a method for adding a date is needed (GetDateInput())
        string date = GetDateInput();
        // quantity is also needed, so a new method is created (GetNumberInput())
        int quantity = GetNumberInput("\n\nPlease insert the number of push-ups you did for the date (no decimals) ('b' to return to previous menu):\n");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO push_ups(date, quantity) VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void DeleteHabit()
    {
        Console.Clear();
        GetAllHabits();

        int habitId = GetNumberInput("\n\nType the id of the habit you wish to delete or hit 'b' for Main Menu.\n");
        if (habitId == -1)
        {
            return; // Go back to the main menu
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM push_ups WHERE Id = {habitId}";

            int rowCount = tableCmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"Habit with Id {habitId} does not exist. Press any key to try again.");
                Console.ReadKey();
                return; // Go back to the main menu
            }
            else
            {
                Console.WriteLine($"Habit with Id {habitId} was removed. Press any key to continue.");
                Console.ReadKey();
            }
        }
    }

    public static void UpdateHabit()
    {
        Console.Clear();
        GetAllHabits();


        var habitId = GetNumberInput("\nEnter the id of the habit you would like to update.\n");

        if (habitId == -1)
        {
            return;
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM push_ups WHERE Id = {habitId})";
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count == 0)
            {
                System.Console.WriteLine($"\nHabit with id {habitId} doesn't exist");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
                return;
            }

            string date = GetDateInput();

            if (date == "b")
            {
                return;
            }
            int quantity = GetNumberInput("\nPlease chose number of pushups you actually want\n");
            if (quantity == -1)
            {
                return;
            }

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"UPDATE push_ups SET date = '{date}', quantity = {quantity} WHERE Id = {habitId}";
            tableCmd.ExecuteNonQuery();
            Console.WriteLine($"\nHabit with Id {habitId} has been updated.");

        }
        Console.WriteLine("Press any key to return to the main menu.");
        Console.ReadKey();
    }

    internal static int GetNumberInput(string message)
    {
        System.Console.WriteLine(message);

        string? userInput = Console.ReadLine();

        if (userInput == "b") return -1;

        if (userInput == null)
        {
            System.Console.WriteLine("\nInput can't be null.");
            return GetNumberInput(message);
        }
        else
        {
            int.TryParse(userInput, out var result);
            return result;
        }

    }

    internal static string GetDateInput()
    {
        while (true)
        {
            Console.WriteLine("\nPlease insert the date (dd-MM-yy) ('b' to return to the previous menu): ");

            string? userInput = Console.ReadLine()?.Trim();

            if (userInput == "b")
            {
                return "b";
            }

            if (!string.IsNullOrEmpty(userInput))
            {

                if (DateTime.TryParseExact(userInput, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the date in the format dd-MM-yy.");
                }
            }
            else
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }
    }
}

public class PushUp
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
