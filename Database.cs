using Microsoft.Data.Sqlite;
using System.Globalization;

namespace habit_logger;

class Database
{
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
            DataSeeder.InsertArbitraryData();
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

            connection.Close();
        }

        return habits;
    }

    public static void AddHabit(string habitName, string unitName)
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO habit(Name, Unit)
                VALUES(@HabitName, @Unit)";
            cmd.Parameters.Add("@HabitName", SqliteType.Text).Value = habitName;
            cmd.Parameters.Add("@Unit", SqliteType.Text).Value = unitName;
            cmd.ExecuteNonQuery();

            connection.Close();
        }

    }

    public static void DeleteRecord(int recordId)
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"DELETE FROM habit_records WHERE Id = @RecordId;";
            cmd.Parameters.Add("@RecordId", SqliteType.Integer).Value = recordId;
            connection.Close();
        }
    }

    public static List<string> GetAllRecords()
    {
        Console.Clear();
        List<string> records = new List<string>();

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
                DateTime date = DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", new CultureInfo("en-US"));
                records.Add($"{reader.GetInt32(0)}. {reader.GetString(1)} - {reader.GetInt32(2)} {reader.GetString(3)} at {date:dd-MM-yy}");
            }
            connection.Close();
        }

        return records;
    }

    public static void InsertRecord(int habitId, string date, int quantity)
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                $"INSERT INTO habit_records(HabitId, Date, Quantity) VALUES(@HabitId, @Date, @Quantity)";
            cmd.Parameters.Add("@HabitId", SqliteType.Integer).Value = habitId;
            cmd.Parameters.Add("@Date", SqliteType.Text).Value = date;
            cmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static bool RecordExists(int recordId)
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM habit_records WHERE id = @RecordId)";
            checkCmd.Parameters.Add("@RecordId", SqliteType.Integer).Value = recordId;
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }
            
            connection.Close();
        }
        return true;
    }

    public static void UpdateRecord(int recordId, string date, int quantity)
    {
        using (var connection = new SqliteConnection(Constants.ConnectionString))
        {
            connection.Open();

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = $"UPDATE habit_records SET Date = @Date, Quantity = @Quantity WHERE Id = @RecordId";
            updateCmd.Parameters.Add("@Date", SqliteType.Text).Value = date;
            updateCmd.Parameters.Add("@Quantity", SqliteType.Integer).Value = quantity;
            updateCmd.Parameters.Add("@RecordId", SqliteType.Text).Value = recordId;
            updateCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static Dictionary<string, object> GenerateReport(Habit habit)
    {
        var habitStats = new Dictionary<string, object>();

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
                if (reader.GetInt32(reader.GetOrdinal("RecordCount")) > 0)
                {
                    habitStats = new Dictionary<string, object>
                    {
                        { "Unit", habit.Unit },
                        { "TotalQuantity", reader.GetInt32(reader.GetOrdinal("TotalQuantity")) },
                        { "RecordCount", reader.GetInt32(reader.GetOrdinal("RecordCount")) },
                        { "AverageQuantity", reader.GetInt32(reader.GetOrdinal("AverageQuantity")) },
                        { "MinQuantity", reader.GetInt32(reader.GetOrdinal("MinQuantity")) },
                        { "MaxQuantity", reader.GetInt32(reader.GetOrdinal("MaxQuantity")) },
                        { "FirstRecord", reader.GetString(reader.GetOrdinal("FirstRecord")) },
                        { "LastRecord", reader.GetString(reader.GetOrdinal("LastRecord")) }
                    };
                }
            }
            connection.Close();
        }
        return habitStats;
    }
}