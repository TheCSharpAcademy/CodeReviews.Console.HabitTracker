using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habit_Logger
{
    public static class Helper
    {

        public static void printError(string msg)
        {
            //Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(msg);

           // Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void printData(dynamic data)
        {
            //Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(data);
           // Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine();
        }
    }
}
