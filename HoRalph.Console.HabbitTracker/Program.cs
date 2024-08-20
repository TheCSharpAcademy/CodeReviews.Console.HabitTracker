using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Reflection.Metadata;
using System.Xml.XPath;
using Microsoft.Data.Sqlite;

namespace habit_tracker
{

class Program
{
    public static string connectionString = @"Data Source = habit-Tracker.db";
    void Main(string[] args)
    {
        string?result="";
        

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                                    HabitID int AUTO_INCREMENT,
                                    Date varchar(255),
                                    Habit varchar(255),
                                    Units varchar(255),
                                    Quantity int)";

            tableCmd.ExecuteNonQuery();

            connection.Close();


        

        }
        bool exitApp = false;
        while(!exitApp)
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("-----------------MAIN MENU-----------------");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine(@"Type 0 to Close Application.
            Type 1 to View All Records.
            Type 2 to Insert Record.
            Type 3 to Delete Record.
            Type 4 to Update Record.
            Type 5 to Delete Table.");
            Console.WriteLine("------------------------------------------");
            result = Console.ReadLine();

            while(!int.TryParse(result,out int choice))
            {
                Console.WriteLine("Invalid Input! Please try again.");
                result = Console.ReadLine();
            }
            
            switch (result)
            {
                case "0":
                return;
                

                case "1":
                //view all records
                break;

                case "2": //insert Record
                break;

                case "3": //Delete Record
                break;

                case "4": //Update Record
                break;
                
                case "5": //drop table
                break;

                default:
                break;

            }
        }
    }


    void InsertRecord()
    {

    }

}




}



