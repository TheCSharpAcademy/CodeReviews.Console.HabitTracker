using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

class Program {
     private static void Main(string[] args) {

          string connectionString = @"Data Source=HabitTracker.db";

          using (var connection = new SqliteConnection(connectionString)) {
               Console.Write("we got here\n");
               connection.Open();
               Console.Write("we got here too\n");
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
     
     int MainMenu() {
          //options
          Console.WriteLine("Type 0 to Close Application.");
          Console.WriteLine("Type 1 to View All Records.");
          Console.WriteLine("Type 2 to Insert Record.");
          Console.WriteLine("Type 3 to Delete Record.");
          Console.WriteLine("Type 4 to Update Record.");
          
          String UserOption = Console.ReadLine();

          return Convert.ToInt32(UserOption);
     }

}