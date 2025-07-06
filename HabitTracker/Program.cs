using Microsoft.Data.Sqlite;

using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
{
    connection.Open();
    
    var command = connection.CreateCommand();
    command.CommandText =
        """
        DROP TABLE IF EXISTS habit;
        CREATE TABLE IF NOT EXISTS habit  (
            id INTEGER PRIMARY KEY,
            habit TEXT NOT NULL,
            habitCount INTEGER NOT NULL
        );

        INSERT INTO habit values (1, 'drinkingWater', 10);
        """;
    command.ExecuteNonQuery();

    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var habitName = reader.GetString(1);
            var habitCount = reader.GetInt32(2);
            Console.WriteLine($"You have done habit {habitName} {habitCount} times!");
        }
    }
}