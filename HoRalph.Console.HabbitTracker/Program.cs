using System;
using System.Xml.XPath;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{

class Program
{
    public static string connectionString = @"Data Source = habit-Tracker.db";
    static void Main(string[] args)
    {
        string?result="";
        

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS HABIT (
                                    HabitID INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Date TEXT,
                                    Habit TEXT,
                                    Units TEXT,
                                    Quantity INTEGER)";

            tableCmd.ExecuteNonQuery();
            
            connection.Close();


        

        }
        bool exitApp = false;
        while(!exitApp)
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("-----------------MAIN MENU-----------------");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine(@"
            Type 0 to Close Application.
            Type 1 to View All Records.
            Type 2 to Insert Record.
            Type 3 to Delete Record.
            Type 4 to Update Record.
            Type 5 to Delete Table.
            ");
            Console.WriteLine("------------------------------------------");
            result = Console.ReadLine();

            while(!int.TryParse(result,out int choice))
            {
                Console.WriteLine("Invalid Input! Please try again.");
                result = Console.ReadLine();
            }
            Console.Clear();
            switch (result)
            {
                case "0":
                return;
                

                case "1": //view all records
                ViewAll();
                break;

                case "2": //insert Record
                InsertRecord();
                break;

                case "3": //Delete Record
                break;

                case "4": //Update Record
                break;
                
                case "5": //drop table
                DeleteTable();
                break;

                default:
                break;

            }
        }
    }

    static void ViewAll()
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"SELECT * FROM HABIT;";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();
            int id = 0;
            string date ="";
            string habit ="";
            string units ="";
            string quantity ="";
            string?pause;

            if (reader.HasRows)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i).PadRight(15));
                    Console.Write("|");
                }
                Console.WriteLine();
              
                while(reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetString(i).PadRight(15));
                        Console.Write("|");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n\n Press any key to return to the menu.");
                pause = Console.ReadLine();
            }
            
            

            else
                Console.WriteLine("Empty  table.");

            reader.Close();
            connection.Close();

        }
    }

    static void InsertRecord()
    {
        Console.WriteLine("Enter a date (MM/DD/YYYY).");
        string?date = Console.ReadLine();
        bool validDate = false;

        while (!validDate)
        {
            if (int.Parse(date.Substring(0,2)) <=12)
                break;
                
            
        }

        
        Console.WriteLine("Enter a habit.");
        string?habit = Console.ReadLine();

        Console.WriteLine("Enter the unit.");
        string?units = Console.ReadLine();

        Console.WriteLine("Enter the quantity.");
        string?quantity = Console.ReadLine();
        
        while (!int.TryParse(quantity,out int result))
        {
            Console.WriteLine("Invalid Number");
            quantity = Console.ReadLine();
        }

        
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @$"INSERT INTO HABIT (Date, Habit, Units, Quantity)
                                    VALUES ('{date}', '{habit}', '{units}', {quantity});";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }


    static void DeleteTable()
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"DROP TABLE HABIT;";

            tableCmd.ExecuteNonQuery();
            connection.Close();

        }

    }

}




}



