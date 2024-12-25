using Spectre.Console;

internal static class MainMenu
{
    private static Dictionary<string, Action> _menuActions = new Dictionary<string, Action>
    {
        { "View all records", HelpersSql.ShowListOfAllRecords },
        { "Create record", CrudRecords.CreateNewRecord },
        { "Delete record", CrudRecords.DeleteRecord },
        { "Update record", CrudRecords.UpdateRecord },
        { "Show report", HelpersReports.ShowReport },
        { "[green]Make random records[/]", DatabaseMock.CreateRandomRecords },
        { "[red]Delete all records[/]", DatabaseMock.DeleteAllRecords },
        { "Exit", () => 
            {
                Console.Clear();
                AnsiConsole.MarkupLine("[yellow]Goodbye![/]");
                Environment.Exit(0);
            }
        }
    };

    internal static void ShowMainMenu()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose an action: ")
                .PageSize(10)
                .AddChoices(_menuActions.Keys));
            _menuActions[choice] ();
        }
    }
}
