using Microsoft.Data.Sqlite;
using System.Globalization;
namespace HabitTrackerApp
{
    internal class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";

        static void Main(string[] args)
        {
            CreateHabitTable();
            CreateHabitTrackerTable();
            PopulateHabitTable();
            PopulateHabitTrackerTable();

            MainMenu();
        }
        #region Database Initialization Methods
        private static void PopulateHabitTable()
        {
            if (TableEmpty("habits"))
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"INSERT INTO habits(name, unit_of_measurement) VALUES
                    ('running', 'km'),
                    ('reading', 'pages'),
                    ('drink water', 'ml'),
                    ('studying', 'hours')";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
        private static void PopulateHabitTrackerTable()
        {
            if (TableEmpty("habit_tracking"))
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @"INSERT INTO habit_tracking(habit_id, date, description, quantity) VALUES
                    (1, '05-05-24', 'morning jog', 5),
                    (1, '06-05-24', 'evening jog', 7),
                    (4, '07-05-24', 'programming', 3),
                    (4, '08-05-24', 'maths', 2),
                    (4, '17-02-24', 'programming session', 2),
                    (1, '16-09-24', 'evening jog', 6),
                    (4, '06-11-24', 'programming session', 1),
                    (4, '30-04-24', 'programming session', 1),
                    (2, '01-03-24', 'GoT', 35),
                    (1, '22-03-24', 'swimming laps', 1),
                    (4, '05-07-24', 'math practice', 2),
                    (2, '06-03-24', 'GoT', 61),
                    (1, '16-05-24', 'evening jog', 9),
                    (4, '24-08-24', 'programming session', 2),
                    (2, '05-07-24', 'GoT', 100),
                    (1, '13-07-24', 'evening jog', 4),
                    (3, '13-07-24', 'bottled water', 1000),
                    (3, '15-07-24', 'bottled water', 1000),
                    (3, '11-07-24', 'bottled water', 1000),
                    (3, '16-07-24', 'bottled water', 1000);";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
        private static void CreateHabitTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        habit_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT,
                        unit_of_measurement TEXT
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        private static void CreateHabitTrackerTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_tracking (
                        tracking_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        habit_id INTEGER,
                        date TEXT,
                        description TEXT,
                        quantity INTEGER,
                        FOREIGN KEY (habit_id) REFERENCES habits(habit_id)
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        #endregion
        #region User Interaction Methods
        static void MainMenu()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("Type 5 to Create new Habit.");
                Console.WriteLine("Type 6 to Delete a Habit.");
                Console.WriteLine("Type 7 to Update a Habit.");
                Console.WriteLine("Type 8 Generate a report.");
                Console.WriteLine("------------------------------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllHabits();
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

                    case "6":
                        DeleteHabit();
                        break;

                    case "7":
                        UpdateHabit();
                        break;

                    case "8":
                        GenerateReport();
                        break;
                    default:
                        Console.WriteLine("Invalid Command. Type 0 - 8.");
                        break;
                }
            }
        }
        private static void GenerateReport()
        {
            Console.Clear();
            GetAllHabits();
            int habitId = GetValidHabitId("Enter the ID of the habit you want to generate a report for:");
            Console.WriteLine("Enter the year you want a report for (YY):");
            string date = Console.ReadLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"SELECT SUM(ht.quantity) AS total
                    FROM habit_tracking ht
                    WHERE ht.habit_id = @HabitId
                    AND SUBSTR(ht.date, 7, 2) = @Date;";
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);
                tableCmd.Parameters.AddWithValue("@Date", date);

                switch (habitId)
                {
                    case 1:
                        Console.WriteLine($"Total km ran for the year: {tableCmd.ExecuteScalar()}km");
                        break;
                    case 2:
                        Console.WriteLine($"Total pages read for the year: {tableCmd.ExecuteScalar()} pages");
                        break;

                    case 3:
                        Console.WriteLine($"Total water drunk for the year: {tableCmd.ExecuteScalar()}ml");
                        break;

                    case 4:
                        Console.WriteLine($"Total hours studied for the year: {tableCmd.ExecuteScalar()} hours");
                        break;

                    default:
                        Console.WriteLine($"Total for the year for habit {habitId}: {tableCmd.ExecuteScalar()}");
                        break;
                }
                connection.Close();

            }
        }
        private static void UpdateHabit()
        {
            Console.Clear();
            GetAllHabits();
            int habitId = GetValidHabitId("Enter the ID of the habit you want to update:");

            string habitName = TableUserInput("Enter the name of the habit:");
            string unitOfMeasurement = TableUserInput("Enter the unit of measurement:");


            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"UPDATE habits SET name = @Name, unit_of_measurement = @UnitOfMeasurement WHERE HabitId = @Habit_id";
                tableCmd.Parameters.AddWithValue("@Name", habitName);
                tableCmd.Parameters.AddWithValue("@UnitOfMeasurement", unitOfMeasurement);
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);

                tableCmd.ExecuteNonQuery();
                connection.Close();

            }

        }
        private static void DeleteHabit()
        {
            Console.Clear();
            GetAllHabits();
            int habitId = GetValidHabitId("Enter the ID of the habit you want to delete:");

            if (!CanDeleteHabit(habitId))
            {
                Console.WriteLine("There are related records in habit_tracking. Please delete them first.");
                return;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"DELETE FROM habits WHERE habit_id = @HabitId";
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        private static void CreateNewHabit()
        {
            string habitName = TableUserInput("Enter the name of the habit:");
            string unitOfMeasurement = TableUserInput("Enter the unit of measurement:");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"INSERT INTO habits(name, unit_of_measurement) VALUES (@Name, @UnitOfMeasurement)";
                tableCmd.Parameters.AddWithValue("@Name", habitName);
                tableCmd.Parameters.AddWithValue("@UnitOfMeasurement", unitOfMeasurement);
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static void Delete()
        {
            Console.Clear();
            GetHabitTrackingRecords();

            var recordId = GetNumberInput($"\n\nPlease type the Id of the record you want to delete or type 0 to return to the Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"Delete from habit_tracking WHERE tracking_id = @RecordID";
                tableCmd.Parameters.AddWithValue("@RecordID", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with Id {recordId} doesn't exist.");
                    Console.ReadKey();
                    Delete();
                }
            }

            Console.WriteLine($"Record with Id {recordId} was deleted.");
            Console.ReadKey();

            MainMenu();
        }
        internal static void Update()
        {
            Console.Clear();
            GetHabitTrackingRecords();

            var recordId = GetNumberInput($"\n\nPlease type the Id of the record you want to update or type 0 to return to the Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_tracking WHERE tracking_id = @RecordID)";
                checkCmd.Parameters.AddWithValue("@RecordID", recordId);

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with ID {recordId} doesn't exist.\n\n");
                    Console.ReadKey();
                    connection.Close();
                    Update();
                }

                GetAllHabits();
                int habitId;
                do
                {
                    habitId = GetNumberInput("\n\n Please enter the habit Id:\n\n");

                } while (!HabitExists(habitId));

                string date = CurrentDate();
                string description = TableUserInput("Enter the description:");
                int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed). Type 0 to return to main menu. \n\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE habit_tracking SET habit_id = @HabitId, date = @Date, description = @Description, quantity = @Quantity WHERE tracking_id = @RecordID";
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);
                tableCmd.Parameters.AddWithValue("@Date", date);
                tableCmd.Parameters.AddWithValue("@Description", description);
                tableCmd.Parameters.AddWithValue("@Quantity", quantity);
                tableCmd.Parameters.AddWithValue("@RecordID", recordId);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        #endregion
        #region Helper methods
        private static int GetValidHabitId(string prompt)
        {
            int habitId;
            do
            {
                habitId = GetNumberInput(prompt);
            } while (!HabitExists(habitId));
            return habitId;
        }
        private static bool TableEmpty(string table)
        {
            int count;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"SELECT COUNT(*) AS count FROM {table}";

                count = Convert.ToInt32(tableCmd.ExecuteScalar());
                connection.Close();
            }
            return count == 0;
        }
        private static bool CanDeleteHabit(int habitId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"SELECT COUNT(*) FROM habit_tracking WHERE habit_id = @HabitId";

                tableCmd.Parameters.AddWithValue("@HabitId", habitId);

                int count = Convert.ToInt32(tableCmd.ExecuteScalar());
                connection.Close();

                return count == 0;
            }
        }
        private static string TableUserInput(string msg)
        {
            string userInput = "empty_table_name";
            do
            {
                Console.WriteLine(msg);
                userInput = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(userInput) || userInput == "empty_table_name")
                {
                    Console.WriteLine("Invalid input. Please enter a non-empty name that is not 'empty_table_name'.");
                }

            } while (string.IsNullOrWhiteSpace(userInput) || userInput == "empty_table_name");
            return userInput;
        }
        private static bool HabitExists(int habitId)
        {
            bool exists = false;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT COUNT(*) FROM habits WHERE habit_id = @HabitId";
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);

                int habitCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                if (habitCount == 0)
                {
                    Console.WriteLine("The specified habit Id doesnt exist.");
                }
                else
                {
                    exists = true;
                }

                connection.Close();
            }

            return exists;
        }
        private static void Insert()
        {
            Console.Clear();
            GetAllHabits();
            int habitId = GetValidHabitId("Please enter the habit Id:");

            string date = CurrentDate();
            string description = TableUserInput("Enter the description:");
            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed). Type 0 to return to main menu. \n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "INSERT INTO habit_tracking(habit_id, date, description, quantity) VALUES(@HabitId, @Date, @Description, @Quantity)";
                tableCmd.Parameters.AddWithValue("@HabitId", habitId);
                tableCmd.Parameters.AddWithValue("@Date", date);
                tableCmd.Parameters.AddWithValue("@Description", description);
                tableCmd.Parameters.AddWithValue("@Quantity", quantity);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        private static string CurrentDate()
        {
            string date = DateTime.Now.ToString("dd-MM-yy");
            bool validInput = false;
            while (validInput == false)
            {
                Console.WriteLine("Use current date? Y/N");
                string userInput = Console.ReadLine().ToUpper();
                switch (userInput)
                {
                    case "Y":
                        //date already set in intialization
                        validInput = true;
                        break;
                    case "N":
                        date = GetDateInput();
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Enter 'Y' to use current date or 'N' to manually input a date");
                        break;
                }
            }

            return date;
        }
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");
            string dateInput = Console.ReadLine();

            if (dateInput == "0") MainMenu();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine($"\n\nInvalid date. Please use the format: (dd-mm-yy):\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }
        internal static int GetNumberInput(string msg)
        {
            Console.WriteLine(msg);
            string numberInput = Console.ReadLine();

            if (numberInput == "0") MainMenu();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number.");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
        internal static void GetAllHabits()
        {
            GetHabitTrackingRecords();
            Console.WriteLine("Habits:");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM habits";

                List<Habits> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Habits
                        {
                            HabitId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            UnitOfMeasurement = reader.GetString(2)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();

                Console.WriteLine("-------------------------------");
                foreach (var item in tableData)
                {
                    Console.WriteLine($"Habit ID: {item.HabitId} - Name: {item.Name} - Unit of Measurement: {item.UnitOfMeasurement}");
                }
                Console.WriteLine("-------------------------------");
            }
        }
        internal static void GetHabitTrackingRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM habit_tracking";

                List<HabitTracking> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new HabitTracking
                        {
                            TrackingId = reader.GetInt32(0),
                            HabitId = reader.GetInt32(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Description = reader.GetString(3),
                            Quantity = reader.GetInt32(4)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                connection.Close();

                Console.WriteLine("-------------------------------");
                foreach (var item in tableData)
                {
                    Console.WriteLine($"{item.TrackingId} - Habit ID: {item.HabitId} - Date: {item.Date.ToString("dd-MMM-yyyy")} - Description: {item.Description} - Quantity: {item.Quantity}");
                }
                Console.WriteLine("-------------------------------");
            }
        }
        #endregion
    }
}
public class Habits
{
    public int HabitId { get; set; }
    public string Name { get; set; }
    public string UnitOfMeasurement { get; set; }
}
public class HabitTracking
{
    public int TrackingId { get; set; }
    public int HabitId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
}