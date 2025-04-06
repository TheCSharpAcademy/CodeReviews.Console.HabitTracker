using Microsoft.Data.Sqlite;
using System.Collections;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HabitTracker
{
    class Program
    {
        static string connectionString = @"Data Source=Habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS coding_practice (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }

            GetUserInput();
        }

        static void GetUserInput()
        {
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("0. Close App");
                Console.WriteLine("1. View all Records");
                Console.WriteLine("2. Insert a Record");
                Console.WriteLine("3. Delete a Record");
                Console.WriteLine("4. Update an existing Record");
                Console.WriteLine("5. Delete all records");
                Console.WriteLine("--------------------------------------------------------\n");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        closeApp = true;                       
                        break;
                    case "1":
                        ViewRecords();
                        break;
                    case "2":
                        AddRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                    case "4":
                        UpdateRecord();
                        break;
                    case "5":
                        DeleteAllRecords();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            Environment.Exit(0);
        }
        private static bool ViewRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM coding_practice";

                List<CodingPractice> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new CodingPractice
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")).ToString("dd-MMM-yyyy"),
                            Quantity = reader.GetInt32(2)
                        });
                    }                  
                }
                else
                {
                    Console.WriteLine("No records found.");
                    connection.Close();
                    return false;
                }

                connection.Close(); //Close database

                Console.WriteLine("----------------------------------------------\n");
                foreach (var row in tableData)
                {
                    Console.WriteLine($"ID: {row.Id} | Date: {row.Date} | Quantity: {row.Quantity}");
                }
            }
            return true;
        }
        private static void AddRecord()
        {
            string date = GetDate();

            int quantity = GetQuantity("\n\nPlease insert the number of hours you spent coding today as an integer, or type 0 to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO coding_practice (date,quantity) VALUES('{date}',{quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }
        }
        private static void DeleteRecord()
        {
            if (!ViewRecords()) return;

            int id = GetQuantity("Enter the ID of the record you want to delete, or type 0 to return to the Main Menu.");

            if (id == 0) GetUserInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE FROM coding_practice WHERE Id = '{id}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if(rowCount == 0)
                {
                    Console.WriteLine("No record found with the given ID.");
                    DeleteRecord();
                }
                connection.Close(); //Close database
            }
        }
        private static void UpdateRecord()
        {
            Console.Clear();
            if (!ViewRecords()) return;

            int id = GetQuantity("Enter the ID of the record you want to update, or type 0 to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); //Open database

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_practice WHERE Id = {id})";
                if(Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                {
                    Console.WriteLine($"No record found with ID {id}.");
                    Console.WriteLine("Type any button to continue.");
                    Console.ReadLine();
                    connection.Close();
                    UpdateRecord();
                }

                string date = GetDate();

                int quantity = GetQuantity("Please insert the number of hours you spent coding today as an integer, or type 0 to return to the main menu.");
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE coding_practice SET Date = '{date}', Quantity = {quantity} WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();

                connection.Close(); //Close database
            }

        }
        private static void DeleteAllRecords()
        {
            Console.WriteLine("Are you sure you want to delete all records? (Y/N)");
            string choice = Console.ReadLine().ToUpper();
            if(choice == "Y")
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open(); //Open database
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        $"DELETE FROM coding_practice";

                    tableCmd.ExecuteNonQuery();

                    connection.Close(); //Close database
                }
            }
            else
            {
                GetUserInput();
            }
        }

        internal static string GetDate()
        {
            Console.WriteLine("Enter Date (Format: dd-MM-yy). Type 0 to return to the main menu.");
            
            string dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();

            while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Clear();
                Console.WriteLine("Invalid date format. Please try again.");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        internal static int GetQuantity(string message)
        {
            Console.WriteLine(message);

            string quantityInput = Console.ReadLine();

            if (quantityInput == "0") GetUserInput();
            
            while(!Int32.TryParse(quantityInput, out _) || Convert.ToInt32(quantityInput) < 0) //Handles negative numbers
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please try again.");
                quantityInput = Console.ReadLine();
            }

            return Convert.ToInt32(quantityInput);
        }
    }
}

public class CodingPractice
{
    public int Id { get; set; }
    public string Date { get; set; }
    public int Quantity { get; set; }
}
