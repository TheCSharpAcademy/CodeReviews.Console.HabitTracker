using HabitTrackerLibrary.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTrackerLibrary.DataAccess
{
    public class SqlData
    {
        private readonly SqliteDataAccess db;
        private readonly string connectionStringName;
        public SqlData(SqliteDataAccess db, string connectionStringName)
        {
            this.db = db;
            this.connectionStringName = connectionStringName;
        }


        public void InsertRecordByHabitName(string habitName, string date, double quantity)
        {
            db.Execute($"insert into records(HabitId, Date, Quantity) values( (select Id from habits where habits.Name = '{habitName}'), '{date}', '{quantity}')");
        }

        public void InsertRecord(int habitId, string date, double quantity)
        {
            db.Execute($"insert into records(HabitId, Date, Quantity) values({habitId}, '{date}', '{quantity}')");
        }

        public void UpdateRecord(string tableName, int recordId, string date, double quantity)
        {
            db.Execute($"update {tableName} set Date = '{date}', quantity = '{quantity}' where Id = {recordId}");
        }

        public void DeleteRecord(string tableName, int recordId)
        {
            db.Execute($"delete from {tableName} where id = '{recordId}'");
        }

        public void DeleteAllRecordsForAHabit(int habitId)
        {
            db.Execute($"delete from records where habitId = '{habitId}'");
        }

        public void InsertHabit(string name, string unitName)
        {
            db.Execute($"insert into habits (Name, UnitsId) values('{name}', (select id from units where units.Name ='{unitName}'))");
        }

        public void UpdateHabit(string name, string units, int recordId)
        {
            db.Execute($"update habits set Name = '{name}', Units = '{units}' where Id = {recordId}");
        }

        public void DeleteHabit(int recordId)
        {
            db.Execute($"delete from habits where id = '{recordId}'");
        }

        public void InsertUnit(string name)
        {
            db.Execute($"insert into units (Name) values( '{name}' )");
        }

        public bool CheckIfHabitExists(string habitName)
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM Habits WHERE Name = '{habitName}' COLLATE NoCase)";
                var test = tableCmd.ExecuteScalar();


                bool habitExists = Convert.ToBoolean(test);

                connection.Close();

                return habitExists;
            }
        }

        public bool CheckIfUnitExists(string unitName)
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM Units WHERE Name = '{unitName}' COLLATE NoCase)";

                bool unitExists = Convert.ToBoolean(tableCmd.ExecuteScalar());

                connection.Close();

                return unitExists;
            }
        }

        public List<RecordModel> GetAllRecords(int habitId)
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"select Id, Date, Quantity from Records where records.HabitId = {habitId}";

                List<RecordModel> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new RecordModel
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", new CultureInfo("en-US")),
                                Quantity = reader.GetDouble(2),
                            }
                         );
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        public List<HabitModel> GetAllHabits()
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"select habits.Id as HabitId, habits.Name as HabitName, Units.Name as UnitsName
                                          from habits
                                          inner join units on habits.UnitsId = Units.Id";

                List<HabitModel> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new HabitModel
                            {
                                HabitId = reader.GetInt32(0),
                                HabitName = reader.GetString(1),
                                UnitName = reader.GetString(2),
                            }
                         );
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        public List<UnitModel> GetAllUnits()
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @$"select units.Id as UnitId, units.Name as UnitName from units";

                List<UnitModel> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new UnitModel
                            {
                                UnitId = reader.GetInt32(0),
                                UnitName = reader.GetString(1),
                            }
                         );
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        public bool RecordExists(string tableName, int recordId = -1)
        {
            using (var connection = new SqliteConnection(connectionStringName))
            {
                connection.Open();
                var checkCmd = connection.CreateCommand();

                if (recordId < 0)
                {
                    checkCmd.CommandText = $"select exists (Select 1 from {tableName})";
                }
                else
                {
                    checkCmd.CommandText = $"select exists (Select 1 from {tableName} where Id = {recordId}";

                }

                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                return (checkQuery == 0) ? false : true;
            }
        }
    }
}
