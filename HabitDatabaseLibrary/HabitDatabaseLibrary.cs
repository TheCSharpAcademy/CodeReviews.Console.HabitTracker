using Newtonsoft.Json;
using System.Data.SQLite;
using ConsoleTables;


namespace HabitDatabaseLibrary
{
    public class Database
    {
        JsonWriter writer;
        public Database()
        {
            // Create log file and directory

            Directory.CreateDirectory("Logs");
            string fileName = $"Logs/databaselog_{DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss_tt")}.json";
            StreamWriter logFile = File.CreateText(fileName);
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Database Log");
            writer.WriteStartArray();

        }

        void LogError(Exception ex, string habitName = "null", string countIncrement = "null", string currentCount="null")
        {
            writer.WriteValue("Failed");
            writer.WritePropertyName("Habit input");
            writer.WriteValue(habitName);
            writer.WritePropertyName("Count Increment");
            writer.WriteValue(countIncrement);
            writer.WritePropertyName("Current Count");
            writer.WriteValue(currentCount);
            writer.WritePropertyName("Exception Message");
            writer.WriteValue(ex.Message);
            writer.WritePropertyName("Stack Trace");
            writer.WriteValue(ex.StackTrace);
            writer.WritePropertyName("DateTime");
            writer.WriteValue(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            writer.WriteEndObject();
        }

        void LogSuccess(string habitName = "null", string countIncrement = "null", string currentCount = "null")
        {
            writer.WriteValue("Success");
            writer.WritePropertyName("Habit input");
            writer.WriteValue(habitName);
            writer.WritePropertyName("Count Increment");
            writer.WriteValue(countIncrement);
            writer.WritePropertyName("Current Count");
            writer.WriteValue(currentCount);
            writer.WritePropertyName("DateTime");
            writer.WriteValue(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            writer.WriteEndObject();
        }

        public void CreateDatabase(string connectionString)
        {

            using (var conn = new SQLiteConnection(connectionString))
            {

                SQLiteCommand cmd = conn.CreateCommand();
                const string tableQuery = "CREATE TABLE IF NOT EXIST Coffee_Consumed ( Date TEXT, Count int )";

                try
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("Operation");
                    writer.WriteValue("Open Database");
                    writer.WritePropertyName("Result");

                    conn.Open();
                    
                    LogSuccess();

                    writer.WriteStartObject();
                    writer.WritePropertyName("Operation");
                    writer.WriteValue("Create Table");
                    writer.WritePropertyName("Result");

                    cmd.CommandText = tableQuery;
                    cmd.ExecuteNonQuery();
                    
                    LogSuccess();
                }
                catch (Exception ex) { LogError(ex); conn.Close(); }
            }
        }

        public SQLiteDataReader ReadData(string connectionString)
        {
            Console.Clear();

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Read Table");
            writer.WritePropertyName("Result");

            using (var conn = new SQLiteConnection(connectionString))
            {
                List<string> data = new List<string>();
                try
                {
                    conn.Open();
                    const string selectQuery = "SELECT * FROM Coffee_Consumed";

                    SQLiteCommand cmd = conn.CreateCommand();

                    cmd.CommandText = selectQuery;

                    SQLiteDataReader reader = cmd.ExecuteReader();

                    conn.Close();
                    LogSuccess();
                    return reader;
                }
                catch (Exception ex) { LogError(ex); conn.Close(); return null; }
            }
            
        }
        public void InsertData(string connnectionString, string date, int count)
        {
            using (var conn = new SQLiteConnection(connnectionString))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Operation");
                writer.WriteValue("Insert Data");
                writer.WritePropertyName("Result");

                try
                {
                    conn.Open();

                    SQLiteCommand cmd = conn.CreateCommand();
                    string insertHabitSQL = $"INSERT INTO Coffee_Consumed (LogDate, Count) VALUES ('{date}', 0);";

                    cmd.CommandText = insertHabitSQL;
                    cmd.ExecuteNonQuery();

                    LogSuccess();

                    conn.Close();
                }
                catch (Exception ex) { LogError(ex); conn.Close(); }
            }
        }

        public void DeleteData(string connectionString, string date)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Operation");
                writer.WriteValue("Delete Data");
                writer.WritePropertyName("Result");

                try
                {
                    conn.Open();
                    
                    SQLiteCommand sqlite_cmd = conn.CreateCommand();
                    string deleteHabitSQL = $"DELETE FROM Coffee_Consumed WHERE Date S= '{date}';";

                    sqlite_cmd.CommandText = deleteHabitSQL;
                    sqlite_cmd.ExecuteNonQuery();
                    LogSuccess();

                    conn.Close();
                }
                catch (Exception ex) { LogError(ex); conn.Close(); }
            }
        }

        public void UpdateData(string connectionString, string date, int count)
        {

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Update Data");
            writer.WritePropertyName("Result");

            using (var conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    SQLiteDataReader reader;
                    int currentCount;

                    cmd.CommandText = $"SELECT Count FROM Coffee_Consumed WHERE Date = '{date}';";
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    currentCount = reader.GetInt32(0);
                    reader.Close();


                    string updateHabitSQL = $"UPDATE Coffee_Consumed SET Count = {currentCount + count} WHERE Date = '{date}'";
                    cmd.CommandText = updateHabitSQL;
                    cmd.ExecuteNonQuery();
                    LogSuccess();
                    conn.Close();
                }
                catch (Exception ex) { LogError(ex); conn.Close(); }
            } 
        }
   
    }
}


