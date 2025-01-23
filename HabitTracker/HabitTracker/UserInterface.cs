using HabitTracker.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    Insert();
                    break;

                default:
                    Console.WriteLine("Invalid input, choose a letter from the menu");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void Insert()
    {
        Console.Write("Give a date in the format dd-mm-yyyy: ");
        string date = ValidateDate(Console.ReadLine());

        Console.Write("Give a quantity: ");
        int quantity = ValidateQuantity(Console.ReadLine());

        DatatBaseOperations.AddData(date, quantity);
    }

    private string ValidateDate(string? date)
    {
        while (date == null || !DateTime.TryParse(date, CultureInfo.CurrentCulture, out _))
        {
            Console.WriteLine("Wrong date format. try again: ");
            date = Console.ReadLine();
        }

        return date;
    }

    private int ValidateQuantity(string? number)
    {
        int parsedNumber;
        while(!int.TryParse(number, out parsedNumber))
        {
            Console.WriteLine("Wrong format, try again: ");
            number = Console.ReadLine();
        }

        return parsedNumber;
    }
}
