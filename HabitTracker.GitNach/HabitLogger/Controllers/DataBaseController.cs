using HabitLogger.Model;
using System.Data.SQLite;

namespace HabitLogger.Controllers
{
    public static class DataBaseController
    {
        private static Habit _habitManager = new Habit();

        private static string _connectionString = $"Data Source={DATABASEFILE};Version=3;";

        private const string DATABASEFILE = "HabitManagerDatabase.sqlite";

        public static void CreateDatabase()
        {
            if (!System.IO.File.Exists(DATABASEFILE))//Preguntamos si no existe el archivo HabitManagerDatabase
            {
                SQLiteConnection.CreateFile(DATABASEFILE);// De ser así, lo creamos
                using (var connection = new SQLiteConnection(_connectionString))//Conexión a la base de datos usando el _connectionString
                {
                    connection.Open();
                    string sqlHabitTable = "CREATE TABLE IF NOT EXISTS Habit (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, MetricValue INTEGER, TypeId INTEGER);";//Este es el Query

                    string sqlHabitTypeTable = "CREATE TABLE IF NOT EXISTS HabitType (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT ,Metric TEXT);";





                    using (var command = new SQLiteCommand(sqlHabitTypeTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SQLiteCommand(sqlHabitTable, connection))//Creamos comando a ejecutar, por un lado el Query y por el otro la conexión a la database donde ejecutar el Query
                    {
                        command.ExecuteNonQuery();
                    }


                }
            }
        }

        public static void AddHabit(Habit habit)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Habit (Date, MetricValue, TypeId) VALUES (@Date, @MetricValue, @TypeId)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Date", habit.Date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@MetricValue", habit.MetricValue);
                    command.Parameters.AddWithValue("@TypeId", habit.Type.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteHabit(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Habit WHERE Id = @Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateItem(int id, Habit updatedHabit)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE Habit SET Date = @Date, MetricValue = @MetricValue, TypeId = @TpeId WHERE Id = @Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Date", updatedHabit.Date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@MetricValue", updatedHabit.MetricValue);
                    command.Parameters.AddWithValue("@TypeId", updatedHabit.MetricValue);
                    command.ExecuteNonQuery();
                }
            }




        }

        public static List<Habit> GetHabits()
        {
            List<Habit> habits = new List<Habit>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Habit.Id, Habit.Date, Habit.MetricValue, HabitType.Id, HabitType.Name, HabitType.Metric FROM Habit JOIN HabitType ON Habit.TypeId = HabitType.Id";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habits.Add(new Habit
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.Parse(reader.GetString(1)),
                            MetricValue = reader.GetInt32(2),
                            Type = new HabitType
                            {
                                Id = (int)reader.GetInt32(3),
                                Name = reader.GetString(4),
                                Metric = reader.GetString(5),
                            }
                        });

                    }
                    return habits;
                }

            }

        }

        public static List<HabitType> GetHabitTypes()
        {
            List<HabitType> habitTypes = new List<HabitType>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM HabitType";
                using (var command = new SQLiteCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habitTypes.Add(new HabitType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Metric = reader.GetString(2)
                        });
                    }
                }
            }
            return habitTypes;
        }

        public static void AddHabitType(HabitType habitType)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();


                string checkSql = "SELECT COUNT(1) FROM HabitType WHERE Name = @Name";
                using (var checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Name", habitType.Name);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        Console.WriteLine($"HabitType '{habitType.Name}' already exists.");
                        return;
                    }

                }

                string sql = "INSERT INTO HabitType (Name, Metric) VALUES (@Name, @Metric)";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", habitType.Name);
                    command.Parameters.AddWithValue("@Metric", habitType.Metric);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
