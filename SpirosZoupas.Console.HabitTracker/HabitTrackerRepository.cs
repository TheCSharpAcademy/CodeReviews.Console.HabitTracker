using Microsoft.Data.Sqlite;
using SpirosZoupas.Console.HabitTracker.Model;
using System;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace habit_tracker
{
    public class HabitTrackerRepository
    {
        const string connectionString = @"Data Source=habit-Tracker.db";

        // user chooses a hobby, then select * from habit_tracker where habitID = id of habit user chose
        // Count(*) of the rows for how many times it happened, Sum(Quantity) for how many [MeasurementUnit] in total
        // Past for month, week, etc. along with past year?
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

            PopulateTablesIfEmpty();
        }

        private void PopulateTablesIfEmpty()
        {
            var random = new Random();
            string[] habitNames = { "Reading", "Exercise", "Meditation", "Journaling", "Coding", "Yoga", "Walking", "Running", "Painting", "Cooking" };
            string[] measurementUnits = { "Minutes", "Hours", "Days", "Repetitions", "Sessions", "Kilometers", "Pages", "Exercises", "Meals" };

            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM habit";
                int habitCount = Convert.ToInt32(cmd.ExecuteScalar());

                if (habitCount == 0)
                {

                    cmd.CommandText =
                        @"INSERT INTO
                        habit(Name, MeasurementUnit)
                    VALUES
                        (@Name, @MeasurementUnit)";

                    var nameParam = cmd.Parameters.Add("@Name", SqliteType.Text);
                    var unitParam = cmd.Parameters.Add("@MeasurementUnit", SqliteType.Text);

                    for (int i = 0; i < 10; i++)
                    {
                        nameParam.Value = habitNames[random.Next(habitNames.Length)];
                        unitParam.Value = measurementUnits[random.Next(measurementUnits.Length)];
                        cmd.ExecuteNonQuery();
                    }
                }

                cmd.CommandText = "SELECT COUNT(*) FROM habit_tracker";
                int habitTrackerCount = Convert.ToInt32(cmd.ExecuteScalar());

                if (habitTrackerCount == 0)
                {
                    cmd.CommandText =
                   @"INSERT INTO
                        habit_tracker(Date, Quantity, HabitId)
                    VALUES
                        (@Date, @Quantity, @HabitId)";


                    var dateParam = cmd.Parameters.Add("@Date", SqliteType.Text);
                    var quantityParam = cmd.Parameters.Add("@Quantity", SqliteType.Integer);
                    var habitIdParam = cmd.Parameters.Add("@HabitId", SqliteType.Integer);

                    var fkCmd = connection.CreateCommand();
                    fkCmd.CommandText = "SELECT Id FROM habit";
                    SqliteDataReader dataReader = fkCmd.ExecuteReader();
                    List<int> habitIds = new List<int>();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            habitIds.Add(dataReader.GetInt32(0));
                        }
                    }

                    for (int i = 0; i < 100; i++)
                    {
                        dateParam.Value = DateTime.Today.AddDays(-random.Next(365)).ToString("dd-MM-yy");
                        quantityParam.Value = random.Next(101);
                        int randomHabitId = random.Next(habitIds.Count);
                        habitIdParam.Value = habitIds[randomHabitId];
                        cmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        public bool Insert(string tableName, Dictionary<string, object> parameters)
        {
            string columns = string.Join(", ", parameters.Keys);
            string values = string.Join(", ", parameters.Keys.Select(k => $"@{k}"));

            string query = $@"
                INSERT INTO 
                    {tableName} ({columns}) 
                VALUES 
                    ({values})";

            Dictionary<string, (object, SqliteType)> typedParameters = parameters.ToDictionary(
                kvp => $"@{kvp.Key}",
                kvp => (kvp.Value, InferSqliteType(kvp.Value))
            );

            return ExecuteNonQuery(query, typedParameters);
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

        public bool Delete(string tableName, int id)
        {
            string query = @$"
                DELETE FROM
                    {tableName}
                WHERE
                    ID = @Id";

            var parameters = new Dictionary<string, (object, SqliteType)>
            {
                { "@Id", (id, SqliteType.Integer) }
            };

            return ExecuteNonQuery(query, parameters);
        }

        public List<HabitRow> GetListOfAllRecords()
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

                List<HabitRow> tableData = new List<HabitRow>();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        tableData.Add(
                            new HabitRow
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

        public List<Habit> GetListOfAllHabits()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"SELECT
                        habit.ID,
                        habit.Name,
                        habit.MeasurementUnit,
                    FROM
                        habit";

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
                                Name = dataReader.GetString(1),
                                MeasurementUnit = dataReader.GetString(2),
                            });
                    }
                }

                connection.Close();

                return tableData;
            }
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

        private SqliteType InferSqliteType(object value)
        {
            return value switch
            {
                int => SqliteType.Integer,
                string => SqliteType.Text,
                _ => throw new ArgumentException($"Unsupported data type: {value.GetType()}")
            };
        }
    }
}
