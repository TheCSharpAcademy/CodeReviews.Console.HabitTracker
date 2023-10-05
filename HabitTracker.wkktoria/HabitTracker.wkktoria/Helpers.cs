using System.Globalization;

namespace HabitTracker.wkktoria;

public static class Helpers
{
    public static void ShowMenu()
    {
        Console.WriteLine("\nMain Menu");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\t- Type v to view all records");
        Console.WriteLine("\t- Type s to view records with selected name");
        Console.WriteLine(
            "\t- Type r to print report (supports only 'running' with 'km' as unit, 'drinking water' with 'glasses' as unit, 'reading' with 'pages' as unit)");
        Console.WriteLine("\t- Type i to insert record");
        Console.WriteLine("\t- Type u to update record");
        Console.WriteLine("\t- Type d to delete record");
        Console.WriteLine("\t- Type m to show main menu");
        Console.WriteLine("\t- Type q to quit application");
    }

    public static void GetUserInput(Database db)
    {
        var quitApp = false;

        while (!quitApp)
        {
            Console.Write("> ");
            var commandInput = Console.ReadLine();

            switch (commandInput?.Trim().ToLower())
            {
                case "v":
                    db.PrintAllRecords();
                    break;
                case "s":
                    db.PrintSelectedRecords();
                    break;
                case "r":
                    db.Report();
                    break;
                case "i":
                    db.Insert();
                    break;
                case "u":
                    db.Update();
                    break;
                case "d":
                    db.Delete();
                    break;
                case "m":
                    ShowMenu();
                    break;
                case "q":
                    quitApp = true;
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    public static string GetDateInput(CultureInfo cultureInfo)
    {
        Console.Write("Enter the date (format: dd-mm-yy): ");

        var dateInput = Console.ReadLine();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", cultureInfo, DateTimeStyles.None,
                   out _))
        {
            Console.WriteLine("Invalid date.");
            Console.Write("Enter the date (format: dd-mm-yy): ");

            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    public static int GetNumberInput(string message)
    {
        Console.Write(message);

        var numberInput = Console.ReadLine();

        while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("Invalid number.");
            Console.Write("Enter an integer value: ");

            numberInput = Console.ReadLine();
        }

        return Convert.ToInt32(numberInput);
    }

    public static string GetStringInput(string message)
    {
        Console.Write(message);

        var strInput = Console.ReadLine();

        while (string.IsNullOrEmpty(strInput) || string.IsNullOrWhiteSpace(strInput))
        {
            Console.WriteLine("It cannot be empty.");
            Console.Write("Enter the text: ");

            strInput = Console.ReadLine();
        }

        return strInput;
    }

    public static string PareString(string str)
    {
        return str.Trim().ToLower();
    }
}