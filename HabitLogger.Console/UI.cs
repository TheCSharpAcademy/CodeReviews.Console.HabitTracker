using Models;

namespace HabitLogger;

public static class UI
{
    private static readonly string[] resultHeaders = ["id", "name", "quantity", "date"];
    public static int DisplayMenu(string title, string[] options)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {title} ===");

            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"\t'{i}' - {options[i]}");
            }

            string? userInput = CaptureMenuSelection();

            bool isValidInt = int.TryParse(userInput, out int selection);
            bool isWithinRange = selection >= 0 && selection < options.Length;

            if (!isValidInt || !isWithinRange)
            {
                DisplayConfirmation("invalid selection");
                continue;
            }

            return selection;
        }
    }

    public static void DisplayConfirmation(string message)
    {
        Console.Clear();
        Console.WriteLine(message + "... Press 'enter' to continue");
        Console.ReadLine();
    }

    public static void DisplayConfirmationNoClear(string message)
    {
        Console.WriteLine(message + "... Press 'enter' to continue");
        Console.ReadLine();
    }

    public static string? CaptureMenuSelection()
    {
        Console.Write("Choose an option from above: ");
        return Console.ReadLine();
    }

    public static string GetUserResponse(string message)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(message + ": ");
            var response = Console.ReadLine();
            if (response == null || response == "")
            {
                DisplayConfirmation("invalid response");
                continue;
            }
            return response;
        }
    }

    public static string GetUserResponseNoClear(string message)
    {
        while (true)
        {
            Console.WriteLine(message + ": ");
            var response = Console.ReadLine();
            if (response == null || response == "")
            {
                DisplayConfirmationNoClear("invalid response");
                continue;
            }
            return response;
        }
    }

    public static int GetUserIntResponse(string message)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(message + ": ");
            var response = Console.ReadLine();
            if (int.TryParse(response, out int selection) && selection > 0)
            {
                return selection;
            }
            else
            {
                DisplayConfirmation("must enter an integer greater than 0");
            }
        }

    }

    public static int GetUserIntResponseNoClear(string message)
    {
        while (true)
        {
            Console.WriteLine("\n" + message + ": ");

            var response = Console.ReadLine();
            if (int.TryParse(response, out int selection) && selection > 0)
            {
                return selection;
            }
            else
            {
                DisplayConfirmation("must enter an integer greater than 0");
            }
        }
    }

    public static void DisplayResults(List<Habit> habits)
    {
        List<string[]> results = [
            resultHeaders
        ];

        foreach (var habit in habits)
        {
            results.Add(habit.ToStringArray());
        }

        Console.Clear();

        if (results.Count == 1)
        {
            Console.WriteLine("No results to view. Press 'enter' to continue");
            Console.ReadLine();
            return;
        }

        int[] columnWidths = new int[results[0].Length];
        for (int i = 0; i < results[0].Length; i++)
        {
            columnWidths[i] = results.Max(row => row[i].Length);
        }

        for (int i = 0; i < results[0].Length; i++)
        {
            Console.Write(results[0][i].PadRight(columnWidths[i]) + " ");
        }
        Console.WriteLine();

        for (int i = 0; i < results[0].Length; i++)
        {
            Console.Write(new string('-', columnWidths[i]) + " ");
        }
        Console.WriteLine();

        for (int row = 1; row < results.Count; row++)
        {
            for (int col = 0; col < results[row].Length; col++)
            {
                Console.Write(results[row][col].PadRight(columnWidths[col]) + " ");
            }
            Console.WriteLine();
        }

        Console.Write("\nPress 'enter' to escape view");
        Console.ReadLine();
    }

    public static void DisplayResultsNoExit(List<Habit> habits)
    {
        List<string[]> results = [
            resultHeaders
        ];

        foreach (var habit in habits)
        {
            results.Add(habit.ToStringArray());
        }

        Console.Clear();

        if (results.Count == 1)
        {
            Console.WriteLine("No results to view. Press 'enter' to continue");
            Console.ReadLine();
            return;
        }

        int[] columnWidths = new int[results[0].Length];
        for (int i = 0; i < results[0].Length; i++)
        {
            columnWidths[i] = results.Max(row => row[i].Length);
        }

        for (int i = 0; i < results[0].Length; i++)
        {
            Console.Write(results[0][i].PadRight(columnWidths[i]) + " ");
        }
        Console.WriteLine();
        for (int i = 0; i < results[0].Length; i++)
        {
            Console.Write(new string('-', columnWidths[i]) + " ");
        }
        Console.WriteLine();

        for (int row = 1; row < results.Count; row++)
        {
            for (int col = 0; col < results[row].Length; col++)
            {
                Console.Write(results[row][col].PadRight(columnWidths[col]) + " ");
            }
            Console.WriteLine();
        }
    }

    public static void DisplayHabitNoExit(Habit habit)
    {
        Console.Clear();

        Console.WriteLine("name:".PadRight(10) + habit.Name);
        Console.WriteLine("quantity:".PadRight(10) + habit.Quantity.ToString());
        Console.WriteLine("date:".PadRight(10) + habit.Date);
    }
}
