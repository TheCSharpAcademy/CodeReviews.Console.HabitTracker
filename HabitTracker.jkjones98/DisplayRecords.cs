using Microsoft.Data.Sqlite;
using ColumnName;
using System.Globalization;

namespace DisplayRecords;

public class AllRecords
{
    GetColumnName currentName = new GetColumnName();
    public void DisplayRecs()
    {
        string currentColumnName = currentName.GetColName();
        
        
        
        string connectionString = @"Data Source=habit-Tracker2.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new();

            // Create sqlitedatareader
            SqliteDataReader reader = tableCommand.ExecuteReader();

            // If reader has rows run code inside if statement
            if(reader.HasRows)
            {
                // While reader is reading the rows run the below while loop
                while(reader.Read())
                {
                    // Add the data from table row 0, row 1, row 2 to the list 
                    tableData.Add(
                        new DrinkingWater
                        {
                            // number in brackets represents the index of the column id(0), date(1), quantity(2)
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Unit = reader.GetInt32(2)
                        }); 
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();

            Console.WriteLine("------------------------------------\n");
            foreach(var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yy")} - {currentColumnName}: {dw.Unit}");
            }
            Console.WriteLine("------------------------------------\n");
        }
    }
}

public class DrinkingWater
{
    public int Id {get; set;}

    public DateTime Date {get; set;}

    public int  Unit {get; set;}
}
