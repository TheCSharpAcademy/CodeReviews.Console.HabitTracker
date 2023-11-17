using Microsoft.Data.Sqlite;
using HabitTracker.SelectReport;

namespace HabitTracker.DisplayReports;

public class DisplaySQLiteReports
{
    string connectionString = @"Data Source=habit-Tracker2.db";
    public void Reports(string operation, string function)
    {
        ReportSwitch reportMenu = new ReportSwitch();

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = operation;

            List<RunningReports> runningQueryTable = new();

            SqliteDataReader reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    runningQueryTable.Add(
                        new RunningReports
                        {
                                Runs = reader.GetInt32(0)
                        }
                    );
                }
            }
            else
                Console.WriteLine("No rows found");

            connection.Close();

            Console.WriteLine("------------------------------------\n");
            Console.WriteLine(function);
            foreach(var r in runningQueryTable)
            {
                Console.WriteLine($"{r.Runs}");
            }
            Console.WriteLine("------------------------------------\n");
        }
        reportMenu.ReportOptions();
    }
}

public class RunningReports
{
    public int Runs { get; set; }
}