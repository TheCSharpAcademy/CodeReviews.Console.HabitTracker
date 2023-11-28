using HabitLogger.Models;
using HabitLogger.UI;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitLogger.Database
{
    internal class DatabaseHelper
    {
        private static SqliteConnection connection = new SqliteConnection(@"Data Source=habit.db");

        public static void InitializeConnection()
        {
            using (connection)
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
	                  Id	    INTEGER NOT NULL UNIQUE,
	                  Date	    TEXT NOT NULL,
	                  Quantity	INTEGER NOT NULL,
	                  PRIMARY   KEY(Id AUTOINCREMENT)
                      );";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<DrinkingWaterModel> AllData
        {
            get
            {
                List<DrinkingWaterModel> data = new();

                using (connection)
                {
                    connection.Open();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = @"SELECT * FROM drinking_water";

                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data.Add(
                                new DrinkingWaterModel
                                {
                                    Id = (int)(long)reader["Id"],
                                    Date = DateTime.ParseExact(reader["Date"].ToString(), "dd-MM-yyyy", new CultureInfo("en-US")),
                                    Quantity = (int)(long)reader["Quantity"]
                                });
                        }
                    }

                    connection.Close();
                }

                return data;
            }
        }

        public static void InsertRow()
        {
            (string date, int quantity) = UserInterface.GetInsertInfo();

            if (!string.IsNullOrEmpty(date))
            {
                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        @$"INSERT INTO [drinking_water]
                       ([Date],[Quantity])   
                       VALUES
                       ('{date}',{quantity});";
                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static void UpdateRow()
        {
            (int id, string date, int quantity) = UserInterface.GetUpdateInfo();

            if (id > 0 && quantity > 0)
            {
                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        @$"UPDATE[drinking_water]
                       SET[Date] = {$"'{date}'"} 
                       ,[Quantity] = {quantity}
                       WHERE [Id] = {id};";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static void DeleteRow()
        {
            int id = UserInterface.GetDeleteInfo();

            if (id > 0)
            {
                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        @$"DELETE FROM [drinking_water]
                       WHERE [Id] = {id};";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
