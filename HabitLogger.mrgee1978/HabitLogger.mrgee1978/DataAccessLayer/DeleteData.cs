using Microsoft.Data.Sqlite;

namespace HabitLogger.mrgee1978.DataAccessLayer;

public class DeleteData
{
    /// <summary>
    /// Deletes a record from the database, based on the given id
    /// </summary>
    /// <param name="id"></param>
    public bool DeleteRecord(int id)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();

                SqliteCommand deleteCommand = connection.CreateCommand();

                // Use a parameterized query to help against SQLInject attacks
                deleteCommand.CommandText = @"DELETE FROM records WHERE Id = @Id";

                deleteCommand.Parameters.AddWithValue("@Id", id);

                deleteCommand.ExecuteNonQuery();
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
    /// Deletes a habit from the database, based on the given id
    /// </summary>
    /// <param name="id"></param>
    public bool DeleteHabit(int id)
    {
        try
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
            {
                connection.Open();

                SqliteCommand deleteCommand = connection.CreateCommand();

                // Use a parameterized query to help against SQLInject attacks
                deleteCommand.CommandText = @"DELETE FROM habits WHERE Id = @Id";

                deleteCommand.Parameters.AddWithValue("@Id", id);

                deleteCommand.ExecuteNonQuery();
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
