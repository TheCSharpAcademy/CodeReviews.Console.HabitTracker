using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabbitLogger.AndreasGuy54
{
    internal static class Helpers
    {
        internal static void GetUserInput()
        {
            Console.Clear();

            bool closeApp = false;

            while(!closeApp)
            {
                Console.WriteLine("\n\nMain Menu");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("------------------------------------------\n");

                int userInput;
                bool validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);

                while(!validInput) 
                {
                    Console.WriteLine("Enter a valid number");
                    validInput = int.TryParse(Console.ReadLine().ToLower().Trim(), out userInput);
                }

                switch (userInput)
                {
                    case 0:
                        Console.WriteLine("\nGoodbye:\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
