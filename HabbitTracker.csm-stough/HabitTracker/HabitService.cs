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
    /// <summary>
    /// This class will serve as a service class for the habits table which "refers" to all other specific habit tables
    /// </summary>
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
                        TableType TEXT,
                        TableUnit TEXT
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        //HABIT TABLE FUNCTIONS//

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
                        list.Add(new HabitTable { ID = reader.GetInt32(0), TableName = reader.GetString(1), HabitName = reader.GetString(2), TableType = reader.GetString(3), TableUnit = reader.GetString(4)});
                    }
                }

                connection.Close();
            }

            return list;
        }

        /// <summary>
        /// Handles inserting a new habit table to track
        /// </summary>
        /// <param name="habitName"></param>
        /// <param name="tableType"></param>
        /// <param name="tableUnit"></param>
        public static HabitTable? InsertHabit(string habitName, string tableType, string tableUnit)
        {
            //transform given habit name into a proper table name
            string tableName = habitName.Replace(' ', '_').ToLower();

            //abort creating new habit table if table name already exists or if name is empty
            if(String.IsNullOrWhiteSpace(habitName) || GetHabitByName(tableName) != null)
            {
                return null;
            }

            HabitTable table = new HabitTable();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"INSERT INTO habit_table(TableName, HabitName, TableType, TableUnit) VALUES('{tableName}', '{habitName}', '{tableType}', '{tableUnit}'); SELECT last_insert_rowid();";

                int id = Convert.ToInt32(command.ExecuteScalar());

                table = new HabitTable(id, habitName, tableName, tableType, tableUnit);

                connection.Close();
            }

            return table;
        }

        public static HabitTable GetHabitByID(int ID)
        {

            HabitTable table = new HabitTable();

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
                         table = new HabitTable { ID = reader.GetInt32(0), TableName = reader.GetString(1), HabitName = reader.GetString(2), TableType = reader.GetString(3), TableUnit = reader.GetString(4) };
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
                        table = new HabitTable { ID = reader.GetInt32(0), TableName = reader.GetString(1), HabitName = reader.GetString(2), TableType = reader.GetString(3), TableUnit = reader.GetString(4) };
                    }
                }

                connection.Close();
            }

            return table;
        }


    }
}
