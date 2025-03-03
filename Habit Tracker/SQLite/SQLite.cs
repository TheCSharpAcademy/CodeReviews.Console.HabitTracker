using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace SQLite
{
    public class SQLite
    {
        SqliteConnection connection = new SqliteConnection("Data Source = calc.db; foreign keys = true");

        public SQLite()
        {
            connection.Open();

            //Retrieve DB tables
            string commandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table'
            ";

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            SqliteDataReader reader = command.ExecuteReader();
            List<string> tables = new List<string>();

            while (reader.Read())
            {
                tables.Add(reader.GetString(0));
            }

            //Check for Habit, or create
            if (!tables.Contains("Habit"))
            {
                commandText = @"
                CREATE TABLE Habit (
                    Habit       TEXT    PRIMARY KEY,
                    Description TEXT    NOT NULL,
                    UoM         TEXT    NOT NULL
                )";
                runCommand(commandText);
            }

            //Check for Log, or create
            if (!tables.Contains("Log"))
            {
                commandText = @"
                CREATE TABLE Log (
                    ID          INTEGER     PRIMARY KEY,
                    Habit       TEXT        NOT NULL,
                    Count       INTEGER     NOT NULL,
                    Date        INTEGER     NOT NULL,
                    FOREIGN KEY (Habit)
                    REFERENCES Habit (Habit)
                        ON DELETE CASCADE
                        ON UPDATE CASCADE
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

        public List<string[]> runSelect(string commandText)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            SqliteDataReader reader = command.ExecuteReader();
            List<string[]> content = new List<string[]>();

            //Iterate through returned rows, building a string[], and adding to content
            while (reader.Read())
            {
                string[] row = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[i] = reader.GetString(i);
                }
                content.Add(row);
            }
            
            return content;
        }

        ~SQLite()
        {
            //Make sure to close connection on destructor
            connection.Close();
        }
    }
}
