using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Habit_Logger
{
    public static class DataBase
    {
        private static string db_source = "Data Source=habit_tracker.db";
        private static SqliteConnection connection = new SqliteConnection(db_source);

        public static void Seed()
        {
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO running(date, kilometers) VALUES('12/1/2023 10:30 AM' , 1)," +
                $"('12/5/2023 7:30 AM' , 1)," +
                $"('12/7/2023 6:30 AM' , 1)," +
                $"('12/9/2023 5:00 PM' , 2)," +
                $"('12/11/2023 8:30 PM' , 2)," +
                $"('12/13/2023 6:30 AM' , 3)";
            tableCmd.ExecuteNonQuery();
        }
        public static void Connect()
        {
            try
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"DROP TABLE IF EXISTS running;
                                        CREATE TABLE IF NOT EXISTS running(
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            date TEXT,
                                            kilometers INTEGER
                                        );";
                tableCmd.ExecuteNonQuery();
                
                
            }
            catch(Exception ex)
            {

                Helper.printError(ex.Message);
            }
           
        }
        public static void Close()
        {
            connection.Close();
        }
        public static void GetAllRecords()
        {

            List<Running> tableData = new();
            try
            {
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM running";
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new Running(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2) ) ); 
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                Console.WriteLine("------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Helper.printData(dw);
                }
                Console.WriteLine("------------------------------------------\n");

            }
            catch (Exception ex)
            {

                Helper.printError(ex.Message);
            }
        }

        public static void Insert()
        {
            Console.WriteLine("Enter running info ");
            string date = Validations.GetDateInput();
            int kilometers = Validations.GetNumberInput("Enter number of kilometers you run");
            try
            {
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO running(date, kilometers) VALUES('{date}' , {kilometers})";
                tableCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Helper.printError(ex.Message);
            }
            Console.Clear();

        }

        public static void Update()
        {
            GetAllRecords();

            var recordId = Validations.GetNumberInput("Please type Id of the record would like to update. Type 0 to return to main manu.");

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM running WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Helper.printError($"Record with Id {recordId} doesn't exist.");

                Update();
            }

            string date = Validations.GetDateInput();

            int kilometers = Validations.GetNumberInput("Please insert number of Kilometers you run (no decimals allowed)");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE running SET date = '{date}', kilometers = {kilometers} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();
            Console.Clear();
        }


        public static void Delete()
        {

            GetAllRecords();

            var recordId = Validations.GetNumberInput("Please type Id of the record would like to Delete. Type 0 to return to main manu.");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM running WHERE id = {recordId}";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Delete();
            }
            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
           
        }
            

    }
}
