using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repository;

public class DatabaseAccessLayer
{

    SqliteConnection connection;
    public DatabaseAccessLayer(string connectionString)
    {

        connection = new SqliteConnection(connectionString);

    }
    public  void Insert(string date, int quantity)
    {
        using(connection)
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "INSERT INTO daily_prayer (Date, Quantity) VALUES (@Date, @Quantity)";
                tableCmd.Parameters.AddWithValue("@Date", date);
                tableCmd.Parameters.AddWithValue("@Quantity", quantity);
                tableCmd.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public  void Update(int Id, int quantity)
    {
        using (connection)
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "Update daily_prayer SET Quantity = @Quantity WHERE Id = @Id";
                tableCmd.Parameters.AddWithValue("@Id", Id);
                tableCmd.Parameters.AddWithValue("@Quantity", quantity);
                tableCmd.ExecuteNonQuery();
            }
            connection.Close();
        }

    }
    public  void Delete(int id)
    {
        using (connection)
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "DELETE FROM daily_prayer WHERE ID = @Id";
                tableCmd.Parameters.AddWithValue("@Id", id);
                tableCmd.ExecuteNonQuery();
            }
            connection.Close();
        }

    }
    public void Retrieve()
    {
        using (connection)
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT Id, Date, Quantity FROM daily_prayer";
                
                using(var reader = tableCmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        int id = reader.GetInt32(0);
                        string date = reader.GetString(1);
                        int quantity = reader.GetInt32(2);

                        Console.WriteLine($"{id}\t {date}\t {quantity}\n");
                    }
                }
            }
            connection.Close();
        }

    }
}