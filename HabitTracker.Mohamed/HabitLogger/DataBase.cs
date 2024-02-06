using Microsoft.Data.Sqlite;

namespace HabitLogger;

public static class DataBase
{
    public static void Connect()
    {
        var connectionString = "Data Source=habit-logger.db";

        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"DROP TABLE IF EXISTS habit_logger;
                  CREATE TABLE IF NOT EXISTS habit_logger (
                  id INTEGER PRIMARY KEY AUTOINCREMENT,
                  date TEXT,
                  kilometers INTEGER
                  )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
            Seed();
        }
    }

    private static void Seed()
    {
        var connectionString = "Data Source=habit-logger.db";

        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"INSERT INTO habit_logger(date , kilometers) VALUES
('01/01/2024',1),
('01/03/2024',1),
('01/05/2024',2),
('01/07/2024',2),
('01/09/2024',3),
('01/11/2024',3)
";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    public static bool IsExist(int id)
    {
        var connectionString = "Data Source=habit-logger.db";
        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_logger WHERE Id = {id})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            connection.Close();
            return checkQuery > 0;
        }
    }
}
