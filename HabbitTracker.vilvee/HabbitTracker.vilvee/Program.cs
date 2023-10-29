using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabbitTracker.vilvee
{
    internal class Program
    {

        static string databaseConnection = @"Data Source=habitTracker.db";

        static void Main(string[] args)
        {
            GetUserInput();
        }

        private static void GetUserInput()
        {
            Console.Clear();
            string habitName;
            bool closeApp = false;
            string message;
            while (closeApp == false)
            {
                string menu = @"

MAIN MENU

[1] CREATE NEW HABIT

[2] GET ALL HABITS

[3] DELETE HABIT

[4] INSERT RECORD

[5] UPDATE RECORD

[6] DELETE RECORD

[0] CLOSE

";
                Console.WriteLine(menu);

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Console.Clear();
                        CreateNewHabit();
                        break;
                    case "2":
                        Console.Clear();
                        GetAllHabits();
                        break;
                    case "3":
                        Console.Clear();
                        DeleteHabit();
                        break;
                    case "4":
                        Console.Clear();
                        InsertRecord(GetTable("\n\nEnter the name of the habit you want to insert a record into?\n\n"));
                        break;
                    case "5":
                        Console.Clear();
                        UpdateRecord(GetTable("\n\nEnter the name of the habit you want to update?\n\n"));
                        break;
                    case "6":
                        Console.Clear();
                        DeleteRecord(GetTable("\n\nEnter the name of the habit you want to delete a record from.\n\n"));
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }

        /// <summary>
        /// Menu 1
        /// </summary>
        private static void CreateNewHabit()
        {
            Console.WriteLine("\n\nPlease write the name of the new habit.\nType 0 to return to Main Menu");
            string habitName = Console.ReadLine();

            if (habitName == "0") GetUserInput();

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();

                //create habit
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $@"CREATE TABLE IF NOT EXISTS {habitName}(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Quantity INTEGER
                            )";
                tableCmd.ExecuteNonQuery();
                connection.Close();

                //populate new habit
                InsertRecord(habitName);
                Console.WriteLine($"Habit {habitName} created");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Menu 2
        /// </summary>
        private static void GetAllHabits()
        {
            
            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"SELECT name FROM sqlite_master 
                    WHERE type = 'table'
                    ORDER BY 1";
                List<string> records = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(0) == "sqlite_sequence")
                        {
                            continue;
                        }
                        records.Add(reader.GetString(0));
                    }
                }

                else
                {
                    Console.WriteLine("No habits found");
                }

                connection.Close();

                Console.WriteLine("---------------------------------------------------");

                foreach (var record in records)
                {
                    Console.WriteLine($"{record}");
                    GetAllRecords(record);
                }
                Console.WriteLine("---------------------------------------------------\n");
            }
        }

        /// <summary>
        /// Menu 3
        /// </summary>
        private static void DeleteHabit()
        {

            var message = "\n\nWhich habit do you want to delete? Enter the row number.\n\n";

            var habitName = GetTable(message);

            if (habitName != null)
            {
                using (var connection = new SqliteConnection(databaseConnection))
                {
                    connection.Open();

                    //create habit
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"DROP TABLE IF EXISTS {habitName}";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();

                    Console.WriteLine($"Habit {habitName} deleted");
                    Console.ReadKey();
                }
            }

        }

        /// <summary>
        /// Menu 4
        /// </summary>
        private static void InsertRecord(string habitName)
        {
            if(habitName != null)
            {
                var date = GetDateInput();

                var quantity = GetNumberInput("\n\nPlease insert number quantity");

                using (var connection = new SqliteConnection(databaseConnection))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"INSERT INTO {habitName} (date, quantity) VALUES('{date}', '{quantity}')";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Menu 5
        /// </summary>
        private static void UpdateRecord(string habitName)
        {

            GetAllRecords(habitName);
            var recordId = GetNumberInput("\n\nWhich row do you want to update? Enter the row number.\n\n");

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();

                //check if record exists
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {habitName} WHERE Id = {recordId})";
                var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                    connection.Close();
                    UpdateRecord(habitName);
                }

                //update record
                var date = GetDateInput();
                var quantity = GetNumberInput("\n\nPlease insert number of glasses");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE {habitName} SET date = '{date}', quantity = {quantity} WHERE Id = {recordId} ";
                tableCmd.ExecuteNonQuery();
                Console.WriteLine($"\n\nRecord {recordId} was successfully updated.\n\n");
                connection.Close();
            }

            GetUserInput();
        }

        /// <summary>
        /// Menu 6
        /// </summary>
        private static void GetAllRecords(string habitName)
        {

            using (var connection = new SqliteConnection(databaseConnection))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM {habitName}";
                List<Habit> records = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        Habit habit = new Habit();

                        if (!reader.IsDBNull(0))
                            habit.Id = reader.GetInt32(0);

                        if (!reader.IsDBNull(1))
                            habit.Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yyyy", new CultureInfo("en-US"));

                        if (!reader.IsDBNull(2))
                            habit.Quantity = reader.GetInt32(2);

                        records.Add(habit);
                    }

                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("-----------------------------------------\n");

                foreach (var record in records)
                {
                    Console.WriteLine($"{record.Id} - {record.Date:dd-mm-yyy} - Quantity: {record.Quantity}");
                }
                Console.WriteLine("-----------------------------------------\n");
            }

        }

        /// <summary>
        /// Menu 7
        /// </summary>
        private static void DeleteRecord(string habitName)
        {

            GetAllHabits();

            if(habitName != null)
            {
                var recordId = GetNumberInput("\n\nWhich row do you want to delete? Enter the row number.\n\n");

                using (var connection = new SqliteConnection(databaseConnection))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"DELETE FROM {habitName} WHERE Id = '{recordId}'";
                    var rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nRecord with Id {recordId} does not exist.\n\n");
                        DeleteRecord(habitName);
                    }

                    Console.WriteLine($"\n\nRecord {recordId} was deleted.\n\n");
                    connection.Close();
                }

                GetUserInput();
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert date (Format: dd-mm-yyyy.\nType 0 to return to Main Menu");
            var dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-mm-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid format. (dd-mm-yyyy)");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            var userInput = Convert.ToInt32(Console.ReadLine());
            if (userInput == 0) GetUserInput();
            return userInput;
        }


        internal static string GetTable(string message)
        {
            GetAllHabits();
           
            Console.WriteLine(message);
            var habitName = Console.ReadLine();
            try
            {
                using (var connection = new SqliteConnection(databaseConnection))
                {
                    connection.Open();

                    //check if record exists
                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText =
                        $"SELECT EXISTS(SELECT * FROM {habitName})";
                    var checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\n\nThis habit does not exist\n\n");
                        connection.Close();
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("This habit does not exist");
                return null;
            }

            return habitName;
            
        }

    }
}

