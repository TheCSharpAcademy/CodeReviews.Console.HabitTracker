using Microsoft.Data.Sqlite;

namespace HabitTracker.Chad1082
{
    internal static class Database
    {

        private static readonly string dbName = "Logger.db";
        private static readonly string connString = @"Data Source=" + dbName;

        public static void SetupDB()
        {
            if (!DatabaseExists())
            {
                Console.WriteLine("Database does not exist, creating...");
                CreateDatabase();
            }
        }
        private static bool DatabaseExists()
        {
            bool doesExist = false;
            string dbLocation = Path.Combine(Environment.CurrentDirectory,dbName);

            doesExist = File.Exists(dbLocation);
            return doesExist;
        }

        private static bool CreateDatabase()
        {
            bool created = false;
            
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"CREATE TABLE ""Steps"" (
	                                    ""EntryID""	INTEGER NOT NULL UNIQUE,
	                                    ""DateAdded""	TEXT NOT NULL,
	                                    ""Steps""	INTEGER NOT NULL,
	                                    PRIMARY KEY(""EntryID"" AUTOINCREMENT)
                                    );";

                command.ExecuteNonQuery();
                conn.Close();
                created = true;

            }

            return created;
        }

        public static void AddEntry(string stepDate, int stepAmount)
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"INSERT INTO ""main"".""Steps""(""DateAdded"",""Steps"") VALUES ($stepDate,$stepAmount);";

                command.Parameters.AddWithValue("$stepDate", stepDate);
                command.Parameters.AddWithValue("$stepAmount", stepAmount);
                command.ExecuteNonQuery();
                conn.Close();
                

            }
        }

        public static List<Entry> GetEntries()
        {
            List<Entry> entries = new();

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM ""main"".""Steps""";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        entries.Add(new Entry
                        {
                            EntryID = int.Parse(reader["EntryID"].ToString()),
                            DateAdded = reader["DateAdded"].ToString(),
                            Steps = int.Parse(reader["Steps"].ToString())
                        });
                    }
                    
                }

                conn.Close();
            }

            
            return entries;
        }

        public static Entry GetSingleEntry(int entryID)
        {
            Entry entry = new();

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM ""main"".""Steps"" WHERE ""EntryID""=$entryId";
                command.Parameters.AddWithValue("$entryId",entryID);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        entry.EntryID = int.Parse(reader["EntryID"].ToString());
                        entry.DateAdded = reader["DateAdded"].ToString();
                        entry.Steps = int.Parse(reader["Steps"].ToString());
                        conn.Close();

                        return entry;
                    }
                }
            }

            return null;
            
        }

        public static bool DeleteEntry(int entryId)
        {
            bool result = false;

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"DELETE FROM ""main"".""Steps"" WHERE ""EntryID"" = $entrytotelete";
                command.Parameters.AddWithValue("$entrytotelete", entryId);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        result = true;
                    }

                }

                conn.Close();
            }

            return result;
        }

        public static bool UpdateEntry(int entryId, string newDate, int newSteps)
        {
            bool result = false;

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"UPDATE ""main"".""Steps"" SET ""DateAdded""=$newDate, ""Steps""=$newSteps WHERE ""EntryID""=$entryID";
                command.Parameters.AddWithValue("$entryID", entryId);
                command.Parameters.AddWithValue("$newDate", newDate);
                command.Parameters.AddWithValue("$newSteps", newSteps);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        result = true;
                    }

                }

                conn.Close();
            }

            return result;
        }

    }
}
