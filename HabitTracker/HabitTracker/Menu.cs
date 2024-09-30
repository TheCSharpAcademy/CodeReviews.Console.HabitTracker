namespace HabitTracker;

public enum Options
{
    ViewHabits,
    LogHabit,
    DeleteHabit,
    UpdateHabit,
    Help,
    Exit,
}
//handles  the CLI interface
public static class Menu
{
    public static void ShowOptions()
    {
        Console.Clear();
        Console.WriteLine("--MainMenu--");
        Console.WriteLine("{1} View Habits");
        Console.WriteLine("{2} Log Habit");
        Console.WriteLine("{3} Delete Habit");
        Console.WriteLine("{4} Update Habit");
       
        Console.WriteLine("{5} Help");
        Console.WriteLine("{6} Exit");
        
        
    }

    public static Options GetOptions()
    {
        int option;
        do
        {
            
            Console.WriteLine("Choose an option :");
            string? readLine = Console.ReadLine();
            int.TryParse(readLine, out option);
           
        } while (option-1 < (int)(Options.ViewHabits) || option-1 > (int)Options.Exit);
        return (Options)(option-1);
    }

    public static void ShowHelp()
    {
        Console.Clear();
        Console.WriteLine("--Help--");
        Console.WriteLine("{1} View Habits -> Display Habits");
        Console.WriteLine("{2} Log Habit -> Save a Habit ");
        Console.WriteLine("{3} Delete Habit -> Delete Habit Log");
        Console.WriteLine("{4} Update Habit -> Manipulate Habits (Name,Date,Quantity)");
        Console.WriteLine("{5} Exit -> Quits application Safely");
        Console.WriteLine("press enter to proceed...");
        Console.ReadLine();
    }
}