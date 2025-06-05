using Main;
using Main.Data;
using Main.Models;
using Main.UI;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Runtime.CompilerServices;
using static Main.Enums;

bool running = true;

Database.Initialize();
while (running)
{
    var choice = MenuService.ShowMainMenu();
    switch (choice)
    {
        case MenuChoice.ManageCategories:
            new CategoryService().ShowMenu();
            break;
        case MenuChoice.ManageHabits:
            new HabitService().ShowMenu();
            break;
        case MenuChoice.Exit:
            running = false;
            break;
    }
}


//while (running)
//{
//    var choice = Menu.ShowMenu();

//    switch (choice)
//    {
//        case CrudChoice.ViewAll:
//            db.GetAll();
//            Console.ReadKey();
//            break;
//        case CrudChoice.Insert:
//            {
//            var choices = Menu.Insert("Insert number of glasses");
//            db.Insert(choices.Item1, choices.Item2);
//            break;
//            }
//        case CrudChoice.Update:
//            {
//            var list = db.GetAll();
//            if (list.Count == 0)
//            {
//                Console.WriteLine("No records to update");
//                Console.ReadKey();
//                break;
//            }
//            var habit = AnsiConsole.Prompt(
//                new SelectionPrompt<Habit>()
//                    .Title("Choose a record to update")
//                    .PageSize(10)
//                    .UseConverter(habit => $"[bold]{habit.Date.ToString("yyyy-mm-dd")}[/]: {habit.Quantity}")
//                    .AddChoices(list));

//            if (habit != null)
//            {
//                var choices = Menu.Insert("Insert number of glasses");
//                db.Update(habit.Id, choices.Item1, choices.Item2 );
//            }
//            break;
//            }
//        case CrudChoice.Delete:
//            {
//            var list = db.GetAll();
//            if (list.Count == 0)
//            {
//                Console.WriteLine("No records to remove");
//                Console.ReadKey();
//                break;
//            }
//            var habit = AnsiConsole.Prompt(
//                new SelectionPrompt<Habit>()
//                    .Title("Choose a record to delete")
//                    .PageSize(10)
//                    .UseConverter(habit => $"[bold]{habit.Date.ToString("yyyy-mm-dd")}[/]: {habit.Quantity}")
//                    .AddChoices(list));

//            if (habit != null)
//            {
//                db.Delete(habit.Id);
//            }
//            break;
//            }
//        case CrudChoice.Exit:
//            running = false;
//            break;
//    }
//}

