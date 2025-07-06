using Microsoft.Data.Sqlite;

string dataSource = "DataSource=habittracker.db";
Initialize(dataSource);

var queries = new Queries(dataSource);
queries.InsertNewHabit("john", "drinkingCoffee", 20, DateTime.Now);

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


class Queries(string connection)
{
    public SqliteConnection Connection = new SqliteConnection(connection); 
    public void InsertNewHabit(string user, string habit, int count, DateTime date) {
        Connection.Open();
        
        using var command = Connection.CreateCommand();
        command.CommandText = @"
                               INSERT INTO
                                   habit (user, habit, count, date)
                               values
                                   ($user, $habit, $count, $date);
                               ";
        command.Parameters.AddWithValue("$user", user);
        command.Parameters.AddWithValue("$habit", habit);
        command.Parameters.AddWithValue("$count", count);
        command.Parameters.AddWithValue("$date", date);
        
        command.ExecuteNonQuery();
        
        // to get the row number, so get a more specific query
        string rowCountQuery = "SELECT COUNT(*) FROM habit;";
        int rowCount = 0;
        using (SqliteCommand rowCountCommand = new SqliteCommand(rowCountQuery, Connection))
        {
            using var reader = rowCountCommand.ExecuteReader();
            while (reader.Read())
            {
                rowCount = reader.GetInt32(0);
            }
        }

        Console.WriteLine($"Amount of rows after insert operation: {rowCount}");
        
        string readQuery = $"select * from habit where id = {rowCount};";
        using (SqliteCommand readCommand = new SqliteCommand(readQuery, Connection))
        {
            using var reader = readCommand.ExecuteReader();
            {
                //reader.Depth
                while (reader.Read())
                {
                    Console.WriteLine($"Added habit {reader.GetString(2)} to user {reader.GetString(1)}, counted {reader.GetInt32(3)} times, on day: {reader.GetDateTime(4)}");
                }
            }
        }
        
        Connection.Close();
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
