using Spectre.Console;
using static HabitTracker.nwdorian.Models.Enums;

namespace HabitTracker.nwdorian;

internal class Menu
{
    internal static void MainMenu()
    {
        bool repeatMenu = true;
        while (repeatMenu == true)
        {

            repeatMenu = false;
            Console.Clear();

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<MenuSelection>()
                    .Title("Welcome to [green]Habit tracker[/]\nWhat would you like to do?")
                    .PageSize(10)
                    .MoreChoicesText("")
                    .AddChoices(MenuSelection.CreateHabit,
                                MenuSelection.ViewAllRecords,
                                MenuSelection.InsertRecord,
                                MenuSelection.DeleteRecord,
                                MenuSelection.UpdateRecord,
                                MenuSelection.WeeklyReport,
                                MenuSelection.CloseApplication)
                                );

            switch (selection)
            {
                case MenuSelection.CreateHabit:
                    DbMethods.CreateHabit(AnsiConsole.Ask<string>("Enter a new habit name:"),
                                          AnsiConsole.Ask<string>("Enter a unit of measurement:")
                                          );
                    MainMenu();
                    break;
                case MenuSelection.ViewAllRecords:
                    DbMethods.GetAllRecords(Helpers.GetHabitByName("Select a habit to preview records."));
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case MenuSelection.InsertRecord:
                    DbMethods.Insert(Helpers.GetHabitByName("Select a habit to insert a new record."));
                    MainMenu();
                    break;
                case MenuSelection.DeleteRecord:
                    DbMethods.Delete(Helpers.GetHabitByName("Select a habit to delete records from."));
                    MainMenu();
                    break;
                case MenuSelection.UpdateRecord:
                    DbMethods.Update(Helpers.GetHabitByName("Select a habit to update records."));
                    MainMenu();
                    break;
                case MenuSelection.WeeklyReport:
                    DbMethods.GetWeeklyRecords(Helpers.GetHabitByName("Select a habit to preview past week of records."));
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    MainMenu();
                    break;
                case MenuSelection.CloseApplication:
                    if (AnsiConsole.Confirm("Are you sure you want to exit?"))
                    {
                        Console.WriteLine("\nGoodbye!");
                    }
                    else
                    {
                        repeatMenu = true;
                    }
                    break;
            }
        }
    }
}
