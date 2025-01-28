using System.Data.SQLite;
using System.Globalization;


namespace HabitLogger
{
    public static class DBController
    {
        private static readonly string connectionString = @"Data Source=habit-Tracker.db";

        static DBController()
        {
            InitializeDB();
            SeedData();
        }

        private static void InitializeDB()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Quantity INTEGER
                );";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void InsertData()
        {
            string date = InputHelpers.GetDateInput();
            int quantity = InputHelpers.GetNumberInput("Insert number of glasses or other measure of your choice (no decimals)\n\n");

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            using SQLiteCommand command = 
            new SQLiteCommand("INSERT INTO drinking_water(Date,Quantity) VALUES (@date, @quantity)",connection);
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.ExecuteNonQuery();

            connection.Close();

        }
        public static void Delete()
        {
            Console.Clear();
            GetAllRecords();
            var recordId = InputHelpers.GetNumberInput("\n\n Select the ID of the record you wish to delete or 0 to return to menu");
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            using SQLiteCommand command = new("DELETE FROM drinking_water WHERE Id=@recordId",connection);
            command.Parameters.AddWithValue("@recordId", recordId);
            int rowCount = command.ExecuteNonQuery();
            if (rowCount == 0)
            {
                Console.WriteLine("The record doesn't exist");
                Delete();
            }
            Console.WriteLine("Record deleted");
            connection.Close();
        }
        public static void GetAllRecords()
        {
            using var connection = new SQLiteConnection(connectionString);

            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = "SELECT * FROM drinking_water";
            List<DrinkingWater> tableData = new();
            SQLiteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)

                        }
                        );
                }

            }
            else
            {
                Console.WriteLine("No data!");
            }
            connection.Close();

            Console.WriteLine("--------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");

            }
            Console.WriteLine("--------------------------------\n");
        }
        public static void Update()
        {
            Console.Clear();
            GetAllRecords();
            var recordId = InputHelpers.GetNumberInput("\n\nType the id of the record you want to edit");

            using var connection = new SQLiteConnection(connectionString);

            connection.Open();
            SQLiteCommand checkCmd = new("SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id=@recordId)", connection);
            checkCmd.Parameters.AddWithValue("@recordId", recordId);
            int checkValue = Convert.ToInt32(checkCmd.ExecuteScalar());
            if (checkValue == 0)
            {
                Console.WriteLine("This records does not exist");
                connection.Close();
                Update();
            }

            string date = InputHelpers.GetDateInput();
            int quantity = InputHelpers.GetNumberInput("Insert number of glasses or other measure of your choice (no decimals)\n\n");
            SQLiteCommand tableCmd =
                new(@"UPDATE drinking_water SET Date= @date, " +
                "Quantity= @quantity " +
                "WHERE Id= @recordId",connection);
            tableCmd.Parameters.AddWithValue("@quantity", quantity);
            tableCmd.Parameters.AddWithValue("@date",date);
            tableCmd.Parameters.AddWithValue("@recordId",recordId); 
            tableCmd.ExecuteNonQuery();
            connection.Close();

        }

        public static void SumWater()
        {
            Console.Clear();

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand result = new("SELECT SUM(Quantity) FROM drinking_water",connection);
            var sum= result.ExecuteScalar();
            connection.Close();

            Console.WriteLine($"You've drunk {sum} of water in the unit you've choosen ");

        }

        public static void LastWeekData()
        {
            Console.Clear( );
            using var connection = new SQLiteConnection( connectionString);
            DateTime currentDate = DateTime.Now;
            DateTime weekAgo = currentDate.AddDays(-7);
            connection.Open();
            SQLiteCommand result = new("SELECT SUM(Quantity) FROM drinking_water WHERE date BETWEEN @weekAgo AND @currentDate", connection);

            result.Parameters.AddWithValue("@currentDate", currentDate.ToString("dd-MM-yy"));
            result.Parameters.AddWithValue("@weekAgo", weekAgo.ToString("dd-MM-yy"));
            var sum=result.ExecuteScalar();
            connection.Close();

            Console.WriteLine($"You've drank {(sum == DBNull.Value ? "no" : sum)  } units of water last week in the unit you've choosen");
        }

        public static void SeedData()
        {
            Random random = new Random();
            int DataEntries = 100;
            DateTime startDate = new DateTime(2024, 1, 1);
            DateTime endDate = DateTime.Now;
            using var connection = new SQLiteConnection(connectionString);
            SQLiteCommand query = new("INSERT INTO drinking_water(Date,Quantity) VALUES (@date, @quantity)",connection);
            connection.Open();
            for (int i = 0; i < DataEntries; i++)
            {
                int range = (endDate - startDate).Days;
                DateTime randomDate = startDate.AddDays(random.Next(range));

                string formattedDate = randomDate.ToString("dd-MM-yy");
                int quantity=random.Next(100);
                query.Parameters.AddWithValue("@date",formattedDate);
                query.Parameters.AddWithValue("@quantity",quantity);
                query.ExecuteNonQuery();
            }
            connection.Close();
        }


    }
}
