using Microsoft.Data.Sqlite;

namespace habit_tracker;

public class DatabaseAccessLayer
{

    SqliteConnection connection;
    public DatabaseAccessLayer(string connectionString)
    {
        connection = new SqliteConnection(connectionString);
    }
    public bool DateExists(string date)
    {
        connection.Open();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = "SELECT COUNT(*) FROM daily_prayer WHERE Date = @Date";
            cmd.Parameters.AddWithValue("@Date", date);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            connection.Close();
            return count > 0;
        }
    }

    public void Insert(string date, int quantity)
    {
        bool dateExists;
        do
        {
            dateExists = DateExists(date);

            if (dateExists)
            {
                Console.WriteLine($"A record with the date {date} already exists. Please enter a unique date.");
                date = Console.ReadLine();
            }

            else
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
        while (dateExists);
    }

    public void Update(string date, int quantity)
    {

        bool dateExists;
        do
        {
            dateExists = DateExists(date);

            if (!dateExists)
            {
                Console.WriteLine($"A record with the date {date} does not exist. Please enter a unique date.");
                date = Console.ReadLine();
            }

            else
            {
                connection.Open();
                using (var tableCmd = connection.CreateCommand())
                {
                    tableCmd.CommandText = "Update daily_prayer SET Quantity = @Quantity WHERE Date = @date";
                    tableCmd.Parameters.AddWithValue("@date", date);
                    tableCmd.Parameters.AddWithValue("@Quantity", quantity);
                    tableCmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        while (!dateExists);
    }
    public void Delete(string date)
    {
        bool dateExists;
        do 
        {
            dateExists = DateExists(date);
            if (dateExists)
            {
                using (connection)
                {
                    connection.Open();
                    using (var tableCmd = connection.CreateCommand())
                    {
                        tableCmd.CommandText = "DELETE FROM daily_prayer WHERE Date = @date";
                        tableCmd.Parameters.AddWithValue("@date", date);
                        tableCmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            else
            {
                Console.WriteLine($"A record with the date {date} does not exist. Please enter a valid date.");
                date = Console.ReadLine();
            }
        }
        while (!dateExists);
        

    }
    public void Retrieve()
    {
        using (connection)
        {
            connection.Open();
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT Id, Date, Quantity FROM daily_prayer ORDER BY Date ASC";

                using (var reader = tableCmd.ExecuteReader())
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

    public void getReport()
    {
        // show avg prayer for last week, year, all time



        using (connection)
        {
            connection.Open();

            // Average for the past week
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT AVG(Quantity) FROM daily_prayer WHERE Date >= date('now', '-7 days')";

                var avgWeek = Convert.ToDouble(tableCmd.ExecuteScalar()).ToString("0");
                Console.WriteLine("Average for past week: " + avgWeek + "/5");
            }

            // Average for the past year
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT AVG(Quantity) FROM daily_prayer WHERE Date >= date('now', '-1 year')";

                var avgYear = Convert.ToDouble(tableCmd.ExecuteScalar()).ToString("0");
                Console.WriteLine("Average for past year: " + avgYear + "/5");
            }

            // Average for all time
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = "SELECT AVG(Quantity) FROM daily_prayer";

                var avgAllTime = Convert.ToDouble(tableCmd.ExecuteScalar()).ToString("0");
                Console.WriteLine($"Average for all time: " + avgAllTime + "/5");
            }

            connection.Close();
        }


    }
}