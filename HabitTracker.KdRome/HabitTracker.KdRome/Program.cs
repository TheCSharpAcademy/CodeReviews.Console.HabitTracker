using Microsoft.Data.Sqlite;
using SQLitePCL;

internal class Program
{
     private static void Main(string[] args) {

          

          string connectionString = @"Data Source=HabitTracker.db";

          using (var connection = new SqliteConnection(connectionString)) {
               Console.Write("we got here\n");
               connection.Open();
               Console.Write("we got here too\n");
               var tableCmd = connection.CreateCommand();

               tableCmd.CommandText = "";

               tableCmd.ExecuteNonQuery(); //means that the data base will not return any values

               connection.Close();
          }
     }
}