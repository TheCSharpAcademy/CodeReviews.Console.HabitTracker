using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabbitTracker
{
    internal class Program
    {
        static string connectionString = @"Data Source=habbit-Tracker.db";
        static void Main(string[] args)
        {


            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand tblCmd = connection.CreateCommand();

                tblCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habbits_table(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Activity TEXT,
                        Date TEXT,
                        Quantity INTEGER,
                        Units TEXT
                        )";

                tblCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.WriteLine("Would you like to seed data? (Y/N)");

            string seedInput = Console.ReadLine();

            if (seedInput.ToLower() == "y")
            {
                SeedData();
            }


            GetUserInput();
        }

        private static void GetUserInput()
        {
            Thread.Sleep(100);
            Console.Clear();
            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nEnter 0 to close application");
                Console.WriteLine("Enter 1 to view all records");
                Console.WriteLine("Enter 2 insert record");
                Console.WriteLine("Enter 3 to delete record");
                Console.WriteLine("Enter 4 to update record");
                Console.WriteLine("Enter 5 to get report");
                Console.WriteLine("--------------------------------------------------------");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
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
                        Report();
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
        }



        private static void GetAllRecords()
        {
            Console.Clear();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM habbits_table ";


                List<Activity> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Activity record = new()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Units = reader.GetString(4)
                        };

                        tableData.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine("No records found");
                }
                connection.Close();

                Console.WriteLine("--------------------------------------------------------\n");

                foreach (Activity record in tableData)
                {
                    Console.WriteLine($"Id: {record.Id} - Activity: {record.Name} - Date: {record.Date.ToString("dd-MMM-yyyy")} - Quantity: {record.Quantity} - Units: {record.Units}");
                }

                Console.WriteLine("--------------------------------------------------------\n");
            }
        }


        private static void Insert()
        {
            string activity = GetActivityInput();

            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease enter units of measurment. No decimals allowed\n\n");

            string units = GetUnitInput();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@activity", activity);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@units", units);
                tableCmd.CommandText =
                    $"INSERT INTO habbits_table (Activity, Date, Quantity, Units) VALUES (@activity, @date, @quantity, @units)";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            int id = GetNumberInput("\n\nEnter the Id of the record you want to delete or press 0 to return to main menu\n\n");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@id", id);
                tableCmd.CommandText =
                    $"DELETE FROM habbits_table WHERE Id = (@id)";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {id} doesn't exist");
                    Thread.Sleep(2000);
                    Delete();
                }

                connection.Close();

                Console.WriteLine($"\n\nRecord with Id {id} has been deleted");
                Thread.Sleep(2000);
                GetUserInput();
            }
        }

        private static void Update()
        {
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.Parameters.AddWithValue("@id", recordId);
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habbits_table WHERE Id = @id)";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    Thread.Sleep(2000);
                    Update();
                }

                string activity = GetActivityInput();

                string date = GetDateInput();

                int quantity = GetNumberInput("\n\nPlease enter units of measurment (no decimals allowed)\n\n");

                string units = GetUnitInput();

                var tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@id", recordId);
                tableCmd.Parameters.AddWithValue("@activity", activity);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@units", units);
                tableCmd.CommandText = $"UPDATE habbits_table SET date = @date, activity = @activity, quantity = @quantity, units = @units WHERE Id = @id";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                GetUserInput();

            }
        }

        private static void Report()
        {
            Console.Clear();
            Console.WriteLine("\n\nWould you like to see report by activity(1), date(2), or quantity(3) ?");

            string reportType = Console.ReadLine();

            switch (reportType)
            {
                case "1":
                    ReportByActivity();
                    break;
                case "2":
                    ReportByDate();
                    break;
                case "3":
                    ReportByQuantity();
                    break;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    Report();
                    break;
            }
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

        private static string GetDateInput()
        {
            Console.WriteLine("\n\nEnter date (Format: dd-mm-yy) or type 0 to return to main menu.\n\n");
            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                GetUserInput();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        private static string GetActivityInput()
        {
            Console.WriteLine("\n\nEnter name of activity");
            string activity = Console.ReadLine();

            while (string.IsNullOrEmpty(activity))
            {
                Console.WriteLine("\n\nInvalid input. Please try again.\n\n");
                activity = Console.ReadLine();
            }

            return activity;
        }

        private static string GetUnitInput()
        {
            Console.WriteLine("\n\nEnter units of measurement");
            string activity = Console.ReadLine();

            while (string.IsNullOrEmpty(activity))
            {
                Console.WriteLine("\n\nInvalid input. Please try again.\n\n");
                activity = Console.ReadLine();
            }

            return activity;
        }

        private static void SeedData()
        {
            string[] activities = { "Running", "Swimming", "Cycling", "Walking", "Yoga", "Weightlifting", "Dancing", "Meditation", "Reading", "Writing" };
            string[] units = { "km", "laps", "miles", "lbs", "minutes", "kg", "hours", "feet", "inches", "pages" };
            string[] dates = { "01-01-21", "02-01-21", "03-01-21", "04-01-21", "05-01-21", "06-01-21", "07-01-21", "08-01-21", "09-01-21", "10-01-21" };
            int[] quantities = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            List<Activity> seedList = new();

            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                Activity activity = new()
                {
                    Name = activities[random.Next(0, activities.Length)],
                    Date = DateTime.Parse(dates[random.Next(0, dates.Length)]),
                    Quantity = quantities[random.Next(0, quantities.Length)],
                    Units = units[random.Next(0, units.Length)]
                };

                seedList.Add(activity);
            }

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                foreach (Activity activity in seedList)
                {
                    SqliteCommand tableCmd = connection.CreateCommand();
                    tableCmd.Parameters.AddWithValue("@activity", activity.Name);
                    tableCmd.Parameters.AddWithValue("@date", activity.Date.ToString("dd-MM-yy"));
                    tableCmd.Parameters.AddWithValue("@quantity", activity.Quantity);
                    tableCmd.Parameters.AddWithValue("@units", activity.Units);
                    tableCmd.CommandText =
                        $"INSERT INTO habbits_table (Activity, Date, Quantity, Units) VALUES (@activity, @date, @quantity, @units)";

                    tableCmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private static void ReportByQuantity()
        {
            Console.Clear();
            int quantity = GetNumberInput("\n\nPlease type the quantity of the records you want to view.\n\n");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.CommandText = $"SELECT * FROM habbits_table WHERE Quantity = @quantity";

                List<Activity> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Activity record = new()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Units = reader.GetString(4)
                        };

                        tableData.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine($"\n\nRecord with quantity {quantity} doesn't exist.\n\n");
                    connection.Close();
                    Thread.Sleep(2000);
                    GetUserInput();
                    return;
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------\n");
                foreach (Activity record in tableData)
                {
                    Console.WriteLine($"Id: {record.Id} - Activity: {record.Name} - Date: {record.Date.ToString("dd-MMM-yyyy")} - Quantity: {record.Quantity} - Units: {record.Units}");
                }

                Console.WriteLine("--------------------------------------------------------\n");
                Console.WriteLine("Press any key to return to main menu");
                Console.ReadLine();
                GetUserInput();
            }
        }

        private static void ReportByDate()
        {
            Console.Clear();
            string date = GetDateInput();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.CommandText = $"SELECT * FROM habbits_table WHERE Date = @date";

                List<Activity> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Activity record = new()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Units = reader.GetString(4)
                        };

                        tableData.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine($"\n\nRecord with date {date} doesn't exist.\n\n");
                    connection.Close();
                    Thread.Sleep(2000);
                    GetUserInput();
                    return;
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------\n");
                foreach (Activity record in tableData)
                {
                    Console.WriteLine($"Id: {record.Id} - Activity: {record.Name} - Date: {record.Date.ToString("dd-MMM-yyyy")} - Quantity: {record.Quantity} - Units: {record.Units}");
                }

                Console.WriteLine("--------------------------------------------------------\n");
                Console.WriteLine("Press any key to return to main menu");
                Console.ReadLine();
                GetUserInput();
            }
        }

        private static void ReportByActivity()
        {
            Console.Clear();
            string activity = GetActivityInput();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@activity", activity);
                tableCmd.CommandText = $"SELECT * FROM habbits_table WHERE Activity = @activity";

                List<Activity> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Activity record = new()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Units = reader.GetString(4)
                        };

                        tableData.Add(record);
                    }
                }
                else
                {
                    Console.WriteLine($"\n\nRecord with activity {activity} doesn't exist.\n\n");
                    connection.Close();
                    Thread.Sleep(2000);
                    GetUserInput();
                    return;
                }

                connection.Close();
                Console.Clear();
                Console.WriteLine("--------------------------------------------------------\n");
                foreach (Activity record in tableData)
                {
                    Console.WriteLine($"Id: {record.Id} - Activity: {record.Name} - Date: {record.Date.ToString("dd-MMM-yyyy")} - Quantity: {record.Quantity} - Units: {record.Units}");
                }

                Console.WriteLine("--------------------------------------------------------\n");
                Console.WriteLine("Press any key to return to main menu");
                Console.ReadLine();
                GetUserInput();
            }
        }
        
    }

    
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        public string Units { get; set; }
    }
}

