using Microsoft.Data.Sqlite;
using Spectre.Console;

internal class Database
{
    public const string ConnectionString = @"Data Source=habits.db";
    public const string CreateHabitName = "Create new habit name";
    public const string DeleteHabitName = "Delete habit name";
    public const string BackWord = "Back";

    internal static void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var tableRecordsCmd = connection.CreateCommand();
            tableRecordsCmd.CommandText = $@"
                CREATE TABLE IF NOT EXISTS habit_records (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    date DATE NOT NULL,
                    habit_name TEXT NOT NULL,
                    value INTEGER NOT NULL,
                    measurement_unit TEXT NOT NULL)";
            tableRecordsCmd.ExecuteNonQuery();

            var tableNamesCmd = connection.CreateCommand();
            tableNamesCmd.CommandText = $@"
                CREATE TABLE IF NOT EXISTS habit_names (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL UNIQUE,
                    measurement_unit TEXT NOT NULL)";
            tableNamesCmd.ExecuteNonQuery();

            var tableIsEmptyCmd = connection.CreateCommand();
            tableIsEmptyCmd.CommandText = $@"SELECT COUNT(*) FROM habit_names";
            var checkQuery = Convert.ToInt32(tableIsEmptyCmd.ExecuteScalar()) == 0;
            if (checkQuery)
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = $@"
                    INSERT INTO habit_names (name, measurement_unit)
                    VALUES
                        ('{DeleteHabitName}', ''),
                        ('{CreateHabitName}', ''),
                        ('Doing Nothing', 'hours'),
                        ('Drinking Water', 'glasses'),
                        ('Exercising', 'minutes'),
                        ('Sleeping', 'hours'),
                        ('Running', 'kilometers'),
                        ('Reading', 'pages'),
                        ('Walking', 'minutes')";
                insertCmd.ExecuteNonQuery();
            }
        }
    }

    internal static bool CheckIfNoRecordsAvailable<T>(IEnumerable<T> collection)
    {
        bool ifNoRecords = false;
        if (collection.Count() == 0)
        {
            ifNoRecords = true;
            AnsiConsole.MarkupLine("[red]No records found.[/]");
            HelpersGeneral.PressAnyKeyToContinue();
        }
        return ifNoRecords;
    }
}
