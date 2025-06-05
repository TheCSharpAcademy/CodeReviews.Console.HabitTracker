using Main.Data;
using Main.UI;
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