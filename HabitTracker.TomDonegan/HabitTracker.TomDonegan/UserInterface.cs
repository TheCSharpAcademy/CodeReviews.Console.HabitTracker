using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker.TomDonegan
{
    internal class UserInterface
    {
        internal static void MainMenu()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Welcome to your Habit Tracker");
            Console.WriteLine("-----------------------------\n");
            Console.WriteLine("What would like to do today?\n");
            Console.WriteLine("1 - Check most recent habit data.");
            Console.WriteLine("2 - Select an individual habit.");
            Console.WriteLine("3 - Create a new habit.");
            Console.WriteLine("4 - Add data to an exisiting habit.");
            Console.WriteLine("5 - Update an existing entry.");
            Console.WriteLine("6 - Delete an entry.");
            Console.WriteLine("7 - Delete a habit.");
            Console.WriteLine("0 - Exit Habit Tracker");

            string menuSelection = Console.ReadLine();

            switch (menuSelection)
            {
                case "1":

                    break;
                case "2":

                    break;
                case "3":

                    break;
                case "4":
                    InsertHabitData();
                    break;
                case "5":

                    break;
                case "6":

                    break;
                case "7":

                    break;
                case "0":
                    Environment.Exit(0);
                    break;
            }


        }

        internal static async void InsertHabitData()
        {
            string date = GetDateInput();
            int quantity = GetQuantityInput();
            //await Database.AsyncDatabaseConnection("SELECT name FROM sqlite_master WHERE type='table'");
            //Database.DatabaseConnection("read",$"SELECT name FROM sqlite_master WHERE type='table'");

            //Database.DatabaseConnection("write", $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})");
            await Database.AsyncDatabaseConnection("write", $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})");

        }

        private static int GetQuantityInput()
        {
            Console.WriteLine("Please insert the quantity of water consumed. Type 0 to return to the main menu.");

            string quantityInput = Console.ReadLine();

            if (quantityInput == "0") MainMenu();

            return Int32.Parse(quantityInput);
        }

        private static string GetDateInput()
        {
            Console.WriteLine("Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu.");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") MainMenu();

            return dateInput;
        }
    }
}
