using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.JsPeanut
{
    class Program
    {
        static string connectionString = @"Data Source=habit-tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "CREATE TABLE IF NOT EXISTS habits(Id INTEGER PRIMARY KEY AUTOINCREMENT, HabitName TEXT, Date TEXT, MeasurementUnit TEXT, Quantity INTEGER)";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();
        }


        static void GetUserInput()
        {
            bool exit = false;
            Console.WriteLine("\n   Welcome to HabitLogger! Here you have a list of what you can do in this application:\n\n0: Exit the application\n1: Insert a habit\n2: Read your habits\n3: Edit your habits\n4: Delete your habits\n5: Statistics\n");
            string userInput = Console.ReadLine();
            while (exit == false)
            {
                switch (userInput)
                {
                    case "0":
                        Console.WriteLine("You have exited the app successfully.");
                        exit = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Insert();
                        break;
                    case "2":
                        GetAllRecords();
                        GetUserInput();
                        break;
                    case "3":
                        Update();
                        break;
                    case "4":
                        Delete();
                        break;
                    case "5":
                        ReportFunctionality();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("That option does not exist.");
                        GetUserInput();
                        break;
                }
            }
        }

        internal static string GetHabitInput()
        {
            Console.WriteLine("Please type the habit you want to insert. Type M to return to the main menu.");

            string habitInput = Console.ReadLine();

            if (habitInput == "M") GetUserInput();

            while (Int32.TryParse(habitInput, out _))
            {
                Console.WriteLine("You inserted a number instead of a habit. Type M to return to the main menu.");
                habitInput = Console.ReadLine();
                if (habitInput == "M") GetUserInput();
            }

            return habitInput;
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("Please type the date with the following format: (DD-MM-YY). Type M to return to the main menu.");
            string dateInput = Console.ReadLine();

            if (dateInput == "M") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type M to return to the main manu or try again:\n\n");
                dateInput = Console.ReadLine();
                if (dateInput == "M")
                {
                    GetUserInput();
                }
            }

            return dateInput;
        }

        internal static string GetMeasureUnit()
        {
            Console.WriteLine("Please type the measurement unit of the habit. Type M to return to the main menu.");
            string measureUnit = Console.ReadLine();

            if (measureUnit == "M") GetUserInput();

            while (Int32.TryParse(measureUnit, out _))
            {
                Console.WriteLine("You inserted a number instead of a measurement unit. Type M to return to the main menu.");
                measureUnit = Console.ReadLine();
                if (measureUnit == "M") GetUserInput();

            }

            return measureUnit;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "M") GetUserInput();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Type M to return to the main menu or try again.\n\n");
                numberInput = Console.ReadLine();
                if (numberInput == "M") GetUserInput();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }

        private static void Insert()
        {
            Console.Clear();

            string habit = GetHabitInput();
            string date = GetDateInput();
            string measure = GetMeasureUnit();
            int quantity = GetNumberInput("Please type the quantity. Type M to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO habits(HabitName, Date, MeasurementUnit, Quantity) VALUES('{habit}', '{date}', '{measure}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                Console.Clear();

                GetUserInput();
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT * FROM habits";

                List<Habit> Habits = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Habits.Add(new Habit
                        {
                            Id = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            MeasurementUnit = reader.GetString(3),
                            Quantity = reader.GetInt32(4)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                foreach (var habit in Habits)
                {
                    Console.WriteLine($"{habit.Id} - {habit.HabitName} - {habit.Date.ToString("dd-MMM-yyyy")} - {habit.Quantity} {habit.MeasurementUnit}");

                }


            }
        }
        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            int recordId = GetNumberInput("Type the ID of the record you want to delete. Type M to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM habits WHERE Id = {recordId}";

                var deletedRows = tableCmd.ExecuteNonQuery();

                if (deletedRows == 0)
                {
                    Console.WriteLine($"Record with ID {recordId} does not exist.");
                    Delete();
                }

                connection.Close();
            }

            GetUserInput();
        }

        internal static void Update()
        {
            GetAllRecords();

            int recordId = GetNumberInput("Type the ID of the record you would like to update. Type M to return to the main menu.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = {recordId})";

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine("Record with ID {recordId} does not exist.");
                    connection.Close();
                    Update();
                }

                string habit = GetHabitInput();

                string date = GetDateInput();

                string measure = GetMeasureUnit();

                int quantity = GetNumberInput("Please type the quantity.");

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"UPDATE habits SET HabitName = '{habit}', Date = '{date}', MeasurementUnit = '{measure}', Quantity = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                GetUserInput();

            }
        }

        static void ReportFunctionality()
        {
            Console.Clear();
            int userInput = GetNumberInput("\n1: See the habits you've been most consistent with \n2: See the habits you've been the least consistent with");

            if (userInput == 1)
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    Console.Clear();
                    connection.Open();

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"SELECT HabitName, COUNT(*) AS 'Count' FROM Habits GROUP BY HabitName HAVING COUNT(*) > 1 ORDER BY Id DESC";

                    tableCmd.ExecuteNonQuery();

                    List<Habit> Habits = new();

                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Habits.Add(new Habit
                            {
                                HabitName = reader.GetString(0),
                                Count = reader.GetInt32(1)
                            });
                        }
                    }
                    var consistentHabits_ = Habits.Max(h => h.Count);

                    IEnumerable<String> ConsistentHabits = Habits.Where(h => h.Count == consistentHabits_).Select(h => h.HabitName);

                    if (ConsistentHabits.Count() == 1)
                    {
                        Console.WriteLine($"The habit you were most consistent with was '{ConsistentHabits.First()}' with a {consistentHabits_}-day streak.");
                    }
                    else if (ConsistentHabits.Count() > 1)
                    {
                        string cHabitString = string.Join(", ", ConsistentHabits);
                        Console.WriteLine($"The habits you were most consistent with were: '{cHabitString}' with a {consistentHabits_}-day streak.");
                    }
                    connection.Close();

                    GetUserInput();
                }
            }

            if (userInput == 2)
            {
                Console.Clear();
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"SELECT HabitName, COUNT(*) AS 'Count' FROM Habits GROUP BY HabitName HAVING COUNT(*) >= 1 ORDER BY Id DESC";

                    tableCmd.ExecuteNonQuery();

                    List<Habit> Habits = new();

                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Habits.Add(new Habit
                            {
                                HabitName = reader.GetString(0),
                                Count = reader.GetInt32(1)
                            });
                        }
                    }
                    var leastConsistentHabits_ = Habits.Min(h => h.Count);

                    IEnumerable<String> leastConsistentHabits = Habits.Where(h => h.Count == leastConsistentHabits_).Select(h => h.HabitName);

                    if (leastConsistentHabits.Count() == 1)
                    {
                        Console.WriteLine($"The habit you were the least consistent with was '{leastConsistentHabits.First()}' with a {leastConsistentHabits_}-day streak.");
                    }
                    else if (leastConsistentHabits.Count() > 1)
                    {
                        string cHabitString = string.Join(", ", leastConsistentHabits);
                        Console.WriteLine($"The habits you were the least consistent with were '{cHabitString}' with a {leastConsistentHabits_}-day streak.");
                    }
                    connection.Close();

                    GetUserInput();
                }

            }
        }

        class Habit
        {
            public int Id { get; set; }
            public string HabitName { get; set; }

            public DateTime Date { get; set; }

            public string MeasurementUnit { get; set; }

            public int Quantity { get; set; }

            public int Count { get; set; }

        }
    }
}
