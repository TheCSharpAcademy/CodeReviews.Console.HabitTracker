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
        if (menuString == "Habits") DeleteHabit();
        else if (menuString == "SubHabits") DeleteSubHabit();
    }

    private void DeleteHabit()
    {

    }

    private void DeleteSubHabit()
    {

    }
}