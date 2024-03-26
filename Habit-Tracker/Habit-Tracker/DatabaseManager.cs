using System;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Globalization;

namespace Habit_Tracker
{
    public class DatabaseManager
    {
        readonly private string dbFile;
        public DatabaseManager(string dbFile)
        {
            this.dbFile = dbFile;
            this.CheckTableExsits();
        }
        public static long GetTodayDate() => DateTime.Today.Ticks;
        private void CheckTableExsits()
        {
            using (var connection = new SqliteConnection($"Data Source={this.dbFile}"))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                   @"create table if not exists drinking_water (
                        drinkgingDate Date primary key,
                        Quantity INTEGER
                    )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        public Boolean InsertDrinkingWater(DateOnly drinkingDate, int quantity)
        {
            string connectionString = $"Data Source={this.dbFile}";

            // Create a connection to the database
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Check if the record already exists
                string checkSql = "SELECT COUNT(*) FROM drinking_water WHERE drinkgingDate = @drinkgingDate";
                using (var checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@drinkgingDate", drinkingDate);
                    int existingRecordsCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (existingRecordsCount > 0)
                    {
                        Console.WriteLine("Record already exists for this date.");
                        return false;
                    }
                }

                // SQL command to insert data into the drinking_water table
                string sql = @"INSERT INTO drinking_water (drinkgingDate, Quantity) 
                           VALUES (@drinkgingDate, @Quantity)";

                // Create a command object
                using (var command = new SQLiteCommand(sql, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@drinkgingDate", drinkingDate);
                    command.Parameters.AddWithValue("@Quantity", quantity);

                    // Execute the command
                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqliteException e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Dispose();
                    }
                }
            }
        }

        public Boolean UpdateDrinkingWater(DateOnly drinkingDate, int quantity)
        {
            string connectionString = $"Data Source={this.dbFile}";

            // Create a connection to the database
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Check if the record already exists
                string checkSql = "SELECT COUNT(*) FROM drinking_water WHERE drinkgingDate = @drinkgingDate";
                using (var checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@drinkgingDate", drinkingDate);
                    int existingRecordsCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (existingRecordsCount == 0)
                    {
                        Console.WriteLine("Record not found!");
                        return false;
                    }
                    else
                    {
                        // SQL command to update data in the drinking_water table
                        string updateSql = @"UPDATE drinking_water SET Quantity = @Quantity WHERE drinkgingDate = @drinkgingDate";
                        using (var updateCommand = new SQLiteCommand(updateSql, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                            updateCommand.Parameters.AddWithValue("@drinkgingDate", drinkingDate);

                            try
                            {
                                int rowsAffected = updateCommand.ExecuteNonQuery();
                                return true;
                            }
                            catch (SqliteException e)
                            {
                                Console.WriteLine(e.Message);
                                return false;
                            }
                        }
                    }
                }
            }
        }

        public Boolean DeleteDrinkingWater(DateOnly drinkingDate)
        {
            string connectionString = $"Data Source={this.dbFile}";

            // Create a connection to the database
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Check if the record already exists
                string checkSql = "SELECT COUNT(*) FROM drinking_water WHERE drinkgingDate = @drinkgingDate";
                using (var checkCommand = new SQLiteCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@drinkgingDate", drinkingDate);
                    int existingRecordsCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (existingRecordsCount == 0)
                    {
                        Console.WriteLine("Record not found!");
                        return false;
                    }
                    else
                    {
                        // SQL command to update data in the drinking_water table
                        string deleteSql = @"delete from drinking_water WHERE drinkgingDate = @drinkgingDate";
                        using (var deleteCommand = new SQLiteCommand(deleteSql, connection))
                        {
                            deleteCommand.Parameters.AddWithValue("@drinkgingDate", drinkingDate);

                            try
                            {
                                int rowsAffected = deleteCommand.ExecuteNonQuery();
                                return true;
                            }
                            catch (SqliteException e)
                            {
                                Console.WriteLine(e.Message);
                                return false;
                            }
                        }
                    }
                }
            }
        }

        public void RetrieveAllRecords()
        {
            string connectionString = $"Data Source={this.dbFile}";

            // Create a connection to the database
            using (var connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // SQL command to retrieve all drinking water records
                string retrieveSql = "SELECT drinkgingDate, Quantity FROM drinking_water order by drinkgingDate";

                // Create a command object
                using (var command = new SQLiteCommand(retrieveSql, connection))
                {
                    // Execute the command and retrieve data
                    using (var reader = command.ExecuteReader())
                    {
                        // Check if there are any records
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No drinking water records found.");
                            return;
                        }

                        // Display headers
                        Console.WriteLine("Drinking Water Records:");
                        Console.WriteLine("Date\t\tQuantity");

                        // Loop through the records and display them
                        while (reader.Read())
                        {
                            string dateString = reader.GetString(0);
                            int quantity = reader.GetInt32(1);

                            Console.WriteLine($"{dateString}\t{quantity}");
                        }
                    }
                }
            }
        }
    } 
}