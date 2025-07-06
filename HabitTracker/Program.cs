using Microsoft.Data.Sqlite;

string dataSource = "DataSource=habittracker.db";
Initialize(dataSource);

return;

static void Initialize(string dataSource)
{
    using var connection = new SqliteConnection(dataSource);
    connection.Open();
        
    var command = connection.CreateCommand();
    command.CommandText =
        """
        DROP TABLE IF EXISTS habit;

        CREATE TABLE habit (
            id INTEGER PRIMARY KEY AUTOINCREMENT ,
            user TEXT NOT NULL,
            habit TEXT NOT NULL,
            count INTEGER NULL,
            date DATETIME NULL
        );
        """;
    command.ExecuteNonQuery();
    connection.Close();
}


class Queries(SqliteConnection connection)
{
    public SqliteConnection Connection = connection; 
    void InsertNewHabit(string user, string habit, int count, DateTime date) {
        using var command = Connection.CreateCommand();
        command.CommandText = $"""
                               INSERT INTO
                                   habit (user, habit, count, date)
                               values
                                   ({user}, {habit}, {count}, {date});
                               """;
    }

    void RetrieveHabits(string user)
    {
    }

    void UpdateHabit(string user, string habit, int count, DateTime date)
    {
    }

    void DeleteHabit(string user, string habit)
    {
    }

}
