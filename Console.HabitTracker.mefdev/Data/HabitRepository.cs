using HabitLogger.Models;
using HabitLogger.Shared;
using Microsoft.Data.Sqlite;

namespace HabitLogger.Data
{
    public class HabitRepository : IMaintanable<Habit>
    {
        private readonly string _connectionString;

        public HabitRepository(string path)
        {
            _connectionString = $"Data Source={path};";
        }
        private SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public void Create(Habit obj)
        {
            using (var connection = GetConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"INSERT INTO habit (name, quantity, date) VALUES(@name, @quantity, @date)";
                    cmd.Parameters.AddWithValue("@name", obj.Name);
                    cmd.Parameters.AddWithValue("@quantity", obj.Quantity);
                    cmd.Parameters.AddWithValue("@date", obj.Date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void Delete(int key)
        {
            using (var connection = GetConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM habit WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", key);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }


        }

        public Habit Retrieve(int key)
        {

            using (var connection = GetConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, name, quantity, date FROM habit WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", key);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                            string quantity = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                            DateTime date = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3);

                            return new Habit(id, name, quantity, date);
                        }
                        return null;

                    }

                }
            }
            
        }

        public void Update(Habit habit)
        {
            using (var connection = GetConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE habit SET name = @name, quantity = @quantity, date = @date WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", habit.Id);
                    cmd.Parameters.AddWithValue("@name", habit.Name);
                    cmd.Parameters.AddWithValue("@quantity", habit.Quantity);
                    cmd.Parameters.AddWithValue("@date", habit.Date);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }
            }


        }

        public Habit RetrieveByName(string habitName)
        {
            using (var connection = GetConnection())
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, name, quantity, date FROM habit WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", habitName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string quantity = reader.GetString(2);
                        DateTime date = reader.GetDateTime(3);
                        return new Habit(id, name, quantity, date);
                    }
                }
            }
        }


    }
}

