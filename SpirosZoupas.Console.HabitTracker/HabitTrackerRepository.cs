using Microsoft.Data.Sqlite;
using SpirosZoupas.Console.HabitTracker.Model;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace habit_tracker
{
    public class HabitTrackerRepository
    {
        const string connectionString = @"Data Source=habit-Tracker.db";

        public void CreateTables()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        MeasurementUnit TEXT
                    )";

                tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_tracker (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(HabitId) REFERENCES habit(Id) ON DELETE CASCADE
                    )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool InsertRecord(int habitId, string date, int quantity)
        {
            string query = @"
                INSERT INTO 
                    habit_tracker(Date, Quantity, HabitId) 
                VALUES 
                    (@Date, @Quantity, @HabitId)";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "@Date", (date, SqliteType.Text) },
                { "@Quantity", (quantity, SqliteType.Integer) },
                { "@HabitId", (habitId, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public bool UpdateRecord(int id, string date, int quantity)
        {
            string query = @"
                UPDATE 
                    habit_tracker 
                SET 
                    Date = @Date, Quantity = @Quantity 
                WHERE 
                    Id = @Id";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "@Date", (date, SqliteType.Text) },
                { "@Quantity", (quantity, SqliteType.Integer) },
                { "@Id", (id, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public bool DeleteRecord(int id)
        {
            string query = @$"
                DELETE FROM
                    habit_tracker
                WHERE
                    ID = @Id";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "Id", (id, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public List<Habit> GetListOfAllRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"SELECT
                        habit_tracker.ID,
                        habit_tracker.Date,
                        habit_tracker.Quantity,
                        habit.Name,
                        habit.MeasurementUnit
                    FROM
                        habit_tracker INNER JOIN
                        habit ON habit.ID = habit_tracker.HabitID";

                SqliteDataReader dataReader = cmd.ExecuteReader();

                List<Habit> tableData = new List<Habit>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        tableData.Add(
                            new Habit
                            {
                                ID = dataReader.GetInt32(0),
                                Date = DateTime.ParseExact(dataReader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = dataReader.GetInt32(2),
                                HabitName = dataReader.GetString(3),
                                MeasurementUnit = dataReader.GetString(4),
                            });
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        public bool InsertHabit(string name, string uom)
        {
            string query = @"
                INSERT INTO 
                    habit(Name, MeasurementUnit) 
                VALUES 
                    (@Name, @MeasurementUnit)";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "@Name", (name, SqliteType.Text) },
                { "@MeasurementUnit", (uom, SqliteType.Text) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public bool UpdateHabit(int id, string name, int measurementUnit)
        {
            string query = @"
                UPDATE 
                    habit 
                SET 
                    Name = @Name, MeasurementUnit = @MeasurementUnit 
                WHERE 
                    Id = @Id";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "@Id", (id, SqliteType.Integer) },
                { "@Name", (name, SqliteType.Text) },
                { "@MeasurementUnit", (measurementUnit, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public bool DeleteHabit(int id)
        {
            string query = @$"
                DELETE FROM
                    habit
                WHERE
                    ID = @Id";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "Id", (id, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        private bool ExecuteNonQuery(string query, Dictionary<string, (object parameterValue, SqliteType Type)> parameters)
        {
            int rowCount;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;

                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.Parameters.Add(param.Key, param.Value.Type);
                        dbParam.Value = param.Value.parameterValue;
                    }

                    rowCount = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            return rowCount != 0;
        }

        public bool DoesRecordExist(int id)
        {
            object? result;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"SELECT
                        *
                    FROM
                        habit_tracker
                    WHERE
                        Id = {id}";

                result = cmd.ExecuteScalar();

                connection.Close();
            }

            return result is not null;
        }

        public int GetHabitIdByName(string name)
        {
            int habitId = -1;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"SELECT
                        Id
                    FROM
                        habit
                    WHERE
                        Name = '{name}'";

                SqliteDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    if (dataReader.HasRows) habitId = dataReader.GetInt32(0);
                }

                connection.Close();
            }

            return habitId;
        }
    }
}
