using System;
using System.Data;
// Add to library list
using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create/Access the database in the same folder as Program.cs no matter where the installation is
            string? appFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            int pathLength = appFolderPath.Length - 16;
            string dbFolderPath = appFolderPath.Substring(0, pathLength);

            string connectionString = $"Data Source={dbFolderPath}HabitTracker.db";
            DataBase dataBase = new();
            dataBase.getConnectionString(dbFolderPath);
            DataSeeder.AutoSeed(connectionString);
            dataBase.MainMenu();
        }
    }
}