using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace Lawang.Habit_Tracker;

public class HabitTracker
{
    private readonly string connectionString = "Data Source=Habit-Tracker.db";

    public SqliteConnection? connection { get; set; } = null;

    public HabitTracker()
    {
    }

    public void CreateTable()
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();

                string createTable = @"CREATE TABLE IF NOT EXISTS run_tracker(Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT,Distance REAL)";

                sqlCmd.CommandText = createTable;

                sqlCmd.ExecuteNonQuery();

                sqlCmd.CommandText = @"SELECT COUNT(Id) FROM run_tracker";
                if (Convert.ToInt32(sqlCmd.ExecuteScalar()) == 0)
                {
                    string seedData = @"INSERT INTO run_tracker (Date, Distance)
                        VALUES('11-01-12', 4.2)";
                    sqlCmd.CommandText = seedData;
                    sqlCmd.ExecuteNonQuery();

                    seedData = @"INSERT INTO run_tracker (Date, Distance)
                        VALUES('12-02-14', 2.4)";
                    sqlCmd.CommandText = seedData;
                    sqlCmd.ExecuteNonQuery();

                    seedData = @"INSERT INTO run_tracker (Date, Distance)
                        VALUES('12-12-12', 1.8)";
                    sqlCmd.CommandText = seedData;
                    sqlCmd.ExecuteNonQuery();
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }
    }

    public int Insert(HabitRecord habitRecord)
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();

                string insertRecord = @$"INSERT INTO run_tracker (Date, Distance)
                VALUES('{habitRecord.Date?.ToString("dd-MM-yy")}', {habitRecord.Distance})";

                sqlCmd.CommandText = insertRecord;

                return sqlCmd.ExecuteNonQuery();

            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        return 0;
    }

    public List<HabitRecord> GetHabitRecords()
    {
        List<HabitRecord> habitRecords = new List<HabitRecord>();
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();

                sqlCmd.CommandText = @"SELECT * FROM run_tracker";
                SqliteDataReader dataReader = sqlCmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        habitRecords.Add(
                            new HabitRecord()
                            {
                                Date = DateOnly.ParseExact(dataReader.GetString(1), "dd-MM-yy"),
                                Distance = double.Parse(dataReader.GetString(2))
                            }
                        );
                    }
                }
            }

            return habitRecords;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        return habitRecords;
    }

    public void ViewAll()
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();

                string viewRecord = @"SELECT * FROM run_tracker";

                sqlCmd.CommandText = viewRecord;

                using var sqlReader = sqlCmd.ExecuteReader();

                if (sqlReader.HasRows)
                {
                    var table = new Table();
                    table.Border = TableBorder.Ascii2;
                    table.ShowRowSeparators = true;
                    table.AddColumns(new string[] { "[springgreen4]Id[/]", "[springgreen4]Date[/]", "[springgreen4]Distance in Km[/]" });
                    
                    while (sqlReader.Read())
                    {
                        table.AddRow("[cyan1]" + sqlReader.GetString(0) + "[/]", sqlReader.GetString(1), sqlReader.GetString(2));
                    }

                    AnsiConsole.Write(table);
                }
                else
                {
                    Console.WriteLine("No rows Found!!");
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }
    }

    public int RowCount(int recordId)
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();

                sqlCmd.CommandText = @$"SELECT COUNT(Id) FROM run_tracker WHERE Id = {recordId}";

                return Convert.ToInt32(sqlCmd.ExecuteScalar());
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        return 0;

    }
    public int Delete(int recordId)
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand sqlCmd = connection.CreateCommand();

                sqlCmd.CommandText = @$"DELETE FROM run_tracker WHERE Id = {recordId}";
                return sqlCmd.ExecuteNonQuery();

            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }
        return 0;
    }

    public int Update(int recordId, HabitRecord habitRecord)
    {
        try
        {
            using (connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sqlCmd = connection.CreateCommand();
                string cmdText = "";

                if (habitRecord.Distance == null)
                {
                    cmdText = @$"UPDATE run_tracker SET Date = '{habitRecord.Date?.ToString("dd-MM-yy")}' WHERE Id = {recordId}";
                }
                else if (habitRecord.Date == null)
                {
                    cmdText = @$"UPDATE run_tracker SET Distance = {habitRecord.Distance} WHERE Id = {recordId}";
                }
                else
                {
                    cmdText = @$"UPDATE run_tracker 
                    SET Date = '{habitRecord.Date?.ToString("dd-MM-yy")}', Distance = {habitRecord.Distance}
                    WHERE Id = {recordId}";
                }

                sqlCmd.CommandText = cmdText;
                return sqlCmd.ExecuteNonQuery();
            }

        }
        catch (SqliteException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            connection?.Close();
        }

        return 0;
    }

}
