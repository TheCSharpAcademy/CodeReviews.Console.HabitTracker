using HabitDatabase;

class Program
{
    static DatabaseManager habitDatabase = new DatabaseManager();
    static void Main(string[] args)
    {
        bool endApp = false;

        Console.WriteLine("Console Habit Logger in C#\r");
        Console.WriteLine("------------------------\n");

        string? userOption = "";
        while (!endApp)
        {
            Console.WriteLine("Type 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.Write("\n");

            userOption = Console.ReadLine();
            HabitOccurrence? selectedHabitOccurrence;
            switch (userOption)
            {
                case "0":
                    endApp = true;
                    break;
                case "1":
                    ViewAllHabitOccurrences();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    selectedHabitOccurrence = SelectHabitOccurrenceFromList();
                    if (selectedHabitOccurrence != null)
                    {
                        if (habitDatabase.DeleteRecord(selectedHabitOccurrence))
                            Console.WriteLine("Instance deleted.");
                        else
                            Console.WriteLine("Unable to delete occurrence.");
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid option");
                    }
                    break;
                case "4":
                    selectedHabitOccurrence = SelectHabitOccurrenceFromList();
                    if (selectedHabitOccurrence != null)
                    {
                        UpdateHabitOccurrence(selectedHabitOccurrence);
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid option");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option.\n");
                    break;
            }

            Console.WriteLine("------------------------\n");
        }

        return;
    }

    private static void InsertRecord()
    {
        bool HabitIdSuccess = readHabitId(out int habitId);
        bool frequencySuccess = readFrequency(out int frequency);
        bool occurrenceDateSuccess = readDate(out DateOnly occurrenceDate);

        Console.Write("\n");

        if (HabitIdSuccess && frequencySuccess && occurrenceDateSuccess)
            if (habitDatabase.InsertOccurrence(occurrenceDate, frequency, habitId))
                Console.WriteLine("Instance inserted.");
            else
                Console.WriteLine("Unable to insert occurrence.");
    }

    private static bool readDate(out DateOnly occurrenceDate)
    {
        Console.WriteLine("Occurrence Date: ");
        string? occurrenceDateInput = Console.ReadLine();
        bool occurrenceDateSuccess = DateOnly.TryParse(occurrenceDateInput, out occurrenceDate);

        return occurrenceDateSuccess;
    }

    private static bool readFrequency(out int frequency)
    {
        Console.WriteLine("Frequency: ");
        string? frequencyInput = Console.ReadLine();
        bool frequencySuccess = int.TryParse(frequencyInput, out frequency);

        return frequencySuccess;
    }

    private static bool readHabitId(out int habitId)
    {
        Console.WriteLine("Available Habits:");
        foreach (Habit habit in habitDatabase.GetAvailableHabits())
        {
            Console.WriteLine(habit.ToString());
        }

        Console.WriteLine("Habit Id: ");
        string? HabitIdInput = Console.ReadLine();
        bool habitIdSuccess = int.TryParse(HabitIdInput, out habitId);

        return habitIdSuccess;
    }

    private static void ViewAllHabitOccurrences()
    {
        var existingOccurrences = habitDatabase.GetAllHabitOccurrences();

        if (existingOccurrences.Count == 0)
        {
            Console.WriteLine("No existing occurrences...");
            return;
        }

        existingOccurrences.Sort();
        Console.WriteLine("Existing Occurrences:");
        foreach (HabitOccurrence habitOccurrence in existingOccurrences)
            Console.WriteLine(habitOccurrence.ToString());
    }

    public static bool UpdateHabitOccurrence(HabitOccurrence originalOccurrence)
    {
        HabitOccurrence updated = originalOccurrence;

        bool HabitIdSuccess = readHabitId(out int habitId);
        if (HabitIdSuccess)
            updated = originalOccurrence with { HabitId = habitId };

        bool frequencySuccess = readFrequency(out int frequency);
        if (frequencySuccess)
            updated = updated with { NumberOfOccurrences = frequency };

        bool occurrenceDateSuccess = readDate(out DateOnly occurrenceDate);
        if (occurrenceDateSuccess)
            updated = updated with { OccurrenceDate = occurrenceDate };

        Console.Write("\n");

        if (habitDatabase.UpdateOccurrence(originalOccurrence, updated))
            Console.WriteLine("Instance updated.");
        else
            Console.WriteLine("Unable to update occurrence.");

        return false;
    }

    private static HabitOccurrence? SelectHabitOccurrenceFromList()
    {
        var existingOccurrences = habitDatabase.GetAllHabitOccurrences();

        if (existingOccurrences.Count == 0)
        {
            Console.WriteLine("No existing occurrences...");
            return null;
        }

        existingOccurrences.Sort();
        Console.WriteLine("Choose Occurrences:");
        for (int i = 0; i < existingOccurrences.Count; i++)
        {
            Console.WriteLine($"({i}) - {existingOccurrences[i].ToString()}");
        }

        string? selectedOccurrenceIndexInput = Console.ReadLine();
        bool parseSuccess = int.TryParse(selectedOccurrenceIndexInput, out int selectedOccurrenceIndex);

        if (parseSuccess && 0 < selectedOccurrenceIndex && selectedOccurrenceIndex < existingOccurrences.Count)
        {
            return existingOccurrences[selectedOccurrenceIndex];
        }
        else
        {
            return null;
        }
    }

}


