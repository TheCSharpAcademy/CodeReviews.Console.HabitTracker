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
            Console.WriteLine("4. View All Habit Results");
            Console.WriteLine("5. Quit");

            int.TryParse(Console.ReadLine(), out userInput);

            if (userInput > 0 && userInput < 6)
            {
                isValidInput = true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("That was not a valid input.\n");
                
            }
        }
        return userInput;
    }

    public static void EnterHabit()
    {
        HabitModel habit = new HabitModel();
        Console.Clear();
        Console.WriteLine("What habit would you like to track: ");
        habit.Habit = Console.ReadLine().ToLower();

        habit.Date = GetDate();

        habit.Quantity = GetQuantity();

        SqliteDataAccess dataAccess = new SqliteDataAccess();
        dataAccess.InsertHabit(habit);
    }

    private static int GetQuantity()
    {
        bool quantityValid = false;
        int quantity = 0;

        while (quantityValid == false)
        {
            Console.WriteLine("Please enter the quantity: ");
            string? quantityInput = Console.ReadLine();

            if (int.TryParse(quantityInput, out quantity))
            {
                quantityValid = true;
            }
            else
            {
                Console.WriteLine("Value must be an integer.");
            }
        }

        return quantity;
    }

    private static string GetDate()
    {
        Console.WriteLine("Please enter the date, or press T to use today: ");
        string dateInputString = Console.ReadLine();

        if (dateInputString.ToLower() == "t")
        {
            return DateTime.Now.ToString("MM/dd/yyyy");
        }
        else
        {
            DateTime parsedDate;
            if (DateTime.TryParse(dateInputString, out parsedDate))
            {
                return parsedDate.ToString("MM/dd/yyyy");
            }
            else
            {
                Console.WriteLine("Invalid date format. Setting date to today.");
                Console.WriteLine("You can update the date from the menu/n");
                return DateTime.Now.ToString("MM/dd/yyyy");
            }
        }
    }

    public static void EditHabit()
    {
        ShowHabits();
        Console.WriteLine("Please enter the Id of the habit you would like to edit: ");
        int habitId = Convert.ToInt32(Console.ReadLine());
        SqliteDataAccess dataAccess = new SqliteDataAccess();
        HabitModel habit = dataAccess.GetHabitById(habitId);

        Console.WriteLine($"Habit: {habit.Habit}");
        Console.WriteLine($"Enter a new Date ({habit.Date}): ");
        habit.Date = GetDate();
        habit.Quantity = GetQuantity();

        dataAccess.UpdateHabit(habit);
    }

    public static void ShowHabits()
    {
        Console.Clear();

        string sqlStatement = "SELECT * FROM Habits";
        SqliteDataAccess dataAccess = new SqliteDataAccess();
        var habits = dataAccess.LoadData(sqlStatement);

        foreach (var habit in habits)
        {
            Console.WriteLine($"Id: {habit.Id}---Habit: {habit.Habit}---Quantity: {habit.Quantity}---Date: {habit.Date}");
        }
    }

    public static void DeleteHabit()
    {
        ShowHabits();
        Console.WriteLine("Please enter the Id of the habit you would like to delete: ");
        int habitId = Convert.ToInt32(Console.ReadLine());
        SqliteDataAccess dataAccess = new SqliteDataAccess();
        dataAccess.DeleteHabit(habitId);
        Console.Clear();
        Console.WriteLine($"Habit ID: {habitId} deleted.");
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
                    EditHabit();
                    break;
                case 3:
                    DeleteHabit();
                    break;
                case 4:
                    ShowHabits();
                    break;
                case 5:
                    // Quit Program
                    return;
            } 
           
        }

    }
}