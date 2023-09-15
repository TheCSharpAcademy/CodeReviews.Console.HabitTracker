using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
namespace DatabaseConnection
{
    public class DataAccess
    {
        private string ConnectionString = @"Data Source=Walking-Habit.db;Version=3;";

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
        }

        public void Insert()
        {
            int StepsCount = VerifyAndGetSteps();
            string date = DateTime.Now.Date.ToString();
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
            int id = VerifyAndGetId();
            int rows;
            using(var connection = new SQLiteConnection(ConnectionString))
            {
                using(var deleteCommand = connection.CreateCommand())
                {
                    connection.Open();
                    deleteCommand.CommandText = $"DELETE FROM WalkingHabit WHERE Id = {id}";
                    rows = deleteCommand.ExecuteNonQuery();
                }
            }
            if(rows == 0)
            {
                Console.WriteLine("No rows deleted");
            }
            else
            {
                Console.WriteLine("Row deleted succesfully");
            }
        }

        private int VerifyAndGetId()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {

        }
        private int VerifyAndGetSteps()
        {
            int steps;
            Console.WriteLine("Enter the number of steps");
            bool success = int.TryParse(Console.ReadLine(), out steps);
            while (!success)
            {
                Console.WriteLine("Invalid value! Try again:");
                success = int.TryParse(Console.ReadLine(), out steps);
            }
            return steps;
        }

    }
}
