namespace HabitTracker.barakisbrown;

using System;
using System.Globalization;

public class Helpers
{
    private readonly string MenuInputString = "Please Select (1-4) OR 0 to exit";
    private readonly string AmountInputString = "Enter your blood sugar reading from the blood sugar meter: ";
    private readonly string DateInputString = "Enter the date [dd-mm-yyyy] OR Enter for todays: ";
    private readonly string IdInputString = "Please Enter ID from the list OR -1 to exit : ";

    public void GetMenu()
    {
        string menu = @"
        
        Main Menu

        What would you like to do?
        
        Type 0 to Close Application.
        Type 1 to View All Records.
        Type 2 to Insert Record
        Type 3 to Delete Record
        Type 4 to Update Record
        Type 5 to Show Report
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
        while (true)
        {
            if (result == string.Empty)
                return DateTime.UtcNow;
            try
            {
                DateTime.TryParseExact(result, "dd-MM-yyyy", new CultureInfo("en-us"), DateTimeStyles.None, out DateTime date);
                return date;
            }
            catch (FormatException _)
            {
                Console.WriteLine("Date Result needs to be entered via dd-MM-yyyy");
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

    public int GetNumberFromList(int[] validOptions)
    {       
        while (true)
        {
            Console.Write(IdInputString);
            var input = Console.ReadLine();
            int option;
            while (string.IsNullOrEmpty(input) || !Int32.TryParse(input, out option))
            {
                Console.WriteLine("Value entered needs to be an id on the list or -1 to exit.");
                Console.Write(IdInputString);
                input = Console.ReadLine();
            }
            if (option == -1 || validOptions.Contains(option))
                return option;
            else
                Console.WriteLine("Value not in the list or -1.");
        }
    }

    public static bool GetYESNO()
    {
       ConsoleKeyInfo input = Console.ReadKey(true);
        if (input.Key == ConsoleKey.Y)
            return true;
        else
            return false;        
    }
}
