
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CrudLibrary
{

    public class SqliteCrudController
    {
        public string ConnectionString { get; private set; } = @"Data Source=habit-tracker.db";
        public string Command { get; private set; } = "";
        public string Habit { get; }
        public string UnitOfMeasure { get; }

        public SqliteCrudController(string habit, string unitOfMeasure)
        {
            Habit = habit;
            UnitOfMeasure = unitOfMeasure;
        }

        public void CreateTable()
        {
            string command = $@"CREATE TABLE IF NOT EXISTS {Habit}(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.CreateCommand().CommandText = command;
                connection.CreateCommand().ExecuteNonQuery();
                connection.Close();
            }
        }

        public void ViewAll()
        {

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand($@"SELECT * from {Habit}", connection))
                {
                    List<HabitInformation> tableData = new List<HabitInformation>();

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                new HabitInformation
                                {
                                    Id = reader.GetInt32(0),
                                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-us")),
                                    Quantity = reader.GetInt32(2)
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                        connection.Close();
                        Console.WriteLine("---------------------------------------------------------------");
                        foreach (var dw in tableData)
                        {
                            Console.WriteLine($"{dw.Id} - Date: {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                        }
                        Console.WriteLine("---------------------------------------------------------------");
                    }
                }
            }
        }


        public void InsertRecord(string date, int quantity)
        {

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand($@"INSERT INTO {Habit} (Date, Quantity)
                                                                   VALUES (@date, @quantity)", connection))
                {
                    command.Parameters.Add("@date", SqliteType.Text).Value = date;
                    command.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateRecordDate(int id, string text)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand($@"UPDATE {Habit}
                                                                       SET Date = @date
                                                                       WHERE Id = @id", connection))
                {
                    command.Parameters.Add("@date", SqliteType.Text).Value = text;
                    command.Parameters.Add("@id", SqliteType.Integer).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateRecordQuantity(int id , int quantity)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand($@"UPDATE {Habit}
                                                                       SET Quantity = @quantity
                                                                       WHERE Id = @id", connection))
                {
                    command.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;
                    command.Parameters.Add("@id", SqliteType.Integer).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAllRecord(int id, string text, int quantity)
        {
            UpdateRecordDate(id, text);
            UpdateRecordQuantity(id, quantity);
        }

        public void DeleteRecord(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand($"DELETE FROM {Habit} WHERE Id = @Id", connection))
                {
                    command.Parameters.Add("@Id", SqliteType.Integer).Value = id;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddSeedData()
        {
            Random random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                int quantity = random.Next(1, 20);
                int randomDays = random.Next(0, 365);
                DateTime randomDate = DateTime.Now.AddDays(-randomDays);
                string formattedDate = randomDate.ToString("dd-MM-yy");
                InsertRecord(formattedDate, quantity);
            }
        }

        private class HabitInformation
        {
            public int Id;
            public DateTime Date;
            public int Quantity;
        };
    }
}