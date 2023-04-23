namespace HabitTracker.barakisbrown;

using Microsoft.Data.Sqlite;

public class DataLayer
{
    private readonly string DatabaseName = "readings.db";
    private readonly string DataSource;

    public DataLayer() 
    {
        DataSource = $"Data Source={DatabaseName}";
        if (!Exist())
        {
            CreateDB();
            InsertTestData();
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
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private bool Exist()
    {
        return File.Exists(DatabaseName);
    }

    public void Insert(Habit row)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "INSERT INTO READINGS(Amount,Date) VALUES(@amount,@date)";
        cmd.Parameters.AddWithValue("@amount", row.Amount);
        cmd.Parameters.AddWithValue("@date", row.Date.ToString("dd-MM-yyyy"));
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void UpdateAmount(int id, int newAmount)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "UPDATE READINGS SET AMOUNT = @amount WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@amount", newAmount);
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void UpdateDate(int id, DateTime date)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "UPDATE READINGS SET Date = @date WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public void DeleteRow(int id)
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        using var cmd = new SqliteCommand();
        cmd.Connection = conn;
        cmd.CommandText = "DELETE FROM READINGS WHERE ID = @ID";
        cmd.Parameters.AddWithValue("@ID", id);
        cmd.Prepare();
        cmd.ExecuteNonQuery();
    }

    public List<Habit> SelectAll()
    {
        using var conn = new SqliteConnection(DataSource);
        conn.Open();

        string stm = "SELECT * FROM READINGS";

        using var cmd = new SqliteCommand(stm, conn);
        using SqliteDataReader rdr = cmd.ExecuteReader();

        List<Habit> habits = new();
        while(rdr.Read())
        {
            Habit newHabit = new();
            newHabit.id = rdr.GetInt32(0);
            newHabit.Amount = rdr.GetInt32(1);
            newHabit.Date = DateTime.ParseExact(rdr.GetString(2), "dd-MM-yyyy", null);
            habits.Add(newHabit);
        }
        return habits;
    }
}
