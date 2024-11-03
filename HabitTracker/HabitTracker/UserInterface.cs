namespace HabitTrackerMain;
using DatabaseController;
using Spectre.Console;
using static HabitTrackerMain.Enums;

public class UserInterface
{
    public void ShowMainMenu(string databasePath)
    {

        bool exitApp = default;
        do
        {

            Console.Clear();
            // Used Spectre console to display a menu for the user to select with keyboard arrow keys from an enum structured list of menu options.
            var mainMenuUserChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<Enums.MenuAction>()
                        .Title("[red]Choose an operator from the following list:[/]")
                        .AddChoices(Enum.GetValues<Enums.MenuAction>()));

            switch (mainMenuUserChoice)
            {
                case MenuAction.InsertRecord:
                    OperationController.InsertRecord(databasePath);
                    break;
                case MenuAction.ViewAllRecords:
                    OperationController.ViewAllRecords(databasePath);
                    break;
                case MenuAction.UpdateRecord:
                    OperationController.UpdateRecord(databasePath); 
                    Console.ReadKey();
                    break;
                case MenuAction.DeleteRecord:
                    OperationController.DeleteRecord(databasePath);
                    break;
                case MenuAction.ViewReports:
                    OperationController.ViewReports(databasePath);
                    break;
                case MenuAction.CloseApplication:
                    AnsiConsole.MarkupLine("[green]Thanks for using the application\nGoodBye![/]");
                    exitApp = true;
                    break;
            }
        }
        while (!exitApp);

    }

}

