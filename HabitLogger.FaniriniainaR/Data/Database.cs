using System.Data.SQLite;

namespace HabitLogger.Data
{
    public static class Database
    {
        static readonly string DbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "habit-logger.db");

        public static SQLiteConnection GetConnection()
        {
            // Crée le fichier s’il n’existe pas
            if (!File.Exists(DbPath))
            {
                SQLiteConnection.CreateFile(DbPath);
                Console.WriteLine("Fichier DB créé : " + DbPath);
            }

            var connection = new SQLiteConnection($"Data Source={DbPath};Version=3;");
            connection.Open();
            return connection;
        }

        public static void InitializeDatabase()
        {
            using var connection = GetConnection();

            var createHabitTypesTable = @"
                CREATE TABLE IF NOT EXISTS HabitTypes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Unit TEXT NOT NULL
                );";

            var createHabitsTable = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    HabitTypeId INTEGER NOT NULL,
                    FOREIGN KEY (HabitTypeId) REFERENCES HabitTypes(Id)
                );";

            using var command = new SQLiteCommand(createHabitTypesTable, connection);
            command.ExecuteNonQuery();

            command.CommandText = createHabitsTable;
            command.ExecuteNonQuery();

            Console.WriteLine("Tables vérifiées/créées.");
        }
    }
}
