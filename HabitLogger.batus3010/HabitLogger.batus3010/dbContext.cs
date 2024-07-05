using Microsoft.Data.Sqlite;

namespace HabitLogger.batus3010
{
    internal class DBContext
    {
        private string _connectionString = @"Data Source=habitDB.db";

        public DBContext()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habit (
                                            Id INTEGER PRIMARY KEY,
                                            Name TEXT,
                                            Quantity INTEGER
                                        )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void InsertIntoDatebase(Habit h)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"INSERT INTO habit (Name, Quantity) VALUES (@Name, @Quantity)";
                insertCmd.Parameters.AddWithValue("@Name", h.Name);
                insertCmd.Parameters.AddWithValue("@Quantity", h.Quantity);
                insertCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool DeleteFromDatabase(int id)
        {

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var deleteCmd = connection.CreateCommand();
                    deleteCmd.CommandText = @"DELETE FROM habit WHERE Id = @Id";
                    deleteCmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = deleteCmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while deleting the record: {ex.Message}");

                return false;
            }
        }

        public bool UpdateDatabase(Habit h, int id_update)
        {

            if (!IsIdInDatabase(id_update))
            {
                Console.WriteLine($"Habit with ID {id_update} does not exist.");
                return false;
            }

            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    var updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = @"UPDATE habit SET Name = @Name, Quantity = @Quantity WHERE Id = @Id";
                    updateCmd.Parameters.AddWithValue("@Name", h.Name);
                    updateCmd.Parameters.AddWithValue("@Quantity", h.Quantity);
                    updateCmd.Parameters.AddWithValue("@Id", id_update);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while updating the record: {ex.Message}");
                return false;
            }
        }

        public void ViewAllRecords()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"SELECT * FROM habit";

                using (var reader = selectCmd.ExecuteReader())
                {
                    const int idWidth = 5;
                    const int nameWidth = 20;
                    const int quantityWidth = 10;

                    string header = $"{PadRight("Id", idWidth)} | {PadRight("Name", nameWidth)} | {PadRight("Quantity", quantityWidth)}";
                    Console.WriteLine(header);
                    Console.WriteLine(new string('-', header.Length));

                    while (reader.Read())
                    {
                        string id = PadRight(reader["Id"].ToString(), idWidth);
                        string name = PadRight(reader["Name"].ToString(), nameWidth);
                        string quantity = PadRight(reader["Quantity"].ToString(), quantityWidth);

                        Console.WriteLine($"{id} | {name} | {quantity}");
                    }
                }

                connection.Close();
            }
        }
        public bool IsEmpty()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"SELECT COUNT(*) FROM habit";

                using (var reader = selectCmd.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(0) == 0;
                }
            }
        }

        // method to check if a id is in database
        public bool IsIdInDatabase(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"SELECT COUNT(*) FROM habit WHERE Id = @Id";
                selectCmd.Parameters.AddWithValue("@Id", id);

                using (var reader = selectCmd.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(0) == 1;
                }
            }
        }

        private string PadRight(string text, int totalWidth)
        {
            return text.PadRight(totalWidth);
        }
    }
}
