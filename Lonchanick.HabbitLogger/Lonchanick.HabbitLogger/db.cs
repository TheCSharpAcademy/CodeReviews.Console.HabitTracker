using Microsoft.Data.SqlClient;

namespace Lonchanick.HabbitLogger;

internal class Db
{
    public static List<Habit> Select()
    {

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Step 2: Define the SELECT query
            string query = "SELECT Id, DateField, Quantity FROM Habit";

            // Step 3: Create a command object
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Step 4: Open connection and execute the query
                connection.Open();

                // Execute the query and obtain a data reader
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Check if the reader has rows
                    if (reader.HasRows)
                    {
                        var HabitList = new List<Habit>();

                        // Read and process each row
                        while (reader.Read())
                        {
                            HabitList.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    //DateField = DateOnly.Parse(reader.GetDateTime(1).ToString()),
                                    DateField = reader.GetDateTime(1).Date,
                                    Quantity = reader.GetInt32(2)
                                });

                            // Process the data as needed
                            //Console.WriteLine($"Id: {id}, DateField: {dateField}, Quantity: {quantity}");
                        }

                        return HabitList;
                    }
                    else
                    {
                        //Console.WriteLine("No rows found.");
                        return null;
                    }
                }
            }
        }
    }

    public static bool Insert(Habit habit)
    {
        string insertCommand = "INSERT INTO Habit (DateField, Quantity) VALUES (@Value1, @Value2)";
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        if (habit is not null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create command and set parameters
                using (SqlCommand command = new SqlCommand(insertCommand, connection))
                {
                    command.Parameters.AddWithValue("@Value1", habit.DateField);
                    command.Parameters.AddWithValue("@Value2", habit.Quantity);

                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected
                    if (rowsAffected > 0)
                    {
                        //Console.WriteLine("Data inserted successfully.");
                        return true;
                    }
                    else
                    {
                        //Console.WriteLine("No data inserted.");
                        return false;
                    }
                }
            }

        }

        return false;

    }
    public static bool Update(Habit habit)
    {
        string updateCommand = "UPDATE Habit" 
            + " SET DateField = @Value1, Quantity = @Value2"
            + " WHERE Id = @ConditionValue";

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        if (habit is not null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create command and set parameters
                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("@Value1", habit.DateField);
                    command.Parameters.AddWithValue("@Value2", habit.Quantity);
                    command.Parameters.AddWithValue("@ConditionValue", habit.Id);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected
                    if (rowsAffected > 0)
                    {
                        //Console.WriteLine("Data inserted successfully.");
                        return true;
                    }
                    else
                    {
                        //Console.WriteLine("No data inserted.");
                        return false;
                    }
                }
            }

        }

        return false;

    }

    public static bool Delete(int id)
    {
        string updateCommand = "DELETE Habit"
            + " WHERE Id = @ConditionValue";

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        if (id > 0)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Create command and set parameters
                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("@ConditionValue", id);


                    // Execute the INSERT command
                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if any rows were affected
                    if (rowsAffected > 0)
                    {
                        //Console.WriteLine("Data inserted successfully.");
                        return true;
                    }
                    else
                    {
                        //Console.WriteLine("No data inserted.");
                        return false;
                    }
                }
            }

        }

        return false;

    }
}
