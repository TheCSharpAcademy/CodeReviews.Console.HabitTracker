using Microsoft.Data.Sqlite;
using System.Globalization;

namespace habit_tracker;

class Program
{
    static string connectionsString = @"Data Source=habit-Tracker.db";
    static bool totalBeersBool;
    static void Main(string[] args)
    {
       
        
        using (var connection = new SqliteConnection(connectionsString)) 
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = 
                @"Create Table if Not Exists drinking_beer (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTEGER
                    )";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        GetUserInput();
    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("Type 5 to Total Beers Drank.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break; 
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    case "5":
                        TotalBeers();
                        break;

                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 5.\n");
                        break; 
                }
        }
    }

    private static void TotalBeers()
    {
        totalBeersBool = true;
        GetAllRecords();
    }

    private static void GetAllRecords()
    {
        Console.Clear();
        using (var connection = new SqliteConnection(connectionsString)) 
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = 
                $"SELECT * FROM drinking_beer ";

            List<DrinkingBeer> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new DrinkingBeer
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }

            }
            else
            {
                Console.WriteLine(" No rows found");
            }

            connection.Close();

            Console.WriteLine("------------------------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
            }
            Console.WriteLine("------------------------------------------\n");

            if(totalBeersBool)
            {
                var totalQuantity = tableData.Sum(dw => dw.Quantity);

                Console.WriteLine($"\n\nTotal amount of beer drank is {totalQuantity}\n");
            }
        }

        totalBeersBool = false;
    }

    private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetNumberInput("\n\nPlease insert number of beers drank (no decimals allowed)\n\n");

        using (var connection = new SqliteConnection(connectionsString)) 
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =  
                $"INSERT INTO drinking_beer(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }   
    }

    private static void Delete()
    {     
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

        using (var connection = new SqliteConnection(connectionsString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from drinking_beer WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Delete();
                }

            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

            GetUserInput();
    }

    private static void Update()
    {       
        GetAllRecords();

        var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

        using (var connection = new SqliteConnection(connectionsString)) 
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_beer WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if( checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                connection.Close();
                Update(); 
            }


            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of beers drank (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_beer SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    internal static string GetDateInput()
    {
       Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main manu.\n\n");

       string dateInput = Console.ReadLine();

       if(dateInput == "0") 
       GetUserInput ();

       while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
       {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy).");
            dateInput = Console.ReadLine();
       }

       return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if(numberInput == "0") GetUserInput();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
            numberInput = Console.ReadLine();
        }
           
        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    public class DrinkingBeer
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }
    }
}
    