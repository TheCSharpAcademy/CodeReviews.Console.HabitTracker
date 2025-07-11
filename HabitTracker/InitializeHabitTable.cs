using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

public static class InitializeDb
{
    public static void CreateHabitTable(string dataSource)
    {
        using var connection = new SqliteConnection(dataSource);
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText =
            """
            DROP TABLE IF EXISTS habit;

            CREATE TABLE habit (
                id INTEGER PRIMARY KEY AUTOINCREMENT ,
                user TEXT NOT NULL,
                habit TEXT NOT NULL,
                count INTEGER NULL,
                date DATETIME NULL
            );
            """;
        command.ExecuteNonQuery();
        connection.Close();
    }

    public static void AddSampleData(string dataSource, string userName)
    {
        var queries = new Queries(dataSource);
        queries.InsertNewHabit(userName, "drinkingCoffee", 20, DateOnly.FromDateTime(DateTime.Now));
        queries.InsertNewHabit(userName, "drinkingWater", 10, DateOnly.FromDateTime(DateTime.Now));
        queries.InsertNewHabit(userName, "exercise", 10, DateOnly.FromDateTime(DateTime.Now));
        queries.InsertNewHabit(userName, "drinkingMoreWater", 10, DateOnly.FromDateTime(DateTime.Now));
        queries.RetrieveHabits(userName);
        queries.UpdateHabit(1, 100);
        queries.RetrieveHabits(userName);
        queries.DeleteHabit(userName, "drinkingCoffee");
        queries.RetrieveHabits(userName);
    }

}
