using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitLoggerApp.Models;

namespace HabitLoggerApp.Helpers
{
    public static class InputHelper
    {
        public static string GetNonEmptyInput(string prompt, Func<string?> readInput, Action<string> writeOutput)
        {
            string? input;
            do
            {
                writeOutput($"{prompt}: ");
                input = readInput();

                if (string.IsNullOrWhiteSpace(input))
                {
                    writeOutput($"{prompt} cannot be empty. Please try again.");
                }

            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }
    }
}
