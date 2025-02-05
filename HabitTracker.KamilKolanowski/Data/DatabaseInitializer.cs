using System;
using System.Data.SQLite;
using System.IO;

namespace HabitTracker.KamilKolanowski.Data;

public static class DatabaseInitializer
{
    private const string DatabaseFileName = "dbapp.db";
    private const string ConnectionString = "Data Source=dbapp.db;Version=3;";

    public static void InitializeDb()
    {
        CreateDatabaseIfNotExists();

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        
        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Habits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Habit TEXT,
                Quantity INTEGER,
                InsertDate INTEGER
            );
        ";
        
        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error creating table: " + ex.Message);
        }
    }

    public static void CreateDatabaseIfNotExists()
    {
        if (!File.Exists(DatabaseFileName))
        {
            Console.WriteLine("Database does not exist. Creating new one...");
            SQLiteConnection.CreateFile(DatabaseFileName);
        }
    }
}