public class ConsoleInteraction
{
    public Options MainOptions()
    {
        int i = 0;
        while (true)
        {
            i++;
            if (i >= 5) Console.Clear();
            DisplayOptions();
            string input = Console.ReadLine();

            if (int.TryParse(input, out int result) && Enum.IsDefined(typeof(Options), result))
            {
                return (Options)result;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Invalid input, try again.");
            Console.ResetColor();

        }
    }

    private void DisplayOptions()
    {
        Console.WriteLine("Pick an option from below:");
        Console.WriteLine("1 - Add new Habit");
        Console.WriteLine("2 - Update previous Habit");
        Console.WriteLine("3 - Delete a habit");
        Console.WriteLine("4 - View all habits");
        Console.WriteLine("5 - Insert Test Habits");
        Console.WriteLine("0 - Exit the application");
    }
}