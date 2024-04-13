using System.Data.SQLite;
using System.Globalization;

namespace HabitTracker
{
    internal class App
    {
        private static string databasePath = @"HabitTracker.db";
        private static string connectionString = $"Data Source={databasePath};Version=3;";
        public static void ViewRecords()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                var cmd = new SQLiteCommand(connection);
                cmd.CommandText = "SELECT * FROM trackerTable";
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Database is empty. Nothing to see. Press any key to continue.");
                        Console.ReadLine();
                        return;
                    }
                    Console.WriteLine("Following is the list :\n");
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int waterIntake = reader.GetInt32(1);
                        string date = reader.GetString(2);
                        int index = date.IndexOf(' ');
                        date = date.Substring(0, index + 1);
                        Console.WriteLine($"ID: {id} ,Intake(glasses/day): {waterIntake} ,Date: {date}\n");
                    }
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
                }
                cmd.Dispose();
                connection.Close();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

        public static void InsertRecord()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                var cmd = new SQLiteCommand(connection);
                Console.WriteLine("Enter the Intake(glasses/day) of water:");
                var input = Console.ReadLine();
                int waterIntake;
                while (!int.TryParse(input, out waterIntake) || waterIntake <= 0)
                {
                    Console.WriteLine("Invalid input pls enter positive integer values.");
                    input = Console.ReadLine();
                }
                Console.WriteLine("Enter the date in exact (\"yyyy/MM/dd\") format\nEg for 5th February 2024 input will be 2024/02/05 ");
                string inputDate = Console.ReadLine();
                string format = "yyyy/MM/dd";
                DateTime date;
                while (!DateTime.TryParseExact(inputDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    Console.WriteLine("Invalid date input.Enter the date in exact (\"yyyy/MM/dd\") format\nEg for 5th February 2024 input will be 2024/02/05 ");
                    inputDate = Console.ReadLine();
                }
                cmd.CommandText = $"INSERT INTO trackerTable(WaterIntake, Date) VALUES({waterIntake}, '{date}')";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Console.WriteLine("Record inserted successfully.Press any key to continue.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

        public static void DeleteRecord()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                var cmd = new SQLiteCommand(connection);
                Console.WriteLine("Enter the id of record you want to delete:");
                var input = Console.ReadLine();
                int id;
                while (!int.TryParse(input, out id))
                {
                    Console.WriteLine("Invalid input pls enter integer value.");
                    input = Console.ReadLine();
                }
                cmd.CommandText = @"SELECT COUNT(*) FROM trackerTable WHERE Id= $id";
                cmd.Parameters.AddWithValue("id", id);
                object result = cmd.ExecuteScalar();
                if (!(result != null && Convert.ToInt32(result) > 0))
                {
                    Console.WriteLine("The given id is not present in the database.Press any key to continue.");
                    Console.ReadLine();
                    return;
                }
                cmd.CommandText = @"DELETE FROM trackerTable WHERE Id = $id";
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Console.WriteLine("Record deleted successfully.Press any key to continue.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }

        }

        public static void UpdateRecord()
        {
            try
            {
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                var cmd = new SQLiteCommand(connection);
                int id;
                Console.WriteLine("Enter the id of record you want to Update:");
                var input = Console.ReadLine();
                while (!int.TryParse(input, out id))
                {
                    Console.WriteLine("Invalid input pls enter integer value.");
                    input = Console.ReadLine();
                }

                cmd.CommandText = @"SELECT COUNT(*) FROM trackerTable WHERE Id= $id";
                cmd.Parameters.AddWithValue("id", id);
                object result = cmd.ExecuteScalar();
                if (!(result != null && Convert.ToInt32(result) > 0))
                {
                    Console.WriteLine("The given id is not present in the database.Press any key to continue.");
                    Console.ReadLine();
                    return;
                }
                int UpdatedWaterIntake;
                Console.WriteLine("Enter the UpdatedWaterIntake of the record you want to Update:");
                input = Console.ReadLine();
                while (!int.TryParse(input, out UpdatedWaterIntake))
                {
                    Console.WriteLine("Invalid input pls enter integer value.");
                    input = Console.ReadLine();
                }
                cmd.CommandText = @"UPDATE trackerTable SET WaterIntake = $UpdatedWaterIntake WHERE Id = $id";
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("UpdatedWaterIntake", UpdatedWaterIntake);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Console.WriteLine("Record updated successfully.Press any key to continue.");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

        }

        public static void CreateDatabase()
        {
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
                using SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                var cmd = new SQLiteCommand(connection);
                cmd.CommandText = @"CREATE TABLE trackerTable(Id INTEGER PRIMARY KEY, WaterIntake INT, Date DATE)";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
        }
    }
}
