using System.Globalization;
using Microsoft.Data.Sqlite;

/*
This app allows you to track your hours-played in terms of 
computer and video games, to help you make better habits.
*/
namespace habit_tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS hours_played (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        Unit TEXT
                        )";

                tableCmd.ExecuteNonQuery();

                // Check if the table is empty, if so, seed it
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM hours_played";
                var count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    SeedDatabase(connection);
                }

                connection.Close();
            }

            GetUserInput();
        }

        private static void SeedDatabase(SqliteConnection connection)
        {
            Console.Clear();
            Console.WriteLine("Please wait, seeding database on first initialisation (up to 20 seconds...)");

            var random = new Random();
            var insertCmd = connection.CreateCommand();
            var numberOfRecords = 100;
            var maxQuantity = 10;

            for (int i = 0; i < numberOfRecords; i++)
            {
                string randomDate = GenerateRandomDate(random);
                int randomQuantity = random.Next(1, maxQuantity); // Random quantity between 1 and 10
                string randomUnit = GetRandomUnit(random);

                insertCmd.CommandText =
                    "INSERT INTO hours_played (Date, Quantity, Unit) VALUES (@date, @quantity, @unit)";
                insertCmd.Parameters.Clear();
                insertCmd.Parameters.AddWithValue("@date", randomDate);
                insertCmd.Parameters.AddWithValue("@quantity", randomQuantity);
                insertCmd.Parameters.AddWithValue("@unit", randomUnit);

                insertCmd.ExecuteNonQuery();
            }
        }

        private static string GenerateRandomDate(Random random)
        {
            DateTime startDate = DateTime.Now.AddYears(-1); // Start date: 1 year ago
            int range = (DateTime.Now - startDate).Days;
            return startDate.AddDays(random.Next(range)).ToString("dd-MM-yy");
        }

        private static string GetRandomUnit(Random random)
        {
            string[] units = { "hours", "minutes" };
            return units[random.Next(units.Length)];
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;

            while (closeApp == false)
            {
                Console.WriteLine("Welcome to the game-tracking app");
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\n1. View all records");
                Console.WriteLine("2. Insert a record");
                Console.WriteLine("3. Delete a record");
                Console.WriteLine("4. Update a record");
                Console.WriteLine("5. Get report for a year");
                Console.WriteLine("0. Exit");
                Console.WriteLine("------------------------------------------\n");

                string command = Console.ReadLine();

                switch (command)
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
                    case "5":
                        GetReportForAYear();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
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
                    $"SELECT * FROM hours_played ";

                List<HoursPlayed> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new HoursPlayed
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                            Unit = reader.GetString(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity} {dw.Unit}");
                }
                Console.WriteLine("------------------------------------------\n");
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert the quantity (minutes or hours) :\n\n");
            string unit = GetStringInput("\n\nPlease insert the unit of measure (i.e minutes or hours) : \n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                // Parameterized query
                tableCmd.CommandText =
                    "INSERT INTO hours_played (Date, Quantity, Unit) VALUES (@date, @quantity, @unit)";

                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@unit", unit);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static string GetStringInput(string message)
        {
            Console.WriteLine(message);

            string stringInput = Console.ReadLine();

            if (stringInput == "0") GetUserInput();

            while (stringInput == null)
            {
                Console.WriteLine("\n\nInvalid input, please try again:");
                stringInput = Console.ReadLine();
            }

            return stringInput;
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                // Parameterized query
                tableCmd.CommandText = "DELETE FROM hours_played WHERE Id = @recordId";

                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. Press any key to continue... \n\n");
                    connection.Close();
                    Console.ReadKey();
                    Delete();
                    return;
                }
                else
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
                }
            }

            GetUserInput();
        }

        internal static void Update()
        {
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Check if the record exists
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM hours_played WHERE Id = @recordId)";
                checkCmd.Parameters.AddWithValue("@recordId", recordId);

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. Press any key to continue... \n\n");
                    connection.Close();
                    Console.ReadKey();
                    Update();
                    return;
                }

                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease insert the quantity (minutes or hours) :\n\n");
                string unit = GetStringInput("\n\nPlease insert the unit of measure (i.e minutes or hours) : \n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "UPDATE hours_played SET Date = @date, Quantity = @quantity, Unit = @unit WHERE Id = @recordId";

                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@unit", unit);
                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// Counts the number of times a habit was performed in a given year (YY)
        /// </summary>
        private static void GetReportForAYear()
        {
            int habitCount = 0;

            Console.WriteLine($"\n\nWhich year would you like a report for? (YY format) : \n\n");

            int yearInput = GetTwoDigitYearFromUser();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var reportCmd = connection.CreateCommand();
                reportCmd.CommandText =
                    @"SELECT COUNT(*) 
                    FROM hours_played 
                    WHERE substr(Date, 7, 2) = @year";  // Extracts the last 2 characters of the year

                reportCmd.Parameters.AddWithValue("@year", (yearInput % 100).ToString("00"));  // Format as two digits

                habitCount = Convert.ToInt32(reportCmd.ExecuteScalar());

                connection.Close();
            }

            Console.WriteLine($"The habit was performed {habitCount} times in {yearInput}.");
        }

        private static int GetTwoDigitYearFromUser()
        {
            int year = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.WriteLine("Please enter a 2-digit year:");

                string input = Console.ReadLine();

                if (input.Length == 2 && input.All(char.IsDigit))
                {
                    year = Convert.ToInt32(input);

                    if (year >= 0 && year <= 99)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid 2-digit year (00-99).");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a 2-digit year.");
                }
            }

            int fullYear = 2000 + year;

            return fullYear;
        }

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

    public class HoursPlayed
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}