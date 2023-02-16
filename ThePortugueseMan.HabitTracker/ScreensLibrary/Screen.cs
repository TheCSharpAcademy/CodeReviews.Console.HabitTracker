using HabitsLibrary;
namespace ScreensLibrary;

public class Screen
{
    HabitsTable habitsTable;
    HabitsSubTable habitsSubTable;

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

    }
}