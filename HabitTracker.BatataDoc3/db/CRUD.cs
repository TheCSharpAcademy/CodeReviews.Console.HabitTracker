using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace HabitTracker.BatataDoc3.db
{

    internal class CRUD
    {
        private SqliteConnection? conn;

        public CRUD()
        {
            conn = null;
        }

        public void startDatabase()
        {
            String sql = @"CREATE TABLE habits(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                date DATE
            )";
            try 
            {
                bool exists = checkIfDbExists();
                Console.WriteLine(exists);
                conn = new SqliteConnection(@"Data Source=db\habits.db");
                conn.Open();
                Console.WriteLine("db connected!");
                if (!exists)
                {
                    Console.WriteLine("Hello");
                    using var command = new SqliteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Table habits created with success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public bool checkIfDbExists()
        {
            if(File.Exists(@"db\habits.db")) return true;
            return false;
        }

        public SqliteConnection getConnection() { return conn; }


        public void InsertRecord(string input, DateTime dt)
        {
            string sql = "INSERT INTO habits (name, date) VALUES(@name, @date)";
            
            using var command = new SqliteCommand(sql, conn);
            command.Parameters.AddWithValue("@name", input);
            command.Parameters.AddWithValue("@date", dt);
            command.ExecuteNonQuery();
        }
    }
}
