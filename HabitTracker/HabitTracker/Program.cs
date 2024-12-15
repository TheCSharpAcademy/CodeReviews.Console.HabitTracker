using System.Globalization;
using Microsoft.Data.Sqlite;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "DataSource=HabitTracker.db";
        
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                "CREATE TABLE IF NOT EXISTS drinking_water (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
        
        GetUserInput();
        
        void GetUserInput()
        {
            bool exitApp = false;

            while (!exitApp)
            {
                Console.Clear();
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. View all records");
                Console.WriteLine("2. Insert record");
                Console.WriteLine("3. Delete record");
                Console.WriteLine("4. Update record");
                Console.WriteLine("5. Exit.");
                Console.WriteLine();
                Console.WriteLine();

                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        GetAllRecords();
                        PressAnyKey();
                        break;
                    case "2":
                        InsertRecord();
                        PressAnyKey();
                        break;
                    case "3":
                        DeleteRecord();
                        PressAnyKey();
                        break;
                    case "4":
                        UpdateRecord();
                        PressAnyKey();
                        break;
                    case "5":
                        exitApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        void GetAllRecords()
        {
            Console.Clear();
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();
                var reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new DrinkingWater
                            {
                                Id = reader.GetInt32(0),
                                Date = DateTime.ParseExact(reader.GetString(1), "dd-mm-yyyy", new CultureInfo("pl-PL")),
                                Quantity = reader.GetInt32(2),
                            });
                    }

                    connection.Close();

                    Console.WriteLine("..............................................");
                    Console.WriteLine("Records found:");
                    foreach (var dw in tableData)
                    {
                        Console.WriteLine(
                            $"ID: {dw.Id} - Date: {dw.Date.ToString("dd-mm-yyyy")} - Quantity: {dw.Quantity}");
                    }

                    Console.WriteLine("..............................................");
                }
                
                else Console.WriteLine("No rows found");
            }
        }

        void InsertRecord()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("Please insert quantity number (measure of your choice), or type 0 to return to menu");
            
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);

                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(Date, Quantity) VALUES(@date, @quantity)";
                
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();
            int id = GetNumberInput("Enter the ID of a record you want to delete, or type 0 to return to main menu");

            if (id == 0)
            {
                GetUserInput();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@id", id);
                
                tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {@id}";
                
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.WriteLine($"No rows found");
                    DeleteRecord();
                }
                
                Console.WriteLine($"Record with ID: {id} was deleted");
                
                connection.Close();
            }
        }

        void UpdateRecord()
        {
            Console.Clear();
            GetAllRecords();
            
            int id = GetNumberInput("Enter the ID of a record you want to update, or type 0 to return to main menu");
            
            if (id == 0)
            {
                GetUserInput();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = {id})";
                
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (checkQuery == 0)
                {
                    Console.WriteLine($"Record with ID: {id} doesn't exist");
                    UpdateRecord();
                }

                string date = GetDateInput();
                int quantity = GetNumberInput("Please insert quantity number (measure of your choice), or type 0 to return to menu");
                
                var tableCmd = connection.CreateCommand();
                tableCmd.Parameters.AddWithValue("@date", date);
                tableCmd.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@id", id);
                
                tableCmd.CommandText =
                    $"UPDATE drinking_water SET Date = '{@date}', Quantity = {@quantity} WHERE Id = {@id}";

                tableCmd.ExecuteNonQuery();
                
                connection.Close();
            }
        }
        
        string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (dd-mm-yyyy), or type 0 to return to main menu");
            string dateInput = Console.ReadLine();
            
            if (dateInput == "0")
            {
                GetUserInput();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-mm-yyyy", new CultureInfo("pl-PL"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Invalid date, the correct format is: (dd-mm-yyyy)");
                dateInput = Console.ReadLine();
            }
            
            return dateInput;
        }

        int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                GetUserInput();
            }

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("Invalid number");
                numberInput = Console.ReadLine();
            }
            
            int finalNumber = Convert.ToInt32(numberInput);

            return finalNumber;
        }

        void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
