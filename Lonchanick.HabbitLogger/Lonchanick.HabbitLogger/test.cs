using Microsoft.Data.SqlClient;

class Program1
{
    static void Main()
    {
        string s = GetConnectionString();

        OpenSqlConnection(s);
        Console.ReadLine();
    }

    private static void OpenSqlConnection(string connectionString)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("ServerVersion: {0}", connection.ServerVersion);
            Console.WriteLine("State: {0}", connection.State);
        }
    }

    static private string GetConnectionString()
    {
        // To avoid storing the connection string in your code, 
        // you can retrieve it from a configuration file, using the 
        // System.Configuration.ConfigurationSettings.AppSettings property 
        return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    }
}