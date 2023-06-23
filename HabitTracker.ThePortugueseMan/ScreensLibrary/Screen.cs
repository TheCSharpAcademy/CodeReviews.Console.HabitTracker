using HabitsLibrary;
using DataBaseLibrary;
using System.Globalization;

namespace ScreensLibrary;

public class Screen
{
    MainTable mainTable;
    AskInput askInput = new();
    DataBaseCommands dbCmd = new();
    DataBaseViews dbView = new();

    public Screen(MainTable mainTable)
    {
        this.mainTable = mainTable;
    }

    public void ViewAllInTable(string tableName)
    {
        int inputNumber;
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("VIEW");
            dbCmd.ViewAll(tableName);

            if (tableName == mainTable.tableName)
            {
                int index = askInput.PositiveNumber("Write the index of the habit you want to see. Or press 0 to return.");
                if (index == 0) { exit = true; continue; }
                else
                {
                    string? subTableName = dbCmd.GetTableNameOrUnitsFromIndex(tableName, index, "TableName");
                    SubMenu(subTableName);
                }
            }
            else //it's a subtable
            {
                    Console.WriteLine("Press 1 to view stats by year or 0 to return to the menu");
                    inputNumber = askInput.PositiveNumber("");
                    if (inputNumber == 0) { exit = true; continue; }
                    else if (inputNumber == 1) YearView(tableName);
                    else Console.WriteLine("Please select a valid option");
            }
        }
    }

    private void YearView(string tableName)
    {
        int yearIn;
        int timesPerYear, totalOfYear;
        bool showError = false;
        askInput.ClearPreviousLines(4); //clears previous prompt
        
        do
        {
            if (!showError) yearIn = askInput.PositiveNumber("Write the year you want to view. Use the last 2 digits of the year.");
            else
            {
                askInput.ClearPreviousLines(3);
                yearIn = askInput.PositiveNumber("Please write a valid year. Use the last 2 digits of the year");
            }
            timesPerYear = dbView.TimesLoggedInYear(tableName, yearIn);
            totalOfYear = dbView.TotalOfYear(tableName, yearIn);
            showError = true;
        } while (timesPerYear == -1 || totalOfYear == -1);

        
        string fullYear = (DateTime.ParseExact(yearIn.ToString(),"yy", new CultureInfo("en-US"))).ToString("yyyy"); //Parses the last 2 digits of the input into a full year for display purposes
        timesPerYear = dbView.TimesLoggedInYear(tableName, yearIn);
        totalOfYear = dbView.TotalOfYear(tableName, yearIn);
        string habitUnits = dbCmd.GetUnitsFromTableName(mainTable.tableName, tableName);

        Console.WriteLine($"In {fullYear} you logged {timesPerYear} times, totalling {totalOfYear} {habitUnits}!");
        askInput.AnyAndEnterToContinue();
    }

    public void Insert(string tableName)
    {
        if (tableName == mainTable.tableName) InsertToMainTable();
        else InsertToSubtable(tableName);
    }

    //Main Table expects string with name of the table of the habit and string with the unit associated with the habit
    private void InsertToMainTable()
    {
        string? habitName;
        bool showError = false;

        do //while the name inserted already exists
        {
            if (!showError) habitName = askInput.LettersNumberAndSpaces("Write the name of your habit or 0 to return.");
            else habitName = askInput.LettersNumberAndSpaces("Habit already exists.");
            if (habitName == "0") return;
            showError = true;
        } while (mainTable.CheckForTableName(mainTable.TransformToSubTableName(habitName)));

        string? habitUnit = askInput.LettersNumberAndSpaces("Write the units of your habit. Or 0 to return");
        if (habitUnit == "0") return;
        mainTable.InsertNew(habitName, habitUnit);
        return;
    }

    //subtable expects string with a date and int with a quantity
    private void InsertToSubtable(string subTableName)
    {
        string? date = askInput.Date("Write a date in the format dd-mm-yy. Or 0 to return");
        if (date == "0") return;

        int quantity = askInput.PositiveNumber("Write the quantity. Or 0 to return");
        if (quantity == 0) return;

        dbCmd.Insert(subTableName, date, quantity);
        
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
            int index = askInput.PositiveNumber("Write the number of the entry you want to delete and press enter." +
                " Or press 0 to return to the Menu");

            if (index == 0) break;
            if (!DeleteEntry(tableName, index))
            {
                Console.WriteLine("Couldn't delete entry");
                if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
                continue;
            }
            else
            {
                Console.WriteLine("Entry deleted successfully!");
            }

            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);
        return;
    }

    //returns true if entry exists and is deleted successfully
    private bool DeleteEntry(string tableName, int index)
    {
        if (!dbCmd.CheckIfIndexExists(index, tableName)) return false;

        if (!dbCmd.DeleteByIndex(index, tableName)) return false;
        return true;
    }

    public void Update(string tableName)
    {
        bool exitScreen = false;
        do
        {
            Console.Clear();
            Console.WriteLine("UPDATE");
            dbCmd.ViewAll(tableName);

            int index = askInput.PositiveNumber("Write the index of the entry you want to update, or press 0 to return.");

            if (index == 0) return;
            else if (UpdateEntry(tableName, index))
            {
                Console.WriteLine("Entry successfully updated");
            }
            else Console.WriteLine("Couldn't update entry");

            if (askInput.ZeroOrAnyKeyAndEnterToContinue()) exitScreen = true;
            else continue;
        } while (!exitScreen);

        return;
    }

    private bool UpdateEntry(string tableName, int index)
    {
        bool showError;
        string? newName;
        string? newTableName;

        if (!dbCmd.CheckIfIndexExists(index, tableName)) return false;
        //if it's the main table - unique habitName and an habit unit are needed
        if (tableName == mainTable.tableName)
        {
            showError = false;
            do
            {
                if (!showError) newName = askInput.LettersNumberAndSpaces("Write the new name of your habit.");
                else newName = askInput.LettersNumberAndSpaces("Habit already exists.");
                newTableName = mainTable.TransformToSubTableName(newName);
                showError = true;
            } while (mainTable.CheckForTableName(newTableName));

            string? newUnit = askInput.LettersNumberAndSpaces("Write the new unit");

            if (!dbCmd.UpdateByIndex(tableName, index, newTableName, newUnit))
            {
                Console.WriteLine("Couldn't update habit!");
                return false;
            }
            else return true;
        }
        //if it's a subTable - date and quantity are needed
        else
        {
            string? newDate = askInput.Date("Insert the new date");
            int newQuantity = askInput.PositiveNumber("Insert the new amount");
            if (!dbCmd.UpdateByIndex(tableName, index, newDate, newQuantity))
            {
                Console.WriteLine("Couldn't update log!");
                return false;
            }
            else return true;
        }
    }

    //menu for the habits
    public void SubMenu(string? subTableName)
    {
        Console.Clear();
        bool invalidCommand = false;

        if (subTableName is null) return;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("HABIT TRACKER");
            Console.WriteLine("\n" + subTableName.TrimEnd(']').TrimStart('['));
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
                case "0": return;
                case "1": ViewAllInTable(subTableName); break;
                case "2":
                    askInput.ClearPreviousLines(1);
                    Console.WriteLine("INSERT");
                    Insert(subTableName); 
                    break;
                case "3": Delete(subTableName); break;
                case "4": Update(subTableName); break;
                default:
                    invalidCommand = true;
                    break;
            }
        }
    }
}