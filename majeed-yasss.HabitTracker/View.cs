using System.Globalization;
using Spectre.Console;

namespace majeed_yasss.HabitTracker;
internal class View
{
    public static Option MainMenu()
    {
        Console.Clear();
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("------------------------------------------\n");
        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("What do you want to do next?")
            .AddChoices(Options.Labels.Keys));
        Console.WriteLine("------------------------------------------\n");

        return Options.Labels[choice];
    }
    public static void Exit()
    {
        Console.WriteLine("\nGoodbye!\n");
        Environment.Exit(0);
    }
    public static bool Records(List<DrinkingWater> tableData)
    {
        Console.Clear();

        if (tableData.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No Records Found![/]");
            Console.WriteLine("------------------------------------------\n");
            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();
            return false;
        }

        Console.WriteLine("------------------------------------------\n");
        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[cyan]Date[/]");
        table.AddColumn("[green]Quantity[/]");
        
        foreach (var dw in tableData)
            table.AddRow(
                $"[yellow]{dw.Id}[/]",
                $"[cyan]{dw.Date:dd-MMM-yyyy}[/]",
                $"[green]{dw.Quantity}[/]"
            );

        AnsiConsole.Write(table);
        Console.WriteLine("------------------------------------------\n");       
        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
        return true;
    }

    public static DrinkingWater SelectFromRecords(List<DrinkingWater> records)
    {
        Console.Clear();
        Console.WriteLine("\n\nSelect a record");
        Console.WriteLine("------------------------------------------\n");

        DrinkingWater choice = AnsiConsole.Prompt(
            new SelectionPrompt<DrinkingWater>()
            .Title("What do you want to do next?")
            .AddChoices(records));

        Console.WriteLine("------------------------------------------\n");

        return choice;
    }
    internal static string GetDateInput()
    {
        string dateInput;
        do dateInput = AnsiConsole.Ask<string>("Please insert the date: (Format: dd-mm-yy)");
        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _));
        return dateInput;
    }
    internal static int GetPositiveInt(string message)
    {
        int n;
        do n = AnsiConsole.Ask<int>(message);
        while (n <= 0);
        return n;
    }
}

