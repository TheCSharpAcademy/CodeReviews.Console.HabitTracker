using HabitTracker.Matija87.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker.Matija87
{
    internal static class MenuCommands
    {
        static readonly string connectionString = @"Data source=habit-Tracker.db";
        internal static void GetAllRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM cigs_smoked";
                
                List<CigsSmoked> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CigsSmoked
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                                Quantity = reader.GetInt32(2)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("\nNo rows found!\n");
                }

                connection.Close();

                foreach (var dw in  tableData)
                {
                    Console.WriteLine($"ID: {dw.Id} - Date: {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++\n\n");
            }
        }
        internal static void InsertRecord()
        {
            string date = Helpers.GetDateInput();

            int quantity = Helpers.GetNumberInput("\nPlease insert number of cigarettes smoked:");

            using (var connection = new SqliteConnection (connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO cigs_smoked(date, quantity) VALUES('{date}', {quantity})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine("\n\n");
        }
        internal static void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            int recordId = Helpers.GetNumberInput("\nType Id of record you want to delete or 0 to return to Main Menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE from cigs_smoked WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with Id {recordId} doesn't exist!");
                    DeleteRecord();
                }
            }
            Console.WriteLine($"\nRecord with Id {recordId} was deleted!\n");
            Program.MainMenu();
        }
        internal static void UpdateRecord()
        {
            GetAllRecords();
            var recordId = Helpers.GetNumberInput("\nType Id of record you want to update or 0 to return to Main Menu");

            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText =
                    $"SELECT EXISTS(SELECT 1 from cigs_smoked WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"Record with Id {recordId} does not exist!");
                    connection.Close();
                    UpdateRecord();
                }

                string date = Helpers.GetDateInput();
                int quantity = Helpers.GetNumberInput("\nEnter number of cigarettes smoked\n");

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE cigs_smoked SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            } 
        }
    }
}
