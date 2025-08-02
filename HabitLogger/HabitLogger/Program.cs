
using System.ComponentModel.Design;
using Microsoft.Data.Sqlite;

class Program
{
    static string connectionString = "Data source=habits.db";
   static void Main(string[] args)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var createCmd = connection.CreateCommand();
            createCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER)";
            createCmd.ExecuteNonQuery();
        }
        getUserInput();
    }

    static void getUserInput()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine("\nMain menu");
            Console.WriteLine("What do u want to do?");
            Console.WriteLine("\nType 0 to close the applicaiton");
            Console.WriteLine("\nType 1 to view all the records");
            Console.WriteLine("\nType 2 to insert records");
            Console.WriteLine("\nType 3 to update records");
            Console.WriteLine("\nType 4 to delete records");
            Console.WriteLine("--------------------------------");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    closeApp = true;
                    break;
                case "1":
                    viewRecords();
                    break;
                case "2":
                    insertRecord();
                    break;
                case "3":
                    updateRecord();
                    break;
                case "4":
                    deleteRecord();
                    break;
                default:
                    Console.WriteLine("Invalid command. Please try again.");
                    break;
            }

        }

    }
    static void insertRecord()
    {
        string date = getUserDate();
        if (date == null) return;
        int quantity = getUserQuantity();
        if(quantity == -1) return;

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO drinking_water(Date , Quantity) VALUES ($Date , $Quantity)";
            insertCmd.Parameters.AddWithValue("$Date", date);
            insertCmd.Parameters.AddWithValue("$Quantity", quantity);
            insertCmd.ExecuteNonQuery();

        }
    }
    
    private static bool isValidDate(string date)
    {
        try
        {
            DateTime.ParseExact(date, "dd-MM-yy", null);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private static string getUserDate()
    {
        Console.WriteLine("Enter the date in this format(dd-mm-yy).Type 0 to return to main menu");
        string dateInput = Console.ReadLine();

        if (dateInput == "0")
        {
            return null;
        }
        if (!isValidDate(dateInput))
        {
            Console.WriteLine("Invalid date format or value. Try again.");
            return getUserDate(); 
        }

        return dateInput;

    }
    private static int getUserQuantity()
    {
        Console.WriteLine("Enter quantity:");
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid number. Try again.");
            return getUserQuantity();
        }
        return quantity;

    }
     static void viewRecords()
     {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var viewCmd = connection.CreateCommand();
            viewCmd.CommandText = @"SELECT * FROM drinking_water";

            using (var Reader = viewCmd.ExecuteReader())
            {
                if(!Reader.HasRows)
                {
                    Console.WriteLine("\n\t\tThere are no records");
                    return;
                }
                while(Reader.Read())
                {
                    int id = Reader.GetInt32(0);
                    string date = Reader.GetString(1);
                    int quantity = Reader.GetInt32(2);
                    Console.WriteLine($"ID:  {id} ||  DATE:  {date} || QUANTITY:  {quantity} GLASSES");
                }
            }
        }
     }
    static void deleteRecord()
    {
        Console.WriteLine("Type an Id to delete the record:");
        string id = Console.ReadLine();
        if (!int.TryParse(id , out int Id) || Id <=0)
        {
            Console.WriteLine("Invalid Id entered");
            getUserInput();
        }
        using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = @"DELETE FROM drinking_water WHERE Id = $Id";
                deleteCmd.Parameters.AddWithValue("$Id", id);
                int affectedRows = deleteCmd.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    Console.WriteLine("DELETE SUCCESSFULL");
                }
                else
                {
                    Console.WriteLine("No records too be found for deletion");
                }
            }
    }
    static void updateRecord()
    {
        Console.WriteLine("Enter an Id to update:");
        string id = Console.ReadLine();
        if (!int.TryParse(id, out int Id) || Id <= 0)
        {
            Console.WriteLine("Invalid Id entered");
            getUserInput();
        }
        string date = getUserDate();
        if (date == null) return;
        int quantity = getUserQuantity();
        if (quantity == -1) return;

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"UPDATE drinking_water SET Quantity = $Quantity, Date = $Date WHERE Id = $Id;";
            updateCmd.Parameters.AddWithValue("$Date", date);
            updateCmd.Parameters.AddWithValue("$Quantity", quantity);
            updateCmd.Parameters.AddWithValue("$Id", id);

            int affectedRows = updateCmd.ExecuteNonQuery();
            if(affectedRows > 0)
            {
                Console.WriteLine("UPDATE SUCCESFULL");
            }
            else
            {
                Console.WriteLine("Id not found");
            }

        }
    }
}
