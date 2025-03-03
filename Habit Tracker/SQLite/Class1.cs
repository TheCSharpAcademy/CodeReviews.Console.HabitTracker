using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace SQLite
{
    public class Database
    {
        SqliteConnection connection = new SqliteConnection("Data Source = calc.db");

        public Database()
        {
            connection.Open();

            //Check if DB tables exist, or creates them
            string commandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table'
                and name = 'Habit'
            ";

            if (!runSelect(commandText).HasRows)
            {
                commandText = @"
                CREATE TABLE Habit(
                    Habit   TEXT    NOT NULL,
                    Description TEXT
                )";
                runCommand(commandText);
            }
        }

        public void runCommand(string commandText)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            command.ExecuteNonQuery();
        }

        public SqliteDataReader runSelect(string commandText)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            SqliteDataReader reader = command.ExecuteReader();
            return reader;
        }

        ~Database()
        {
            connection.Close();
        }
    }
}
