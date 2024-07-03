// --------------------------------------------------------------------------------------------------
// HabitTracker.Data.Managers.SqliteDataManager
// --------------------------------------------------------------------------------------------------
// Partial class for data manager methods specific to the Table/View creation instead of entities.
// NOTE: connection.Close() is technically not required with a using statement,
// but will leave in as it was part of the tutorial.
// --------------------------------------------------------------------------------------------------
using Microsoft.Data.Sqlite;

namespace HabitTracker.Data.Managers;

public partial class SqliteDataManager
{
    #region Constants

    private static readonly string CreateTableHabitQuery =
        @"
        CREATE TABLE IF NOT EXISTS habit
        (
             habit_id INTEGER PRIMARY KEY AUTOINCREMENT
            ,name TEXT
            ,measure TEXT
            ,is_active INTEGER DEFAULT 1
        )
        ;";

    private static readonly string CreateTableHabitLogQuery =
        @"
        CREATE TABLE IF NOT EXISTS habit_log
        (
             habit_log_id INTEGER PRIMARY KEY AUTOINCREMENT
            ,habit_id INTEGER
            ,date TEXT
            ,quantity INTEGER
            ,FOREIGN KEY(habit_id) REFERENCES habit(habit_id)
        )
        ;";

    private static readonly string CreateViewHabitReportQuery =
        @"
        CREATE VIEW IF NOT EXISTS vw_habit_report
        AS
        SELECT
             habit_id
            ,name
            ,measure
            ,is_active
        FROM
            habit
        ;";

    private static readonly string CreateViewHabitLogReportQuery =
        @"
        CREATE VIEW IF NOT EXISTS vw_habit_log_report
        AS
        SELECT
             h.habit_id
            ,h.name
            ,h.measure
            ,hl.habit_log_id
            ,hl.date
            ,hl.quantity
        FROM
            habit AS h JOIN
            habit_log AS hl ON h.habit_id = hl.habit_id
        ORDER BY
             hl.date
            ,h.name
        ;";

    #endregion
    #region Properties

    public string ConnectionString { get; init; }

    #endregion
    #region Constructor

    public SqliteDataManager(string connectionString)
    {
        ConnectionString = connectionString;
        Initialise();
    }

    #endregion
    #region Methods: Private - Initialise

    private void Initialise()
    {
        // Put all table creation methods here, in dependency order.
        CreateTableHabit();
        CreateTableHabitLog();
        CreateViewHabitReport();
        CreateViewHabitLogReport();
    }

    #endregion
    #region Methods: Private - Create

    private void CreateTableHabit()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = CreateTableHabitQuery;
        command.ExecuteNonQuery();
        
        connection.Close();
    }

    private void CreateTableHabitLog()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = CreateTableHabitLogQuery;
        command.ExecuteNonQuery();

        connection.Close();
    }

    private void CreateViewHabitReport()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = CreateViewHabitReportQuery;
        command.ExecuteNonQuery();

        connection.Close();
    }

    private void CreateViewHabitLogReport()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = CreateViewHabitLogReportQuery;
        command.ExecuteNonQuery();

        connection.Close();
    }

    #endregion
}
