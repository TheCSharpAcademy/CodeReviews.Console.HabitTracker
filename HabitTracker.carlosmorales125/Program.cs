using HabitTracker.carlosmorales125;

var dbContext = new DbContext();
dbContext.CreateDatabase();

var habitService = new HabitService(dbContext);

MenuService.PresentWelcomeMessage();

while (true) 
{
    var choice = MenuService.PresentMenu();
    
    switch (choice)
    {
        case MenuChoice.ShowAllHabits:
            habitService.ShowAllHabits();
            break;
        case MenuChoice.AddHabit:
            var addHabit = MenuService.PresentAddHabitMenu();
            habitService.AddHabit(addHabit);
            break;
        case MenuChoice.DeleteHabit:
            var id = MenuService.PresentGetIdMenu();
            habitService.DeleteHabit(id);
            break;
        case MenuChoice.UpdateHabit:
            var updateHabit = MenuService.PresentUpdateHabitMenu();
            habitService.UpdateHabit(updateHabit);
            break;
        case MenuChoice.Exit:
            MenuService.PresentGoodbyeMessage();
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}