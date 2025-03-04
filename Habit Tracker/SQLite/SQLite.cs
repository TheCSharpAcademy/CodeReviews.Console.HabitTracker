using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace SQLite
{
    public class SQLite
    {
        SqliteConnection connection = new SqliteConnection("Data Source = habit.db; foreign keys = true");

        public SQLite()
        {
            connection.Open();

            //Retrieve DB tables
            List<string> tables = GetTables();

            //Check for Habit table, or create
            if (!tables.Contains("Habit"))
            {
                RunCommand(@"
                CREATE TABLE Habit (
                    Habit       TEXT    PRIMARY KEY,
                    Description TEXT    NOT NULL,
                    UoM         TEXT    NOT NULL;
                )");
            }

            //Check for Log table, or create
            if (!tables.Contains("Log"))
            {
                RunCommand(@"
                CREATE TABLE Log (
                    ID          INTEGER     PRIMARY KEY,
                    Habit       TEXT        NOT NULL,
                    Count       INTEGER     NOT NULL,
                    Date        INTEGER     NOT NULL,
                    FOREIGN KEY (Habit)
                    REFERENCES Habit (Habit)
                        ON DELETE CASCADE
                        ON UPDATE CASCADE;
                )");
            }
        }

        public List<string> GetTables()
        {
            string commandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table';
            ";

            SqliteDataReader reader = RunSelect(commandText);
            return ReturnColumn(reader);
        }

        public List<string[]> GetHabits()
        {
            string commandText = @"
                SELECT Habit, Description, UoM
                FROM Habit;
            ";

            SqliteDataReader reader = RunSelect(commandText);
            return ReturnRow(reader);
        }

        public void CreateHabit(string habitName, string habitDescription, string habitUoM)
        {
            string commandText = @"
                INSERT INTO Habit (habit, description, uom)
                VALUES ($habit, $description, $uom);
            ";

            KeyValuePair<string, string>[] parameters = new KeyValuePair<string, string>[3]
            {
                new KeyValuePair<string, string>("$habit", habitName),
                new KeyValuePair<string, string>("$description", habitDescription),
                new KeyValuePair<string, string>("$uom", habitUoM)
            };

            RunCommand(commandText, parameters);
        }

        private void RunCommand(string commandText, KeyValuePair<string, string>[]? parameters = null)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            //If parameters are passed, add to command
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    command.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
            }

            command.ExecuteNonQuery();
        }

        private SqliteDataReader RunSelect(string commandText)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = commandText;

            SqliteDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Returns all rows of a RunSelect() that only has a single column
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<string> ReturnColumn(SqliteDataReader reader)
        {
            List<string> content = new List<string>();

            while (reader.Read())
            {
                content.Add(reader.GetString(0));
            }

            return content;
        }

        /// <summary>
        /// Returns all rows of a RunSelect()
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<string[]> ReturnRow(SqliteDataReader reader)
        {
            List<string[]> content = new List<string[]>();

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
