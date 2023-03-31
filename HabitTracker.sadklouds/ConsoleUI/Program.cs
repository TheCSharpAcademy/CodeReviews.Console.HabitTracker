
using Microsoft.Extensions.Configuration;
using HabbitTrackerLibrary;
using HabbitTrackerLibrary.sadklouds;
using ConsoleUI;
using ConsoleUI.Helpers;

internal class Program
{

    private static void Main(string[] args)
    {
        SqliteDataAccess db = new(GetConnectionString());
        db.ConnectDB();
        CRUD crud = new(db);


        int Operation;
        while (true)
        {
            MenuHelper.MainMenu();
            Console.Write("Which operation do you wish to perform?: ");
            string input = Console.ReadLine();

            if (Int32.TryParse(input, out Operation))
            {
                switch (Operation)
                {
                   
                    case 1:
                        crud.GetHabitRecords();
                        break;
                    case 2:
                        crud.AddHabitRecord();
                        break;
                    case 3:
                        crud.DeleteHabitRecord();
                        break;
                    case 4:
                        crud.UpdateHabitRecord();
                        break;

                    case 0:
                        Console.WriteLine("Exiting Habit Tracker...");
                        return;
                    default:
                        Console.WriteLine("Unknown command was given.");
                        break;
                }
            }

        }

    }
    private static string GetConnectionString(string connectionStringName = "Default")
    {
        string output = "";

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var config = builder.Build();

        output = config.GetConnectionString(connectionStringName);
        return output;
    }
}