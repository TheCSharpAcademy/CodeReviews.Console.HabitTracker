namespace HabitTracker.barakisbrown;

using Microsoft.Data.Sqlite;

public class DataLayer
{
    private readonly string DatabaseName = "readings.db";
    private readonly string DataSource;
    private readonly string TableName = "readings";

    public DataLayer() 
    {
        DataSource = $"Data Source={DatabaseName}";
        if (!Exist())
        {
            CreateDB();
        }        
    }

    private void CreateDB()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"
            CREATE TABLE ""READINGS"" (""ID"" INTEGER UNIQUE,
	                                    ""Amount"" INTEGER NOT NULL,
	                                    ""Date""	TEXT NOT NULL,
	                                    PRIMARY KEY(""ID"" AUTOINCREMENT))
        ";

        try
        {
            ExecuteNonQuery(cmd);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Creating Database. ");
            Console.WriteLine("Exception Message is {0}",ex.Message);
            throw;
        }
    }

    private bool Exist()
    {
        return File.Exists(DatabaseName);
    }

    public bool Insert(Habit row)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "INSERT INTO READINGS(Amount,Date) VALUES(@amount,@date)";
        cmd.Parameters.AddWithValue("@amount", row.Amount);
        cmd.Parameters.AddWithValue("@date", row.Date.ToString("dd-MM-yyyy"));
        cmd.Prepare();
        return (ExecuteNonQuery(cmd) == 1);            
    }

    public bool UpdateAmount(int id, int newAmount)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "UPDATE READINGS SET AMOUNT = @amount WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@amount", newAmount);
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        return ExecuteNonQuery(cmd) == 1;
    }

    public bool UpdateDate(int id, DateTime date)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "UPDATE READINGS SET Date = @date WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        return ExecuteNonQuery(cmd) == 1;
    }

    public bool DeleteRow(int id)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "DELETE FROM READINGS WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        return (ExecuteNonQuery(cmd) == 1);
    }

    public List<Habit>? SelectAll()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        string stm = "SELECT * FROM READINGS";

        using var cmd = new SqliteCommand(stm, conn);
        using SqliteDataReader rdr = cmd.ExecuteReader();

        if (rdr.HasRows)
        {
            List<Habit> habits = new();
            while (rdr.Read())
            {
                Habit newHabit = new();
                newHabit.Id = rdr.GetInt32(0);
                newHabit.Amount = rdr.GetInt32(1);
                newHabit.Date = DateTime.ParseExact(rdr.GetString(2), "dd-MM-yyyy", null);
                habits.Add(newHabit);
            }
            return habits;
        }
        return null;
    }

    private int ExecuteNonQuery(SqliteCommand cmd)
    {
        try
        {
            return cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: Something went wrong.");
            Console.WriteLine("Exception Message as follows {0}",ex.Message);
            throw;
        }
    }

    public bool IsTableEmpty()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT COUNT(*) FROM {TableName}";
        int numberRows = Convert.ToInt32(cmd.ExecuteScalar());

        return (numberRows == 0);
    }

    public int GetRowCount()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT Max(id) FROM {TableName}";
        int numberRows = Convert.ToInt32(cmd.ExecuteScalar());
        return numberRows;
    }

    public bool IdExist(int id)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT ID FROM READINGS WHERE ID = {id}";
        int numberRows = Convert.ToInt32(cmd.ExecuteScalar());
        return (numberRows > 0);
    }

    public int[]? GetValidId()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT ID FROM READINGS";
        cmd.Connection = conn;

        using SqliteDataReader reader = cmd.ExecuteReader();
        

        if (reader.HasRows)
        {
            List<int> rowsReturned = new List<int>();
            while (reader.Read())
            {
                rowsReturned.Add(reader.GetInt32(0));
            }
            return rowsReturned.ToArray();
        }
        return null;
    }

    public int MAX()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = new SqliteCommand("SELECT MAX(AMOUNT) FROM READINGS", conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int AVG()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = new SqliteCommand("SELECT MIN(AMOUNT) FROM READINGS", conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int MIN()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = new SqliteCommand("SELECT AVG(AMOUNT) FROM READINGS", conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    internal int Beyond200()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        var cmd = new SqliteCommand("SELECT COUNT(AMOUNT) FROM READINGS WHERE AMOUNT > 200", conn);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
