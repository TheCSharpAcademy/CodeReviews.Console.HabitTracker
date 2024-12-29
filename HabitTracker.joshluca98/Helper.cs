namespace HabitTracker.joshluca98;

public static class Helper
{
    public static string GetDateInput()
    {
        Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");
        string dateInput = Console.ReadLine();
        return dateInput;
    }

    public static int GetNumberInput(string message)
    {
        Console.WriteLine(message);
        string numberInput = Console.ReadLine();
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }
}