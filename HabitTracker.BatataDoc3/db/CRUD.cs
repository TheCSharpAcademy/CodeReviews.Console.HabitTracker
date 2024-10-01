using Microsoft.Data.Sqlite;


namespace HabitTracker.BatataDoc3.db
{

    internal class Crud
    {
        private SqliteConnection? conn;

        public Crud()
        {
            conn = null;
        }

        public bool StartDatabase()
        {
            String sql = @"CREATE TABLE habits(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                measure TEXT,
                quantity INT,
                date DATE
            )";

            bool exists = CheckIfDbExists();
            try 
            {
                conn = new SqliteConnection(@"Data Source=db\habits.db");
                conn.Open();
                Console.WriteLine("db connected!");
                if (!exists)
                {
                    using var command = new SqliteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table habits created with success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return !exists;

        }


        public bool CheckIfDbExists()
        {
            if(File.Exists(@"db\habits.db")) return true;
            return false;
        }

        public SqliteConnection GetConnection() { return conn; }


        public void InsertRecord(string input, string measure, int quantity, DateTime dt)
        {
            string sql = "INSERT INTO habits (name, measure, quantity, date) VALUES(@name, @measure, @quantity, @date)";
            
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@name", input);
            command.Parameters.AddWithValue("@measure", measure);
            command.Parameters.AddWithValue("@quantity", quantity);
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
                    string measure = reader.GetString(2);
                    int quantity = reader.GetInt32(3);
                    DateTime dt = reader.GetDateTime(4);
                    Habit h = new Habit(id, name, measure, quantity, dt);
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

        public bool UpdateRecord(int id, string habit, string measure, int quantity)
        {
            string sql = @"UPDATE habits 
                            SET name = @name,
                                measure = @measure,
                                quantity = @quantity
                            WHERE id = @id";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@name", habit);
            command.Parameters.AddWithValue("@measure", measure);
            command.Parameters.AddWithValue("@quantity", quantity);
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

        public bool UpdateRecord(int id, int quantity)
        {
            string sql = @"UPDATE habits 
                            SET quantity = @quantity
                            WHERE id = @id";
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@id", id);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }
    }
}
