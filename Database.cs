using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using System.Globalization;

namespace habit_logger;

class Database
{
    static void InsertArbitraryData()
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            Console.WriteLine("running checkCmd");
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = @"
                SELECT COUNT(*) FROM habit
                Where Name In('Drinking Water', 'Coding', 'Running', 'Working Out')
            ";
            SqliteDataReader reader = checkCmd.ExecuteReader();

            if (reader.Read() && reader.GetInt32(0) == 0)
            {
                // generate generic habits
                string insertHabitQuery = @"
                    INSERT INTO habit(Name, Unit)
                    Values
                        ('Drinking Water', 'glasses'),
                        ('Coding', 'hrs'),
                        ('Running', 'kms'),
                        ('Working Out', 'hrs')";

                var insertHabitCmd = connection.CreateCommand();
                insertHabitCmd.CommandText = insertHabitQuery;
                Console.WriteLine("running insertHabitCmd");

                insertHabitCmd.ExecuteNonQuery();

                DateTime startDate = DateTime.Now.AddDays(-100);
                var habits = GetAllHabits();
                Random random = new Random();
                foreach (var habit in habits)
                {
                    // generate random data for those habits (we dont care about what the value actually is)
                    for (int i = 0; i < 20; i++)
                    {
                        DateTime randomDate = startDate.AddDays(random.Next(0, 101));
                        int quantity = random.Next(1, 1000);

                        string insertDataQuery = @"
                            INSERT INTO habit_records(HabitId, Date, Quantity)
                            VALUES(@HabitId, @Date, @Quantity)
                        ";

                        var insertDataCmd = connection.CreateCommand();
                        insertDataCmd.CommandText = insertDataQuery;
                        insertDataCmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habit.Id;
                        insertDataCmd.Parameters.Add("@Date", SqliteType.Text).Value = randomDate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
                        insertDataCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
                        insertDataCmd.ExecuteNonQuery();
                    }
                }

            }

