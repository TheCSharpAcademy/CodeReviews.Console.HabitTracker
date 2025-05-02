using habitTracker;
using Microsoft.Data.Sqlite;
using System.Globalization;


namespace HabitTracker
{
    public class HabitDatabase
    {
        static string connectionString = @"Data Source=habitTracker.db";

        public void CreateTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {

                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Unit TEXT
                        )";

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habits_records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        HabitId INTEGER,
                        Quantity INTEGER,
                        Date TEXT,
                        FOREIGN KEY (HabitId) REFERENCES habits(Id)
                        )";

                connection.Close();

            }
        }
        public void SeedData()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT COUNT(*) FROM habits";
                long count = (long)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    var insertCmd = connection.CreateCommand();
                    insertCmd.CommandText = @"
                INSERT INTO habits (HabitType, Unit)
                VALUES 
                ('Drink Water', 'glasses'),
                ('Read Book', 'minutes'),
                ('Exercise', 'minutes')";
                    insertCmd.ExecuteNonQuery();
                    var insertCmd2 = connection.CreateCommand();
                    insertCmd.CommandText = @"
                INSERT INTO habits_records (HabitId, Date, Quantity)
                VALUES 
                (1, '01-01-23', 2),
                (2, '02-01-23', 30),
                (3, '03-01-23', 45)";
                    insertCmd2.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public List<Habit> AllRecordsDb()
        {
            List<Habit> tableData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @$"SELECT
                        habits_records.ID,
                        habits_records.Date,
                        habits_records.Quantity,
                        habits.Name,
                        habits.Unit
                    FROM
                        habits_records INNER JOIN
                        habits ON habits.ID = habits_records.HabitID";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                            HabitType = reader.GetString(3),
                            Unit = reader.GetString(4),
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                return tableData;
            }
        }

        public bool RecordExists(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habits_records WHERE Id = @id)";
                cmd.Parameters.AddWithValue("@id", id);

                int exists = Convert.ToInt32(cmd.ExecuteScalar());
                return exists == 1;
            }
        }

        public Habit FindHabitByName(string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"SELECT * FROM habits WHERE Name = @name";
                tableCmd.Parameters.AddWithValue("@name", name);

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Habit
                        {
                            Id = reader.GetInt32(0),
                            HabitType = reader.GetString(1),
                            Unit = reader.GetString(2),
                        };
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
            }
            return null;
        }
        public void InsertHabitRecord(int habitId, string date, int quantity)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"
                    INSERT INTO habits_records (HabitId, Date, Quantity)
                    VALUES (@habitId, @date, @quantity)";
                tableCmd.Parameters.AddWithValue("@habitId", habitId);
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertRecord(string habitName, string unit)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"
                    INSERT INTO habits (Name, Unit)
                    VALUES (@habitName, @unit)";
                tableCmd.Parameters.AddWithValue("@habitName", habitName);
                tableCmd.Parameters.AddWithValue("@unit", unit);

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool UpdateHabitRecord(int id, string date, int quantity)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM habits_records WHERE Id = @id)";
                checkCmd.Parameters.AddWithValue("@id", id);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists == 0)
                {
                    return false;
                }

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = @"
            UPDATE habits_records 
            SET Date = @date, Quantity = @quantity 
            WHERE Id = @id";
                updateCmd.Parameters.AddWithValue("@date", date);
                updateCmd.Parameters.AddWithValue("@quantity", quantity);
                updateCmd.Parameters.AddWithValue("@id", id);

                updateCmd.ExecuteNonQuery();

                return true;
            }
        }

        public bool DeleteHabitRecordById(int recordId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "DELETE FROM habits_records WHERE Id = @recordId";
                tableCmd.Parameters.AddWithValue("@recordId", recordId);

                int rowCount = tableCmd.ExecuteNonQuery();
                return rowCount > 0;
            }
        }

    }
}
