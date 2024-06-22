using System.Globalization;
using Microsoft.Data.Sqlite;
namespace Habit_Tracker
{
    class DatabaseManager
    {
        public static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main (string[] args)
        {
            using(var connection = new SqliteConnection(connectionString))
            {

            connection.Open();

            var tablecmd = connection.CreateCommand();

            tablecmd.CommandText = 
            @"CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER)";

            tablecmd.ExecuteNonQuery();

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
               Console.WriteLine("\nType 0 to close Application.");
               Console.WriteLine("Type 1 to View All Records");
               Console.WriteLine("Type 2 to Insert Record.");
               Console.WriteLine("Type 3 to Delete Record.");
               Console.WriteLine("Type 4 to Update Record.");
               Console.WriteLine("-----------------------------------------\n");


                string command = Console.ReadLine();


                switch (command)
               {
                case "0":
                    Console.WriteLine("\nGoodBye\n");
                    closeApp = true;
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
                default: 
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
               }

            }
        }
        public static void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
        
             using(var connection = new SqliteConnection(connectionString))
            {   

            connection.Open();

            var tablecmd = connection.CreateCommand();

            tablecmd.CommandText = 
            $"Insert INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tablecmd.ExecuteNonQuery();

            connection.Close();
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");


            string dateInput = Console.ReadLine();


            if (dateInput == "0") GetUserInput();

            while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again:\n\n");

                dateInput = Console.ReadLine();

            }
            return dateInput;
        }
        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);


            string numberInput = Console.ReadLine();


            if (numberInput == "0") GetUserInput();

            while(!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");

                numberInput = Console.ReadLine();

            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
        private static void GetAllRecords()
        {
            Console.Clear();
            using(var connection = new SqliteConnection(connectionString))
            {   

                connection.Open();

                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = 
                $"SELECT * from drinking_water";

                List<DrinkingWater> tableData = new();
            
                SqliteDataReader reader = tablecmd.ExecuteReader();
            
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-UK")),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }
                else
                {
                    System.Console.WriteLine("No rows found");

                }

                connection.Close();

                Console.WriteLine("-----------------------\n");
                foreach (var dw in tableData)
                {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("-------------------------\n");
            }   
            

            
        }
        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu \n\n");
            using(var connection = new SqliteConnection(connectionString))
            {   

                connection.Open();

                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = 
                $"DELETE from drinking_water WHERE Id = '{recordId}'";

                int rowCount = tablecmd.ExecuteNonQuery();

                if(rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesnt't exist.\n\n");
                    Delete();
                }

            }

        }
        internal static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type id of the record you would like to update. Type 0 to return to the Main Menu. \n\n");

            using(var connection = new SqliteConnection(connectionString))
            {   

                connection.Open();

                var checkCmd = connection.CreateCommand();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput();

                int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice.(doesnt hold decimals)\n\n");

                var tablecmd = connection.CreateCommand();

                tablecmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordId}";

                tablecmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
    public class DrinkingWater
    {
        public int Id {get; set;}
        public DateTime Date {get; set;}
        public int Quantity {get; set;}

    }
}
