using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;

namespace HabitTracker
{
    internal class DataAccess
    {
        private string _connectionString;

        public DataAccess()
        {
            _connectionString = @"Data Source=habitTrackerDB.db";
        }

        public void CreateTables()
        {
            string sqlCreateHabitsTable =
                @"CREATE TABLE IF NOT EXISTS habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                    Name TEXT NOT NULL UNIQUE,
                    UNIT TEXT NOT NULL)";

            string sqlCreateHabitRecordsTable =
                @"CREATE TABLE IF NOT EXISTS habit_records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                    HabitId INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES habits (Id) ON DELETE CASCADE)";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand cmd1 = new SqliteCommand(sqlCreateHabitsTable, connection);
                cmd1.ExecuteNonQuery();

                InitializeDefaultHabits();

                SqliteCommand cmd2 = new SqliteCommand(sqlCreateHabitRecordsTable, connection);
                cmd2.ExecuteNonQuery();

                GenerateTestRecords();
                
                connection.Close();
            }
        }

        public void InitializeDefaultHabits()
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string sqlCheckCommand = @"
                    SELECT COUNT(*) FROM habits
                    WHERE Name IN('Reading', 'Running', 'Coding', 'Drinking water')";

                SqliteCommand checkCmd = new SqliteCommand(sqlCheckCommand, connection);
                int checkCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkCount == 0)
                {
                    string insertQuery = @"
                        INSERT INTO habits(Name, Unit)
                        VALUES
                            ('Reading', 'pages'),
                            ('Running', 'kms'),
                            ('Walking', 'steps'),
                            ('Coding', 'hrs'),
                            ('Drinking water', 'glasses')";

                    SqliteCommand insertCommand = new SqliteCommand(insertQuery, connection);
                    insertCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void InsertRecord()
        {
            Habit selectedHabit = SelectHabit();
            if (selectedHabit == null) return;

            string date = Helpers.GetDateInput();
            int quantity = Helpers.GetNumberInput($"\nPlease insert a number of {selectedHabit.Unit} (no decimals allowed):");

            string sqlCommand = "INSERT INTO habit_records(HabitId, Date, Quantity) VALUES(@habitId, @date, @quantity)";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand cmd = new SqliteCommand(sqlCommand, connection);
                cmd.Parameters.AddWithValue("@habitId", selectedHabit.Id);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"Record for {selectedHabit.Name} added successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public void SelectAllRecords()
        {
            string sqlCommand =
                @"SELECT habit_records.id, habits.Name, habit_records.Date, habit_records.Quantity, habits.Unit
                FROM habit_records
                JOIN  habits ON habits.Id = habit_records.HabitId
                ORDER BY habits.name";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand cmd = new SqliteCommand(sqlCommand, connection);

                List<HabitRecord> tableData = new List<HabitRecord>();
                SqliteDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new HabitRecord()
                        {
                            Id = reader.GetInt32(0),
                            HabitName = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd.MM.yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Unit = reader.GetString(4)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No data found. Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                connection.Close();

                Console.WriteLine("=== ALL HABITS RECORDS ===\n");
                Console.WriteLine($"{"ID", -5} {"Habit", -20} {"Date", -12} {"Quantity", -10} {"Unit", -10}");
                Console.WriteLine(new string('-', 60));

                foreach (var record in tableData)
                {
                    Console.WriteLine($"{record.Id, -5} {record.HabitName, -20} {record.Date:dd.MM.yy}     {record.Quantity, -10} {record.Unit, -10}");
                }
                Console.WriteLine(new string('-', 60));
                Console.ReadKey();
            }
        }

        public void UpdateRecord()
        {
            Console.Clear();
            SelectAllRecords();

            int recordId = Helpers.GetNumberInput("\nPlease enter the ID of the record you want to update or type \"0\" to return to Main Menu.");
            if (recordId == 0) return;

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string checkCommand =
                    @"SELECT habit_records.Id, habits.Name, habits.Unit
                    FROM habit_records
                    JOIN habits ON habit_records.HabitId = habits.Id
                    WHERE habit_records.Id = @recordId";

                SqliteCommand checkCmd = new SqliteCommand(checkCommand, connection);
                checkCmd.Parameters.AddWithValue("@recordId", recordId);
                SqliteDataReader reader = checkCmd.ExecuteReader();

                if (!reader.Read())
                {
                    Console.WriteLine($"\nRecord with Id {recordId} doesn't exist. Press any key to continue...\n");
                    reader.Close();
                    connection.Close();
                    Console.ReadKey();
                    return;
                }

                string habitName = reader.GetString(1);
                string unit = reader.GetString(2);
                reader.Close();

                Console.WriteLine($"\nUpdating record for habit: *{habitName}*");

                string date = Helpers.GetDateInput();
                int quantity = Helpers.GetNumberInput($"\nPlease insert a number of {unit} (no decimals allowed):");

                string updateCommand = "UPDATE habit_records SET Date = @date, Quantity = @quantity WHERE Id = @recordId";
                SqliteCommand updateCmd = new SqliteCommand(updateCommand, connection);
                updateCmd.Parameters.AddWithValue("@date", date);
                updateCmd.Parameters.AddWithValue("@quantity", quantity);
                updateCmd.Parameters.AddWithValue("@recordId", recordId);
                updateCmd.ExecuteNonQuery();
                connection.Close();

                Console.WriteLine("\nRecord updated successfully! Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void DeleteRecord()
        {
            Console.Clear();
            SelectAllRecords();

            int idRecord = Helpers.GetNumberInput("\n\nPlease enter the ID of the record you want to delete. Enter \"0\" to return to Main Menu.");

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"DELETE FROM habit_records WHERE Id = @idRecord";
                cmd.Parameters.AddWithValue("@idRecord", idRecord);
                int rowCount =  cmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with the Id {idRecord} doesn't exist.\n");
                    DeleteRecord();
                }

                Console.WriteLine($"\n\nRecord with the Id {idRecord} was deleted. Press any key to continue...");

                connection.Close();
                Console.ReadKey();
            }
        }

        public void AddHabit()
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW HABIT ===\n");

            Console.WriteLine("Enter habit name or type 0 to return to Main Menu:\n");
            string habitName = Console.ReadLine();

            while (string.IsNullOrEmpty(habitName))
            {
                Console.WriteLine("Habit name can't be empty!");
                habitName = Console.ReadLine();
            }

            Console.WriteLine("\nEnter unit of measurement (e.g., glasses, minutes, pages, etc.):\n");
            string unitType = Console.ReadLine();

            while (string.IsNullOrEmpty(unitType))
            {
                Console.WriteLine("Unit can't be empty!");
                unitType = Console.ReadLine();
            }

            string sqlInsertCommand = "INSERT INTO habits(Name, Unit) VALUES(@name, @unit)";


            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand cmd = new SqliteCommand(sqlInsertCommand, connection);
                cmd.Parameters.AddWithValue("@name", habitName);
                cmd.Parameters.AddWithValue("@unit", unitType);
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"\nHabit <{habitName}> has been added successfully. Press any key to continue...");
            Console.ReadKey();
        }

        public void DeleteHabit()
        {
            Console.Clear();
            ShowAllHabits();

            int habitId = Helpers.GetNumberInput("\nEnter the habit ID you want to delete or type \"0\" to return to Main Menu.");
            if (habitId == 0) return;

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string checkRecord = "SELECT COUNT(*) FROM habit_records WHERE HabitId = @habitId";
                SqliteCommand checkCmd = new SqliteCommand(checkRecord, connection);
                checkCmd.Parameters.AddWithValue("@habitId", habitId);
                int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (recordCount > 0)
                {
                    Console.WriteLine($"\nWarning: This habit has {recordCount} records. Deleteting this habit will alse delete all its records.");
                    Console.WriteLine("Are you sure? (y/n):");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation != "y" && confirmation != "yes")
                    {
                        Console.WriteLine("Operation cancelled.");
                        connection.Close();
                        Console.ReadKey();
                        return;
                    }
                }

                string deleteCommand = "DELETE FROM habits WHERE id = @habitId";
                SqliteCommand deleteCmd = new SqliteCommand(deleteCommand, connection);
                deleteCmd.Parameters.AddWithValue("@habitId", habitId);
            }
        }

        public List<Habit> GetAllHabits()
        {
            string sqlCommand = "SELECT * FROM habits ORDER BY Name";
            List<Habit> habits = new List<Habit>();

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand cmd = new SqliteCommand( sqlCommand, connection);
                SqliteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    habits.Add(new Habit()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Unit = reader.GetString(2)
                    });
                }

                connection.Close();
            }

            return habits;
        }

        public void ShowAllHabits()
        {
            Console.Clear();
            List<Habit> habits = GetAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits found.");
                return;
            }

            Console.WriteLine("=== Your Habits ===\n");

            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}. {habit.Name} {habit.Unit}");
            }

            Console.WriteLine("=====================\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public Habit SelectHabit()
        {
            List<Habit> habits = GetAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("No habits available. Add a habit first.");
                Console.ReadKey();
                return null;
            }

            Console.WriteLine("=== SELECT HABIT ===");

            foreach (var habit in habits)
            {
                Console.WriteLine($"{habit.Id}. {habit.Name} {habit.Unit}");
            }
            Console.WriteLine(new string('-', 20));

            int habitId = Helpers.GetNumberInput("Enetr habit ID or 0 to return to Main Menu:\n");

            if (habitId == 0) return null;
            
            Habit selectedHabit = null;
            foreach (var h in habits)
            {
                if (h.Id == habitId)
                {
                    selectedHabit = h;
                    break;
                }
            }

            if (selectedHabit == null)
            {
                Console.WriteLine("Invalid habit ID.");
                Console.ReadKey();
                return SelectHabit(); // ?
            }

            return selectedHabit;
        }

        public void GetHabitStatistics()
        {
            Habit selectedHabit = SelectHabit();
            if (selectedHabit == null) return;

            Console.Clear();
            Console.WriteLine($"=== STATISTICS FOR: {selectedHabit.Name.ToUpper()} ===");

            string sqlCommand =
                @"SELECT
	                COUNT(*) AS RecordCount,
	                SUM(Quantity) AS TotalQuantity,
	                AVG(Quantity) AS AverageQuantity,
	                MIN(Quantity)AS MinQuantity,
	                MAX(Quantity) AS MaxQuantity,
	                MIN(Date) AS FirstRecord,
	                MAX(Date) AS LastRecord
                FROM habit_records
                WHERE HabitId = @habitId";

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand cmd = new SqliteCommand(sqlCommand, connection);
                cmd.Parameters.AddWithValue("@habitId", selectedHabit.Id);
                SqliteDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int recordCount = reader.GetInt32("RecordCount");

                    if (recordCount == 0)
                    {
                        Console.WriteLine("No records found for this habit.");
                    }
                    else
                    {
                        int totalQuantity = reader.GetInt32("TotalQuantity");
                        double averageQuantity = reader.GetFloat("AverageQuantity");
                        int minQuantity = reader.GetInt32("MinQuantity");
                        int maxQuantity = reader.GetInt32("MaxQuantity");
                        string firstRecord = reader.GetString("FirstRecord");
                        string lastRecord = reader.GetString("LastRecord");
                        //DateTime firstRecord = DateTime.ParseExact(reader.GetString("FirstRecord"), "dd.MM.yy", new CultureInfo("en-US"));
                        //DateTime lastRecord = DateTime.ParseExact(reader.GetString("LastRecord"), "dd.MM.yy", new CultureInfo("en-US"));

                        Console.WriteLine($"Total {selectedHabit.Unit}: {totalQuantity}");
                        Console.WriteLine($"Total record: {recordCount}");
                        Console.WriteLine($"Average per record: {averageQuantity:F1} {selectedHabit.Unit}");
                        Console.WriteLine($"Minimum: {minQuantity} {selectedHabit.Unit}");
                        Console.WriteLine($"Maximum: {maxQuantity} {selectedHabit.Unit}");
                        Console.WriteLine($"First record: {firstRecord}");
                        Console.WriteLine($"Last record: {lastRecord}");
                    }
                }
                reader.Close();
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        public void GenerateTestRecords()
        {
            List<Habit> habits = GetAllHabits();
            Random random = new Random();
            DateTime startDate = DateTime.Now.AddDays(-100);

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string checkCommand = "SELECT COUNT(*) FROM habit_records";
                SqliteCommand checkCmd = new SqliteCommand(checkCommand, connection);
                int existingRecords = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (existingRecords > 0)
                {
                    connection.Close();
                    return;
                }

                foreach (var habit in habits)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        DateTime randomDate = startDate.AddDays(random.Next(0, 101));
                        string dateString = randomDate.ToString("dd.MM.yy");
                        int quantity = GenerateRandomQuantity(habit.Name, random);

                        string sqlCommand =
                            @"INSERT INTO habit_records(HabitId, Date, Quantity)
                            VALUES(@habitId, @date, @quantity)";

                        SqliteCommand cmd = new SqliteCommand(sqlCommand, connection);
                        cmd.Parameters.AddWithValue("@habitId", habit.Id);
                        cmd.Parameters.AddWithValue("@date", dateString);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        private int GenerateRandomQuantity(string unit, Random random)
        {
            switch (unit.ToLower())
            {
                case "pages":
                    return random.Next(1, 101);
                case "kms":
                    return random.Next(1, 21);
                case "hrs":
                    return random.Next(1, 9);
                case "glasses":
                    return random.Next(1, 9);
                case "walking":
                    return random.Next(2000, 10001);
                default:
                    return random.Next(1, 11);
            }
        }
    }
}
