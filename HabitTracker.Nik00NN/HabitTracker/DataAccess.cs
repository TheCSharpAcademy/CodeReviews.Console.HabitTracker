using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    public class DataAccess
    {
        private readonly string ConnectionString = "";
        
        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                using(var createTableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    createTableCommand.CommandText =
                        @"CREATE TABLE IF NOT EXISTS WalkingHabit ( Id INTEGER PRIMARY KEY AUTOINCREMENT,
                          Date TXT,
                          Steps INTEGER)";
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
        public void ViewRecords()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM WalkingHabit";

                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)}   {reader.GetInt32} steps");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No habit has been logged");
                    }
                    Console.WriteLine();
                }
            }
        }
        public void InsertRecord()
        {
            string date = DateTime.Now.Date.ToString();
            int steps = GetSteps();

            using(var connection = new SqliteConnection(ConnectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO WalkingHabit (Date,Steps) VALUES ('{date}','{steps}')";
                    command.ExecuteNonQuery();
                }
            }
            
        }

        public void UpdateRecord(int id,int steps)
        {
            using(var connection = new SqliteConnection(ConnectionString))
            {
                using(var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = $"UPDATE WalkingHabit SET Steps = {steps} WHERE Id = {id}";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecord(int id)
        {
            using(var connection = new SqliteConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();

                    command.CommandText = $"DELETE FROM WalkingHabit WHERE Id = {id}";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
