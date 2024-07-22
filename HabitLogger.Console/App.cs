using DB;
using Models;

namespace HabitLogger;

public class App(DbContext dbContext)
{
    private readonly DbContext db = dbContext;

    public void Run()
    {
        db.SeedDatabase();

        bool shouldExit = false;
        while (true)
        {
            var userChoice = UI.DisplayMenu("Habit Logger Main Menu", ["Exit", "Log a habit", "View habits", "Update habits", "Delete habits"]);

            switch (userChoice)
            {
                case 0:
                    shouldExit = true;
                    break;
                case 1:
                    LogHabit();
                    break;
                case 2:
                    ViewHabits();
                    break;
                case 3:
                    UpdateHabits();
                    break;
                case 4:
                    DeleteHabits();
                    break;
            }

            if (shouldExit)
            {
                break;
            }
        }
    }

    private void LogHabit()
    {
        var habitName = UI.GetUserResponse("Enter the name of the habit");
        var quantity = UI.GetUserIntResponse("Enter the quantity for the habit");
        var date = "";
        while (!DateTime.TryParseExact(date, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
        {
            date = UI.GetUserResponse("Enter the date in the format 'dd-mm-yy'");
        }
        Habit habit = new(null, habitName, quantity, date);
        db.CreateHabit(habit);
    }

    private void ViewHabits()
    {
        bool shouldExit = false;
        while (true)
        {
            var userChoice = UI.DisplayMenu("View Habits", ["Return to main menu", "View all entries for a single habit", "View all entries", "View all habits for a date"]);

            switch (userChoice)
            {
                case 0:
                    shouldExit = true;
                    break;
                case 1:
                    var userInput = UI.GetUserResponse("Enter the name of the habit you wish to view");
                    var habitStrings = db.GetEntriesForHabitByName(userInput);
                    UI.DisplayResults(habitStrings);
                    break;
                case 2:
                    habitStrings = db.GetAllHabitEntries();
                    UI.DisplayResults(habitStrings);
                    break;
                case 3:
                    var dateString = "";
                    while (!DateTime.TryParseExact(dateString, "dd-MM-yy", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                    {
                        dateString = UI.GetUserResponse("Enter the date in the format 'dd-mm-yy' to view habit entries for");
                    }
                    habitStrings = db.GetHabitsByDate(dateString);
                    UI.DisplayResults(habitStrings);
                    break;
            }

            if (shouldExit)
            {
                break;
            }
        }
    }

    private void UpdateHabits()
    {
        var habits = db.GetAllHabitEntries();
        UI.DisplayResultsNoExit(habits);

        Habit? habit = null;
        int id = -1;
        while (habit == null)
        {
            id = UI.GetUserIntResponseNoClear("Enter the id of the habit you wish to update");
            habit = db.GetHabitEntryById(id);
        }

        UI.DisplayHabitNoExit(habit);
        Console.WriteLine();
        var nameString = UI.GetUserResponseNoClear("Enter the updated name for the habit entry");

        UI.DisplayHabitNoExit(habit);
        Console.WriteLine();
        var dateString = UI.GetUserResponseNoClear("Enter the updated date for the habit entry");

        UI.DisplayHabitNoExit(habit);
        Console.WriteLine();
        var quantity = UI.GetUserIntResponseNoClear("Enter the updated quantity for the habit entry");

        var updatedHabit = new Habit(id, nameString, quantity, dateString);

        db.UpdateHabit(updatedHabit);
    }

    private void DeleteHabits()
    {
        var habitStrings = db.GetAllHabitEntries();
        UI.DisplayResultsNoExit(habitStrings);

        Habit? habit = null;
        int id = -1;
        while (habit == null)
        {
            id = UI.GetUserIntResponseNoClear("Enter the id of the habit you wish to delete");
            habit = db.GetHabitEntryById(id);
        }

        db.DeleteHabit(id);
    }

}
