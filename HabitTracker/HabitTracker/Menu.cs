using System;

namespace HabitTracker
{
    internal class Menu
    {
        public static void GetUserInput()
        {
            Console.Clear();
            bool runApp = true;

            while (runApp)
            {
                Console.Clear();
                string habitName = null;
                Console.WriteLine("Welcome to a habit tracker, wich option do you want to use?");

                Console.WriteLine("Options:");

                Console.WriteLine("a. Delete record");

                Console.WriteLine("b. Insert record");

                Console.WriteLine("c. View All Records");

                Console.WriteLine("d. Update record");

                Console.WriteLine("e. Exit app");

                string option = Console.ReadLine();

                


                switch (option)
                {
                    case "e":
                        Console.WriteLine("GoodBye");
                        runApp = false;
                        Environment.Exit(0);
                        break;
                    case "a":
                        DatabaseHelpers.Delete();
                        break;
                    case "b":
                        DatabaseHelpers.Insert();
                        break;
                    case "c":
                        DatabaseHelpers.ViewHabits(ref habitName);
                        break;
                    case "d":
                        DatabaseHelpers.Update();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\n\nInvalid option. Press any key to return to main menu!");
                        Console.ReadLine();
                        break;
                }
            }
        }
        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease enter the date: (Format: yyyy-mm-dd). Type 0 to return to main menu.\n\n");

            
            string dateInput = Console.ReadLine();

            
            while(!DateTime.TryParse(dateInput.Trim(), out _) && dateInput.Length > 10)
            {
                Console.WriteLine("\n\nInvalid date. Try again!");
                dateInput = Console.ReadLine(); 
            }
            if (dateInput == "0") GetUserInput();

            return dateInput;
        }
        internal static int GetNumberInput(string message)
        {
            Console.WriteLine($"\n\n{message}. Type 0 to return to main menu\n\n");

            string numberInput = Console.ReadLine();
            int result = 0;
            while(!int.TryParse(numberInput, out result))
            {
                Console.WriteLine("\n\nInvalid input. Please enter integer");
                numberInput = Console.ReadLine();
            }

            if (numberInput == "0") GetUserInput();

            return result;
        }
        internal static string GetMeasureInput(string message) 
        {
            Console.WriteLine($"\n\n{message}. Type 0 to return to main menu\n\n");
            
            string measureInput = Console.ReadLine();

            if (measureInput == "0") GetUserInput();

            return measureInput;
        }
    }
}
