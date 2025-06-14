using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLoggerApp.Utils
{
    public static class Helpers
    {
        public static string GetNonEmptyInput(string prompt)
        {
            string? input;
            do
            {
                Console.Write($"{prompt}: ");
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine($"{input} cannot be empty. Please try again.");
                }

            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }
    }
}
