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

            string sqlText =
                @"CREATE TABLE WaterDringking
                (
                    Id          INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,  
                    Date        TEXT    NOT NULL,
                    Quantity    INTEGER NOT NULL
                )";

            var command = new SqliteCommand(sqlText, connection);

            command.ExecuteNonQuery();
        }
    }
}
