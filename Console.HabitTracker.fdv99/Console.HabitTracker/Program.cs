using System;
using DataAccessLibrary;
using HabitData;

internal class Program
{
    public static int UserInput()
    {
        bool isValidInput = false;
        int userInput = 0;

        while (isValidInput == false)
        {
            // Ask user what they want to do
            Console.WriteLine("Please select an Option to get Started: ");
            Console.WriteLine("1. Enter a Habit");
            Console.WriteLine("2. Edit a Habit");
            Console.WriteLine("3. Delete a Habit");
            Console.WriteLine("4. Search Habit Results");
            Console.WriteLine("5. Quit");

            int.TryParse(Console.ReadLine(), out userInput);

            if (userInput > 0 && userInput < 6)
            {
                isValidInput = true;
            }
            else
            {
                Console.WriteLine("That was not a valid input.\n");
            }
        }
        return userInput;
    }

    public static void EnterHabit()
    {
        HabitModel habit = new HabitModel();
        Console.WriteLine("What habit would you like to track: ");
        habit.Habit = Console.ReadLine();
        Console.WriteLine("Please enter the date, or press T to use today: ");
        string dateInputString = Console.ReadLine();
        if (dateInputString.ToLower() == "t")
        {
            habit.Date = DateTime.Now.Date;
        }
        else 
        {
            DateTime parsedDate;
            if (DateTime.TryParse(dateInputString, out parsedDate))
            {
                habit.Date = parsedDate;
            }
            else
            {
                Console.WriteLine("Invalid date format. Setting date to today.");
                habit.Date = DateTime.Now.Date;
            }
        }
    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Habit Tracker!\n");

        // Check that database exists in current working directory and if not create one
        DataBaseCreate dbCreate = new DataBaseCreate();
        dbCreate.InitializeDatabase();

        int userInput = UserInput();
        switch (userInput)
        {
            case 1:
                // Enter a Habit
                EnterHabit();

                break;
            case 2:
                // Edit Habit
                break;
            case 3:
                // Delete Habit
                break;
            case 4:
                // Search Habit
                break;
            case 5:
                // Quit Program
                break;
        }

    }
}