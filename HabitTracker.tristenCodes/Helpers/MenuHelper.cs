using Controllers;
using Services;

namespace Helpers;
static class MenuHelper
{
    public static void DisplayMainMenu()
    {
        Console.WriteLine("Press one of the following\n1. Add a habit\n2. Update a habit\n3. Remove a habit\n4. Show habits\n");
    }

    public static void AddHabit(DBService dbService)
    {
        Habit newHabitEntry = new();
        bool validMenuSelection;

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
            newHabitEntry.Name = habitName!;

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
                newHabitEntry.Date = dateTime;
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

        dbService.AddEntry(newHabitEntry);
    }

    public static void UpdateEntry(DBService dbService)
    {
        Habit newHabitEntry = new();
        Habit previousHabitEntry = new();

        var reader = dbService.GetAllEntries(); // Corrected 'dBService' to 'dbService'

        List<int> validHabitIds = [];

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var habitName = reader.GetString(1);
                var occurences = reader.GetInt32(2);
                var date = reader.GetDateTime(3);
                validHabitIds.Add(id);
                Console.WriteLine($"Id: {id}, Habit Name: {habitName}, Date: {DateHelper.ConvertDateToString(date)}, Occurences: {occurences}");
            }
        }

        bool isHabitIdValid = false;
        do
        {
            Console.WriteLine("Which habit would you like to update? Enter the id of the habit you would like to update.");
            var habitIdSelection = Console.ReadLine();

            isHabitIdValid = int.TryParse(habitIdSelection, out int parsedHabitId);
            if (isHabitIdValid)
            {
                previousHabitEntry = dbService.GetEntryById(parsedHabitId);
            }
        } while (!isHabitIdValid);

        Console.WriteLine($"Previous habit name: {previousHabitEntry?.Name}");

        Console.WriteLine("Enter the new title for the habit, or press enter to leave it the same as it previously was");
        var newHabitName = Console.ReadLine();

        Console.WriteLine("Enter the new number of occurences, or press enter to leave it the same as it previously was");
        var newOccurrences = Console.ReadLine();

        Console.WriteLine("Enter the new date, or press enter to leave it the same as it previously was");
        var newDate = Console.ReadLine();


    }
}