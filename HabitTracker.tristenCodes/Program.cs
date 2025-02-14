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
        bool validMenuSelection = true;
        do
        {
            Console.WriteLine("Enter the name of the habit");

            string habitName = Console.ReadLine();
            if (string.IsNullOrEmpty(habitName))
            {
                Console.WriteLine("Habit name cannot be empty.");
                validMenuSelection = false;
                continue;
            }

            validMenuSelection = true;
            habitEntry.Habit = habitName!;

        } while (!validMenuSelection);

        do
        {
            try
            {
                Console.WriteLine("Enter the date the habit occurred in MM\\DD\\YYYY format.");
                string habitDate = Console.ReadLine();
                if (string.IsNullOrEmpty(habitDate))
                {
                    Console.WriteLine("Date cannot be empty.");
                    validMenuSelection = false;
                    continue;
                }
                DateTime dateTime = DateHelper.ConvertStringToDateTime(habitDate);
                habitEntry.Date = dateTime;
            }
            catch
            {
                Console.WriteLine("Invalid entry. Enter the date the habit occurred in MM\\DD\\YYYY format.");
                validMenuSelection = false;
                continue;
            }

            validMenuSelection = true;

        } while (!validMenuSelection);


        do
        {
            try
            {
                Console.WriteLine("How many times did this habit occur?");
                string habitOccurences = Console.ReadLine();
                if (string.IsNullOrEmpty(habitOccurences))
                {
                    Console.WriteLine("Occurences cannot be empty.");
                    validMenuSelection = false;
                    continue;
                }

                bool attemptParse = int.TryParse(habitOccurences, out int parsedOccurrences);
                if (!attemptParse)
                {
                    Console.WriteLine("Failed to parse occurrences to integer format.");
                    validMenuSelection = false;
                    continue;
                }
            }
            catch
            {
                Console.WriteLine("Invalid entry. Enter the number of times the habit occurred.");
                validMenuSelection = false;
                continue;
            }

            validMenuSelection = true;

        } while (!validMenuSelection);

        dBService.AddEntry(habitEntry);
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

