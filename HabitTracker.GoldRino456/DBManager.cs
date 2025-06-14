using Microsoft.Data.Sqlite;

namespace HabitTracker.GoldRino456
{
    //Safer singleton logic by Jon Skeet, C# in Depth: https://csharpindepth.com/articles/singleton
    //Per the article, there are definitely better ways to go about this and honestly even for this project it may be a bit overkill,
    //but I wanted to try implementing the "Lock" for this project.
    public sealed class DBManager
    {
        #region Singleton Logic & Properties
        private static DBManager? _instance;
        private static readonly object _instanceLock = new object();

        public static DBManager Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DBManager();
                    }
                    return _instance;
                }

            }
        }
        #endregion

        private const string _connectionString = "Data Source=../../../habitTracker.db";
        private const string _createUserTableSQL = "CREATE TABLE IF NOT EXISTS Habits (id INTEGER PRIMARY KEY AUTOINCREMENT, date TEXT, habitType TEXT, quantity INTEGER, unitOfMeasurement TEXT)";

        public List<Habit> GetAllExistingHabitEntries()
        {
            List<Habit> habits = new();

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM Habits", connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);
                            string habitType = reader.GetString(2);
                            int quantity = reader.GetInt32(3);
                            string unitOfMeasurement = reader.GetString(4);

                            habits.Add(new Habit(id, date, habitType, quantity, unitOfMeasurement));
                        }
                    }
                }

                connection.Close();
            }

            return habits;
        }

        public bool GetExistingHabitByID(int id, out Habit? habit)
        {
            habit = null;
            bool isHabitFound = false;

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM Habits WHERE id = @Id", connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = id;

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int habitId = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);
                            string habitType = reader.GetString(2);
                            int quantity = reader.GetInt32(3);
                            string unitOfMeasurement = reader.GetString(4);

                            habit = new Habit(habitId, date, habitType, quantity, unitOfMeasurement);
                            isHabitFound = true;
                        }
                    }
                }

                connection.Close();
            }

            return isHabitFound;
        }

        public bool ConfirmHabitExists(int id)
        {
            Habit? habit;
            return GetExistingHabitByID(id, out habit);
        }

        public bool DeleteExistingHabit(int id)
        {
            int rowsAffected;

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("DELETE FROM Habits WHERE id = @Id", connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = id;

                    rowsAffected = command.ExecuteNonQuery();
                }

                connection.Close();
            }

            if(rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public void UpdateExistingHabit(Habit habit)
        {
            if(habit.ID == 0) //ID not set.
            {
                return;
            }

            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("UPDATE Habits SET date = @Date, habitType = @HabitType, quantity = @Quantity, unitOfMeasurement = @UnitOfMeasurement WHERE id = @Id", connection))
                {
                    command.Parameters.Add("@Date", SqliteType.Text).Value = habit.Date;
                    command.Parameters.Add("@HabitType", SqliteType.Text).Value = habit.HabitType;
                    command.Parameters.Add("@Quantity", SqliteType.Integer).Value = habit.Quantity;
                    command.Parameters.Add("@UnitOfMeasurement", SqliteType.Text).Value = habit.UnitOfMeasurement;
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = habit.ID;

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void AddHabitToDB(Habit habit)
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("INSERT INTO Habits (date, habitType, quantity, unitOfMeasurement) VALUES (@Date, @HabitType, @Quantity, @UnitOfMeasurement)", connection))
                {
                    command.Parameters.Add("@Date", SqliteType.Text).Value = habit.Date;
                    command.Parameters.Add("@HabitType", SqliteType.Text).Value = habit.HabitType;
                    command.Parameters.Add("@Quantity", SqliteType.Integer).Value = habit.Quantity;
                    command.Parameters.Add("@UnitOfMeasurement", SqliteType.Text).Value = habit.UnitOfMeasurement;

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void CreateTableIfDoesNotExist()
        {
            using (SqliteConnection connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(_createUserTableSQL, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
