using System;
using System.Linq;

namespace HabitTracker.SamGannon
{
    internal class Menu
    {
        private string connectionString = @"Data Source=habit-Tracker.db";
        private DataAccess dataFunctions;

        public void GetUserInput()
        {
            dataFunctions = new DataAccess(connectionString);

            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.Clear();
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Type R to View Reports");

                string commandInput = Console.ReadLine().ToUpper();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Console.Clear();
                        dataFunctions.GetAllRecords();
                        break;
                    case "2":
                        dataFunctions.InsertRecord();
                        break;
                    case "3":
                        dataFunctions.DeleteRecord();
                        break;
                    case "4":
                        dataFunctions.UpdateRecord();
                        break;
                    case "R":
                        ReportsMenu();
                        break;
                    default:
                        Console.WriteLine("Invalid command, please enter a number 0 to 4");
                        break;

                }
            }
        }

        private void ReportsMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\nREPORTS MENU");
            Console.WriteLine("\nWhich report would you like to see?");
            Console.WriteLine("\nType 0 to go back");
            Console.WriteLine("Type 1 to see days with most cups");

            string commandInput = Console.ReadLine();

            switch(commandInput)
            {
                case "0":
                    GetUserInput();
                    break;
                case "1":
                    dataFunctions.GetRecordsByCups();
                    break;
                default:
                    Console.WriteLine("Invalid command please try again");
                    ReportsMenu();
                    break;
            }
            
   
        }
    }
}
