using mrgee1978.HabitLogger.Controllers;
using Spectre.Console;

namespace mrgee1978.HabitLogger.Views;

public class HabitLogger
{
    private AddData _addData = new AddData();
    private DeleteData _deleteData = new DeleteData();
    private UpdateData _updateData = new UpdateData();
    private ViewData _viewData = new ViewData();

    public HabitLogger()
    {
        DatabaseController.CreateDatabase();
    }
    /// <summary>
    /// Provides the menu to run the program
    /// </summary>
    public void Run()
    {
        var isProgramRunning = true;

        while (isProgramRunning)
        {
            Console.Clear();
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Welcome to the Habit Logger\nPlease choose on of the following options:")
                .AddChoices(
                    "Add Habit",
                    "Delete Habit",
                    "Update Habit",
                    "View Habits",
                    "Add Record",
                    "Delete Record",
                    "Update Record",
                    "View Records",
                    "Quit"));

            switch (userChoice)
            {
                case "Add Habit":
                    _addData.AddHabit();
                    break;
                case "Delete Habit":
                    _deleteData.DeleteHabit();
                    break;
                case "Update Habit":
                    _updateData.UpdateHabit();
                    break;
                case "View Habits":
                    _viewData.ViewHabits();
                    break;
                case "Add Record":
                    _addData.AddRecord();
                    break;
                case "Delete Record":
                    _deleteData.DeleteRecord();
                    break;
                case "Update Record":
                    _updateData.UpdateRecord();
                    break;
                case "View Records":
                    _viewData.ViewRecords();
                    break;
                case "Quit":
                    AnsiConsole.MarkupLine("[blue]\nThank you for using the Habit Logger[/]");
                    isProgramRunning = false;
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Invalid option!\n[/]");
                    break;
            }
            Console.WriteLine("Press Any Key to Continue: ");
            Console.ReadKey();
        }
    }
}
