using Microsoft.Data.Sqlite;

namespace HabitLogger
{
    public class DatabaseHelper
    {
        public void InsertHabit(string habit, string UnitOfMeasurement, double units)
        {
            using (var connection = new SqliteConnection("Data Source=Logger.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO Logger (Habit, ""Unit of Measurement"", Units) VALUES ($habit, $UnitOfMeasurement, $units)";
                command.Parameters.AddWithValue("$habit", habit);
                command.Parameters.AddWithValue("$UnitOfMeasurement", UnitOfMeasurement);
                command.Parameters.AddWithValue("$units", units);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        public bool ShowValues()
        {
            bool IsEmpty = true;
            using (var connection = new SqliteConnection("Data Source=Logger.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = @"SELECT * FROM Logger";
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("| ID | Habit          | Unit of Measurement | Units  |");
                    Console.WriteLine("-----------------------------------------------------");

                    while (reader.Read())
                    {
                        IsEmpty = false;
                        var id = reader.GetInt32(0);
                        var habit = reader.GetString(1);
                        var unitOfMeasurement = reader.GetString(2);
                        var units = reader.GetString(3);
                        Console.WriteLine($"| {id,-3}| {habit,-15}| {unitOfMeasurement,-20}| {units,-7}|");
                    }
                    Console.WriteLine("\n");
                }
                connection.Close();
            }
            return IsEmpty; 
        }
        public void DeleteAllRecords()
        {
           using (var connection = new SqliteConnection("Data Source=Logger.db"))
           {
                connection.Open();
                var command = connection.CreateCommand();
                 command.CommandText = @"DELETE FROM LOGGER";
                 command.ExecuteNonQuery();
                 connection.Close();
            }              
        }

        public bool CheckIfHabitExists(int id)
        {
            bool exists = false;
            using (var connection = new SqliteConnection("Data Source=Logger.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM LOGGER WHERE ID = $id";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var RowID = reader.GetInt32(0);
                        var habit = reader.GetString(1);
                        var unitOfMeasurement = reader.GetString(2);
                        var units = reader.GetString(3);
                        
                        Console.WriteLine($"| {RowID,-3}| {habit,-15}| {unitOfMeasurement,-20}| {units,-7}|");
                        exists = true;
                    }
                    else
                    {
                        Console.WriteLine($"Habit with ID {id} does not exist.");
                        Console.WriteLine("\n");
                    }
                }
                connection.Close();
                return exists;
            }
        }

        public void DeleteRecord(int id)
        {
            using (var connection = new SqliteConnection("Data Source=Logger.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                Console.WriteLine(id);
                command.CommandText = @"DELETE FROM Logger WHERE ID = $id";
                command.Parameters.AddWithValue("$id", id);

                int rowCount = command.ExecuteNonQuery();
                if (rowCount == 0)  
                {
                    Console.WriteLine($"Record with ID {id} does not exist.\n");
                }
                else
                {
                    Console.WriteLine("Record deleted!\n");
                }
                connection.Close();
            }
        }

        public void UpdateRecord(int id, double increment)
        {    
            using (var connection = new SqliteConnection("Data Source=Logger.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE Logger SET Units = Units + $ans WHERE ID = $id";
                command.Parameters.AddWithValue("$id", id);
                command.Parameters.AddWithValue("$ans", increment);
                command.ExecuteNonQuery();
                connection.Close(); 
            }   
        }
    }
}
