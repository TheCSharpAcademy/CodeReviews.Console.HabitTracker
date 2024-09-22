using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker
{
    internal class Program
    {
        static string connectionString = @"Data Source=habitTracker.db";

        static void Main(string[] args)
        {
            Batteries.Init();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                //Create habits table
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HabitName TEXT,
                        Unit TEXT
                    )";
                tableCmd.ExecuteNonQuery();

                //drinking_water includes a foreign key to include a HabitId column
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY (HabitId) REFERENCES habits(Id)
                    )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            SeedData();

            GetUserInput();
        }

        private static void SeedData()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM habits";
                int habitCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (habitCount == 0)
                {
                    var insertHabitCmd = connection.CreateCommand();
                    insertHabitCmd.CommandText =
                        @"INSERT INTO habits(HabitName, Unit) 
                            VALUES('Drinking Water', 'glasses'),
                                ('Running', 'km'),
                                ('Reading', 'pages'),
                                ('Meditation', 'minutes')";
                    insertHabitCmd.ExecuteNonQuery();

                    var getHabitIdCmd = connection.CreateCommand();
                    getHabitIdCmd.CommandText = "SELECT Id FROM habits WHERE HabitName = 'Drinking Water'";
                    int drinkingWaterHabitId = Convert.ToInt32(getHabitIdCmd.ExecuteScalar());

                    var random = new Random();
                    for (int i = 0; i < 100; i++)
                    {
                        var insertRecordCmd = connection.CreateCommand();
                        insertRecordCmd.CommandText =
                            @"INSERT INTO drinking_water(Date, Quantity, HabitId) 
                              VALUES(@date, @quantity, @habitId)";

                        var randomDate = DateTime.Now.AddDays(-random.Next(0, 30)).ToString("dd-MM-yy");
                        var randomQuantity = random.Next(1, 11);

                        insertRecordCmd.Parameters.AddWithValue("@date", randomDate);
                        insertRecordCmd.Parameters.AddWithValue("@quantity", randomQuantity);
                        insertRecordCmd.Parameters.AddWithValue("@habitId", drinkingWaterHabitId);

                        insertRecordCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine("Seed data for Drinking Water habit has been inserted.");
                }

                connection.Close();
            }
        }

        static void GetUserInput()
        {
            Console.Clear();

            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Exit.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Records.");
                Console.WriteLine("Type 3 to Delete Records.");
                Console.WriteLine("Type 4 to Update Records.");
                Console.WriteLine("Type 5 to Create a New Habit");
                Console.WriteLine("------------------------------------------");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
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
                        CreateNewHabit();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid Commnad. Type a number from 0 to 4.\n");
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
                    $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-us")),
                            Quantity = reader.GetInt32(2),
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();

                Console.WriteLine("-----------------------------------------------");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yy")} - Quantity: {dw.Quantity}");

                    Console.WriteLine("-----------------------------------------------");
                }

            }
        }

        private static void Insert()
        {
            Console.Clear();
            string date = GetDateInput("\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu");
            int quantity = GetNumberInput("\nPlease insert number of glasses of other measure of your choice (no decimals allowed).\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    "INSERT INTO drinking_water(date,quantity) VALUES(@date, @quantity)";

                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete. (Type 0 to go back to Main Menu.\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                $"DELETE from drinking_water WHERE Id = @id";

                tableCmd.Parameters.AddWithValue("@id", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                    Delete();
                }

                connection.Close();
            }
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update. (Type 0 to go back to Main Menu.\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = @Id)";
                checkCmd.Parameters.AddWithValue("@Id", recordId);

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput("\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to Main Menu");
                int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice. (no decimals allowed)\n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date = @date, quantity = @quantity WHERE Id = @id";

                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@id", recordId);

                tableCmd.ExecuteNonQuery();

                connection.Close();

            }
        }

        private static void CreateNewHabit()
        {
            Console.Clear();
            string habitName = GetStringInput("Enter the name of the new habit");
            string unit = GetStringInput("Enter the unit of measurement");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"INSERT INTO habits(HabitName, Unit) 
                        VALUES(@habitName, @unit)";

                tableCmd.Parameters.AddWithValue("@habitName", habitName);
                tableCmd.Parameters.AddWithValue("@unit", unit);

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"New habit '{habitName}' with unit '{unit}' created successfully!");
        }

        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                GetUserInput();
            }

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;

        }

        private static string GetDateInput(string message)
        {
            Console.WriteLine(message);
            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                GetUserInput();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: (dd-mm-yy). Try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        private static string GetStringInput(string message)
        {
            string stringInput = "";

            while (true)
            {
                Console.WriteLine(message);
                stringInput = Console.ReadLine();

                if (string.IsNullOrEmpty(stringInput))
                {
                    Console.WriteLine("\nInput cannot be empty. Please try again.");
                }
                else if (stringInput.Length < 3)
                {
                    Console.WriteLine("\nInput must be at least 3 characters long. Please try again.");
                }
                else
                {
                    break; // Valid input, exit the loop
                }
            }

            return stringInput;
        }
    }

}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}