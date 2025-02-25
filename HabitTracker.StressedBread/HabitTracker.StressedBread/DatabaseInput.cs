using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    internal class DatabaseInput
    {
        Helpers helpers = new();
        string connectionString = @"Data Source=HabitTracker.db";
        internal void CreateTable()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand tableCommand = connection.CreateCommand();

                tableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";

                tableCommand.ExecuteNonQuery();
            }
        }
        internal void Insert()
        {
            string lineRead;
            string date = "";
            int quantity = 0;

            Console.Clear();

            Console.WriteLine("Insert date in dd/mm/yyyy format.");
            lineRead = Console.ReadLine();
            date = helpers.ValidateString(lineRead);

            Console.WriteLine("Insert quantity.");
            lineRead = Console.ReadLine();
            quantity = helpers.ValidateInt(lineRead);

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand insertCommand = connection.CreateCommand();

                insertCommand.CommandText = @"INSERT INTO drinking_water (Date, Quantity)
                                               VALUES (@date, @quantity)
                                               ";
                insertCommand.Parameters.Add("@date", SqliteType.Text).Value = date;
                insertCommand.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;

                insertCommand.ExecuteNonQuery();
            }
        }
        internal void Update()
        {
            string lineRead;
            string date = "";
            int quantity = 0;
            int index = 0;

            Console.Clear();

            Console.WriteLine("Choose which row you want to update.");
            lineRead = Console.ReadLine();
            index = helpers.ValidateInt(lineRead);

            Console.WriteLine("Insert date in dd/mm/yyyy format.");
            lineRead = Console.ReadLine();
            date = helpers.ValidateString(lineRead);

            Console.WriteLine("Insert quantity.");
            lineRead = Console.ReadLine();
            quantity = helpers.ValidateInt(lineRead);

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand updateCommand = connection.CreateCommand();

                updateCommand.CommandText = @"UPDATE drinking_water
                                       SET Date = @date, Quantity = @quantity
                                       WHERE ID = @index
                                       ";

                updateCommand.Parameters.Add("@index", SqliteType.Integer).Value = index;
                updateCommand.Parameters.Add("@date", SqliteType.Text).Value = date;
                updateCommand.Parameters.Add("@quantity", SqliteType.Integer).Value = quantity;

                updateCommand.ExecuteNonQuery();
            }
        }
        internal void Delete()
        {
            int index;

            Console.Clear();

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Choose the index of a row you want to delete.");
            string lineRead = Console.ReadLine();
            index = helpers.ValidateInt(lineRead);

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand deleteCommand = connection.CreateCommand();
                

                deleteCommand.CommandText = @"DELETE FROM drinking_water
                                              WHERE ID = @index
                                              ";

                deleteCommand.Parameters.Add("@index", SqliteType.Integer).Value = index;

                deleteCommand.ExecuteNonQuery();
            }
        }
        internal void View()
        {
            Console.Clear();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand selectCommand = connection.CreateCommand();

                selectCommand.CommandText = @"SELECT * FROM drinking_water
                                       ";

                using (SqliteDataReader reader = selectCommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            var columnValue = reader.GetValue(i);

                            Console.Write($@"{columnName}: {columnValue} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
