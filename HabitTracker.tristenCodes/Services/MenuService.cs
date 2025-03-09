using HabitTracker.tristenCodes.Models;
using HabitTracker.tristenCodes.Helpers;
using HabitTracker.tristenCodes.Services;

namespace HabitTracker.tristenCodes.Services;
static class MenuService
{
    public static void DisplayMainMenu()
    {
        Console.WriteLine("Press one of the following\n1. Add a habit\n2. Update a habit\n3. Remove a habit\n4. Show habits\n5. Exit program");
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

    public static void UpdateHabit(DBService dbService)
    {
        Habit newHabitEntry = new();
        Habit previousHabitEntry = new();

        var reader = dbService.GetAllEntries();

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
            Console.WriteLine("\nWhich habit would you like to update? Enter the id of the habit you would like to update.");
            var habitIdSelection = Console.ReadLine();

            isHabitIdValid = int.TryParse(habitIdSelection, out int parsedHabitId);
            if (isHabitIdValid)
            {
                try
                {
                    previousHabitEntry = dbService.GetEntryById(parsedHabitId);
                }
                catch
                {
                    Console.WriteLine($"Habit with id {parsedHabitId} not found. Enter an existing habit id.");
                    isHabitIdValid = false;
                }
            }
        } while (!isHabitIdValid);


        Console.WriteLine($"Previous habit name: {previousHabitEntry?.Name}");

        Console.WriteLine("Enter the new title for the habit, or press enter to leave it the same as it previously was");
        var newHabitName = Console.ReadLine();

        if (string.IsNullOrEmpty(newHabitName)) newHabitEntry.Name = previousHabitEntry!.Name;
        else newHabitEntry.Name = newHabitName;

        bool validNewOccurrences;
        do
        {
            Console.WriteLine("Enter the new number of occurences, or press enter to leave it the same as it previously was");
            var newOccurrences = Console.ReadLine();
            if (string.IsNullOrEmpty(newOccurrences)) newHabitEntry.Occurences = previousHabitEntry!.Occurences;

            validNewOccurrences = int.TryParse(newOccurrences, out int parsedOccurrences);

            if (validNewOccurrences) newHabitEntry.Occurences = parsedOccurrences;
            else Console.WriteLine("Invalid entry. Try again.");
        } while (!validNewOccurrences);

        bool validNewDate = false;
        do
        {
            Console.WriteLine("Enter the new date, or press enter to leave it the same as it previously was");
            var newDate = Console.ReadLine();

            if (string.IsNullOrEmpty(newDate)) newHabitEntry.Date = previousHabitEntry!.Date;
            else
            {
                try
                {
                    DateTime dateTime = DateHelper.ConvertStringToDateTime(newDate);
                    newHabitEntry.Date = dateTime;
                    validNewDate = true;
                }
                catch
                {
                    validNewDate = false;
                    Console.WriteLine("Invalid date entered. MM/DD/YYYY format required.");
                }
            }
        } while (!validNewDate);


        dbService.UpdateEntry(previousHabitEntry!, newHabitEntry);

    }

    public static void RemoveHabit(DBService dbService)
    {
        bool validHabitId = false;
        do
        {
            Console.WriteLine("Enter the ID of the habit you would like to remove: ");
            string habitId = Console.ReadLine();
            if (!string.IsNullOrEmpty(habitId) && EntryHelper.IsValidId(habitId, dbService))
            {
                dbService.DeleteEntry(int.Parse(habitId));
                validHabitId = true;
            }

        } while (!validHabitId);
    }
}