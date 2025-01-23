using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker;

internal class UserInterface
{
    internal void ShowMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("Hello! what do you want to do in the habit tracker?\n");
            Console.WriteLine("i: Input data");
            Console.WriteLine("u: Update data");
            Console.WriteLine("d: Delete data");
            Console.WriteLine("v: View all data");
            Console.WriteLine("q: Quit");

            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "i":


                    break;

                default:
                    Console.WriteLine("Invalid input, choose a letter from the menu");
                    Console.ReadKey();
                    break;
            }
        }
    }

}
