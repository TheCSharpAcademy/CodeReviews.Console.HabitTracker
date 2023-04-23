namespace HabitTracker.barakisbrown;

using System;
using System.Globalization;

public class Helpers
{
    private readonly string MenuInputString = "Please Select (1-4) OR 0 to exit";
    private readonly string AmountInputString = "Enter your blood sugar reading from the blood sugar meter: ";
    private readonly string DateInputString = "Enter the date [dd-mm-yyyy]: ";

    public void GetMenu()
    {
        string menu = @"
        
        Main Menu

        What would you like to do?
        
        Type 0 to Close Application.
        Type 1 to View All Records.
        Type 2 to Insert Record
        Type 3 to Delete Record
        Type 4 to Update Record.
        ----------------------------------
        ";

        Console.WriteLine(menu);
    }

    public int GetAmount()
    {
        Console.Write(AmountInputString);
        string? result = Console.ReadLine();
        int amount;

        while (string.IsNullOrEmpty(result) || !Int32.TryParse(result, out amount))
        {
            Console.WriteLine("Your answer needs to be a positive interger.");
            Console.Write(AmountInputString);
            result = Console.ReadLine();
        }
        return amount;
    }

    public DateTime GetDate()
    {
        Console.Write(DateInputString);
        string? result = Console.ReadLine();
        DateTime date;

        while (true)
        {
            try
            {
               DateTime.TryParseExact(result,"dd-MM-yyyy",new CultureInfo("en-us"),DateTimeStyles.None,out date);
               return date;
            }
            catch (FormatException _)
            {
                Console.WriteLine("Date Result needs to be entered via dd-mm-yyyy");
                Console.WriteLine("Please try again.");
                Console.Write(DateInputString);
                result = Console.ReadLine();
            }
        }
    }

    public int GetMenuSelection()
    {
        Console.Write(MenuInputString);
        int option;

        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            try
            {
                option = int.Parse(input.KeyChar.ToString());
                return option;
            }
            catch (FormatException _)
            {
                Console.WriteLine();
                Console.WriteLine("Input needs to be between 0 and 4 only. Please try again.");
                Console.Write(MenuInputString);
            }
        }
    }
}
