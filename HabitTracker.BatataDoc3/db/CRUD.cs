using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace HabitTracker.BatataDoc3.db
{

    internal class CRUD
    {
        private SqliteConnection? conn;

        public CRUD()
        {
            conn = null;
        }

        public void startDatabase()
        {
            String sql = @"CREATE TABLE habits(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                date DATE
            )";
            try 
            {
                bool exists = checkIfDbExists();
                Console.WriteLine(exists);
                conn = new SqliteConnection(@"Data Source=db\habits.db");
                conn.Open();
                Console.WriteLine("db connected!");
                if (!exists)
                {
                    Console.WriteLine("Hello");
                    using var command = new SqliteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table habits created with success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public bool checkIfDbExists()
        {
            if(File.Exists(@"db\habits.db")) return true;
            return false;
        }

        public SqliteConnection getConnection() { return conn; }


        public void InsertRecord(string input, DateTime dt)
        {
            string sql = "INSERT INTO habits (name, date) VALUES(@name, @date)";
            
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@name", input);
            command.Parameters.AddWithValue("@date", dt);
            command.ExecuteNonQuery();
        }


        public List<Habit> GetAllRecords()
        {
            List<Habit> habits = new List<Habit>();
            string sql = "SELECT * FROM habits";

            using var command = new SqliteCommand(sql, conn);
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1); 
                    DateTime dt = reader.GetDateTime(2);
                    Habit h = new Habit(id, name, dt);
                    habits.Add(h);
                }
            }
            return habits;

        }

        public bool DeleteRecord(int id)
        {
            string sql = "DELETE FROM habits WHERE id = @id";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);

            var deletedRow = command.ExecuteNonQuery();
            if (deletedRow == 0) return false;
            return true;
        }


        public bool CheckIfTheIdExists(int id)
        {
            Console.WriteLine(id);
            string sql = @"SELECT EXISTS(
                        SELECT 1
                        FROM habits
                        WHERE id = @id
                        );";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@id", id);
            int existsRow = Convert.ToInt32(command.ExecuteScalar());
            return existsRow == 1;
        }

        public bool UpdateRecord(int id, string habit)
        {
            string sql = @"UPDATE habits 
                            SET name = @name
                            WHERE id = @id";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@name", habit);
            command.Parameters.AddWithValue("@id", id);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }

        public bool UpdateRecord(int id,  DateTime dt)
        {
            string sql = @"UPDATE habits 
                            SET date = @dt
                            WHERE id = @id";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@dt", dt);
            command.Parameters.AddWithValue("@id", id);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }
    }
}
