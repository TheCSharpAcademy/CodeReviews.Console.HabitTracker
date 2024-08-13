using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Habits;

internal class HabitTracker
{
    public HabitTracker()
    {
        // check for/create database
    }

    public void Start()
    {
        Console.WriteLine("Welcome to the Habit Tracker!\n\n");

        while (true)
        {
            Console.WriteLine("MAIN MENU\n");
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("-----------------------------------------------\n");

            string? userInput = Console.ReadLine() ?? "";

            // Exit on 0.
            if (userInput.Equals("0"))
            {
                Console.WriteLine("Thank you for using the Habit Tracker.\n");
                return;
            }

            // Handle user selection.
            switch (userInput)
            {
                case "1":
                    //TODO ViewAllRecords()
                    break;
                case "2":
                    //TODO InsertRecord()
                    break;
                case "3":
                    //TODO DeleteRecord()
                    break;
                case "4":
                    //TODO UpdateRecord()
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
