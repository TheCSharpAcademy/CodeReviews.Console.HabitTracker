using Microsoft.Data.Sqlite;
using SpirosZoupas.Console.HabitTracker.Model;
using System.Globalization;

namespace habit_tracker
{
    public class HabitTrackerRepository
    {
        const string connectionString = @"Data Source=habit-Tracker.db";

        public void CreateTable()
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

                var x = tableCmd.ExecuteNonQuery();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_tracker (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER,
                        HabitId INTEGER,
                        FOREIGN KEY(HabitId) REFERENCES habit(Id)
                    )";

                var y = tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool Insert(int habitId, string date, int quantity)
        {
            string query = @"
                INSERT INTO 
                    drinking_water(Date, Quantity, HabitId) 
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

        public bool Update(int id, string date, int quantity)
        {
            string query = @"
                UPDATE 
                    drinking_water 
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

        public bool Delete(int id)
        {
            string query = @$"
                DELETE FROM
                    drinking_water
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
                        habit_tracker.*,
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
                        drinking_water
                    WHERE
                        Id = {id}";

                result = cmd.ExecuteScalar();

                connection.Close();
            }

            return result is not null;
        }

        public int GetHabitIdByName(string name)
        {
            SqliteDataReader dataReader;
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
                        Name = {name}";

                dataReader = cmd.ExecuteReader();

                connection.Close();
            }

            return dataReader.HasRows ? dataReader.GetInt32(0) : -1;
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
    }
}
