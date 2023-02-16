using HabitsLibrary;
using DataBaseLibrary;
namespace ScreensLibrary;

public class Screen
{
    HabitsTable habitsTable;
    HabitsSubTable habitsSubTable;
    AskInput askInput = new();
    DataBaseCommands dbCmd = new();

    public Screen(HabitsTable habitsTable,HabitsSubTable habitsSubTable)
    {
        this.habitsTable = habitsTable;
        this.habitsSubTable = habitsSubTable;
    }

    private string GetTableNameFromMenuName(string menuName)
    {
        if (menuName == "Habits") return habitsTable.tableName;
        else if (menuName == "SubHabits") return null;

        return null;
    }
    public void ViewAll(string menuString)
    {
        Console.Clear();
        dbCmd.ViewAll(GetTableNameFromMenuName(menuString));
        askInput.AnyAndEnterToContinue();
    }

    public void Insert(string menuString)
    {
        if (menuString == "Habits") InsertHabit();
        else if (menuString == "SubHabits") InsertSubHabit();
    }

    private void InsertHabit() 
    {
        string habitName;
        bool showError = false;
        do
        {
            if (!showError) habitName = askInput.LettersNumberAndSpaces("Write the name of your habit.");
            else habitName = askInput.LettersNumberAndSpaces("Habit already exists.");
            showError= true;
        } while (habitsTable.CheckForHabitNameInTable(habitName));

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

            if (!DeleteEntry(menuString))
            {
                Console.WriteLine("Couldn't delete entry");
                if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
                continue;
            }
            else Console.WriteLine("Entry deleted successfully!");
            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);
        return;

    }

    private bool DeleteEntry(string menuString)
    {
        string tableName = GetTableNameFromMenuName(menuString);
        dbCmd.ViewAll(GetTableNameFromMenuName(menuString));
        int index = askInput.Digits("Write the number of the entry you want to delete and press enter.");
        if (!dbCmd.CheckIndex(index, tableName)) return false;

        if (!dbCmd.DeleteByIndex(index,tableName)) return false;
        return true;
    }

    public void Update(string menuString)
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("UPDATE");
            dbCmd.ViewAll(GetTableNameFromMenuName(menuString));
            if (UpdateEntry(menuString)) Console.WriteLine("Entry successfully updated");
            else Console.WriteLine("Couldn't update entry");

            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);
        return;

    }

    private bool UpdateEntry(string menuString)
    {
        string tableName = GetTableNameFromMenuName(menuString);
        int index = askInput.Digits("Write the index of the entry you want to update");
        if (!dbCmd.CheckIndex(index, tableName)) return false;

        if (menuString == "Habits")
        {
            string newName = askInput.LettersNumberAndSpaces("Write the new name");
            string newUnit = askInput.LettersNumberAndSpaces("Write the new unit");
            if (!dbCmd.Update(tableName, index, newName, newUnit)) return false;
            else return true;
        }
        return false;
    }

}