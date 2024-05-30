using System.Globalization;

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

    public Habit AddHabit()
    {
        Habit habit = new()
        {
            Name = GetValidString(1, 100, "Please enter a name for your habit (1-100 characters):"),
            Measurement = GetValidString(1, 75, "Please specify how you wish to measure your habit (e.g., km, glasses of water) (1-75 characters):"),
            Quantity = GetValidInt(1, 1000, "Please enter the quantity for your habit (1-1000):"),
            Frequency = GetValidString(1, 25, "Please specify the frequency of tracking this habit (e.g., daily, weekly) (1-25 characters):"),
            Notes = GetValidString(0, int.MaxValue, "Add any additional comments/notes (optional):"),
            Status = GetValidString(1, 15, "Please enter the current status of this habit (e.g., Complete, Ongoing) (1-15 characters):")
        };

        return habit;
    }

    private string GetValidString(int min, int max, string message)
    {
        string input;
        bool invalidLength;
        do
        {
            Console.WriteLine($"{message}");
            input = Console.ReadLine() ?? string.Empty;
            invalidLength = input!.Length < min || input.Length > max;
            if (invalidLength)
            {
                Console.WriteLine($"Input needs to be at least {min} character(s) and no more than {max} characters.");
            }
        } while (invalidLength);
        return input;
    }

    private int GetValidInt(int min, int max, string message)
    {
        string input;
        bool invalidNumber;
        int res;
        do
        {
            Console.WriteLine($"{message}");
            input = Console.ReadLine();
            int.TryParse(input, out res);
            invalidNumber = !int.TryParse(input, out res) || res < min || res > max;
            if (invalidNumber)
            {
                Console.WriteLine($"Input needs to be a number between {min} and {max}. Please enter a valid number (e.g., 5).");
            }
        } while (invalidNumber);
        return res;
    }

    public void DisplayAllHabits(List<Habit> habitList)
    {
        Console.WriteLine("Please resize your console window to view the table properly.");
        Console.ReadKey();
        Console.Clear();

        Console.WriteLine($"{"ID",-5} {"Name",-20} {"Measurement",-15} {"Quantity",-10} {"Frequency",-10} {"Date Created",-20} {"Date Updated",-20} {"Notes",-30} {"Status",-10}");
        Console.WriteLine(new string('-', 150));

        foreach (var habit in habitList)
        {
            Console.WriteLine(habit);
        }
    }

    public int SelectId()
    {
        return GetValidInt(1, int.MaxValue, "Choose a valid ID for selection.");
    }
}