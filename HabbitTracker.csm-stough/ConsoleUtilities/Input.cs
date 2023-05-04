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
        /// <summary>
        /// Allows the user to enter an integer number
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static int GetInt(string prompt)
        {
            Console.WriteLine(prompt + ":");
            string integer = Console.ReadLine();
            int input = 0;

            while(!int.TryParse(integer, out input))
            {
                Console.WriteLine($"Invalid Input...\n {prompt}");
                integer = Console.ReadLine();
            }

            return input;
        }

        /// <summary>
        /// Allows the user to enter a floating point number
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static float GetFloat(string prompt)
        {
            Console.WriteLine(prompt + ":");
            string floating = Console.ReadLine();
            float input = 0;

            while (!float.TryParse(floating, out input))
            {
                Console.WriteLine($"Invalid Input...\n {prompt}");
                floating = Console.ReadLine();
            }

            return input;
        }

        /// <summary>
        /// Allows user to enter a formatted DateTime object
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime GetDate(string prompt, string format)
        {
            Console.WriteLine(prompt + ":");
            string date = Console.ReadLine();
            DateTime dateTime;

            while (!DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                Console.WriteLine($"Invalid Input...\n{prompt}");
                date = Console.ReadLine();
            }

            return dateTime;
        }

        /// <summary>
        /// Allows a user to choose from a menu of options
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="choices"></param>
        /// <returns></returns>
        public static string GetChoice(string prompt, params string[] choices)
        {
            Menu choiceMenu = new Menu(prompt);

            int num = 1;
            string selected = "";

            choices.ToList().ForEach(choice =>
            {
                choiceMenu.AddOption(num.ToString(), choice, () => { selected = choice; });
                num++;
            });

            choiceMenu.SelectOption(false);

            return selected;
        }
    }
}
