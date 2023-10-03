using ConsoleTables;
using HabitTracker.Models;

namespace HabitTracker.Services;

public class Helpers
{
    private static readonly string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public static string ValidateMainMenu(string input)
    {
        List<string> numbers = new() { "0", "1", "2", "3", "4" };

        while (input.Length > 1 || !numbers.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Your Input Should Be 0, 1, 2, 3, or 4");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Your Selection: ");
            input = Console.ReadLine();
            return ValidateMainMenu(input);
        }

        return input;
    }

    public static string ValidateEntry(string input, string sentence)
    {
        string strippedInput = input.Replace(" ", "");

        foreach (char letter in strippedInput)
        {
            while (!alphabet.Contains(letter))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Input Must Be Using The 26-Letter English Alphabet");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{sentence}: ");

                input = Console.ReadLine();
                return ValidateEntry(input, sentence);
            }
        }
        return input;
    }

    public static string ValidateYesNo(string input, string sentence)
    {
        List<string> options = new() { "y", "n" };

        while (!options.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Entry Must Be Y or N");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{sentence}: ");

            input = Console.ReadLine();
            return ValidateYesNo(input, sentence); 
        }

        return input;
    }

    public static int ValidateNumericInput(string input, string sentence)
    {
        int result;
        while (!int.TryParse(input, out result))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be An Integer.");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{sentence}: ");

            input = Console.ReadLine();
            return ValidateNumericInput(input, sentence);
        }

        return result;
    }

    public static string ValidateOneTwo(string input, string sentence)
    {
        List<string> options = new() { "1", "2" };

        while (!options.Contains(input))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Input Must Be 1 or 2");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{sentence}: ");

            input = Console.ReadLine();
            return ValidateOneTwo(input, sentence);
        }

        return input;
    }

    public static string VerifyEmptyOrChanged(string oldEntry, string newEntry)
    {
        if (String.IsNullOrEmpty(newEntry) || newEntry == oldEntry)
        {
            return oldEntry;
        }
        else
        {
            return newEntry;
        }
    }

    public static void PrintHabitChart(List<Habit> habits)
    {

        var table = new ConsoleTable("Id", "Name", "Date Created", "Count", "Description");

        for (int i = 0; i < habits.Count; i++)
        {
            int habId = habits[i].Id;
            string habName = habits[i].Name;
            DateTime habDate = habits[i].Date;
            int habCount = habits[i].Count;
            string habDesc = habits[i].Description;


            table.AddRow(habId, habName, habDate, habCount, habDesc);
        }

        table.Write();
    }

    public static int ValidateIndexSelection(int selectedId, List<Habit> habits, string sentence)
    {
        while (!habits.Any(x => x.Id == selectedId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] Selected Number Does Not Exist");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{sentence}: ");

            string input = Console.ReadLine();
            selectedId = ValidateNumericInput(input, "Your Selection");
            return ValidateIndexSelection(selectedId, habits, sentence);
        }

        return selectedId;
    }
}