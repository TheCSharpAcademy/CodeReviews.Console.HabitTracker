using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Models;

namespace HabitLoggerApp.Data
{
    public class DatabaseManager(string dbPath)
    {
        public string _connectionString = $"Data Source={dbPath};Version=3;";

        public void InitializeDatabase()
        {
            if (!File.Exists("HabitLogger.db"))
            {
                SQLiteConnection.CreateFile("HabitLogger.db");
                CreateTables();
                SeedData();
            }
        }


        private void CreateTables()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string createHabitTable = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    UnitOfMeasure TEXT NOT NULL
                );";

            string createHabitEntriesTable = @"
                CREATE TABLE IF NOT EXISTS HabitEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                );";

            using var command1 = new SQLiteCommand(createHabitTable, connection);
            command1.ExecuteNonQuery();

            using var command2 = new SQLiteCommand(createHabitEntriesTable, connection);
            command2.ExecuteNonQuery();
        }


        public void AddHabit(Habit habit)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string insertHabit = "INSERT INTO Habits (Name, UnitOfMeasure) VALUES (@Name, @UnitOfMeasure)";
            using var command = new SQLiteCommand(insertHabit, connection);
            command.Parameters.AddWithValue("@Name", habit.Name);
            command.Parameters.AddWithValue("@UnitOfMeasure", habit.UnitOfMeasure);
            command.ExecuteNonQuery();
        }

        public List<Habit> GetHabits()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string selectHabits = "SELECT Id, Name, UnitOfMeasure FROM Habits";
            using var command = new SQLiteCommand(selectHabits, connection);
            using var reader = command.ExecuteReader();
            var habits = new List<Habit>();
            while (reader.Read())
            {
                habits.Add(new Habit
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    UnitOfMeasure = reader.GetString(2)
                });
            }
            return habits;
        }

        public bool SeedData()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var checkCommand = new SQLiteCommand("SELECT COUNT(*) FROM Habits", connection);
            var count = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (count > 0)
            {
                return false;
            }


            var habits = new List<Habit>
            {
                new() { Name = "Drink Water", UnitOfMeasure = "Glasses" },
                new() { Name = "Exercise", UnitOfMeasure = "Km" },
                new() { Name = "Read", UnitOfMeasure = "Pages" }
            };

            foreach (var habit in habits)
            {
                var insertHabit = new SQLiteCommand("INSERT INTO Habits (Name, UnitOfMeasure) VALUES (@Name, @UnitOfMeasure)", connection);
                insertHabit.Parameters.AddWithValue("@Name", habit.Name);
                insertHabit.Parameters.AddWithValue("@UnitOfMeasure", habit.UnitOfMeasure);
                insertHabit.ExecuteNonQuery();
            }

            var habitIdLookup = new List<int>();
            var getIds = new SQLiteCommand("SELECT Id FROM Habits", connection);
            using var reader = getIds.ExecuteReader();
            while (reader.Read())
                habitIdLookup.Add(Convert.ToInt32(reader["Id"]));

            var random = new Random();
            for (int i=0;i<100;i++)
            {
                int habitId = habitIdLookup[random.Next(habitIdLookup.Count)];
                int quantity = random.Next(1, 10);
                string date = DateTime.Now.AddDays(-random.Next(0, 30)).ToString("yyyy-MM-dd");

                var insertEntry = new SQLiteCommand("INSERT INTO HabitEntries (HabitId, Quantity, Date) VALUES (@HabitId, @Quantity, @Date)", connection);

                insertEntry.Parameters.AddWithValue("@HabitId", habitId);
                insertEntry.Parameters.AddWithValue("@Quantity", quantity);
                insertEntry.Parameters.AddWithValue("@Date", date);
                insertEntry.ExecuteNonQuery();
            }

            return true;
        }

        public void AddHabitEntry(HabitEntry entry)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string insertEntry = "INSERT INTO HabitEntries (HabitId, Quantity, Date) VALUES (@HabitId, @Quantity, @Date)";
            using var command = new SQLiteCommand(insertEntry, connection);
            command.Parameters.AddWithValue("@HabitId", entry.HabitId);
            command.Parameters.AddWithValue("@Quantity", entry.Quantity);
            command.Parameters.AddWithValue("@Date", entry.Date.ToString("yyyy-MM-dd"));
            command.ExecuteNonQuery();
        }

        public void DeleteHabit(int habitId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string deleteHabit = "DELETE FROM Habits WHERE Id = @Id";
            using var command = new SQLiteCommand(deleteHabit, connection);
            command.Parameters.AddWithValue("@Id", habitId);
            command.ExecuteNonQuery();
        }

        public List<HabitEntry> GetEntriesByHabit(int habitId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();
            string selectEntries = "SELECT Id, HabitId, Quantity, Date FROM HabitEntries WHERE HabitId = @HabitId";
            using var command = new SQLiteCommand(selectEntries, connection);
            command.Parameters.AddWithValue("@HabitId", habitId);
            using var reader = command.ExecuteReader();
            var entries = new List<HabitEntry>();
            while (reader.Read())
            {
                entries.Add(new HabitEntry
                {
                    Id = reader.GetInt32(0),
                    HabitId = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    Date = DateTime.Parse(reader.GetString(3))
                });
            }
            return entries;
        }

        public void DeleteHabitEntry(int entryId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string deleteQuery = "DELETE FROM HabitEntries WHERE Id = @Id";
            using var command = new SQLiteCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@Id", entryId);
            command.ExecuteNonQuery();
        }
        public void UpdateHabitEntry(HabitEntry entry)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string updateQuery = "UPDATE HabitEntries SET Quantity = @Quantity, Date = @Date WHERE Id = @Id";
            using var command = new SQLiteCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@Quantity", entry.Quantity);
            command.Parameters.AddWithValue("@Date", entry.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Id", entry.Id);

            command.ExecuteNonQuery();
        }
    }
}