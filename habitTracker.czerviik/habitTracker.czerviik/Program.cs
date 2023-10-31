using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

string connectionString = @"Data Source=habit-Tracker.db";
using (var connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = 
        @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";

    tableCmd.ExecuteNonQuery();

    connection.Close();
}

GetUserInput();
void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close App.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("_____________________________________________\n");

        string command = Console.ReadLine();
        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                closeApp = true;
                break;
            /* case "1":
                 GetAllRecords():
                 break;*/
            case "2":
                Insert();
                break;
            
        }
        
         void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO drinking_water(date, Quantity) VALUES('{date}',{quantity})";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

         string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.");
            string dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();
            return dateInput;
        }

        int GetNumberInput()
        {
            Console.WriteLine("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}