using HabitTracker.Services;
using HabitTracker.Models;

namespace HabitTracker;

public class Habits
{
    public static void NewHabitEntry()
    {
        Console.Clear();
        Console.WriteLine("--------------------");
        Console.WriteLine("Creating A New Habit");
        Console.WriteLine("--------------------");
        Console.Write("New Habit Name: ");

        string newHabitName = Console.ReadLine();
        newHabitName = Helpers.ValidateEntry(newHabitName, "New Habit Name");

        int newCount = 0;
        
        Console.Write("\nDescription Of New Habit: ");

        string newHabitDescription = Console.ReadLine();
        newHabitDescription = Helpers.ValidateEntry(newHabitDescription, "Description Of New Habit");

        Habit newHabit = new()
        {
            Name = newHabitName,
            Date = DateTime.Now,
            Count = newCount,
            Description = newHabitDescription
        };
        
        Database.SaveEntry(newHabit);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Your New Habit Is Being Tracked!");
        Thread.Sleep(2000);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        MainMenu.ShowMenu();
    }

    public static void SelectEditToHabit()
    {
        Console.Clear();
        Console.WriteLine("-----------------------");
        Console.WriteLine("Editing A Current Habit");
        Console.WriteLine("-----------------------");
        Console.WriteLine("Select A Habit Below To Edit");

        List<Habit> currentHabits = Database.GetHabits();
        Helpers.PrintHabitChart(currentHabits);

        Console.Write("Your Selection: ");
        string input = Console.ReadLine();
        int selectedIndex = Helpers.ValidateNumericInput(input, "Your Selection");
        selectedIndex = Helpers.ValidateIndexSelection(selectedIndex, currentHabits, "Your Selection");

        Habit selectedHabit = Database.GetSelectedHabit(selectedIndex);

        Console.WriteLine("What Would You Like To Do");
        Console.WriteLine("1 - Edit Habit Entry");
        Console.WriteLine("2 - Update Habit Count");

        Console.Write("Your Selection: ");
        string input2 = Console.ReadLine();
        string selectedAction = Helpers.ValidateOneTwo(input2, "Your Selection");

        switch (selectedAction)
        {
            case "1":
                Console.Clear();
                EditHabit(selectedHabit, selectedIndex);
                break;

            case "2":
                IncreaseHabitCounter(selectedHabit, selectedIndex);
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Couldn't Determine Selected Action.");
                SelectEditToHabit();
                break;
        }
    }

    public static void EditHabit(Habit selectedHabit, int selectedIndex)
    {

        string oldName = selectedHabit.Name;
        int oldCount = selectedHabit.Count;
        string oldDescription = selectedHabit.Description;

        Console.WriteLine("While updating the habit, if you want to leave the item the same, then press enter with no text written");
        Console.WriteLine($"\nCurrent Habit Name: {oldName}");
        Console.Write("\nNew Habit Name: ");

        string newName = Console.ReadLine();
        newName = Helpers.VerifyEmptyOrChanged(oldName, newName);

        Console.WriteLine($"\nCurrent Count: {oldCount}");
        Console.Write("Would You Like To Reset The Counter? Y/N: ");

        string input = Console.ReadLine().ToLower();
        input = Helpers.ValidateYesNo(input, "Would You Like To Reset The Count? Y/N: ");

        int newCount = oldCount;

        if (input.ToLower() == "y")
        {
            newCount = 0;
        }
        else
        {
            newCount = oldCount;
        }

        Console.WriteLine($"\nCurrent Description: {oldDescription}");
        Console.Write("New Description: ");

        string newDescription = Console.ReadLine();
        newDescription = Helpers.VerifyEmptyOrChanged(oldDescription, newDescription);

        Habit updateHabit = new()
        {
            Name = newName,
            Date = DateTime.Now,
            Count = newCount,
            Description = newDescription
        };

        Database.UpdateEntry(selectedIndex, updateHabit);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Your Habit Has Been Updated!");
        Thread.Sleep(2000);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        MainMenu.ShowMenu();
    }

    public static void IncreaseHabitCounter(Habit selectedHabit, int selectedIndex)
    {
        Habit oldHabit = new()
        {
            Name = selectedHabit.Name,
            Date = selectedHabit.Date,
            Count = selectedHabit.Count,
            Description = selectedHabit.Description
        };

        int newCount = oldHabit.Count + 1;

        Habit newHabit = new()
        {
            Name = oldHabit.Name,
            Date = oldHabit.Date,
            Count = newCount,
            Description = oldHabit.Description
        };

        Database.UpdateEntry(selectedIndex, newHabit);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Your Habit Count Has Been Increased.");
        Thread.Sleep(2000);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        MainMenu.ShowMenu();
    }

    public static void DeleteHabit()
    {
        Console.Clear();
        Console.WriteLine("------------------------");
        Console.WriteLine("Deleting A Current Habit");
        Console.WriteLine("------------------------");
        Console.WriteLine("Select A Habit Below To Delete");

        List<Habit> currentHabits = Database.GetHabits();
        Helpers.PrintHabitChart(currentHabits);

        Console.WriteLine("Enter The ID Of The Habit You'd Like To Delete.");
        Console.Write("Your Selection: ");
        string input = Console.ReadLine();
        int selectedId = Helpers.ValidateNumericInput(input, "Your Selection");
        selectedId = Helpers.ValidateIndexSelection(selectedId, currentHabits, "Your Selection");

        var habit = currentHabits.First(x => x.Id == selectedId);

        Database.DeleteEntry(habit);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Your Habit Has Been Deleted");
        Thread.Sleep(2000);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        MainMenu.ShowMenu();
    }

    public static void ViewAllHabits()
    {
        Console.Clear();
        Console.WriteLine("------------------------");
        Console.WriteLine("Currently Tracked Habits");
        Console.WriteLine("------------------------");

        List<Habit> currentHabits = Database.GetHabits();
        Helpers.PrintHabitChart(currentHabits);

        MainMenu.ShowMenu();
    }
}