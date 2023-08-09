
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

Console.WriteLine("Hello, World!");

string connectionString = @"Data Source=HabitTracker.db";


using (var connection = new SqliteConnection(connectionString)) {
     //opens the connection
     connection.Open();
     //table command
     var tableCmd = connection.CreateCommand();

     tableCmd.CommandText = "";

     tableCmd.ExecuteNonQuery(); //means that the data base will not return any values

     connection.Close();
}