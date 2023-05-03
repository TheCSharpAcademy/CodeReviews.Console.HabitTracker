using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUtilities
{
    /// <summary>
    /// A wrapper class for Console used to ge certain data types from the user
    /// </summary>
    public class Input
    {

        public static int GetInt(string prompt)
        {
            Console.WriteLine(prompt + ":");
            string integer = Console.ReadLine();
            int input = 0;

            while(!int.TryParse(integer, out input))
            {
                Console.WriteLine("Invalid Input...\n {0}", prompt);
                integer = Console.ReadLine();
            }

            return input;
        }

        public static float GetFloat(string prompt)
        {
            Console.WriteLine(prompt + ":");
            string floating = Console.ReadLine();
            float input = 0;

            while (!float.TryParse(floating, out input))
            {
                Console.WriteLine("Invalid Input...\n {0}", prompt);
                floating = Console.ReadLine();
            }

            return input;
        }

        public static DateTime GetDate(string prompt, string format)
        {
            Console.WriteLine(prompt + ":");
            string date = Console.ReadLine();
            DateTime dateTime;

            while (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine("Invalid Input...\n{0}", prompt);
                date = Console.ReadLine();
            }

            return dateTime;
        }
    }
}
