using Spectre.Console;
using static habbitTracker.fatihskalemci.Enums;
using System.Globalization;

namespace habbitTracker.fatihskalemci;

internal class UserInterface
{
    private readonly DataBaseConnection dataBase = new();

    internal void MainMenu()
    {
        bool exit = false;
        dataBase.CreateTable();
        while (!exit)
        {
            Console.Clear();

            var menuSelection = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>()
                .Title("Please select the action you want to perform")
                .AddChoices(Enum.GetValues<MenuOptions>()));

            switch (menuSelection)
            {
                case MenuOptions.Add:
                    dataBase.AddEntry();
                    break;
                case MenuOptions.Update:
                    dataBase.UpdateEntry();
                    break;
                case MenuOptions.Delete:
                    dataBase.DeleteEntry();
                    break;
                case MenuOptions.ShowEntries:
                    dataBase.ShowEtries();
                    break;
                case MenuOptions.Exit:
                    exit = true;
                    break;
            }
        }
    }

    static internal int getIntegerInput(string message)
    {
        Console.WriteLine(message);
        string? userInput = Console.ReadLine();
        int integerInput;

        while (!Int32.TryParse(userInput, out integerInput))
        {
            Console.WriteLine("Please enter a valid integer");
            userInput = Console.ReadLine();
        }
        return integerInput;
    }

    static internal string getDateInput()
    {
        Console.WriteLine("Please enter the date of the entry as dd-mm-yy(0 to return main menu, Press enter for today");
        string? userInput = Console.ReadLine();
        if (userInput == "")
        {
            userInput = DateTime.Now.ToString("dd-MM-yy");
        }
        else if (userInput == "0")
        {
            return userInput;
        }
        else
        {
            while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("tr-TR"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Please check your date format (dd-mm-yy)");
                userInput = Console.ReadLine();
            }
        }

        return userInput;
    }
}
