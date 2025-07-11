using Microsoft.Data.Sqlite;

namespace HabitTracker.vagabundoo;

public class Queries(string connectionString)
{
    public SqliteConnection Connection = new(connectionString); 
    public void InsertNewHabit(string user, string habit, int count, DateOnly day) {
        
        Connection.Open();
        using var command = Connection.CreateCommand();
        command.CommandText = $@"
                               INSERT INTO
                                   habit (user, habit, count, date)
                               values
                                   ('{user}', '{habit}', {count}, $date);
                               ";
        command.Parameters.AddWithValue("$date", day);
        command.ExecuteNonQuery();
        Connection.Close();
    }
   
    public List<Dictionary<string, object>> RetrieveHabits(string user)
    {
        var list = new List<Dictionary<string, object>>();
        string readQuery = $"SELECT * FROM habit WHERE user = '{user}' ORDER BY id;";
        using (SqliteCommand readCommand = new SqliteCommand(readQuery, Connection))
        {
            Connection.Open();
			using var reader = readCommand.ExecuteReader();
            
            while (reader.Read())
            {
                var  dict = new Dictionary<string, object>()
                {
                    { "id", reader.GetInt32(0) },
                    { "user", reader.GetString(1) },
                    { "habit", reader.GetString(2) },
                    { "count", reader.GetInt32(3) },
                    { "date", reader.GetDateTime(4) },
                };
                list.Add(dict);
            }
            Connection.Close();
            
        }
        return list;
    }

    public void UpdateHabit(int id,int count)
    {
        Connection.Open();
        string updateQuery = $"update habit set count = {count} where id = {id}; ";
        using var command = Connection.CreateCommand();
        command.CommandText = updateQuery;
        command.ExecuteNonQuery();
        Connection.Close();
    }

    public void DeleteHabit(string user, string habit)
    {
        Connection.Open();
        string updateQuery = $"delete from habit where user = '{user}' and habit = '{habit}'; ";
        using var command = Connection.CreateCommand();
        command.CommandText = updateQuery;
        command.ExecuteNonQuery();
        Connection.Close();
    }

    public void DeleteHabitById(int id)
    {
        Connection.Open();
        string updateQuery = $"delete from habit where id = {id}; ";
        using var command = Connection.CreateCommand();
        command.CommandText = updateQuery;
        command.ExecuteNonQuery();
        Connection.Close();
    }
}
