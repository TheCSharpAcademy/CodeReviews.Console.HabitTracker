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
            habit.Date = DateTime.Now.ToString("MM/dd/yyyy");
            Console.WriteLine(habit.Date);
        }
        else 
        {
            DateTime parsedDate;
            if (DateTime.TryParse(dateInputString, out parsedDate))
            {
                habit.Date = parsedDate.ToString("MM/dd/yyyy");
            }
            else
            {
                Console.WriteLine("Invalid date format. Setting date to today.");
                Console.WriteLine("You can update the date from the menu");
                habit.Date = DateTime.Now.ToString("MM/dd/yyyy");
            }
        }

        bool quantityValid = false;
       
        string quantityInput = "";
        int quantity;

        while (quantityValid == false)
        {
            Console.WriteLine("Please enter the quantity: ");
            quantityInput = Console.ReadLine();

            if (int.TryParse(quantityInput, out quantity) == true)
            {
                habit.Quantity = quantity;
                quantityValid = true;
            }
            else
            {
                Console.WriteLine("Value must be an integer.");
            }
        }

        SqliteDataAccess dataAccess = new SqliteDataAccess();
        dataAccess.InsertHabit(habit);

    }

    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Habit Tracker!\n");

        // Check that database exists in current working directory and if not create one
        DataAccessHelpers dbCreate = new DataAccessHelpers();
        dbCreate.InitializeDatabase();

        while (true)
        {
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
                    return;
            } 
            Console.Clear();
        }

    }
}