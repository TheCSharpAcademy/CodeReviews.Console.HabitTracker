using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;
using SQLitePCL;

class Program {
     private static void Main(string[] args) {

          SQLitePCL.Batteries.Init();

          string connectionString = @"Data Source=Habit-Tracker.db";

          using (var connection = new SqliteConnection(connectionString)) {
               
               connection.Open();
               
               var tableCmd = connection.CreateCommand();

               tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                         Id INTEGER PRIMARY KEY AUTOINCREMENT,
                         Date TEXT,
                         Quantity INTEGER
                         )";

               tableCmd.ExecuteNonQuery(); //means that the data base will not return any values

               connection.Close();
               
          }
     }
}