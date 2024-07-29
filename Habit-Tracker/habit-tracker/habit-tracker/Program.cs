using Microsoft.Data.Sqlite;
using System.Globalization;

namespace Habit_Tracker
{
    class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                    {
    
                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS user_habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Habit_name TEXT,
                        Units TEXT
                        )";

                    tableCmd.ExecuteNonQuery();
                }
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habits_info (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Quantity INTEGER,
                        Date TEXT,
                        Habits_id INTEGER,
                        FOREIGN KEY (Habits_id) REFERENCES user_habits (Id) ON DELETE CASCADE
                        )";

                    tableCmd.ExecuteNonQuery();
                }
            }

            SeedData();

            GetUserInput();
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("---------------------------------------------\n");

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
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        private static void GetAllRecords()
        {
            //Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"SELECT
                        user_habits.Id AS UserHabitId,
                        user_habits.Habit_name,
                        user_habits.Units,
                        habits_info.Id AS HabitsInfoId,
                        habits_info.Quantity,
                        habits_info.Date,
                        habits_info.Habits_id
                      FROM user_habits
                      INNER JOIN habits_info
                      ON UserHabitId = HabitsInfoId";

                List<UserHabits> userHabitsTable = new();
                List<HabitsInfo> habitsInfoTable = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userHabitsTable.Add(
                        new UserHabits
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("UserHabitId")),
                            HabitName = reader.GetString(reader.GetOrdinal("Habit_name")),
                            Units = reader.GetString(reader.GetOrdinal("Units"))
                        });

                        habitsInfoTable.Add(
                            new HabitsInfo
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("HabitsInfoId")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Date = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("Date")), "dd-MM-yyyy", new CultureInfo("en-US")),
                                HabitsId = reader.GetInt32(reader.GetOrdinal("Habits_id"))
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                foreach (var habit in userHabitsTable)
                {
                    Console.WriteLine($"\nID - {habit.Id}: Date\t\t{habit.HabitName} - {habit.Units}");

                    foreach (var info in habitsInfoTable)
                    {
                        if (info.Id == habit.Id)
                        {
                            Console.WriteLine($"\t{info.Date.ToString("dd-MM-yyyy")}\t\t{info.Quantity}");
                        }
                    }
                }
                Console.WriteLine("---------------------------------------------\n");
            }
        }
        private static void Insert()
        {
            string date = GetDateInput();

            string habit_name = GetStringInput("\nPlease enter the name of the habit you wish to add");

            string units = GetStringInput("\nPlease enter the units in which you would like to measure this habit. Ie, kilometers for running, cups for coffee, pages for books");

            int quantity = GetNumberInput("\nPlease insert the number of units you would like to add. I.e number of kilometers, number of cups, pages of a book.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "INSERT INTO user_habits (Habit_name, Units) VALUES (@HabitName, @Units)";
                    tableCmd.Parameters.AddWithValue("@HabitName", habit_name);
                    tableCmd.Parameters.AddWithValue("@Units", units);
                    tableCmd.ExecuteNonQuery();
                }

                long habitId;
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "SELECT last_insert_rowid()";
                    habitId = (long)tableCmd.ExecuteScalar();
                }

                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "INSERT INTO habits_info (Quantity, Date, Habits_id) VALUES (@Quantity, @Date, @HabitsId)";
                    tableCmd.Parameters.AddWithValue("@Quantity", quantity);
                    tableCmd.Parameters.AddWithValue("@Date", date);
                    tableCmd.Parameters.AddWithValue("@HabitsId", habitId);
                    tableCmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu.\n\n");

            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM user_habits WHERE Id = {recordId}";

                int rowCount = tableCmd.ExecuteNonQuery();


                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                        Delete();
                    }
                }

            Console.WriteLine($"\n\nRecord with Id{recordId} was deleted. \n\n");

            GetUserInput();
        }


        internal static void Update()
        {
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM user_habits WHERE Id = @RecordId)";
                checkCmd.Parameters.AddWithValue("@RecordId", recordId);
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    connection.Close();
                    Update();
                }

                string habit_name = GetStringInput("\nPlease enter the name of the updated habit");

                string units = GetStringInput("\nPlease enter the units in which you would like to measure the updated habit, i.e. hours, cups, kilometers");

                int quantity = GetNumberInput("\n\nPlease insert the updated measure of your choice, i.e. a number. (no decimals allowed)\n\n");
                string date = GetDateInput();

                var updateUserHabitsCmd = connection.CreateCommand();
                updateUserHabitsCmd.CommandText = $"UPDATE user_habits SET Habit_name = @HabitName, Units = @Units WHERE ID = @RecordId";
                updateUserHabitsCmd.Parameters.AddWithValue("@HabitName", habit_name);
                updateUserHabitsCmd.Parameters.AddWithValue("@Units", units);
                updateUserHabitsCmd.Parameters.AddWithValue("@RecordId", recordId);
                updateUserHabitsCmd.ExecuteNonQuery();

                var updateHabitsInfoCmd = connection.CreateCommand();
                updateHabitsInfoCmd.CommandText = $"UPDATE habits_info SET Quantity = @Quantity, DATE = @Date WHERE Habits_id = @RecordId";
                updateHabitsInfoCmd.Parameters.AddWithValue("@Quantity", quantity);
                updateHabitsInfoCmd.Parameters.AddWithValue("Date", date);
                updateHabitsInfoCmd.Parameters.AddWithValue("@RecordId", recordId);

                updateHabitsInfoCmd.ExecuteNonQuery();


                connection.Close();
            }

        }
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\n Please insert the date: (Format: dd-mm-yyyy). Type 0 to return to main menu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yyyy. Type 0 to return to main menu or try again:\n\n");
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

        
        internal static string GetStringInput(string message)
        {
            string userInput;
            Console.WriteLine(message);
            userInput = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userInput))
            {
                    Console.WriteLine("\n\nInvalid input. Try again.\n\n");
                    userInput = Console.ReadLine();
            }
          
            return userInput;
        }

        internal static void SeedData()
        {
            Random random = new Random();
            string[] habitNames = { "Running", "Cooking", "Sleeping", "Meditation", "Martial Arts", "Guitar", "Studying", "No Screen Time", "Reading" };
            string[] units = { "Miles", "Days", "Hours", "Minutes", "Classes", "Hours", "Hours", "Days", "Pages" };

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < 10; i++)
                {
                    var tableCmd = connection.CreateCommand();
                    string habitName = habitNames[random.Next(habitNames.Length)];
                    string unit = units[random.Next(units.Length)];

                    tableCmd.CommandText = "INSERT INTO user_habits (Habit_name, units) VALUES (@HabitName, @Units)";
                    tableCmd.Parameters.AddWithValue("@HabitName", habitName);
                    tableCmd.Parameters.AddWithValue("@Units", unit);
                    tableCmd.ExecuteNonQuery();

                    // Get last inserted id for foreign key
                    tableCmd.CommandText = "SELECT last_insert_rowid()";
                    long habitId = (long)tableCmd.ExecuteScalar();

                    int quantity = random.Next(1, 19);
                    DateTime date = DateTime.Now.AddDays(-random.Next(0, 31));
                    string formatDate = date.ToString("dd-MM-yyyy");
                  

                    tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = "INSERT INTO habits_info (Quantity, Date, Habits_id) VALUES (@Quantity, @Date, @HabitsId)";
                    tableCmd.Parameters.AddWithValue("Quantity", quantity);
                    tableCmd.Parameters.AddWithValue("Date", formatDate);
                    tableCmd.Parameters.AddWithValue("@HabitsId", habitId);
                    tableCmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Console.WriteLine("Seed data generated successfully.");
        }
    }

    public class UserHabits
    {
        public int Id { get; set; }
        public string HabitName { get; set; }

        public string Units { get; set; }
    }

    public class HabitsInfo
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public int HabitsId { get; set; }
    }
}
