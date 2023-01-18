
namespace yashsachdev.HabitTracker;

public class ConnectionClass
{
    SqliteConnection cnn = null;
    string Result;
    public void Connect()
    {
        try
        {
            using (SqliteConnection cnn = new SqliteConnection())
            {
                cnn.Open();
                Result = GetConnectionInformation(cnn);
            }
        }
        catch(Exception ex)
        {
            Result = ex.Message;
        }
        finally { cnn.Close(); }    
    }
    public string GetConnectionInformation(SqliteConnection cnn)
    {
        StringBuilder sb = new StringBuilder(1024); 
        var con = new SqliteConnection("Data Source=hello.db");
        sb.AppendLine("Connection String:" + con.ConnectionString); 
        sb.AppendLine("State:" + con.State.ToString()); 
        sb.AppendLine("Connection Timeout" + con.ConnectionTimeout.ToString());
        sb.AppendLine("Database" + con.Database);
        sb.AppendLine("Data source" + con.DataSource);
        sb.AppendLine("Server Version" + con.ServerVersion);
        return sb.ToString();
    }

    
}
