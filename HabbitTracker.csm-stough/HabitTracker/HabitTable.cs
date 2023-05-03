using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class HabitTable
    {

        public int ID;
        public string HabitName;
        public string TableName;
        public string TableUnit;

        string connectionString = @"Data Source=habit-tracker.db";

        public HabitTable(int ID, string HabitName, string TableName, string TableUnit)
        {
            this.ID = ID;
            this.HabitName = HabitName;
            this.TableName = TableName;
            this.TableUnit = TableUnit;
        }
      
        public HabitRecord Insert(DateTime date, string value)
        {
            HabitRecord record;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO {TableName}(Date, Value) VALUES('{date.Date.ToShortDateString()}', '{value}'); SELECT last_insert_rowid();";

                int id = Convert.ToInt32(command.ExecuteScalar());

                record = new HabitRecord(id, date, value);

                connection.Close();
            }

            return record;
        }

        public List<HabitRecord> GetAllRecords()
        {
            List<HabitRecord> records = new List<HabitRecord>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM {TableName}";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        records.Add(new HabitRecord(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2)));
                    }
                }

                connection.Close();
            }

            return records;
        }

        public void Delete(HabitRecord record)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"DELETE FROM {TableName} WHERE Id={record.Id}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(HabitRecord record)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"UPDATE {TableName} SET Date='{record.Date.Date.ToShortDateString()}', Value={record.Value} WHERE Id={record.Id}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
