using HabitTracker.K_MYR.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.K_MYR
   
{
    class SQLiteOperations
    {
        private static readonly string connectionString = @"Data Source=habit-Tracker.db";
       
        static internal void CreateDbIfNotExists()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Exercise 
                    (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static int Delete(int id)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE FROM Exercise WHERE Id = '{id}'";

            return tableCmd.ExecuteNonQuery();
        }

        internal static void Insert(string table, string date, int quantity)
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO {table} (Date, Quantity) VALUES ('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static List<HabitRecord> SelectAll()
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM EXERCISE";

            List<HabitRecord> tableData = new();
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows) 
            {
                while (reader.Read())
                {
                    tableData.Add(new HabitRecord
                    {
                        Id = reader.GetInt32(0),
                        //Table = "Exercise",
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }                
            }
            
            connection.Close();
            return tableData;
        }

        internal static int RecordExists(int id, string table = "Exercise")
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {table} WHERE Id = {id})";
            return Convert.ToInt32(checkCmd.ExecuteScalar());
        }

        internal static void Update(int id, string date, int quantity, string table = "Exercise")
        {
            using SqliteConnection connection = new(connectionString);
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE {table} SET date = '{date}', quantity = {quantity} WHERE Id = {id}";
            
            tableCmd.ExecuteNonQuery();
            connection.Close();

        }
    }
}
