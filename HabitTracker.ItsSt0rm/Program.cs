using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.ItsSt0rm
{
    internal class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_tracker (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Measure TEXT,
            Quantity INTEGER
            )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();

        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the app");
                Console.WriteLine("Type 1 to View all records");
                Console.WriteLine("Type 2 to Insert record");
                Console.WriteLine("Type 3 to Delete record");
                Console.WriteLine("Type 4 to Update record");
                Console.WriteLine("--------------------------------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
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
                        Console.WriteLine("\nInvalid command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM habit_tracker ";

                List<HabitTracker> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new HabitTracker
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Measure = reader.GetString(2),
                                Quantity = reader.GetInt32(3)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("------------------------------------\n");
                foreach (var habitTracker in tableData)
                {
                    Console.WriteLine($"{habitTracker.Id} - {habitTracker.Date.ToString("dd-MMM-yyyy")} - Measure: {habitTracker.Measure} - Quantity: {habitTracker.Quantity}");
                }
                Console.WriteLine("------------------------------------\n");
            }
        }


        private static void Insert()
        {
            string date = GetDateInput();

            Console.WriteLine("Please insert the measure of your habit.");
            string measure = Console.ReadLine();

            int quantity = GetNumberInput($"\n\nPlease insert number of {measure}." +
                "(No decimals allowed)\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO habit_tracker(date, measure, quantity) VALUES('{date}','{measure}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to return to main menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

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

        private static void Update()
        {        
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record that you would like to update. Type 0 to return to main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

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
    }

    public class HabitTracker
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Measure { get; set; }
        public int Quantity { get; set; }
    }
}