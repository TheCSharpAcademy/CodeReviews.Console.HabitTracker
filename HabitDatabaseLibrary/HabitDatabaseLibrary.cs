using System.Data.SQLite;
using ConsoleTables;


namespace HabitDatabaseLibrary
{
    public class Database
    {
        public void CreateDatabase(string connectionString)
        {

            using (var conn = new SQLiteConnection(connectionString))
            {

                SQLiteCommand cmd = conn.CreateCommand();
                const string tableQuery = "CREATE TABLE IF NOT EXISTS Coffee_Consumed ( LogDate TEXT, Count int );";

                try
                {
                    conn.Open();
                    cmd.CommandText = tableQuery;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex) { conn.Close(); }
            }
        }

        public bool ReadData(string connectionString, string check = "", bool ifExists = false)
        {
            Console.Clear();

            using (var conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    const string selectQuery = "SELECT * FROM Coffee_Consumed";

                    SQLiteCommand cmd = conn.CreateCommand();

                    cmd.CommandText = selectQuery;

                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (ifExists)
                    {
                        while (reader.Read())
                        {
                            if (reader["LogDate"].ToString().Contains(check))
                            {
                                reader.Close();
                                conn.Close();
                                return true;
                            }
                        }
                        reader.Close();
                        conn.Close();
                        return false;
                    }
                    else
                    {


                        var table = new ConsoleTable("Date", "Cups of coffee Consumed.");
                        while (reader.Read())
                        {
                            table.AddRow(reader["LogDate"], reader["Count"]);
                        }
                        table.Write();
                        reader.Close();
                        conn.Close();
                        return false;
                    }
                    
                }
                catch (Exception ex) { conn.Close(); return false; }
            }
            
        }
        public void InsertData(string connnectionString, string date, int count)
        {
            using (var conn = new SQLiteConnection(connnectionString))
            {

                try
                {
                    conn.Open();

                    SQLiteCommand cmd = conn.CreateCommand();
                    string insertHabitSQL = $"INSERT INTO Coffee_Consumed (LogDate, Count) VALUES ('{date}', {count});";

                    cmd.CommandText = insertHabitSQL;
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception ex) { conn.Close(); }
            }
        }

        public void DeleteData(string connectionString, string date)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    
                    SQLiteCommand sqlite_cmd = conn.CreateCommand();
                    string deleteHabitSQL = $"DELETE FROM Coffee_Consumed WHERE LogDate='{date}';";

                    sqlite_cmd.CommandText = deleteHabitSQL;
                    sqlite_cmd.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception ex) { conn.Close(); }
            }
        }

        public void UpdateData(string connectionString, string date, int count, int currentCount)
        {


            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = $"UPDATE Coffee_Consumed SET Count={currentCount + count} WHERE LogDate='{date}';";
                    using (var cmd = new SQLiteCommand(updateQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }

            }
            catch (Exception ex) { Console.Write(ex); }
        }
        public int GetCurrentCount(string connectionString, string date)
        {


            try
            {
                int currentCount;

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string selectQuery = $"SELECT Count FROM Coffee_Consumed WHERE LogDate='{date}';";
                    using (var cmd = new SQLiteCommand(selectQuery, conn))
                    {
                        cmd.CommandText = selectQuery;
                        currentCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    conn.Close();
                }
                return currentCount;

            }
            catch (Exception ex) { Console.Write(ex); Console.ReadLine(); return 0; }
        }

    }
}


