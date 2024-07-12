﻿using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class LocalDatabaseAdoService
    {
        private readonly string _connectionString;
        private SqliteConnection? _connection;

        public LocalDatabaseAdoService(string connectionString)
        {
            _connectionString = connectionString;
            Init();
        }

        private void Init()
        {
            if (_connection != null)
            {
                return;
            }

            try
            {
                _connection = new SqliteConnection(_connectionString);
                _connection.Open();

                CreateTables();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateTables()
        {
            CreateTableHabits();
            CreateTableHabitRecords();
        }

        private void CreateTableHabits()
        {
            string newTableSql =
                @$"
                    CREATE TABLE IF NOT EXISTS {Constants.TableHabits} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    MeasurementMethod TEXT NOT NULL);
                ";
            CreateTable(Constants.TableHabits, newTableSql.ToString());
        }

        private void CreateTableHabitRecords()
        {
            string newTableSql =
                @$"
                    CREATE TABLE IF NOT EXISTS {Constants.TableHabitRecords} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    NumberOfApproachesPerDay INTEGER NOT NULL,
                    HabitId INTEGER,
                    FOREIGN KEY(HabitId) REFERENCES {Constants.TableHabits}(Id) ON DELETE CASCADE);
                ";
            CreateTable(Constants.TableHabitRecords, newTableSql.ToString());
        }

        private void CreateTable(string tableName, string newTableSql)
        {
            string tableExistQuery =
                @$"
                    SELECT name FROM sqlite_master 
                    WHERE type='table' AND name = '{tableName}';
                ";
            SqliteCommand command = _connection!.CreateCommand();
            command.CommandText = tableExistQuery;
            SqliteDataReader reader = command.ExecuteReader();

            string? tableExists = null;
            if (reader.HasRows && reader.Read()) {
                tableExists = reader.GetValue(0).ToString();
            }

            if (!string.IsNullOrEmpty(tableExists) && tableExists == tableName)
            {
                return;
            }

            command = _connection!.CreateCommand();
            command.CommandText = newTableSql;
            command.ExecuteNonQuery();
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }

        private List<Habit> ExecuteReaderHabits(string sql)
        {
            SqliteCommand command = _connection!.CreateCommand();
            command.CommandText = sql;
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<Habit> habits = new List<Habit>();
                while (reader.Read()) {
                    Habit habit = new Habit()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        MeasurementMethod = reader.GetString(2),
                    };
                    habits.Add(habit);
                }
                
                return habits;
            }
            return new List<Habit>();
        }

        private List<HabitRecord> ExecuteReaderHabitRecords(string sql)
        {
            SqliteCommand command = _connection!.CreateCommand();
            command.CommandText = sql;
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                List<HabitRecord> habitRecords = new List<HabitRecord>();
                while (reader.Read())
                {
                    HabitRecord habitRecord = new HabitRecord()
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetValue(1).ToString()!),
                        NumberOfApproachesPerDay = reader.GetInt32(2),
                        HabitId = reader.GetInt32(3),
                    };
                    habitRecords.Add(habitRecord);
                }

                return habitRecords;
            }
            return new List<HabitRecord>();
        }

        // Habits
        public Habit CreateHabit(Habit obj)
        {
            string createQuery = 
                @$"
                    INSERT INTO {Constants.TableHabits} (Name, MeasurementMethod) 
                    VALUES ('{obj.Name}', '{obj.MeasurementMethod}') 
                    RETURNING *;
                ";
            return ExecuteReaderHabits(createQuery).First();
        }

        public Habit? GetLastHabit()
        {
            string getLastQuery = 
                @$"
                    SELECT * FROM {Constants.TableHabits} 
                    ORDER BY Id DESC LIMIT 1;
                ";
            return ExecuteReaderHabits(getLastQuery).FirstOrDefault();
        }

        // HabitRecords
        public HabitRecord CreateHabitRecord(HabitRecord obj, int habitId)
        {
            string createQuery = 
                @$"
                    INSERT INTO {Constants.TableHabitRecords} (Date, NumberOfApproachesPerDay, HabitId) 
                    VALUES ('{obj.Date}', '{obj.NumberOfApproachesPerDay}', '{habitId}') 
                    RETURNING *;
                ";
            return ExecuteReaderHabitRecords(createQuery).First();
        }

        public IEnumerable<HabitRecord> GetAllHabitRecords(int habitId)
        {
            string getAllQuery = 
                @$"
                    SELECT * FROM {Constants.TableHabitRecords} 
                    WHERE HabitId={habitId};
                ";
            return ExecuteReaderHabitRecords(getAllQuery);
        }

        public bool IsExistDateRecord(DateTime date)
        {
            string isExistDateQuery = 
                @$"
                    SELECT * FROM {Constants.TableHabitRecords} 
                    WHERE Date='{date}';
                ";
            return ExecuteReaderHabitRecords(isExistDateQuery).Count > 0;
        }

        public HabitRecord? GetHabitRecordById(int id, int habitId)
        {
            string getHabitRecordByIdQuery = 
                @$"
                    SELECT * FROM {Constants.TableHabitRecords} 
                    WHERE Id={id} AND HabitId={habitId};
                ";
            return ExecuteReaderHabitRecords(getHabitRecordByIdQuery).FirstOrDefault();
        }

        public HabitRecord UpdateHabitRecord(HabitRecord obj)
        {
            string updateQuery = 
                @$"
                    REPLACE INTO {Constants.TableHabitRecords} (Id, Date, NumberOfApproachesPerDay, HabitId) 
                    VALUES ({obj.Id}, '{obj.Date}', '{obj.NumberOfApproachesPerDay}', '{obj.HabitId}') 
                    RETURNING *;
                ";
            return ExecuteReaderHabitRecords(updateQuery).First();
        }

        public HabitRecord? DeleteHabitRecord(int id, int habitId)
        {
            string deleteQuery = 
                @$"
                    DELETE FROM {Constants.TableHabitRecords} 
                    WHERE Id={id} AND HabitId={habitId} 
                    RETURNING *;
                ";
            return ExecuteReaderHabitRecords(deleteQuery).FirstOrDefault();
        }
    }
}
