using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class DataAccess
    {
        private readonly string ConnectionString = @"Data Source=WalkingHabit-Tracker.db";
        
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
            Console.Clear();
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
                            Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)}   {reader.GetInt32(2)} steps");
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
            Console.Clear();
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

        public void UpdateRecord()
        {
            Console.Clear();
            if (HaveRows())
            {
                int id = GetId();
                int steps = GetSteps();
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = $"UPDATE WalkingHabit SET Steps = {steps} WHERE Id = {id}";
                        rows = command.ExecuteNonQuery();
                    }
                }
                if (rows != 0)
                {
                    Console.WriteLine("Row updated succesfuly");
                }
                else
                {
                    Console.WriteLine("No row has been deleted");
                }
            }
            else
            {
                Console.WriteLine("No habit logged");
                return;
            }
        }

        public void DeleteRecord()
        {
            Console.Clear();
            if (HaveRows())
            {
                int id = GetId();
                int rows = 0;
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = $"DELETE FROM WalkingHabit WHERE Id = {id}";
                        rows = command.ExecuteNonQuery();
                    }
                }
                if (rows != 0)
                {
                    Console.WriteLine("Row deleted succesfuly");
                }
                else
                {
                    Console.WriteLine("No row has been deleted");
                }
            }
            else
            {
                Console.WriteLine("No habit logged");
                return;
            }
        }

        private int GetSteps()
        {
            Console.WriteLine("Enter how many steps u did today:");
            bool succes = int.TryParse(Console.ReadLine(), out int steps);
            while (!succes)
            {
                Console.WriteLine("Invalid value! Try again:");
                succes = int.TryParse(Console.ReadLine(), out steps);
            }
            return steps;
        }

        private int GetId()
        {
            Console.WriteLine("Enter the record's id:");
            bool success = int.TryParse(Console.ReadLine(), out int id);
            while (!success)
            {
                Console.WriteLine("Invalid value ! Try again:");
                success = int.TryParse(Console.ReadLine(), out id);
            }
            return id;
        }

        private bool HaveRows()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = "SELECT * FROM WalkingHabit";

                    var reader = command.ExecuteReader();

                    if (reader.HasRows) return true;

                    Console.WriteLine("No habit has been logged");
                    return false;
                }
            }
        }
    }
}
