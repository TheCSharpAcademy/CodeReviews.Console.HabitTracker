using System.Globalization;

namespace HabitTracker.iGoodw1n;

internal static class IOHelpers
{
    public static void ShowMenu()
    {
        Console.WriteLine("\n\nMain Menu");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("Type 5 to View Annual Report.");
        Console.WriteLine("-------------------------\n");
    }

    public static T GetParsedUserInput<T>(string textToShow) where T : IParsable<T>
    {
        Console.Write(textToShow);

        var input = Console.ReadLine();
        T? result;

        while (!T.TryParse(input, CultureInfo.InvariantCulture, out result) && result is not null)
        {
            Console.WriteLine("Something goes wrong!");
            Console.Write(textToShow);
            input = Console.ReadLine();
        }

        return result!;
    }

    public static string GetUserInput(string textToShow)
    {
        Console.Write(textToShow);

        return Console.ReadLine()!;
    }
}
