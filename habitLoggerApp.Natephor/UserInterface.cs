namespace HabitLoggerApp;

public class UserInterface
{
    public string GetUserInput(string message)
    {
        string? userInput = null;
        while (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine(message);
            userInput = Console.ReadLine();
        }

        return userInput;
    }

    public int GetUserNumberInput(string message)
    {
        string? quantityInput;
        int quantity;
        do
        {
            Console.WriteLine(message);
            quantityInput = Console.ReadLine();
        } while (!int.TryParse(quantityInput, out quantity));

        return quantity;
    }

    public string GetUserDateInput(string message)
    {
        string? dateInput;
        DateTime parsedDate;
        do
        {
            Console.WriteLine(message);
            dateInput = Console.ReadLine();
        } while (!DateTime.TryParse(dateInput, out parsedDate));

        return parsedDate.ToString("d");
    }

    public void DisplayMenu(bool clearConsole = false)
    {
        if (clearConsole)
            Console.Clear();

        Console.WriteLine("Main Menu");
        Console.WriteLine("==========================\n");
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1 - View All Habits");
        Console.WriteLine("2 - Create a new habit");
        Console.WriteLine("3 - Edit an existing habit");
        Console.WriteLine("4 - Delete a habit");
        Console.WriteLine("5 - Exit");
    }

    public void DisplayHabits(List<Habit> habits)
    {
        Console.WriteLine("ID - BODY - QUANTITY - DATE");
        Console.WriteLine("===============================");
        if (habits.Count == 0)
            Console.WriteLine("No habits found.");
        else
            foreach (var habit in habits)
                Console.WriteLine(habit);

        Console.WriteLine("\nPress any key to continue:");
        Console.ReadKey();
    }
}