            connection.Close();

        }
    }

    public static bool HasHabits()
    {
        int count = 0;
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(Name) FROM habit";
            count = Convert.ToInt32(cmd.ExecuteScalar());
        }

        return count > 0;
    }

    public static void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            // create initial tables for storage
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS habit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Unit TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS habit_records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Date Text NOT NULL,
                    Quantity INTEGER NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES habit (Id) ON DELETE CASCADE
                );";
            tableCmd.ExecuteNonQuery(); // no need to return anything
            connection.Close();
        }

        if (Constants.DebugMode)
        {
            InsertArbitraryData();
        }

        Helpers.GetUserMenuInput();
    }

    public static List<Habit> GetAllHabits()
    {
        List<Habit> habits = new List<Habit>();
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM habit;";


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
        }

        return habits;
    }

    private static bool IsValidHabitName(string name)
    {
        // This regex ensures that habit name:
        // > Starts with a letter
        // > Contains only letters
        // > No numbers, special symbols (except _ and -)
        return Regex.IsMatch(name, @"^[a-zA-Z][a-zA-Z _-]*$");
    }

    public static void AddNewHabit()
    {
        Console.Clear();

        Console.WriteLine("Currently logged habits:");
        Helpers.ListAllHabits();
        var habits = GetAllHabits();

        Console.WriteLine("Provide the table name for the new habit.");
        string newHabit = Console.ReadLine();

        while (!IsValidHabitName(newHabit) || habits.Any(h => h.Name.Contains(newHabit)))
        {
            Console.WriteLine("Habit name invalid or habit already exists.");
            newHabit = Console.ReadLine();
        }

        Console.WriteLine("Please provide the unit name for the new habit");
        string unit = Console.ReadLine();

        while (unit.Trim().Length == 0 || string.IsNullOrEmpty(unit))
        {
            Console.WriteLine("Unit name needs to be at least 1 letter long.");
            unit = Console.ReadLine();
        }

        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO habit(Name, Unit)
                VALUES(@HabitName, @Unit)";
            cmd.Parameters.Add("@HabitName", SqliteType.Text).Value = newHabit;
            cmd.Parameters.Add("@Unit", SqliteType.Text).Value = unit;
            cmd.ExecuteNonQuery();
            Console.WriteLine("New Habit Added.");
        }

    }

    public static void DeleteRecord()
    {
        if (!HasHabits())
        {
            Console.WriteLine("There are no habits to delete records from.");
            return;
        }
        Console.Clear();
        ShowAllRecords();

        var recordId = Helpers.GetNumberInput("Please type the Id of the record you want to delete or type 0 to go back to Main Menu");

        if (recordId == 0) return;

        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM habit_records WHERE Id = '{recordId}'";

            int rowCount = cmd.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exist.");
            }
            connection.Close();
        }
    }

    public static void ShowAllRecords(bool stopTerminal = false)
    {
        Console.Clear();
        if (!HasHabits())
        {
            Console.WriteLine("There are no habits to show records of.");
            return;
        }
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT hr.Id, h.Name, hr.Quantity, h.Unit, hr.Date
                FROM habit_records hr
                LEFT JOIN habit h ON hr.HabitId = h.Id;
            ";

            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // we need to format the date back to original format we asked for (yyyy-MM-dd) > (dd-MM-yy)
                DateTime date = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", new CultureInfo("en-US"));
                Console.WriteLine($"{reader.GetInt32(0)}. {reader.GetString(1)} - {reader.GetInt32(2)} {reader.GetString(3)} at {date:dd-MM-yy}");
            }
            connection.Close();
        }

        if (stopTerminal) Console.ReadKey();
    }

    public static void InsertNewRecord()
    {
        if (!HasHabits())
        {
            Console.WriteLine("There are no habits to insert into.");
            return;
        }

        Habit habit = Helpers.PickHabit();

        string date = Helpers.GetDateInput();
        if (date == "0") return;
        int quantity = Helpers.GetNumberInput();

        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                 $"INSERT INTO habit_records(HabitId, Date, Quantity) VALUES(@HabitId, @Date, @Quantity)";
            cmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habit.Id;
            cmd.Parameters.Add("@Date", SqliteType.Text).Value = date;
            cmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
            cmd.ExecuteNonQuery();
            Console.WriteLine("New record added successfully.");
            connection.Close();
        }
    }

    public static void UpdateRecord()
    {
        Console.Clear();
        if (!HasHabits())
        {
            Console.WriteLine("There are no habits to update results of.");
            return;
        }
        ShowAllRecords();

        var recordId = Helpers.GetNumberInput("Please type the Id of the record you want to update or type 0 to go back to Main Menu");
        if (recordId == -1) return;

        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM habit_records WHERE id = @RecordId)";
            checkCmd.Parameters.Add("@RecordId", SqliteType.Text).Value = recordId;
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"Record with Id {recordId} doesn't exist.");
                connection.Close();
                UpdateRecord();
                return;
            }

            string date = Helpers.GetDateInput();
            if (date == "0") return;
            int quantity = Helpers.GetNumberInput();

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE habit_records SET Date = @Date, Quantity = @Quantity WHERE Id = @RecordId";
            updateCmd.Parameters.Add("@Date", SqliteType.Text).Value = date;
            updateCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
            updateCmd.Parameters.Add("@RecordId", SqliteType.Text).Value = recordId;
            updateCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static void GenerateReport()
    {
        var habit = Helpers.PickHabit();

        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT 
	                COUNT(*) AS RecordCount,
	                SUM(Quantity) AS TotalQuantity,
	                AVG(Quantity) AS AverageQuantity,
	                MIN(Quantity)AS MinQuantity,
	                MAX(Quantity) AS MaxQuantity,
	                MIN(Date) AS FirstRecord,
	                MAX(Date) AS LastRecord
                FROM habit_records 
                WHERE HabitId = @HabitId;
            ";

            cmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habit.Id;

            SqliteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                if (reader.GetInt32(reader.GetOrdinal("RecordCount")) == 0)
                {
                    Console.WriteLine("No records found for this habit");
                }
                else
                {
                    Console.WriteLine(@$"
                        {habit.Unit} : {reader.GetInt32(reader.GetOrdinal("TotalQuantity"))}
                        Total Record: {reader.GetInt32(reader.GetOrdinal("RecordCount"))}
                        Average per record: {reader.GetInt32(reader.GetOrdinal("AverageQuantity"))} {habit.Unit}
                        Minimum: {reader.GetInt32(reader.GetOrdinal("MinQuantity"))}
                        Maximum: {reader.GetInt32(reader.GetOrdinal("MaxQuantity"))}
                        First record: {reader.GetString(reader.GetOrdinal("FirstRecord"))}
                        Last Record: {reader.GetString(reader.GetOrdinal("LastRecord"))}
                    ");

                    // prevent showing the menu
                    Console.ReadKey();
                }
            }
        }
    }
}