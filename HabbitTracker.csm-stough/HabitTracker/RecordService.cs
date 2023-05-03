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
    internal class RecordService
    {

        static string connectionString = @"Data Source=habit-tracker.db";

        public static void Init()
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

        public static WaterRecord Insert(DateTime date, int quantity)
        {

            WaterRecord record = new WaterRecord();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO drinking_water(date, quantity) VALUES('{date.Date.ToShortDateString()}', {quantity}); SELECT last_insert_rowid();";

                int id = Convert.ToInt32(command.ExecuteScalar());

                record = new WaterRecord(date, quantity, id);

                connection.Close();
            }

            return record;
        }

        public static List<WaterRecord> GetAll()
        {
            List<WaterRecord> list = new List<WaterRecord>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM drinking_water";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new WaterRecord(reader.GetDateTime(1), reader.GetInt32(2), reader.GetInt32(0)));
                    }
                }

                connection.Close();
            }

            return list;
        }

        public static WaterRecord FindByID(int id)
        {
            WaterRecord record = new WaterRecord();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM drinking_water WHERE Id={id}";

                SqliteDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        record = new WaterRecord(reader.GetDateTime(1), reader.GetInt32(2), reader.GetInt32(0));
                    }
                }

                connection.Close();
            }

            return record;
        }

        public static void Delete(WaterRecord record)
        {

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"DELETE FROM drinking_water WHERE Id={record.ID}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Update(WaterRecord record)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"UPDATE drinking_water SET Date='{record.Date.Date.ToShortDateString()}', Quantity={record.Quantity} WHERE Id={record.ID}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
