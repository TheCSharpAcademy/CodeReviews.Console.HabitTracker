
using Microsoft.Data.Sqlite;
using System.Data.SqlTypes;

Console.WriteLine("Hello, World!");

string connectionString = @"Data Source=HabitTracker.db";


using (var connection = new SqliteConnection(connectionString)) {



}