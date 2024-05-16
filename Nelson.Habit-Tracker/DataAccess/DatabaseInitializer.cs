using System.Data.SQLite;
using System.Globalization;
using Nelson.Habit_Tracker.Models;
using Nelson.Habit_Tracker.UserConsoleInteraction;

namespace Nelson.Habit_Tracker.DataAccess
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private const string DatabaseName = "habit_tracker.db";
        private const string ConnectionString = "Data Source=" + DatabaseName + ";Version=3;";
        private readonly IConsoleInteraction _consoleInteraction;
        readonly List<Habit> databases = [];

        public DatabaseInitializer(IConsoleInteraction consoleInteraction)
        {
            _consoleInteraction = consoleInteraction;
        }

        public void InitializeDatabase()
        {
            // Check if the database is already initialized
            if (System.IO.File.Exists(DatabaseName))
            {
                return;
            }
            // Create a new SQLite database file if it doesn't exist    
            SQLiteConnection.CreateFile(DatabaseName);

            // Create tables
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Habits (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Date TEXT,
                            Name TEXT,
                            Measurement TEXT,
                            Quantity INTEGER
                        );";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void GetFromDatabase()
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @"SELECT * FROM Habits";

            using var command = new SQLiteCommand(createTableQuery, connection);

            // Read the rows from the database
            SQLiteDataReader reader = command.ExecuteReader();

            // Read all rows and add them to the list
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var habit = new Habit
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", CultureInfo.InvariantCulture),
                        Name = reader.GetString(2),
                        Measurement = reader.GetString(3),
                        Quantity = reader.GetInt32(4)
                    };
                    databases.Add(habit);
                }

            // Display the rows
            _consoleInteraction.ShowMessage("---------------------------------------------");
            foreach (var row in databases)
            {
                _consoleInteraction.ShowMessage($"{row.Id} - {row.Date:dd-MM-yyyy} - {row.Name} - {row.Measurement} - {row.Quantity}");
            }
            databases.Clear();
            _consoleInteraction.ShowMessage("---------------------------------------------");
            }
            else
            {
                _consoleInteraction.ShowMessageTime("There are no habits stored in the database.");
            }
        }

        public void InsertToDatabase(DateTime date, string name, string measure, int quantity)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @$"
                INSERT INTO Habits(Date, Name, Measurement, Quantity)
                VALUES ('{date.ToString("dd-MM-yyyy")}', '{name}', '{measure}', {quantity})";

            using var command = new SQLiteCommand(createTableQuery, connection);
            command.ExecuteNonQuery();
        }

        public void UpdateToDatabase(int id, DateTime date, string name, string measure, int quantity)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM Habits WHERE Id = {id})";
            int exists = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (exists == 0)
            {
                _consoleInteraction.ShowMessageTime($"\n\nHabit with ID {id} does not exist.\n\n");
                return;
            }

            string createTableQuery = @$"
                UPDATE Habits
                SET Date = '{date.ToString("dd-MM-yyyy")}', Name = '{name}', Measurement = '{measure}', Quantity = {quantity}
                WHERE Id = {id}";

            using var tableCommand = new SQLiteCommand(createTableQuery, connection);
            tableCommand.ExecuteNonQuery();

            _consoleInteraction.ShowMessageTime($"\n\nHabit with ID {id} updated successfully.");
        }

        public void DeleteFromDatabase(int ID)
        {
            using var connection = new SQLiteConnection(ConnectionString);
            connection.Open();

            string createTableQuery = @$"
                DELETE FROM Habits
                WHERE Id = {ID}";

            using var command = new SQLiteCommand(createTableQuery, connection);

            int rowCount = command.ExecuteNonQuery();

            if (rowCount > 0)
            {
                _consoleInteraction.ShowMessageTime("Habit deleted successfully.");
            }
            else
            {
                _consoleInteraction.ShowMessageTime($"Habit with ID {ID} does not exist.");
            }
        }
    }
}