using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.Database;

internal static class DatatBaseOperations
{
    internal static void CreateDatabase()
    {
        using(var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE WaterDrinking
                (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,  
                    Date        TEXT    NOT NULL,
                    Quantity    INTEGER NOT NULL
                )";

            command.ExecuteNonQuery();
        }
    }

    internal static void AddData(string date, int quantity)
    {
        using (var connection = new SqliteConnection("Data Source=HabitTracker.db"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @$"INSERT INTO TABLE WaterDrinking (Date, Quantity)
                Values('{date}', {quantity}";

            command.ExecuteNonQuery();
        }
    }
}
