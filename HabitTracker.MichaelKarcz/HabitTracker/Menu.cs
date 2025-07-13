using System.Globalization;
using HabitTrackerLibrary;
using HabitTrackerLibrary.Models;

namespace HabitTracker;
internal static class Menu
{
    internal static void StartMainMenuLoop()
    {
        Console.Clear();

        bool closeApp = false;

        while (closeApp == false)
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("What would you like to do? ");
            Console.WriteLine("0 - Close Application");
            Console.WriteLine("1 - View All Records");
            Console.WriteLine("2 - Insert Record");
            Console.WriteLine("3 - Delete Record");
            Console.WriteLine("4 - Update Record");
            Console.WriteLine("-------------------------");

            string commandInput = Console.ReadLine();

            switch (commandInput)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    break;
                case "1":
                    ShowAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Invalid command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    internal static void ShowAllRecords()
    {
        List<Habit> allEntries = SqlHelper.GetAllRecords();

        Console.WriteLine("\nBike Riding Tracking");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("Id\t|\tDate\t\t|\tMiles ");
        Console.WriteLine("----------------------------------------------------");
        foreach (Habit entry in allEntries)
        {
            Console.WriteLine($"{entry.Id}\t|\t{entry.Date}\t|\t{entry.Miles}");
        }
        Console.WriteLine("----------------------------------------------------");
    }

    internal static void InsertRecord()
    {
        Habit habit = new Habit();

        Console.Clear();
        Console.WriteLine("\nPlease provide the date you rode your bike and the miles you traveled.");

        habit.Date = GetDateInput();
        habit.Miles = GetNumberInput("\nEnter the number of miles you rode your bike. ");

        Console.Clear();
        Console.WriteLine("Record inserted!\n");
        SqlHelper.InsertSingleRecord(habit);
    }

    internal static void DeleteRecord()
    {
        Console.Clear();
        ShowAllRecords();

        int recordID = GetNumberInput("\nPlease select a record ID to delete.");

        bool deleteSuccessful = SqlHelper.DeleteRecord(recordID);

        if (!deleteSuccessful)
        {
            Console.WriteLine("The record with the ID provided could not be deleted.");
        }
        else
        {
            Console.WriteLine($"\nRecord with ID={recordID} has been deleted.");
        }
    }

    internal static void UpdateRecord()
    {
        Console.Clear();
        ShowAllRecords();
        
        List<int> allIds = SqlHelper.GetAllIds();
        int recordId;
        do 
        { 
            recordId = GetNumberInput("\nPlease select a record ID to update.");
        }
        while (!ValidateRecordId(recordId));

        Habit updatedHabitInfo = new Habit();
        Console.Write("Please provide the new information for this record. ");
        updatedHabitInfo.Date = GetDateInput();
        updatedHabitInfo.Miles = GetNumberInput("\nEnter the updated miles you rode. ");

        bool updateSuccessful = SqlHelper.UpdateRecord(recordId, updatedHabitInfo);

        if (!updateSuccessful)
        {
            Console.WriteLine("There was an error updating that record.");
        }
        else
        {
            Console.WriteLine($"The record with the ID={recordId} was updated with the information Date={updatedHabitInfo.Date} and Miles={updatedHabitInfo.Miles}.");
        }
    }

    internal static string GetDateInput()
    {
        Console.WriteLine("\nEnter the date in the format dd-mm-yy. Type 0 to enter today's date.");

        string dateInput = Console.ReadLine();

        if (dateInput == "0") dateInput = DateTime.Now.Date.ToString("dd-MM-yy");

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.WriteLine("\n\nInvalid date. Format must be dd-mm-yy. Try again, or type 0 to enter today's date.");
            dateInput = Console.ReadLine();
            if (dateInput == "0") dateInput = DateTime.Now.Date.ToString("dd-MM-yy");
        }

        return dateInput;
    }

    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        while (string.IsNullOrEmpty(numberInput) || !int.TryParse(numberInput, out int number) || number < 0)
        {
            Console.WriteLine("Invalid number format. Please enter a positive number without decimals.");
            numberInput = Console.ReadLine();
        }

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }

    private static bool ValidateRecordId(int id)
    {
        List<int> allIds = SqlHelper.GetAllIds();

        if (allIds.Contains(id))
        {
            return true;
        }
        else
        {
            Console.WriteLine(("The given Id could not be found."));
            return false;
        }
    }
}
