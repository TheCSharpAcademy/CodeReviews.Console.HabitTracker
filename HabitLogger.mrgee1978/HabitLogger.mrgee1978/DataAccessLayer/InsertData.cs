using Microsoft.Data.Sqlite;

namespace HabitLogger.mrgee1978.DataAccessLayer
{
    public class InsertData
    {
        /// <summary>
        /// Inserts a record into the database records table
        /// returns true if successful false otherwise
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="quantity"></param>
        /// <param name="habitId"></param>
        public bool InsertRecord(string date, int quantity, int habitId)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
                {
                    connection.Open();
                    SqliteCommand insertCommand = connection.CreateCommand();

                    // Use a parameterized query to help against SQLInject attacks
                    insertCommand.CommandText = $"INSERT INTO records (" +
                        $"Date, Quantity, HabitId) VALUES(@Date, @Quantity, @HabitId)";

                    insertCommand.Parameters.AddWithValue("@Date", date);
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                    insertCommand.Parameters.AddWithValue("HabitId", habitId);

                    insertCommand.ExecuteNonQuery();
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
        /// Inserts a habit into the database habits table
        /// returns true if successful false otherwise
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="unitOfMeasurement"></param>
        public bool InsertHabit(string name, string unitOfMeasurement)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(DatabaseCreation.DatabaseConnectionString))
                {
                    connection.Open();
                    SqliteCommand insertCommand = connection.CreateCommand();

                    // Use a parameterized query to help against SQLInject attacks
                    insertCommand.CommandText = $"INSERT INTO habits (Name, UnitOfMeasurement) VALUES(@Name, @UnitOfMeasurement)";

                    insertCommand.Parameters.AddWithValue("Name", name);
                    insertCommand.Parameters.AddWithValue("UnitOfMeasurement", unitOfMeasurement);

                    insertCommand.ExecuteNonQuery();
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
}
