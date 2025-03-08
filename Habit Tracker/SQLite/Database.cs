using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Database
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
                    UoM         TEXT    NOT NULL
                );");
            }

            //Check for Log table, or create
            if (!tables.Contains("Log"))
            {
                RunCommand(@"
                CREATE TABLE Log (
                    ID          INTEGER     PRIMARY KEY,
                    Habit       TEXT        NOT NULL,
                    Count       INTEGER     NOT NULL,
                    Date        TEXT        NOT NULL,
                    FOREIGN KEY (Habit)
                        REFERENCES Habit (Habit)
                        ON DELETE CASCADE
                        ON UPDATE CASCADE
                );");
            }
        }

        public List<string[]> GetHabits()
        {
            string commandText = @"
                SELECT Habit, Description, UoM
                FROM Habit;
            ";

            return ReturnRows(RunSelect(commandText));
        }

        public void CreateHabit(string habitName, string habitDescription, string habitUoM)
        {
            string commandText = @"
                INSERT INTO Habit (Habit, Description, UoM)
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

        public void UpdateHabit(string habit, string? newName, string? newDescription, string? newUoM)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            List<string> habitArray = new List<string> { newName, newDescription, newUoM };
#pragma warning restore CS8604 // Possible null reference argument.

            if (habitArray.Count > 0)
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                string commandText = "UPDATE Habit SET";

                //Add each parameter if not-null, catering for additional ',' required as necessary
                if (habitArray[0] != "")
                {
                    commandText += $" Habit = $newName";
                    if (habitArray[1] != "" || habitArray[2] != "") 
                        commandText += ",";
                    parameters.Add(new KeyValuePair<string, string>("$newName", newName));

                }
                if (habitArray[1] != "")
                {
                    commandText += $" Description = $newDescription";
                    if (habitArray[2] != "")
                        commandText += ",";
                    parameters.Add(new KeyValuePair<string, string>("$newDescription", newDescription));
                }
                if (habitArray[2] != "")
                {
                    commandText += $" UoM = $newUoM";
                    parameters.Add(new KeyValuePair<string, string>("$newUoM", newUoM));
                }

                commandText += $" WHERE habit = $habit;";
                parameters.Add(new KeyValuePair<string, string>("$habit", habit));

                RunCommand(commandText, parameters.ToArray());
            }
            else
            {
                throw new ArgumentException("No inputs provided...");
            }
        }

        public void DeleteHabit(string habit)
        {
            string commandText = @"
                DELETE FROM Habit
                WHERE habit = $habit
            ;";

            KeyValuePair<string, string>[] parameters = new KeyValuePair<string, string>[1]
            {
                new KeyValuePair<string, string>("$habit", habit)
            };

            RunCommand(commandText, parameters);
        }

        public List<string[]> GetLogs()
        {
            string commandText = @"
                SELECT ID, Log.Habit, Count, UOM, Date
                FROM Log
                LEFT JOIN Habit ON Log.Habit = Habit.Habit;
                ORDER BY ID DESC
            ";

            return ReturnRows(RunSelect(commandText));
        }

        public void CreateLog(string habit, int count, DateTime date)
        {
            string commandText = @"
                INSERT INTO Log (Habit, Count, Date)
                VALUES ($habit, $count, $date);
            ";

            KeyValuePair<string, string>[] parameters = new KeyValuePair<string, string>[3]
            {
                new KeyValuePair<string, string>("$habit", habit),
                new KeyValuePair<string, string>("$count", count.ToString()),
                new KeyValuePair<string, string>("$date", date.ToString("yyyy-MM-dd"))
            };

            RunCommand(commandText, parameters);
        }

        public void DeleteLog(string id)
        {
            string commandText = @"
                DELETE FROM Log
                WHERE ID = $id
            ;";

            KeyValuePair<string, string>[] parameters = new KeyValuePair<string, string>[1]
            {
                new KeyValuePair<string, string>("$id", id)
            };

            RunCommand(commandText, parameters);
        }

        private List<string> GetTables()
        {
            string commandText = @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table';
            ";

            SqliteDataReader reader = RunSelect(commandText);
            return ReturnColumn(reader);
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
        private List<string[]> ReturnRows(SqliteDataReader reader)
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
