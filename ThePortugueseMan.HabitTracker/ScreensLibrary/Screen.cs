using HabitsLibrary;
namespace ScreensLibrary;

public class Screen
{
    HabitsTable habitsTable;
    HabitsSubTable habitsSubTable;
    AskInput askInput = new();

    public Screen(HabitsTable habitsTable,HabitsSubTable habitsSubTable)
    {
        this.habitsTable = habitsTable;
        this.habitsSubTable = habitsSubTable;
    }
    public void ViewAll(string menuString)
    {
        Console.Clear();
        if (menuString == "Habits") ViewAllHabits();
        else if (menuString == "SubHabits") ViewAllSubHabits();
        else return;

        Console.ReadLine();
    }

    private void ViewAllHabits()
    {
        habitsTable.ViewAll();
        return;
    }

    private void ViewAllSubHabits()
    {
        return;
    }

    public void Insert(string menuString)
    {
        if (menuString == "Habits") InsertHabit();
        else if (menuString == "SubHabits") InsertSubHabit();
    }

    private void InsertHabit() 
    {
        string habitName = askInput.LettersNumberAndSpaces("Write the name of your habit.");
        string habitUnit = askInput.LettersNumberAndSpaces("Write the units of your habit.");
        habitsTable.InsertNewHabit(habitName, habitUnit);
        return;
    }

    private void InsertSubHabit()
    {
        return;
    }

    public void Delete(string menuString) 
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("DELETE");

            if (!DeleteHabit()) Console.WriteLine("Couldn't delete entry");
            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);
        return;

    }

    private bool DeleteHabit()
    {
        habitsTable.ViewAll();
        int index = askInput.Digits("Write the number of the you want to delete and press enter.");
        if (!habitsTable.CheckForHabitByIndex(index)) return false;

        habitsTable.DeleteHabitByIndex(index);
        return true;
    }

    private bool DeleteSubHabit()
    {
        return false;
    }

    public void Update(string menuString)
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("UPDATE");

            if (!DeleteHabit()) Console.WriteLine("Couldn't delete entry");
            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);
        return;

    }

    private bool UpdateHabit()
    {
        habitsTable.ViewAll();
        return false;
    }

}