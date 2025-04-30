using Spectre.Console;
using Golvi1124.HabitLogger.src.Helpers;
using Golvi1124.HabitLogger.src.Operations;


namespace Golvi1124.HabitLogger.src.Menus;
public class MenuOptions
{
    private readonly HabitOperations _habitOperations;
    private readonly RecordOperations _recordOperations;
    private readonly SearchOperations _searchOperations;
    public MenuOptions()
    {
        _habitOperations = new HabitOperations(); // Instantiate HabitOperations
        _recordOperations = new RecordOperations(); // Instantiate RecordOperations
        _searchOperations = new SearchOperations(); // Instantiate SearchOperations
    }

    HelperMethods helper = new();


    public void HabitMenu()
    {
        var isHabitMenuRunning = true;

        while (isHabitMenuRunning)
        {
            var habitChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[bold cyan]Choose a Habit operation:[/]")
                    .AddChoices(
                        "View Habits",
                        "Add Habit",
                        "Update Habit",
                        "Delete Habit",
                        "Back")
            );

            switch (habitChoice)
            {
                case "View Habits":
                    var habits = helper.GetHabits(); // Call the updated GetHabits method to get the list of habits
                    _habitOperations.ViewHabits(habits); // Pass the list of habits to the ViewHabits method
                    break;
                case "Add Habit":
                    _habitOperations.AddHabit();
                    break;
                case "Update Habit":
                    _habitOperations.UpdateHabit();
                    break;
                case "Delete Habit":
                    _habitOperations.DeleteHabit();
                    break;
                case "Back":
                    isHabitMenuRunning = false;
                    break;
            }
        }
    }


    public void RecordMenu()
    {
        var isRecordMenuRunning = true;

        while (isRecordMenuRunning)
        {
            var recordChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[bold cyan]Choose a Record operation:[/]")
                    .AddChoices(
                    "View Records",
                    "Add Record",
                    "Update Record",
                    "Delete Record",
                    "Back")
            );

            switch (recordChoice)
            {
                case "View Records":
                    var records = _recordOperations.GetRecords(); // Fetch records directly using GetRecords
                    _recordOperations.ViewRecords(records); // Pass records to ViewRecords
                    break;
                case "Add Record":
                    _recordOperations.AddRecord();
                    break;
                case "Update Record":
                    _recordOperations.UpdateRecord();
                    break;
                case "Delete Record":
                    _recordOperations.DeleteRecord();
                    break;
                case "Back":
                    isRecordMenuRunning = false;
                    break;
            }
        }
    }

    public void SearchMenu()
    {
        var isSearchMenuRunning = true;

        while (isSearchMenuRunning)
        {
            var searchChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[bold cyan]Choose a Search operation:[/]")
                    .AddChoices(
                        "Chart of All Habits",
                        "Top 3 Habits",
                        "Average Habit Quantity",
                        "See Entries for Specific Habit",
                        "See Entries for Specific Month",
                        "Back")
            );
            switch (searchChoice)
            {
                case "Chart of All Habits":
                    _searchOperations.ShowChart();
                    break;
                case "Top 3 Habits":
                    _searchOperations.ShowTopHabits();
                    break;
                case "Average Habit Quantity":
                    _searchOperations.ShowAverage();
                    break;
                case "See Entries for Specific Habit":
                    _searchOperations.ShowSpecificHabit();
                    break;
                case "See Entries for Specific Month":
                    _searchOperations.ShowSpecificMonth();
                    break;
                case "Back":
                    isSearchMenuRunning = false;
                    break;

            }
        }
    }
}
