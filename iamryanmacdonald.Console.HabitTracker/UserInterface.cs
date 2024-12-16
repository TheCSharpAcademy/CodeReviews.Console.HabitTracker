using System.Text.RegularExpressions;
using Spectre.Console;
using static iamryanmacdonald.Console.HabitTracker.Enums;

namespace iamryanmacdonald.Console.HabitTracker;

internal class UserInterface
{
    private readonly Database _database;

    internal UserInterface(Database database)
    {
        _database = database;
    }

    internal void MainMenu()
    {
        var closeApp = false;
        while (!closeApp)
        {
            System.Console.Clear();

            var actionChoice =
                AnsiConsole.Prompt(new SelectionPrompt<MenuAction>()
                    .Title("What would you like to do?")
                    .UseConverter(a => Regex.Replace(a.ToString(), "(\\B[A-Z])", " $1"))
                    .AddChoices(Enum.GetValues<MenuAction>()));

            switch (actionChoice)
            {
                case MenuAction.CloseApplication:
                    AnsiConsole.MarkupLine("Goodbye!");
                    closeApp = true;
                    break;
                case MenuAction.DeleteRecord:
                    var deletableRecords = _database.GetAllRecords();

                    if (deletableRecords.Count == 0)
                    {
                        AnsiConsole.MarkupLine("No records found.");
                    }
                    else
                    {
                        var recordToDelete = AnsiConsole.Prompt(new SelectionPrompt<DrinkingWater>()
                            .Title("Select a record to delete:")
                            .UseConverter(r => $"{r.Date:MMMM d, yyyy} - {r.Quantity}")
                            .AddChoices(deletableRecords));
                        _database.DeleteRecord(recordToDelete);
                        AnsiConsole.MarkupLine(
                            $"Successfully deleted a record on {recordToDelete.Date:MMMM d, yyyy} with a value of {recordToDelete.Quantity}.");
                    }

                    break;
                case MenuAction.InsertRecord:
                    var date = AnsiConsole.Ask<DateOnly>("Please insert the date: (Format: yyyy-mm-dd)");
                    var quantity =
                        AnsiConsole.Ask<int>(
                            "Please insert number of glasses or other measure of your choice (no decimals allowed)");
                    _database.AddRecord(date, quantity);
                    AnsiConsole.MarkupLine(
                        $"Successfully inserted a record on {date:MMMM d, yyyy} with a value of {quantity}.");

                    break;
                case MenuAction.UpdateRecord:
                    var updateableRecords = _database.GetAllRecords();

                    if (updateableRecords.Count == 0)
                    {
                        AnsiConsole.MarkupLine("No records found.");
                    }
                    else
                    {
                        var recordToUpdate = AnsiConsole.Prompt(new SelectionPrompt<DrinkingWater>()
                            .Title("Select a record to update:")
                            .UseConverter(r => $"{r.Date:MMMM d, yyyy} - {r.Quantity}")
                            .AddChoices(updateableRecords));
                        var updatedDate = AnsiConsole.Ask(
                            "Please insert the date (Format: yyyy-mm-dd)",
                            recordToUpdate.Date);
                        var updatedQuantity =
                            AnsiConsole.Ask(
                                "Please insert number of glasses or other measure of your choice (no decimals allowed)",
                                recordToUpdate.Quantity);
                        _database.UpdateRecord(recordToUpdate, updatedDate, updatedQuantity);
                        AnsiConsole.MarkupLine(
                            $"Successfully updated a record on {updatedDate:MMMM d, yyyy} with a value of {updatedQuantity}.");
                    }

                    break;
                case MenuAction.ViewAllRecords:
                    var allRecords = _database.GetAllRecords();

                    if (allRecords.Count == 0)
                    {
                        AnsiConsole.MarkupLine("No records found.");
                    }
                    else
                    {
                        var table = new Table();
                        table.Border(TableBorder.Rounded);

                        table.AddColumn("ID");
                        table.AddColumn("Date");
                        table.AddColumn("Quantity");

                        foreach (var record in allRecords)
                            table.AddRow(
                                record.Id.ToString(),
                                record.Date.ToString("MMMM dd, yyyy"),
                                record.Quantity.ToString()
                            );

                        AnsiConsole.Write(table);
                    }

                    break;
            }

            AnsiConsole.MarkupLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }
}