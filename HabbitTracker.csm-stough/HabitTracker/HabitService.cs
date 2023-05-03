using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class HabitService
    {

        static string connectionString = @"Data Source=habit-tracker.db";

        public static void Init()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS habit_table (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        TableName TEXT,
                        HabitName TEXT,
                        TableUnit TEXT
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<HabitTable> GetAllHabits()
        {
            List<HabitTable> list = new List<HabitTable>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM habit_table";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new HabitTable(reader.GetInt32(0), reader.GetString(2), reader.GetString(1), reader.GetString(3)));
                    }
                }

                connection.Close();
            }

            return list;
        }

        public static HabitTable? InsertHabit(string habitName, string tableUnit)
        {
            //transform given habit name into a proper table name
            string tableName = habitName.Replace(' ', '_').ToLower();

            //abort creating new habit table if table name already exists or if name is empty
            if(String.IsNullOrWhiteSpace(habitName) || GetHabitByName(tableName) != null)
            {
                return null;
            }

            HabitTable table = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO habit_table(TableName, HabitName, TableUnit) VALUES('{tableName}', '{habitName}', '{tableUnit}'); SELECT last_insert_rowid();";

                int id = Convert.ToInt32(command.ExecuteScalar());

                table = new HabitTable(id, habitName, tableName, tableUnit);

                connection.Close();
            }

            CreateHabitTable(tableName);

            return table;
        }

        public static HabitTable? GetHabitByID(int ID)
        {

            HabitTable table = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM habit_table WHERE Id={ID}";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                         table = new HabitTable(reader.GetInt32(0), reader.GetString(2), reader.GetString(1), reader.GetString(3));
                    }
                }

                connection.Close();
            }

            return table;
        }

        public static HabitTable? GetHabitByName(string name)
        {

            HabitTable? table = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM habit_table WHERE TableName='{name}'";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        table = new HabitTable(reader.GetInt32(0), reader.GetString(2), reader.GetString(1), reader.GetString(3));
                    }
                }

                connection.Close();
            }

            return table;
        }

        public static void Delete(HabitTable table)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"DELETE FROM habit_table WHERE Id={table.ID}";

                command.ExecuteNonQuery();        

                connection.Close();
            }
        }

        private static void CreateHabitTable(string tableName)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS {tableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Value TEXT
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
