using Microsoft.Data.Sqlite;
using System.Globalization;

namespace Habit_Tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {
            SQLitePCL.Batteries.Init();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HabitType TEXT,
                        Date TEXT,
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
            while (closeApp == false)
            {
                Console.WriteLine("\n\n MAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("Type 5 to Clear All Records.");
                Console.WriteLine("Type 6 to View Records by Date Range.");
                Console.WriteLine("Type 7 to View Records for a Specific Habit.");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!");
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
                    case "5":
                        ClearDatabase();
                        break;
                    case "6":
                        GetAllRecords(true);
                        break;
                    case "7":
                        ViewHabitRecords();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 7.\n");
                        break;
                }
            }
        }

        private static void ViewHabitRecords()
        {
            Console.WriteLine("Enter the habit you want to view records for:");
            string habitType = Console.ReadLine();

            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM habits WHERE HabitType = @habitType";
                tableCmd.Parameters.AddWithValue("@habitType", habitType);

                List<Habit> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            HabitType = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yyyy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No records found for this habit.");
                }

                connection.Close();

                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++\n");
                foreach (var habit in tableData)
                {
                    Console.WriteLine($"{habit.Id} - {habit.HabitType} - {habit.Date.ToString("dd-MM-yyyy")} - Quantity: {habit.Quantity}");
                }
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++\n");
            }
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to return to main menu. \n\n");

            if (recordId == 0)
                return;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    return;
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\n\nPlease insert number of measure of your choice " +
                    "(no decimals allowed)\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habits SET date = @date, quantity = @quantity WHERE Id = @recordId";
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or Type 0 to return to the Main Menu");

            if (recordId == 0) 
                return;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE from habits WHERE Id = @recordId ";
                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    return;
                }
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

            GetUserInput();
        }

        private static void GetAllRecords(bool filterByDate = false)
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                if (filterByDate)
                {
                    string startDate = GetDateInput("\n\nPlease enter the start date (dd-MM-yyyy): ");
                    string endDate = GetDateInput("\n\nPlease enter the end date (dd-MM-yyyy): ");
                    tableCmd.CommandText = $"SELECT * FROM habits WHERE Date BETWEEN '{startDate}' AND '{endDate}'";
                }
                else
                {
                    tableCmd.CommandText = "SELECT * FROM habits";
                }

                List<Habit> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            HabitType = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yyyy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No records found.");
                }

                connection.Close();

                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++\n");
                foreach (var habit in tableData)
                {
                    Console.WriteLine($"{habit.Id} - {habit.HabitType} - {habit.Date.ToString("dd-MM-yyyy")} - Quantity: {habit.Quantity}");
                }
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++\n");
            }
        }

        private static void Insert()
        {
            Console.WriteLine("Enter the habit you want to track (e.g., drinking water, exercise, reading):");
            string habitType = Console.ReadLine();

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of units (e.g., glasses, hours) for the habit\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "INSERT INTO habits(HabitType, Date, Quantity) VALUES(@habitType, @date, @quantity)";

                tableCmd.Parameters.AddWithValue("@habitType", habitType);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static string GetDateInput(string message = "\n\nPlease insert the date: (Format: dd-MM-yyyy). Type 0 to return to main menu.")
        {
            Console.WriteLine(message);

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yyyy). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        private static int GetNumberInput(string message)
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

        private static void ClearDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "DELETE FROM habits";
                tableCmd.ExecuteNonQuery();

                Console.WriteLine("All records have been deleted.");
                connection.Close();
            }
        }
    }

    public class Habit
    {
        public int Id { get; set; }
        public string HabitType { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
