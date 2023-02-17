using HabitsLibrary;
using DataBaseLibrary;
namespace ScreensLibrary;

public class Screen
{
    HabitsTable habitsTable;
    AskInput askInput = new();
    DataBaseCommands dbCmd = new();

    public Screen(HabitsTable habitsTable)
    {
        this.habitsTable = habitsTable;
    }

    public void ViewAll(string tableName)
    {
        Console.Clear();
        dbCmd.ViewAll(tableName);

        if (tableName == habitsTable.tableName)
        {
            int index = askInput.Digits("Write the index of the habit you want to see. Or press 0 to return.");
            if (index == 0) return;


            else
            {

                string habitTableName = habitsTable.GetTableNameOrUnitsFromIndex(tableName, index, "TableName");
                SubMenu(habitTableName);
            }
        }
        askInput.AnyAndEnterToContinue();
    }

    public void Insert(string tableName)
    {
        if (tableName == "HabitsTable") InsertHabit();
        else InsertSubHabit();
    }

    private void InsertHabit()
    {
        string habitName;
        bool showError = false;

        do {
            if (!showError) habitName = askInput.LettersNumberAndSpaces("Write the name of your habit.");
            else habitName = askInput.LettersNumberAndSpaces("Habit already exists.");
            showError = true;
        } while (habitsTable.CheckForHabitNameInTable(habitName));

        string habitUnit = askInput.LettersNumberAndSpaces("Write the units of your habit.");
        habitsTable.InsertNewHabit(habitName, habitUnit);
        return;
    }

    private void InsertSubHabit()
    {
        return;
    }

    public void Delete(string tableName)
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("DELETE");

            dbCmd.ViewAll(tableName);
            int index = askInput.Digits("Write the number of the entry you want to delete and press enter." +
                " Or press 0 to return to the Menu");

            if (index == 0) break;
            if (!DeleteEntry(tableName, index))
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

    private bool DeleteEntry(string tableName, int index)
    {
        if (!dbCmd.CheckIndex(index, tableName)) return false;

        if (!dbCmd.DeleteByIndex(index, tableName)) return false;
        return true;
    }

    public void Update(string tableName
        )
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("UPDATE");
            dbCmd.ViewAll(tableName);

            int index = askInput.Digits("Write the index of the entry you want to update, or press 0 to return.");

            if (index == 0) return;
            else if (UpdateEntry(tableName, index)) Console.WriteLine("Entry successfully updated");
            else Console.WriteLine("Couldn't update entry");

            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);

        return;

    }

    private bool UpdateEntry(string tableName, int index)
    {
        bool showError;
        string newName;

        if (!dbCmd.CheckIndex(index, tableName)) return false;

        if (tableName == "Habits")
        {
            showError = false;
            do
            {
                if (!showError) newName = askInput.LettersNumberAndSpaces("Write the new name of your habit.");
                else newName = askInput.LettersNumberAndSpaces("Habit already exists.");
                showError = true;
            } while (habitsTable.CheckForHabitNameInTable(newName));

            string newUnit = askInput.LettersNumberAndSpaces("Write the new unit");

            if (!dbCmd.Update(tableName, index, newName, newUnit))
            {
                Console.WriteLine("Couldn't update habit!");
                return false;
            }
            else return true;
        }

        else return false;
    }

    public void SubMenu(string tableName)
    {
        Console.Clear();
        bool closeApp = false;
        bool invalidCommand = false;
        while (!closeApp)
        {
            Console.Clear();
            Console.WriteLine("HABIT TRACKER");
            Console.WriteLine("\nHABIT NAME");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Return to the Main Menu.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 do Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("----------------------------------------");
            if (invalidCommand)
            {
                Console.Write("Invalid Command. Please choose one of the commands above");
            }
            Console.Write("\n");
            string? commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0": closeApp = true; return;
                case "1": ViewAll(tableName); break;
                case "2": Insert(tableName); break;
                case "3": Delete(tableName); break;
                case "4": Update(tableName); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
    }
}