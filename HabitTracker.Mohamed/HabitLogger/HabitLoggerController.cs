using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker;

public class HabitLoggerController
{
    private string connectionString = "Data Source=habit-logger.db";
    private SqliteConnection connection;
    private SqliteCommand tableCmd;

    public HabitLoggerController()
    {
        this.connection = new SqliteConnection(this.connectionString);
        this.tableCmd = this.connection.CreateCommand();
    }

    public List<HabitLogger> GetAllRecords()
    {
        this.connection.Open();
        List<HabitLogger> tableData = new();

        try
        {
            this.tableCmd.CommandText = $"SELECT * FROM habit_logger ";
            SqliteDataReader reader = this.tableCmd.ExecuteReader();

            while (reader.Read())
            {
                tableData.Add( new HabitLogger(
                     reader.GetInt32(0),
                     DateTime.ParseExact(reader.GetString(1), "MM/dd/yyyy", new CultureInfo("en-US")),
                     reader.GetInt32(2)
                ) );
            }
            reader.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        this.connection.Close();
        return tableData;
    }

    public void Insert(string date ,int kilometers)
    {
        this.connection.Open();
        try
        {
            this.tableCmd.CommandText = $@"INSERT INTO habit_logger(date , kilometers) VALUES('{date}',{kilometers})";
            this.tableCmd.ExecuteNonQuery();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        this.connection.Close();
    }

    public void Update(int id , string date, int kilometers)
    {

        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habit_logger WHERE Id = {id})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {id} doesn't exist.\n\n");
            connection.Close();
            return;
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE habit_logger SET date = '{date}', kilometers = {kilometers} WHERE Id = '{id}'";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    public void Delete(int id)
    {
        try
        {
            this.connection.Open();
            this.tableCmd.CommandText = $@"DELETE FROM habit_logger WHERE Id={id}";
            this.tableCmd.ExecuteNonQuery();
            this.connection.Close();
        }catch(Exception ex)
        {
            Console.WriteLine($"Err : {ex.Message}");
        }
    }
}
