using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HabitTracker.DatabaseManager;

namespace HabitTracker
{
    internal class DatabaseManager
    {

        public struct Entry
        {
            public int Index;
            public DateTime Date;
            public int Quantity;

            public Entry(DateTime Date, int Quantity)
            {
                this.Date = Date;
                this.Quantity = Quantity;
                this.Index = -1;
            }
        }

        string connectionString = @"Data Source=habit-tracker.db";

        //Create DB if not already exists
        public DatabaseManager()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Insert(Entry entry)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    $@"INSERT INTO drinking_water(date, quantity) VALUES('{entry.Date.ToShortDateString()}', {entry.Quantity})";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<Entry> Read()
        {

            List<Entry> list = new List<Entry>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM drinking_water";

                SqliteDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        Entry e = new(
                            DateTime.ParseExact(reader.GetString(1), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            reader.GetInt32(2));
                        e.Index = reader.GetInt32(0);
                        list.Add(e);
                    }
                }

                //command.ExecuteNonQuery();

                connection.Close();
            }

            return list;
        }

        public Entry Read(int index)
        {

            Entry entry = new Entry();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    $@"SELECT * FROM drinking_water WHERE Id={index}";

                SqliteDataReader reader = command.ExecuteReader();

                entry.Index = reader.GetInt32(0);
                entry.Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US"));
                entry.Quantity = reader.GetInt32(2);

                command.ExecuteNonQuery();

                connection.Close();
            }

            return entry;
        }
    }
}
