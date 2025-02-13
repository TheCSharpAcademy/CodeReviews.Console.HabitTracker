using Services;
using DTO;
using Helpers;

// Establish DB Connection
DBService dBService = new DBService("Data source=local.db");
string menuSelection;

// TODO: Update an entry - HAVE THIS DISPLAY ALL OCCURENCES FIRST
Console.WriteLine
(@"
 Press one of the following
 1. Add a habit
 2. Update a habit
 3. Remove a habit
 4. Show habits
 ");

menuSelection = Console.ReadLine() ?? "";


switch (menuSelection)
{
    case "1":
        // Add a habit
        HabitEntry habitEntry = new();
        bool validMenuSelection = false;
        while (!validMenuSelection)
        {
                validMenuSelection = true;
                Console.WriteLine("Enter the name of the habit");
                string habitName = Console.ReadLine();

                if (string.IsNullOrEmpty(habitName))
                {
                    validMenuSelection = false;
                    Console.WriteLine("Habit name cannot be empty.");
                }

                habitEntry.Habit = habitName;

        } 

        while (!validMenuSelection)
        {
            try
            {
                validMenuSelection = true;
                Console.WriteLine("Enter the date the habit occurred in MM\\DD\\YYYY format.");
                string habitDate = Console.ReadLine();
                if (habitDate == null)
                {
                    throw new ArgumentNullException("Habit date cannot be null");
                }
                DateHelper.ConvertStringToDateTime(habitDate);
            }
            catch
            {
                Console.WriteLine("Invalid entry. Enter the date the habit occurred in MM\\DD\\YYYY format.");
                validMenuSelection = false;
            }
        } 


        do
        {
            try
            {
                Console.WriteLine("How many times did this habit occur?");
                string habitOccurences = Console.ReadLine();
            }
            catch (System.Exception)
            {
                Console.WriteLine("Invalid entry. Enter the number of times the habit occurred.");
                validMenuSelection = false;
            }
        } while (!validMenuSelection);



        // return a success or failure message
        break;
    case "2":
        // Show all habits
        // Update a habit 
        break;
    case "3":
        // Remove a habit 
        break;
    case "4":
        // Show a habits
        break;
    default:
        Console.WriteLine("Invalid Entry");
        break;
}

