using Newtonsoft.Json;
using System.Data.SQLite;
using ConsoleTables;
using System.Reflection.PortableExecutable;

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
        public SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection($"Data Source=database.db;Version=3;New=True;Compress=True;");

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Open Database");
            writer.WritePropertyName("Result");

            try
            {
                sqlite_conn.Open();
                LogSuccess();
            }

            catch (Exception ex)
            {
                LogError(ex);
            }


            return sqlite_conn;
        }

        public void CreateTable(SQLiteConnection conn)
        {
            const string habitTableSQL = "CREATE TABLE Habit_Table (HabitName varchar(255), Count int);";
            SQLiteCommand sqlite_cmd = conn.CreateCommand();

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Create Table");
            writer.WritePropertyName("Result");

            try
            {
                sqlite_cmd.CommandText = habitTableSQL;
                sqlite_cmd.ExecuteNonQuery();
                LogSuccess();
            }

            catch (SQLiteException ex)
            {
                LogError(ex);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            
        }

        public void ReadData(SQLiteConnection conn)
        {
            Console.Clear();

            SQLiteDataReader reader;

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Read Table");
            writer.WritePropertyName("Result");

            try
            {
                SQLiteCommand sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = "SELECT * FROM Habit_Table;";

                reader = sqlite_cmd.ExecuteReader();
                var table = new ConsoleTable("Habit Name", "Habit Count");

                while (reader.Read())
                {
                    table.AddRow(reader["HabitName"], reader["Count"]);
                }
                table.Write();


                LogSuccess();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        public void InsertData(SQLiteConnection conn, string habitName)
        {
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            string insertHabitSQL = $"INSERT INTO Habit_Table (HabitName, Count) VALUES ('{habitName}', 0);";
            
            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Insert Data");
            writer.WritePropertyName("Result");

            try
            {
                sqlite_cmd.CommandText = insertHabitSQL;
                sqlite_cmd.ExecuteNonQuery();
                LogSuccess(habitName: habitName);
            }
            catch (Exception ex)
            {
                LogError(ex, habitName: habitName);
            }
        }

        public void DeleteData(SQLiteConnection conn, string habitName)
        {
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            string deleteHabitSQL = $"DELETE FROM Habit_Table WHERE HabitName='{habitName}';";
            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Delete Data");
            writer.WritePropertyName("Result");
            try
            {
                sqlite_cmd.CommandText = deleteHabitSQL;
                sqlite_cmd.ExecuteNonQuery();
                LogSuccess(habitName: habitName);
            }
            catch (Exception ex)
            {
                LogError(ex, habitName: habitName);
            }
        }

        public void UpdateData(SQLiteConnection conn, string habitName, int count)
        {
            SQLiteCommand sqlite_cmd = conn.CreateCommand();
            SQLiteDataReader reader;
            int currentCount;
            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Update Data");
            writer.WritePropertyName("Result");
            try
            {
                
                sqlite_cmd.CommandText = $"SELECT Count FROM Habit_Table WHERE HabitName = '{habitName}';";
                reader = sqlite_cmd.ExecuteReader();
                reader.Read();
                currentCount = reader.GetInt32(0);
                reader.Close();
                
                
                string updateHabitSQL = $"UPDATE Habit_Table SET Count = {currentCount + count} WHERE habitName = '{habitName}'";
                sqlite_cmd.CommandText = updateHabitSQL;
                sqlite_cmd.ExecuteNonQuery();
                LogSuccess(habitName: habitName, currentCount: currentCount.ToString(), countIncrement: count.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void CloseConnection(SQLiteConnection conn)
        {

            writer.WriteStartObject();
            writer.WritePropertyName("Operation");
            writer.WriteValue("Close Connection");
            writer.WritePropertyName("Result");
            
            try
            {
                conn.Close();
                LogSuccess();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            writer.WriteEndArray();
            writer.Close();
        }
    }
}
