using Microsoft.Data.SqlClient;

namespace Lonchanick.HabbitLogger;

internal class Db
{
    static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial"
    +" Catalog=master;Integrated Security=True;Connect"
    +" Timeout=30;Encrypt=False;Trust Server Certificate=False;Application"
    +" Intent=ReadWrite;Multi Subnet Failover=False";

    public static List<Habit> Select()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT Id, DateField, Quantity FROM Habit";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var HabitList = new List<Habit>();

                        while (reader.Read())
                        {
                            HabitList.Add(
                                new Habit
                                {
                                    Id = reader.GetInt32(0),
                                    DateField = reader.GetDateTime(1).Date,
                                    Quantity = reader.GetInt32(2)
                                });
                        }
                        return HabitList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public static bool Insert(Habit habit)
    {
        string insertCommand = "INSERT INTO Habit (DateField, Quantity) VALUES (@Value1, @Value2)";

        if (habit is not null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertCommand, connection))
                {
                    command.Parameters.AddWithValue("@Value1", habit.DateField);
                    command.Parameters.AddWithValue("@Value2", habit.Quantity);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
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

        if (habit is not null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("@Value1", habit.DateField);
                    command.Parameters.AddWithValue("@Value2", habit.Quantity);
                    command.Parameters.AddWithValue("@ConditionValue", habit.Id);


                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
            }
        }
        return false;

    }

    public static bool Delete(int id)
    {
        string updateCommand = "DELETE Habit"
            + " WHERE Id = @ConditionValue";

        if (id > 0)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateCommand, connection))
                {
                    command.Parameters.AddWithValue("@ConditionValue", id);
                    
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return true;
                    else
                        return false;
                }
            }

        }
        return false;
    }
}
