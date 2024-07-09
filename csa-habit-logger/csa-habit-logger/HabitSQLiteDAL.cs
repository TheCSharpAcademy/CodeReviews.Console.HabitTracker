using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.SQLite;

namespace csa_habit_logger
{
    public class HabitSqLiteDal
    {
        public string ConnectionString { get; private set; }

        public HabitSqLiteDal(string databaseName, string connString)
        {
            ConnectionString = connString;

            // reject null or empty strings
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ArgumentNullException("connString", "connString needs to have a valid value");
            }
            
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("databaseName", "databaseName needs to have a valid value");
            }

            // Create database file if it doesn't exist
            if (!File.Exists(databaseName))
            {
                SQLiteConnection.CreateFile(databaseName);
            }
        }

        public bool Add(NewHabit habit)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"INSERT INTO habits (name, unit) VALUES (@name, @unit)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", habit.Name);
                        cmd.Parameters.AddWithValue("@unit", habit.Unit);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        
        public Habit? Read(int ID)
        {
            Habit? habit = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"SELECT * FROM habits WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        int id = 0;
                        string? idString = reader["id"].ToString() ?? string.Empty;
                        id = Int32.Parse(idString);

                        string? name = reader["name"].ToString() ?? string.Empty;

                        string? unit = reader["unit"].ToString() ?? string.Empty;

                        habit = new Habit(id, name, unit);
                    }

                    reader.Close();
                    conn.Close();

                    return habit;
                }
            }
            catch
            {
                return null;
            }
        }

        public Habit? GetHabitByName(string name)
        {
            Habit? habit = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"SELECT * FROM habits WHERE name = @name";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        int id = 0;
                        string? idString = reader["id"].ToString() ?? string.Empty;
                        id = Int32.Parse(idString);

                        string? habitName = reader["name"].ToString() ?? string.Empty;

                        string? unit = reader["unit"].ToString() ?? string.Empty;

                        habit = new Habit(id, habitName, unit);
                    }

                    reader.Close();

                    conn.Close();

                    return habit;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool Update(int ID, Habit newHabit)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlCommandString = @"UPDATE habits " +
                                              @"SET name = @name, unit = @unit " +
                                              @"WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@name", newHabit.Name);
                    cmd.Parameters.AddWithValue("@unit", newHabit.Unit);
                    cmd.ExecuteNonQuery();
                      
                    conn.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int ID)
        {   
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlCommandString = @"DELETE FROM habits WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<Habit> GetAll()
        {
            List<Habit> list = new List<Habit>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"SELECT * FROM habits";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            int id = 0;
                            string? idString = reader["id"].ToString() ?? string.Empty;
                            id = Int32.Parse(idString);

                            string? name = reader["name"].ToString() ?? string.Empty;

                            string? unit = reader["unit"].ToString() ?? string.Empty;

                            list.Add(new Habit(id, name, unit));
                        }

                        reader.Close();
                    }

                    conn.Close();
                }
            }
            catch
            {
                return list;
            }

            return list;
        }
    
        public void InitialiseDatabase()
        {
            // Create the habits table if it doesn't exist in the database
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                string getTablesSQLString = "SELECT name FROM sqlite_master WHERE type='table' AND name='habits';";
                SQLiteCommand getTableCommand = new SQLiteCommand(getTablesSQLString, conn);
                SQLiteDataReader reader = getTableCommand.ExecuteReader();
                if (!reader.HasRows)
                {
                    string createTableString = "CREATE TABLE habits (" +
                                                    "id INTEGER NOT NULL," +
                                                    "name STRING NOT NULL, " +
                                                    "unit STRING NOT NULL," +
                                                    "PRIMARY KEY(id))";
                    SQLiteCommand cmd = new SQLiteCommand(createTableString, conn);
                    cmd.ExecuteNonQuery();
                }
                reader.Close();

                getTablesSQLString = "SELECT name FROM sqlite_master WHERE type='table' AND name='records';";
                getTableCommand = new SQLiteCommand(getTablesSQLString, conn);
                reader = getTableCommand.ExecuteReader();
                if (!reader.HasRows)
                {
                    string createTableString = "CREATE TABLE records (" +
                                                    "id INTEGER NOT NULL," +
                                                    "habitId INTEGER NOT NULL, " +
                                                    "amount STRING NOT NULL, " +
                                                    "dtime INTEGER NOT NULL," +
                                                    "PRIMARY KEY (id)," +
                                                    "FOREIGN KEY (habitId) REFERENCES habits(id))";
                    SQLiteCommand cmd = new SQLiteCommand(createTableString, conn);
                    cmd.ExecuteNonQuery();
                }
                reader.Close();

                conn.Close();
            }
        }

        public bool AddInstance(NewHabitRecord instance)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"INSERT INTO records (habitId, amount, dtime) VALUES (@habitId, @amount, @dtime)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn))
                    {
                        cmd.Parameters.AddWithValue("@habitId", instance.Habit.ID);
                        cmd.Parameters.AddWithValue("@amount", instance.Amount);
                        cmd.Parameters.AddWithValue("@dtime", (instance.DateTime - DateTime.UnixEpoch).TotalSeconds);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public HabitRecord? ReadInstance(int ID)
        {
            HabitRecord? instance = null;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"SELECT * FROM records WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        int id = 0;
                        string? idString = reader["id"].ToString() ?? string.Empty;
                        id = Int32.Parse(idString);

                        int habitId = 0;
                        string? habitIdString = reader["habitId"].ToString() ?? string.Empty;
                        habitId = Int32.Parse(habitIdString);

                        int amount= 0;
                        string? amountString = reader["amount"].ToString() ?? string.Empty;
                        amount = Int32.Parse(amountString);

                        int time = 0;
                        string? timeString = reader["dtime"].ToString() ?? string.Empty;
                        time = Int32.Parse(timeString);

                        reader.Close();

                        Habit? habit = Read(habitId);

                        if (habit is null) return null;

                        instance = new HabitRecord(id, habit, time, amount);
                    }

                    conn.Close();

                    return instance;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateInstance(int ID, HabitRecord newHabit)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlCommandString = @"UPDATE records " +
                                              @"SET habitId = @habitId, dtime = @datedtimetime, amount = @amount " +
                                              @"WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@habitId", newHabit.Habit.ID);
                    cmd.Parameters.AddWithValue("@amount", newHabit.Amount);
                    cmd.Parameters.AddWithValue("@dtime", (newHabit.DateTime - DateTime.UnixEpoch).TotalSeconds);
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteInstance(int ID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();

                    string sqlCommandString = @"DELETE FROM records WHERE id = @id";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    cmd.Parameters.AddWithValue("@id", ID);

                    cmd.ExecuteNonQuery();

                    conn.Close();

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<HabitRecord> GetAllInstances()
        {
            List<HabitRecord> list = new List<HabitRecord>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string sqlCommandString = @"SELECT * FROM records";
                    SQLiteCommand cmd = new SQLiteCommand(sqlCommandString, conn);
                    SQLiteDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = 0;
                            string? idString = reader["id"].ToString() ?? string.Empty;
                            id = Int32.Parse(idString);

                            int habitId = 0;
                            string? habitIdString = reader["habitId"].ToString() ?? string.Empty;
                            habitId = Int32.Parse(habitIdString);

                            int amount = 0;
                            string? amountString = reader["amount"].ToString() ?? string.Empty;
                            amount = Int32.Parse(amountString);

                            int time = 0;
                            string? timeString = reader["dtime"].ToString() ?? string.Empty;
                            time = Int32.Parse(timeString);

                            Habit? habit = Read(habitId);

                            if (habit is null) continue;

                            list.Add(new HabitRecord(id, habit, time, amount));
                        }

                        reader.Close();
                    }

                    conn.Close();
                }
            }
            catch
            {
                return list;
            }

            return list;
        }
        
        public void SeedDatabase()
        {
            List<NewHabit> habits = new List<NewHabit>();
            habits.Add(new NewHabit ("squat", "reps"));
            habits.Add(new NewHabit ("press", "presses"));
            habits.Add(new NewHabit ("bench", "reps"));
            habits.Add(new NewHabit ("jumps", "jumps"));
            habits.Add(new NewHabit ("water", "cups"));

            if (!(GetTableRowCount("habits") > 0))
            {
                foreach (var h in habits)
                {
                    Add(h);
                }
            }

            Habit? habit1 = GetHabitByName("squat");
            Habit? habit3 = GetHabitByName("water");

            if (!(GetTableRowCount("records") > 0))
            {
                if (habit1 is not null && habit3 is not null)
                {
                    AddInstance(new NewHabitRecord(habit1, DateTime.Parse("2023-01-01 10:57:42"), 5));
                    AddInstance(new NewHabitRecord(habit3, DateTime.Parse("2024-12-31 15:44:12"), 300));
                }
            }
        }
    
        private long GetTableRowCount(string tableName)
        {
            long count = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
                {
                    conn.Open();
                    string checkDataQuery = $"SELECT COUNT(*) FROM {tableName}";
                    using (SQLiteCommand cmd = new SQLiteCommand(checkDataQuery, conn))
                    {
                        count = (long) cmd.ExecuteScalar();
                        return count;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
