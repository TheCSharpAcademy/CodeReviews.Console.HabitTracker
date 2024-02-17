using HabitTracker.Dejmenek.DataAccess.Interfaces;
using HabitTracker.Dejmenek.Helpers;
using HabitTracker.Dejmenek.Models;
using System.Data.SQLite;

namespace HabitTracker.Dejmenek.DataAccess.Repositories
{
    public class HabitRepository : IHabitRepository
    {
        private readonly string _connectionString;

        public HabitRepository(string connectionString) {
            _connectionString = connectionString;
            CreateTable();

            if (IsEmptyTable()) {
                SeedData();
            }
        }

        public void CreateTable() {
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS Habits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        QuantityUnit TEXT NOT NULL,
                        Date TEXT NOT NULL
                    );
                ";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SeedData()
        {
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                List<Habit> habits = RandomHabits.GenerateRandomHabits();

                string sql = @"
                    INSERT INTO Habits (Name, Description, Quantity, QuantityUnit, Date)
                    VALUES (@Name, @Description, @Quantity, @QuantityUnit, @Date);   
                ";

                using (var command = new SQLiteCommand(sql, connection)) {
                    foreach (var habit in habits) {
                        command.Parameters.AddWithValue("@Name", habit.Name);
                        command.Parameters.AddWithValue("@Description", habit.Description);
                        command.Parameters.AddWithValue("@Quantity", habit.Quantity);
                        command.Parameters.AddWithValue("@QuantityUnit", habit.QuantityUnit);
                        command.Parameters.AddWithValue("@Date", habit.Date);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public bool IsEmptyTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"SELECT EXISTS (SELECT 1 FROM Habits);";

                using (var command = new SQLiteCommand(sql, connection)) {
                    long count = (long) command.ExecuteScalar();
                    connection.Close();

                    if (count == 0)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        }


        public void AddHabit(string name, string description, string date, int quantity, string quantityUnit) {
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = @"
                    INSERT INTO Habits (Name, Description, Quantity, QuantityUnit, Date)
                    VALUES (@Name, @Description, @Quantity, @QuantityUnit, @Date);   
                ";

                using (var command = new SQLiteCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@QuantityUnit", quantityUnit);
                    command.Parameters.AddWithValue("@Date", date);

                    command.ExecuteNonQuery();
                }

            }
        }

        public void UpdateHabit(int id, string date, int quantity)
        {
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = @"
                    UPDATE Habits
                    SET Date = @Date, Quantity = @Quantity
                    WHERE Id = @Id;
                ";

                using (var command = new SQLiteCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteHabit(int id) {
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = "DELETE FROM Habits WHERE Id = @Id;";

                using (var command = new SQLiteCommand(sql, connection)) {
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Habit> GetAllHabits()
        {
            List<Habit> habits = new List<Habit>();
            using (var connection = new SQLiteConnection(_connectionString)) {
                connection.Open();
                string sql = "SELECT * FROM Habits;";

                using (var command = new SQLiteCommand(sql, connection)) {
                    using (var reader = command.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read())
                            {
                                habits.Add(new Habit
                                {
                                    Id = reader.GetInt64(0),
                                    Name = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Quantity = reader.GetInt64(3),
                                    QuantityUnit = reader.GetString(4),
                                    Date = reader.GetString(5),
                                });
                            }
                        }
                    }
                }
            }

            return habits;
        }
    }
}
