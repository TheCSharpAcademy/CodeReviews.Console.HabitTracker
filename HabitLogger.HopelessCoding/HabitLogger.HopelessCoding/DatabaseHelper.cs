using System.Data.SQLite;

namespace DatabaseHelpers.HopelessCoding
{
    public class DbHelpers
    {
        public static string connectionString = @"Data Source=habit-Tracker.db;Version=3";

        internal static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery =
                    @"CREATE TABLE IF NOT EXISTS daily_calories (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                using (var tableCmd = new SQLiteCommand(createTableQuery, connection))
                {
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal static bool DateAlreadyExists(string date, string id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkQuery = @"SELECT count(*) FROM daily_calories WHERE Date = @Date";

                if (!string.IsNullOrEmpty(id))
                {
                    checkQuery += " AND Id != @Id";
                }

                using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Date", date);

                    if (!string.IsNullOrEmpty(id))
                    {
                        checkCmd.Parameters.AddWithValue("@Id", id);
                    }

                    int rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rowCount > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool IdExists(string id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkQuery = @"SELECT count(*) FROM daily_calories WHERE Id = @Id";

                using (var checkCmd = new SQLiteCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Id", id);

                    int rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (rowCount > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static void ViewRecords()
        {
            Console.WriteLine("Below are all the records from the database sorted by date\n");

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string viewQuery = @"SELECT * FROM daily_calories ORDER BY Date DESC";

                using (var command = new SQLiteCommand(viewQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("ID\t\tDate\t\t\tCalories");
                        Console.WriteLine("================================================");

                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["Id"]);
                            string day = reader["Date"].ToString();
                            int calories = Convert.ToInt32(reader["Quantity"]);

                            Console.WriteLine($"{id}\t\t{day}\t\t{calories}");
                        }
                        Console.WriteLine("\n----------------------------");
                    }
                }
            }
        }

        internal static string GetValidDateInput()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            while (true)
            {
                Console.Write("Please enter the date in YYYY-MM-DD format or press Enter to use current date: ");
                string inputDate = Console.ReadLine();

                // Use the current date if the user doesn't provide one
                string date = string.IsNullOrEmpty(inputDate) ? currentDate : inputDate;

                if (DateTime.TryParse(date, out _))
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a date in valid format.");
                }
            }
        }

        internal static int GetValidCaloriesInput()
        {
            while (true)
            {
                Console.Write("Please, fill the daily calories: ");

                if (int.TryParse(Console.ReadLine(), out int validCalories) && validCalories >= 0)
                {
                    return validCalories;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer value.");
                }
            }
        }
    }
}
