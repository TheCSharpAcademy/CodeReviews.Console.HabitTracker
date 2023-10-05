using HabitTracker.K_MYR.Models;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace HabitTracker.K_MYR
{
    static internal class Helpers
    {
        static internal void GetUserInput()
        {
            bool closeApp = false;

            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("MAIN MENU".PadLeft(19));
                Console.WriteLine("-----------------------------");
                Console.WriteLine("0 - View All Records");
                Console.WriteLine("1 - Insert Record");
                Console.WriteLine("2 - Delete Record");
                Console.WriteLine("3 - Update Records");
                Console.WriteLine("4 - Exit Application");
                Console.WriteLine("-----------------------------");

                string Input = Console.ReadLine();

                switch (Input)
                {
                    case "0":
                        ShowAllRecords();
                        Console.WriteLine("Press enter to go back to the main menu");
                        Console.ReadLine();
                        break;
                    case "1":
                        InsertRecords();
                        break;
                    case "2":
                        DeleteRecord();
                        break;
                    case "3":
                        UpdateRecord();
                        break;
                    case "4":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice (0-4)!");
                        break;
                }
            }            
        }

        private static void ShowAllRecords()
        {
            Console.Clear();
            List<HabitRecord> tableData = SQLiteOperations.SelectAll();

            Console.WriteLine("----------------------------------");

            if (tableData.Count != 0)
            {
                foreach (var row in tableData)
                {
                    Console.WriteLine($"{row.Id} | {row.Date:dd-mm-yyy} - Quantity: {row.Quantity}");
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            
            Console.WriteLine("----------------------------------\n");            
        }

        private static void InsertRecords()
        {
            string dateInput = GetDateInput();
            int numberInput = GetNumberInput("Please insert quantity (no decimals allowed)");

            SQLiteOperations.Insert("Exercise", dateInput, numberInput);
        }
       

        private static void DeleteRecord()
        {
            Console.Clear();
            ShowAllRecords();
            int numberInput;

            do
            {
                numberInput = GetNumberInput("\nPlease enter the Id of the record you want to delete. Type 0 to return to the main menu");

                int rowCount = SQLiteOperations.Delete(numberInput);

                if (rowCount == 0)
                {
                    Console.WriteLine($"\nRecord with Id {numberInput} doesn't exist");
                }
                else
                {
                    Console.Clear();
                    ShowAllRecords();
                    Console.WriteLine($"Record with Id {numberInput} has been deleted!");
                }
            } while (numberInput != 0);
            
        }

        private static void UpdateRecord()
        {
            Console.Clear();
            ShowAllRecords();
            int numberInput;

            do
            {
                numberInput = GetNumberInput("Please enter the Id of the record you want to update. Type 0 to return to the main menu");

                if (SQLiteOperations.RecordExists(numberInput) == 1)
                {
                    string date = GetDateInput();
                    int quantity = GetNumberInput("Please insert quantity (no decimals allowed)");
                    SQLiteOperations.Update(numberInput, date, quantity);
                    Console.Clear();
                    ShowAllRecords();
                    Console.WriteLine($"Record with the Id {numberInput} has been updated\n");
                    
                }
                else
                {
                    Console.WriteLine("\nRecord with Id {id} doesn't exist");
                }
            } while (numberInput != 0); 
        }

        private static string GetDateInput()
        {
            Console.WriteLine("Please enter the date: (Format: dd-mm-yy)");
            string Input = Console.ReadLine();

            while (!DateTime.TryParseExact(Input, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid date. (Format dd-mm-yy");
                Input = Console.ReadLine();
            }

            return Input;
        }

        private static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();            

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\nInvalid input");
                numberInput = Console.ReadLine();
            }           

            return Convert.ToInt32(numberInput);
        }

    }
}

    
