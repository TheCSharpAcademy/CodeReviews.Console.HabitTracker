using System;
using System.Data.SQLite;
namespace DatabaseConnection
{
    public class DataAccess
    {
        private readonly string ConnectionString = @"Data Source=Walking-Habit.db;Version=3;";

        public void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var createTableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    createTableCommand.CommandText = "CREATE TABLE IF NOT EXISTS WalkingHabit (Id INTEGER PRIMARY KEY AUTOINCREMENT,Date TEXT,NumberOfSteps INTEGER)";
                    createTableCommand.ExecuteNonQuery();
                }
            }
        }
        public void ViewAll()
        {
            Console.Clear();
            using(var connection = new SQLiteConnection(ConnectionString))
            {
                using(var viewCommand = connection.CreateCommand())
                {
                    connection.Open();
                    viewCommand.CommandText = "SELECT * FROM WalkingHabit";

                    var reader = viewCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)} - {reader.GetDouble(2)} steps");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No habit has been logged");
                    }
                }
            }
        }
        public void Insert()
        {
            string date = DateTime.Now.Date.ToString();
            if (!ValidateDate(date))
            {
                Console.WriteLine("Date exists.You should update it if you did more steps today.");
                return;
            }
            int StepsCount = VerifyAndGetSteps();
            int rows;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var insertCommand = connection.CreateCommand())
                {
                    connection.Open();
                    insertCommand.CommandText = $"INSERT INTO WalkingHabit(Date,NumberOfSteps) VALUES ('{date}','{StepsCount}')";
                    rows = insertCommand.ExecuteNonQuery();
                }
            }
            if (rows == 0)
            {
                Console.WriteLine("No rows added");
            }
            else
            {
                Console.WriteLine("Row added succesfully");
            }
        }
        public void Delete()
        {
            if (!HaveRows())
            {
                Console.WriteLine("No habit logged");
                return;
            }
            Console.WriteLine("Enter the row's id you want to delete:");
            int id = VerifyAndGetId();
            int rows;
            using(var connection = new SQLiteConnection(ConnectionString))
            {
                using(var deleteCommand = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        deleteCommand.CommandText = $"DELETE FROM WalkingHabit WHERE Id = {id}";
                        rows = deleteCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }       
                }
            } 
        }
        private int VerifyAndGetId()
        {
            bool success = int.TryParse(Console.ReadLine(), out int id);
            while (!success)
            {
                Console.WriteLine("Invalid value! Try again:");
                success = int.TryParse(Console.ReadLine(), out id);
            }
            return id;
        }
        public void Update()
        {
            if (!HaveRows())
            {
                Console.WriteLine("No habit logged");
                return;
            }
            Console.WriteLine("Enter the row's id you want to update:");
            int id = VerifyAndGetId();
            int steps = VerifyAndGetSteps();
            using(var connection = new SQLiteConnection(ConnectionString))
            {
                using(var updateCommand = connection.CreateCommand())
                {
                    connection.Open();
                    updateCommand.CommandText = $"UPDATE WalkingHabit SET NumberOfSteps = {steps} WHERE Id = {id};";
                    updateCommand.ExecuteNonQuery();
                }
            }
        }
        private int VerifyAndGetSteps()
        {
            Console.WriteLine("Enter the number of steps");
            bool success = int.TryParse(Console.ReadLine(), out int steps);
            while (!success)
            {
                Console.WriteLine("Invalid value! Try again:");
                success = int.TryParse(Console.ReadLine(), out steps);
            }
            return steps;
        }
        private bool ValidateDate(string Date)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var viewCommand = connection.CreateCommand())
                {
                    connection.Open();

                    viewCommand.CommandText = "SELECT * FROM WalkingHabit";

                    var reader = viewCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                       
                            if(reader.GetString(1) == Date)
                            {
                                return false;
                            }
                        }
                        
                    }
                    return true;
                }
            }
        }
        private bool HaveRows()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var viewCommand = connection.CreateCommand())
                {
                    connection.Open();
                    viewCommand.CommandText = "SELECT * FROM WalkingHabit";

                    var reader = viewCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
    }

