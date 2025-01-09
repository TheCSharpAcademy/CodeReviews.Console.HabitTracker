using HabitLoggerLibrary.Repository;
using Microsoft.Data.Sqlite;

namespace HabitLoggerLibrary.DbManager;

internal sealed class DatabaseManager(SqliteConnection connection) : IDatabaseManager
{
    public void SetUp()
    {
        var command = connection.CreateCommand();

        command.CommandText = @$"CREATE TABLE IF NOT EXISTS {IHabitsRepository.TableName} (
    id INTEGER PRIMARY KEY,
    habit TEXT NOT NULL,
    unit_of_measure TEXT NOT NULL,
    UNIQUE(habit)
)";
        command.ExecuteNonQuery();

        command = connection.CreateCommand();
        command.CommandText = @$"
BEGIN;
CREATE TABLE IF NOT EXISTS habit_logs (
            id INTEGER PRIMARY KEY,
            habit_id INTEGER NOT NULL,
            quantity INTEGER NOT NULL,
            habit_date DATE NOT NULL,
            UNIQUE(habit_id, habit_date),
            CONSTRAINT fk_habit_id FOREIGN KEY(habit_id) REFERENCES {IHabitsRepository.TableName}(id) ON DELETE CASCADE);
CREATE INDEX IF NOT EXISTS habit_date_idx ON habit_logs (habit_date);
COMMIT;";
        command.ExecuteNonQuery();
    }

    public SqliteConnection GetConnection()
    {
        return connection;
    }
}