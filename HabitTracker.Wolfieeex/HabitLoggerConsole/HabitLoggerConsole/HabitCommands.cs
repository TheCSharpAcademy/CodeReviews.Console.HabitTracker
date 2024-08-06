using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLoggerConsole
{
    internal class HabitCommands
    {
        internal static void InsertHabit()
        {
            //*
            using (var connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();

                //var tableCmd = connection.CreateCommand();

                //tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS going_to_gym (
                //                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                //                        Date TEXT,
                //                        Sets TEXT
                //                        )";

                //tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static void ViewAllHabits()
        {
            throw new NotImplementedException();
        }
        internal static void UpdateHabit()
        {
            throw new NotImplementedException();
        }
        internal static void DeleteHabit()
        {
            throw new NotImplementedException();
        }
    }
}
