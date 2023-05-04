using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTracker
{
    internal class DbViewer
    {

        public static void ViewRecords()
        {

            int counter = 0;
            Console.Clear();
            using (var connection = new SqliteConnection(Program.ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                var viewType = Helpers.GetViewType();

                switch (viewType.ToLower())
                {
                    case "0":
                        Console.Clear();
                        return;

                    case "all":
                        tableCmd.CommandText = $"SELECT * FROM habits";
                        break;

                    case "activity":
                        Console.WriteLine("\n\nEnter the activity type of the records you want to view: ");
                        var activityRecord = Console.ReadLine();
                        tableCmd.CommandText = $"SELECT * FROM habits WHERE Activity LIKE '%{activityRecord}%'";
                        break;

                    case "date":
                        tableCmd.CommandText = Helpers.GetDateViewType();
                        break;
                }

                var tableData = new List<Habit>();
                var amountData = new List<Int32>();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                //Add specified records to list of Habits
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new Habit
                            {
                                Id = reader.GetInt32(0), //returns values of column(i) specified
                                Date = DateTime.ParseExact(reader.GetString(1), "MM-dd-yyyy", new CultureInfo("en-US")),
                                Activity = reader.GetString(2),
                                Unit = reader.GetString(3),
                                Amount = reader.GetInt32(4)
                            });
                        amountData.Add(reader.GetInt32(4));
                        counter++;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n\nNo records found.");
                }

                connection.Close();

                // Add up elements in sumData list
                var summedAmount = amountData.Sum();


                //Print records in list
                Console.WriteLine("===================================================================================");
                foreach (var ex in tableData)
                {
                    Console.WriteLine(@$"ID: {ex.Id}  ||  Date: {ex.Date.ToString("MM-dd-yyyy")}  ||  Activity: {ex.Activity}  ||  Units: {ex.Amount}/{ex.Unit}");
                }
                Console.WriteLine("===================================================================================");

                Console.WriteLine($"\nTotal Units for specified records: {summedAmount}");
            }
            Console.WriteLine($"Total Entries: {counter}");
            Console.WriteLine("=======================================");
        }
    }
}


