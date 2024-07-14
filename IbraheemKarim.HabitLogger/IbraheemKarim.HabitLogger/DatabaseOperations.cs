using IbraheemKarim.HabitLogger.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace IbraheemKarim.HabitLogger
{
    public static class DatabaseOperations
    {
        private static string GetConnectionString(string sectionName)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                 .Build();

            string? connectionString = config.GetSection(sectionName).Value;

            if (connectionString is null)
                throw new InvalidOperationException
                    ("An issue occured, you might've entered a wrong sectionName or there's an issue in the configuration file");
            else
                return connectionString;
        }

        public static void CreateHabitTrackerDatabaseIfItDoesNotExist()
        {
            var connString = GetConnectionString("constrMaster");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery =
                        @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'HabitTrackerDB')
                          BEGIN
                               CREATE DATABASE HabitTrackerDB;
                          END;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database (there might be a mistake in the connection string)");
                    Environment.Exit(0);
                }
            }
        }

        public static void CreateTableIfItDoesNotExist()
        {
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery =
                        @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DrinkingWater' AND schema_id = SCHEMA_ID('dbo'))
                          BEGIN
                              CREATE TABLE dbo.DrinkingWater (
                                  Id INT PRIMARY KEY IDENTITY(1,1),
                                  Quantity INT,
								  AddedOnDate DATETIME2
                              );
                          END;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database (there might be a mistake in the connection string)");
                    Environment.Exit(0);
                }
            }
        }
        
        public static void PrintAllRows()
        {
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery = "SELECT * FROM DrinkingWater;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var row = new DrinkingWaterHabit();
                            while (reader.Read())
                            {
                                row.Id = reader.GetInt32("Id");
                                row.Quantity = reader.GetInt32("Quantity");
                                row.AddedOn = reader.GetDateTime("AddedOnDate");
                                
                                Console.WriteLine(row);
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database");
                }
            }
        }

        public static void DeleteRecord(int recordId)
        {
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery = "DELETE FROM DrinkingWater WHERE Id = @Id;";

                    SqlParameter idParameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = recordId,
                    };

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add(idParameter);

                        if (command.ExecuteNonQuery() > 0)
                            Console.WriteLine($"The row has been successfully deleted.");
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database");
                }
            }
        }

        public static void InsertRecord(int quantity, DateTime addedOn)
        {
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery = "INSERT INTO DrinkingWater VALUES (@Quantity, @AddedOn);";

                    SqlParameter quantityParameter = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = quantity,
                    };
                    
                    SqlParameter AddedonParameter = new SqlParameter
                    {
                        ParameterName = "@AddedOn",
                        SqlDbType = SqlDbType.DateTime2,
                        Direction = ParameterDirection.Input,
                        Value = addedOn,
                    };

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add(quantityParameter);
                        command.Parameters.Add(AddedonParameter);

                        if (command.ExecuteNonQuery() > 0)
                            Console.WriteLine($"The row has been added successfully.");
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database");
                }
            }
        }

        public static void UpdateRecord(int recordId, int quantity, DateTime addedOn)
        {            
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery =
                        @"UPDATE DrinkingWater
                          SET Quantity = @Quantity, AddedOnDate = @AddedOn
                          WHERE Id = @Id;";

                    SqlParameter idParameter = new SqlParameter
                    {
                        ParameterName = "@Id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = recordId,
                    };

                    SqlParameter quantityParameter = new SqlParameter
                    {
                        ParameterName = "@Quantity",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = quantity,
                    };

                    SqlParameter AddedonParameter = new SqlParameter
                    {
                        ParameterName = "@AddedOn",
                        SqlDbType = SqlDbType.DateTime2,
                        Direction = ParameterDirection.Input,
                        Value = addedOn,
                    };

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add(idParameter);
                        command.Parameters.Add(quantityParameter);
                        command.Parameters.Add(AddedonParameter);

                        if (command.ExecuteNonQuery() > 0)
                            Console.WriteLine($"The row has been successfully updated.");
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database");
                }
            }
        }

        public static HashSet<int> GetAvailableRowIds()
        {
            var connString = GetConnectionString("constrHabitTrackerDB");

            using (var connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();

                    var sqlQuery = "SELECT * FROM DrinkingWater;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var AvailableIds = new HashSet<int>();
                            while (reader.Read())
                            {
                                AvailableIds.Add(reader.GetInt32("Id"));
                            }
                            return AvailableIds;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"Error: An error occured while trying to connect to the database");
                    return new HashSet<int>();
                }
            }
        }
    }
}
