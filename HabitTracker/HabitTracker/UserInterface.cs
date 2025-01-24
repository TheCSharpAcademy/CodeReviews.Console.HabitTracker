using HabitTracker.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitTracker.Models;

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

                case "u":
                    Update();
                    break;

                default:
                    Console.WriteLine("Invalid input, choose a letter from the menu");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ViewData()
    {
        var Data = DatatBaseOperations.GetAll();

        foreach (var item in Data)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"{nameof(item.Id)}: {item.Id}");
            Console.WriteLine($"{nameof(item.Date)}: {item.Date.ToString("dd-MM-yyyy")}");
            Console.WriteLine($"{nameof(item.Quantity)}: {item.Quantity}");
            Console.WriteLine("-----------------------------\n");
        }
    }

    private void Update()
    {
        ViewData();
        Console.Write("Choose the id of the habit you want to update: ");
        int id = ValidateNumber(Console.ReadLine());

        while (!DatatBaseOperations.Exists(id))
        {
            Console.WriteLine("this id does not exist, choose a different one!");
            id = ValidateNumber(Console.ReadLine());
        }

        Console.Write("Give a date in the format dd-mm-yyyy: ");
        string date = ValidateDate(Console.ReadLine());

        Console.Write("Give a quantity: ");
        int quantity = ValidateNumber(Console.ReadLine());

        DatatBaseOperations.Update(id, date, quantity);
    }
    private void Insert()
    {
        Console.Write("Give a date in the format dd-mm-yyyy: ");
        string date = ValidateDate(Console.ReadLine());

        Console.Write("Give a quantity: ");
        int quantity = ValidateNumber(Console.ReadLine());

        DatatBaseOperations.AddData(new WaterDrinkingHabit() { Date = DateTime.Parse(date), Quantity = quantity});
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

    private int ValidateNumber(string? number)
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
