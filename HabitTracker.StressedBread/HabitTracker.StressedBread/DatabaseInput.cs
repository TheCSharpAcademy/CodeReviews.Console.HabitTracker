using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    internal class DatabaseInput
    {
        Helpers helpers = new();
        DatabaseService databaseService = new();
        internal void CreateTable()
        {
            string commandText = @"CREATE TABLE IF NOT EXISTS drinking_water ( 
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Date TEXT,
                                Quantity INTEGER
                                )";
            databaseService.ExecuteCommand(commandText);
        }
        internal void Insert()
        {
            Console.Clear();

            string? date = helpers.ValidateDate("Insert date in dd/mm/yyyy format.");
            int quantity = helpers.ValidateInt("Insert quantity.");

            string commandText = @"INSERT INTO drinking_water (Date, Quantity)
                       VALUES (@date, @quantity)
                       ";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"date", date),
                new SqliteParameter(@"quantity", quantity)
            };

            databaseService.ExecuteCommand(commandText, parameters);
        }
        internal void Update()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which row you want to update.");


            string selectText = @"SELECT ID FROM drinking_water
                                  WHERE ID = @index";
            string updateText = @"UPDATE drinking_water
                                  SET Date = @date, Quantity = @quantity
                                  WHERE ID = @index
                                  ";
            List<SqliteParameter> selectParameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index)
            };

            using (SqliteDataReader reader = databaseService.ExecuteRead(selectText, selectParameters))
            {
                if (reader.Read())
                {
                    string? date = helpers.ValidateDate("Insert date in dd/mm/yyyy format.");
                    int quantity = helpers.ValidateInt("Insert quantity.");

                    List<SqliteParameter> updateParameters = new List<SqliteParameter>()
                    {
                        new SqliteParameter(@"index", index),
                        new SqliteParameter(@"date", date),
                        new SqliteParameter(@"quantity", quantity)
                    };

                    reader.Close();
                    databaseService.ExecuteCommand(updateText, updateParameters);
                    Console.WriteLine("Succefully updated! Press any key to return to menu.");
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Specified row doesn't exist! Press any key to return to menu.");
                }
                Console.ReadKey();
            }
        }
        internal void Delete()
        {
            Console.Clear();

            int index = helpers.ValidateInt("Choose which row you want to update.");

            string selectText = @"SELECT ID FROM drinking_water
                                  WHERE ID = @index";
            string deleteText = @"DELETE FROM drinking_water
                                  WHERE ID = @index";
            List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                new SqliteParameter(@"index", index),
            };

            using (SqliteDataReader reader = databaseService.ExecuteRead(selectText, parameters))
            {
                if (reader.Read())
                {
                    reader.Close();
                    databaseService.ExecuteCommand(deleteText, parameters);
                    Console.WriteLine("Succefully deleted! Press any key to return to menu.");
                }
                else
                {
                    reader.Close();
                    Console.WriteLine("Specified row doesn't exist! Press any key to return to menu.");
                }
                Console.ReadKey();
            }
        }
        internal void View()
        {
            Console.Clear();

            string commandText = @"SELECT * FROM drinking_water
                                   ";

            using (SqliteDataReader reader = databaseService.ExecuteRead(commandText))
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

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
