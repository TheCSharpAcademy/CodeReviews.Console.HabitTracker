using Microsoft.Data.Sqlite;

namespace HabitLogger.mrgee1978.DataAccessLayer;

public class UpdateData
{
    /// <summary>
    /// Updates a record in the database given the provided parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="date"></param>
    /// <param name="quantity"></param>
    /// <param name="habitId"></param>
    public bool UpdateRecord(int id, string date, int quantity)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();

                SqliteCommand updateCommand = connection.CreateCommand();

                // Use a parameterized query to help against SQLInject attacks
                updateCommand.CommandText = @"UPDATE records 
                    SET Date = @Date, Quantity = @Quantity
                    WHERE Id = @Id";

                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Date", date);
                updateCommand.Parameters.AddWithValue("@Quantity", quantity);

                updateCommand.ExecuteNonQuery();
                return true;
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"{ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Updates a habit in the database given the provided parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="unitOfMeasurement"></param>
    public bool UpdateHabit(int id, string name, string unitOfMeasurement)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();

                SqliteCommand updateCommand = connection.CreateCommand();

                // Use a parameterized query to help against SQLInject attacks
                updateCommand.CommandText = @"UPDATE habits 
                SET Name = @Name, UnitOfMeasurement = @UnitOfMeasurement 
                WHERE Id = @Id";

                updateCommand.Parameters.AddWithValue("@Id", id);
                updateCommand.Parameters.AddWithValue("@Name", name);
                updateCommand.Parameters.AddWithValue("@UnitOfMeasurement", unitOfMeasurement);

                updateCommand.ExecuteNonQuery();
                return true;
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"{ex.Message}");
            return false;
        }
    }
}
