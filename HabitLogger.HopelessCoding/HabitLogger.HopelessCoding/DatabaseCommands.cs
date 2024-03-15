using System.Data.SQLite;
using DatabaseHelpers.HopelessCoding;

namespace DatabaseCommands.HopelessCoding
{
    public class DbCommands
    {
        internal static void AddNewRecord()
        {
            Console.WriteLine("Adding a new record to the database\n");

            string date = DbHelpers.GetValidDateInput();
            if (DbHelpers.DateAlreadyExists(date, null))
            {
                Console.WriteLine("Record for the given date already exists.");
                Console.WriteLine("----------------------------");
                return;
            }

            int dailyCalories = DbHelpers.GetValidCaloriesInput();

            using (var connection = new SQLiteConnection(DbHelpers.connectionString))
            {
                connection.Open();

                string addNewQuery = @"INSERT INTO daily_calories (Date, Quantity)
                                            VALUES (@Date, @Quantity);";

                using (var addCmd = new SQLiteCommand(addNewQuery, connection))
                {
                    addCmd.Parameters.AddWithValue("@Date", date);
                    addCmd.Parameters.AddWithValue("@Quantity", dailyCalories);

                    int rowsAffected = addCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nRecord successfully added!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to add record!");
                    }
                }
                Console.WriteLine("----------------------------");
            }
        }

        internal static void UpdateRecord()
        {
            Console.WriteLine("Updating record from the database\n");
            Console.Write("Please enter the ID of the record which you want to update: ");

            string idToUpdate = Console.ReadLine();

            if (!DbHelpers.IdExists(idToUpdate))
            {
                Console.WriteLine($"\nRecord with the ID={idToUpdate} does not exists.");
                Console.WriteLine("----------------------------");
                return;
            }

            string date = DbHelpers.GetValidDateInput();
            if (DbHelpers.DateAlreadyExists(date, idToUpdate))
            {
                Console.WriteLine("Record for the given date already exists.");
                Console.WriteLine("----------------------------");
                return;
            }

            int newCalories = DbHelpers.GetValidCaloriesInput();

            using (var connection = new SQLiteConnection(DbHelpers.connectionString))
            {
                connection.Open();

                string updateQuery = @"UPDATE daily_calories
                                    SET Date = @Date, Quantity = @newCalories
                                    WHERE Id = @ID";

                using (var updateCmd = new SQLiteCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@ID", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@Date", date);
                    updateCmd.Parameters.AddWithValue("@newCalories", newCalories);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nThe record was successfully updated!");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update the record!");
                    }
                    updateCmd.Parameters.Clear();
                    Console.WriteLine("----------------------------");
                }
            }
        }

        internal static void DeleteRecord()
        {
            Console.WriteLine("Deleting a record from the database\n");
            Console.Write("Please enter the ID of the record which you want to delete: ");
            string idToDelete = Console.ReadLine();

            if (!DbHelpers.IdExists(idToDelete))
            {
                Console.WriteLine($"\nRecord with the ID={idToDelete} does not exists.");
                Console.WriteLine("----------------------------");
                return;
            }

            using (var connection = new SQLiteConnection(DbHelpers.connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM daily_calories WHERE Id = @ID";

                using (var deleteCmd = new SQLiteCommand(deleteQuery, connection))
                {
                    deleteCmd.Parameters.AddWithValue("@ID", idToDelete);

                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"\nRecord with ID={idToDelete} deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine($"\nDelete of record with the ID={idToDelete} was not successful.");
                    }
                    deleteCmd.Parameters.Clear();
                    Console.WriteLine("----------------------------");
                }
            }
        }
    }
}
