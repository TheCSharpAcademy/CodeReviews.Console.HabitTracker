using HabitTracker;
using Spectre.Console;

string dataSource = "DataSource=habittracker.db";
InitializeDb.CreateHabitTable(dataSource); 

var userName = AnsiConsole.Ask<string>("What is your name?");

AnsiConsole.MarkupLine($"[green] Hello {userName}![/]");
AnsiConsole.MarkupLine($"Adding some example data to the habit tracker.");
InitializeDb.AddSampleData(dataSource, userName);

var menu = new UserInterface(dataSource, userName);
menu.MainMenu();

enum MenuOption
{
    InsertHabit,
    SeeHabits,
    UpdateHabit,
    RemoveHabit,
    ExitApplication,
}

