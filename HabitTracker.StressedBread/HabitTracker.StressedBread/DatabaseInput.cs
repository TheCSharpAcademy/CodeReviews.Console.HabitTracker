using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    internal class DatabaseInput
    {
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
            string date = "";
            int quantity = 0;

            Console.Clear();
            Console.WriteLine("Insert date in dd/mm/yyyy format.");
            date = Console.ReadLine();
            Console.WriteLine("Insert quantity.");
            quantity = int.Parse(Console.ReadLine());


            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand insertCommand = connection.CreateCommand();

                insertCommand.CommandText = $@"INSERT INTO drinking_water (Date, Quantity)
                                       VALUES ('{date}', {quantity}
                                       )";

                insertCommand.ExecuteNonQuery();
            }
        }
        internal void Update()
        {
            string date = "";
            int quantity = 0;
            int index = 0;

            Console.Clear();
            Console.WriteLine("Choose which row you want to update.");
            index = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert date in dd/mm/yyyy format.");
            date = Console.ReadLine();
            Console.WriteLine("Insert quantity.");
            quantity = int.Parse(Console.ReadLine());


            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand updateCommand = connection.CreateCommand();

                updateCommand.CommandText = $@"UPDATE drinking_water
                                       SET Date = '{date}', Quantity = {quantity}
                                       WHERE ID = {index}
                                       ";

                updateCommand.ExecuteNonQuery();
            }
        }
        internal void Delete()
        {
            int index = 0;

            Console.Clear();

            Console.WriteLine("------------------------------------");
            Console.WriteLine("Choose which row you want to delete.");
            index = int.Parse(Console.ReadLine());


            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand deleteCommand = connection.CreateCommand();

                deleteCommand.CommandText = $@"DELETE FROM drinking_water
                                       WHERE ID = {index}
                                       ";

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

                selectCommand.CommandText = $@"SELECT * FROM drinking_water
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
