using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;
using static System.Console;

namespace nikosnick13.HabitLogger
{
    internal class Program
    {


        static string connectionString = @"Data Source=habit-Traker.db";

        static void Main(string[] args)
        {


            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var tableCmd = conn.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS  dring_water(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT, 
                        Quantity  INTEGER
                    )";

                tableCmd.ExecuteNonQuery();

                conn.Close();



            }

            UserInput();

            ReadKey();

        }

        //USER INPUT MENY
        public static void UserInput()
        {
            Clear();
            ForegroundColor = ConsoleColor.Green;

            bool isAppRunning = false;

            while (!isAppRunning)
            {
               
                WriteLine("\n\nMAIN Menu");
                WriteLine("\nWhat would you like to do?");
                WriteLine("\n\tType 0 to Close Application.");
                WriteLine("\tType 1 to Inser a Record ");
                WriteLine("\tType 2 to View All Records");
                WriteLine("\tType 3 to Delete a Record");
                WriteLine("\tType 4 to Update a Record");
                WriteLine("============================================\n");

                string UserInput = ReadLine();

                switch (UserInput)
                {
                    case "0":
                        WriteLine("\nGoodbye!! \n");
                        isAppRunning = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Insert();
                        break;
                    case "2":
                        VeiwAll();
                        break;

                    case "3":
                        Delete();
                        break;
                    case "4":
                        Updade();
                        break;
                    default:
                        WriteLine("Invalid Command.Please type a number from 0 to 4");
                        break;
                }

            }
        }

        #region Insert

        private static void Insert()
        {
            Clear();
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlaese Insert a number of glasses or another measures of your choice (no decimals is allowed \n\n) ");

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var insertUserCmd = conn.CreateCommand();
                insertUserCmd.CommandText = $"INSERT INTO dring_water(date,quantity ) VALUES ('{date}',{quantity})";
                insertUserCmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        //Get the input for Date as string
        private static string GetDateInput()
        {
            WriteLine("Please insert tha date: dd-MM-yyyy. Type 0 to return to main menu.");

            string dateInput = ReadLine();

            if (dateInput == "0") UserInput();

            //Validation input for date
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"),DateTimeStyles.None,out _)) 
            {
                ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yyyy). Type 0 to return to main manu or try again:\n\n");
                ForegroundColor = ConsoleColor.Green;
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }
        //Get the input for quanti as  int
        private static int GetNumberInput(string msq)
        {
            WriteLine(msq);

            string quantityInput = ReadLine();
            

            //Validation input for quantity
            while (!Int32.TryParse(quantityInput,out _) || Convert.ToInt32(quantityInput) < 0) 
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                quantityInput = Console.ReadLine();
            }

            if (quantityInput == "0") UserInput();

            int finalQuantity = Convert.ToInt32(quantityInput);

            return finalQuantity;
        }

        #endregion

        #region ViewAll
        private static void VeiwAll()
        {
           
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                var viewAllCmd = conn.CreateCommand();

                viewAllCmd.CommandText =
                    $"SELECT * FROM dring_water";
                viewAllCmd.ExecuteNonQuery();

                //The list is generic how store objects of type DrinkingWater.
                List<DringWater> tableData = new List<DringWater>();

                SqliteDataReader reader = viewAllCmd.ExecuteReader();

                //Check if has rows 
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DringWater
                            {
                                Id = reader.GetInt32(0),
                                // Date = DateTime.ParseExact(reader.GetString(1), "d-M-yy", new CultureInfo("en-US")),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yyyy", new CultureInfo("en-US")),

                                Quantity = reader.GetInt32(2)
                            });
                        
                    }
                    foreach (var a in tableData)
                    {
                        WriteLine("---------------------------------------");
                        WriteLine($"|ID:{a.Id} | DATE: {a.Date.ToString("dd-MM-yyyy")} | Quantity: {a.Quantity}|");
                        WriteLine("---------------------------------------\n");
                    }
                }
                else
                {
                    WriteLine("No rows founds");
                }
                conn.Close();
            }
        }

        #endregion

        #region Delete
        private static void Delete()
        {
            Clear();
            VeiwAll();

            var grapId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to go back to Main Menu\n\n");

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var deleteInsert = conn.CreateCommand();

                deleteInsert.CommandText = $"DELETE FROM dring_water  WHERE Id = '{grapId}'";

                int rowtable = deleteInsert.ExecuteNonQuery();

                if (rowtable == 0)
                {
                    WriteLine($"\t\n\nThe reconrd with id {grapId} dosent exist \n\n");
                    Delete();
                }
            }
            WriteLine($"\t\n\nThe record with id {grapId}  was delete \n\n");
            UserInput();
        }




        #endregion

        #region Update
        private static void Updade()
        {
            
            VeiwAll();
            var grapId = GetNumberInput("\n\nPlease type the Id of the record you want to edit or type 0 to go back to Main Menu \n\n");
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var checkCmd = conn.CreateCommand();

                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM dring_water WHERE  Id = {grapId})";

                int checkQuary = Convert.ToInt32(checkCmd.ExecuteScalar());

                //We check the record if exist in db
                if (checkQuary == 0)
                {
                     
                    WriteLine($"\n\n Record with id {grapId} doesen't exist");
                    conn.Close();
                    ReadKey();
                    Updade();
                }

                var updateDate = GetDateInput();
                int updareQuantity = GetNumberInput("\n\nPlaese Update the numbers of glasses or another measures of your choice (no decimals is allowed \n\n");            
                
                var editCmd = conn.CreateCommand();
                editCmd.CommandText = $"UPDATE dring_water SET Date = '{updateDate}',Quantity =  {updareQuantity}  WHERE Id = {grapId}";

                editCmd.ExecuteScalar();
                conn.Close();
            }




        }

        #endregion
    }

    public class DringWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }

}
