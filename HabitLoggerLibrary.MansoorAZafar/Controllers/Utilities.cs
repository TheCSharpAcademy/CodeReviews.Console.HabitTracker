using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitLoggerLibrary.MansoorAZafar.Controllers
{
    internal class Utilities
    {
        public static void PressToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void GetValidDateddMMyyFormat(ref DateTime date, string message="Please Enter the date\n> ", string errorMessage="Invalid Answer\nPlease Enter the date\n> ")
        {
            Console.Write(message);
            while(!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.Write(errorMessage);
            }
        }

        public static void GetValidQuantity(ref int quantity, 
            string message = "Please Enter the quantity\n> ", 
            string errorMessage = "Invalid Answer\nPlease Enter the quantity\n> ",
            int lowerRange = int.MinValue,
            int highRange = int.MaxValue)
        {
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out quantity) || (quantity < lowerRange || quantity > highRange)) 
            {
                Console.Write(errorMessage);
            }
        }

        public static void GetValidStringInput(ref string input, string message= "> ", string errorMessage= "Invalid Input\n Please Enter the data\n> ")
        {
            Console.Write(message);
            input = Console.ReadLine();
            while(string.IsNullOrEmpty(input))
            {
                Console.Write(errorMessage);
                input = Console.ReadLine();
            }
        }
    }
}
