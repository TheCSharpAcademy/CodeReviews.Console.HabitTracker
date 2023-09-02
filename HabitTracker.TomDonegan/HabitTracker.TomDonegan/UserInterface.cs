using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.TomDonegan
{
    internal class UserInterface
    {
        internal static void MainMenu()
        {
            bool runningTracker = false;

            while (!runningTracker)
            {
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Welcome to your Habit Tracker");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("What would like to do today?\n");
                Console.WriteLine("1 - View all habit data.");
                Console.WriteLine("2 - Add new habit entry.");
                Console.WriteLine("3 - Update an existing entry.");
                Console.WriteLine("4 - Delete an entry.");
                Console.WriteLine("0 - Exit Habit Tracker");

                string menuSelection = Console.ReadLine();

                switch (menuSelection)
                {
                    case "1":
                        viewAllHabitData("");
                        break;
                    case "2":
                        InsertHabitData();
                        break;
                    case "3":
                        UpdateHabitData();
                        break;
                    case "4":
                        DeleteEntry();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        internal static async void viewAllHabitData(string viewSelection)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("        All Habit Records        ");
            Console.WriteLine("---------------------------------\n");
            if (string.IsNullOrEmpty(viewSelection))
            {
                await Database.AsyncDatabaseConnection("read", $"SELECT * FROM drinking_water");
            }
            else
            {
                await Database.AsyncDatabaseConnection(
                    "read",
                    $"SELECT * FROM drinking_water WHERE Id = '{viewSelection}'"
                );
            }
            Console.ReadLine(); // Need to bypass this for record deletion ???
        }

        internal static async void InsertHabitData()
        {
            string date = GetDateInput();
            double quantity = GetQuantityInput();

            Console.WriteLine($"Adding: Date: {date} Quantity: {quantity}L to the database.");
            await Database.AsyncDatabaseConnection(
                "write",
                $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})"
            );
            Console.WriteLine("New habit data added successfully.");
            Console.ReadLine();
        }

        internal static async void DeleteEntry()
        {
            bool runDelete = false;

            while (!runDelete)
            {
                await Database.AsyncDatabaseConnection("read", $"SELECT * FROM drinking_water");

                Console.WriteLine("-----------------------------");
                Console.WriteLine("Habit Record Deleter");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("Please type the Id of the record you want to delete.");

                string deleteSelection = Console.ReadLine();
                Console.Clear();

                await Database.AsyncDatabaseConnection("delete", $"SELECT COUNT(*) FROM drinking_water WHERE Id = '{deleteSelection}'");

                Console.WriteLine("-----------------------------");
                Console.WriteLine("Habit Record Deleter");
                Console.WriteLine("-----------------------------\n");
                Console.WriteLine("Are you sure you want the delete the following record? (y/n)");
                string confirmDelete = Console.ReadLine();

                switch (confirmDelete)
                {
                    case "y":
                        await Database.AsyncDatabaseConnection(
                            "delete",
                            $"DELETE FROM drinking_water WHERE Id = '{deleteSelection}'"
                        );
                        runDelete = true;
                        break;
                    case "n":
                        Console.Clear();
                        break;
                }
            }
            MainMenu();
        }

        internal static async void UpdateHabitData()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("     Habit Record Updater    ");
            Console.WriteLine("-----------------------------\n");
            string dateSelection = GetDateInput();
            //string query = $"SELECT * FROM drinking_water WHERE Date = '{dateSelection}'";
            await Database.AsyncDatabaseConnection(
                "read",
                $"SELECT * FROM drinking_water WHERE Date = '{dateSelection}'"
            );
            //await Database.AsyncDatabaseConnection("read", $"INSERT INTO drinking_water (Date, Quantity) VALUES ('{date}', {quantity})");
        }

        internal static async void CheckForEntry(string date)
        {
            await Database.AsyncDatabaseConnection(
                "read",
                $"SELECT * FROM drinking_water WHERE Date = '{date}'"
            );
            MainMenu();
        } // Done

        private static double GetQuantityInput()
        {
            Console.WriteLine(
                "Please insert the quantity of water consumed. Type 0 to return to the main menu."
            );

            string quantityInput = Console.ReadLine();

            while (!double.TryParse(quantityInput, out _))
            {
                Console.WriteLine("Your quantity must be a number. Try again.");
                quantityInput = Console.ReadLine();
            }

            if (quantityInput == "0")
                MainMenu();

            return double.Parse(quantityInput);
        } // Done

        private static string GetDateInput()
        {
            Console.WriteLine(
                "Please insert the date: (Format: dd-mm-yy). Type 0 to return to the main menu."
            );

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
                MainMenu();

            string requiredFormat = @"\d{2}-\d{2}-\d{2}";

            while (!Regex.IsMatch(dateInput, requiredFormat) || dateInput.Length < 6)
            {
                Console.WriteLine(
                    "Please enter the date in the required format and length. Try again."
                );
                dateInput = Console.ReadLine();
            }
            ;

            int dayNumber = Convert.ToInt32(dateInput[..2]);
            int monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));

            while (dayNumber < 01 || dayNumber > 31 || monthNumber < 01 || monthNumber > 12)
            {
                Console.WriteLine(
                    "Day date must be between 01 and 31.\nMonth date must be between 01 and 12. Try again."
                );
                dateInput = Console.ReadLine();
                dayNumber = Convert.ToInt32(dateInput[..2]);
                monthNumber = Convert.ToInt32(dateInput.Substring(3, 2));
            }
            ;

            return dateInput;
        }

        internal static void HabitTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Record", typeof(string));
            table.Columns.Add("Date", typeof(string));
            table.Columns.Add("Quantity", typeof(int));

            table.Rows.Add("Test", "Test", 5);

            // Display column titles
            Console.WriteLine(
                $"{table.Columns[0].ColumnName}, {table.Columns[1].ColumnName}, {table.Columns[2].ColumnName}"
            );

            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"{row["Record"]}, {row["Date"]}, {row["Quantity"]}");
            }

            Console.ReadLine();
        }
    }
}
