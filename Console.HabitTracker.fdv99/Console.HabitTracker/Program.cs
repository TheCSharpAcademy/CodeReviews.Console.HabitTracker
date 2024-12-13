using System;
using System.Data.SQLite;
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
            Console.WriteLine("5. Report on a Habit");
            Console.WriteLine("6. Quit Program");

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

        habit.Habit = GetInput("What habit would you like to track: ").ToLower();

        habit.Date = GetDate();

        habit.Quantity = GetQuantity();

        SqliteDataAccess dataAccess = new SqliteDataAccess();
        dataAccess.InsertHabit(habit);
    }

    private static int GetQuantity()
    {
        while (true)
        {
            string quantityInput = GetInput("Please enter the quantity: ");
            if (int.TryParse(quantityInput, out int quantity))
            {
                return quantity;
            }

            Console.WriteLine("Value must be an integer.");
        }
    }

    private static string GetDate()
    {
        string dateInputString = GetInput("Please enter the date, or press T to use today: ");

        if (dateInputString.ToLower() == "t")
        {
            return DateTime.Now.ToString("MM/dd/yyyy");
        }

        if (DateTime.TryParse(dateInputString, out DateTime parsedDate))
        {
            return parsedDate.ToString("MM/dd/yyyy");
        }

        Console.WriteLine("Invalid date format. Setting date to today.");
        return DateTime.Now.ToString("MM/dd/yyyy");
    }

    public static void EditHabit()
    {
        ShowHabits();
        int habitId = GetValidHabitId("Please enter the Id of the habit you would like to edit: ");

        SqliteDataAccess dataAccess = new SqliteDataAccess();
        HabitModel habit = dataAccess.GetHabitById(habitId);

        Console.WriteLine($"Habit: {habit.Habit}");
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
        int habitId = GetValidHabitId("Please enter the Id of the habit you would like to delete: ");

        SqliteDataAccess dataAccess = new SqliteDataAccess();

        dataAccess.DeleteHabit(habitId);

        Console.Clear();
        Console.WriteLine($"\nHabit ID: {habitId} deleted.\n");
    }

    public static void ReportHabit()
    {
        ShowHabits();
        string habitName = GetInput("\nPlease enter the habit you would like a report on: ").ToLower();

        if (string.IsNullOrWhiteSpace(habitName))
        {
            Console.WriteLine("Invalid input. Please enter a valid habit name.");
            return;
        }

        SqliteDataAccess dataAccess = new SqliteDataAccess();

        try
        {
            string sqlStatement = "SELECT * FROM Habits WHERE Habit = @HabitName";
            SQLiteParameter parameter = new SQLiteParameter("@HabitName", habitName);

            var habits = dataAccess.LoadData(sqlStatement, parameter);

            if (habits == null || habits.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Habit does not exist.\n");
                return;
            }
            
            int totalQuantity = 0;

            foreach (var habit in habits)
            {
                totalQuantity += habit.Quantity;
            }
            Console.Clear();
            Console.WriteLine($"\nTotal quantity of {habitName}: {totalQuantity}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static int GetValidHabitId(string prompt)
    {
        while (true)
        {
            string input = GetInput(prompt);
            if (int.TryParse(input, out int habitId))
            {
                return habitId;
            }

            Console.WriteLine("Invalid input. Please enter a valid habit Id.");
        }
    }

    private static string GetInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
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
                    ReportHabit();
                    break;
                case 6:
                    // Quit Program
                    return;
            } 
           
        }

    }
}