namespace CodeReviews.Console.HabitTracker.selnoom.Views;

using CodeReviews.Console.HabitTracker.selnoom.Data;
using CodeReviews.Console.HabitTracker.selnoom.Model;
using System.Text.RegularExpressions;
using SC = System.Console;

internal static class Menu
{
    static WaterIntakeRepository waterRepository = new();
    internal static void ShowMenu()
    {
        bool exitProgram = false;
        string userInput;


        while (exitProgram == false)
        {
            SC.Clear();
            SC.WriteLine("Main Menu");
            SC.WriteLine("\n\nPlease select an option:");
            SC.WriteLine
                (
                "1 - View all records\n" +
                "2 - Create record\n" +
                "3 - Delete record\n" +
                "4 - Update record\n" +
                "0 - Exit program"
                );
            SC.WriteLine("----------------------------------");

            userInput = SC.ReadLine();
            while (!Regex.IsMatch(userInput, "^[0-4]$"))
            {
                SC.WriteLine("\nInvalid input. Please choose from the options.");
                userInput = SC.ReadLine();
            }

            switch (userInput)
            {
                case "1":
                    SC.Clear();
                    PrintAllRecords(GetAllRecords());
                    SC.WriteLine("\nPress enter to continue:");
                    SC.ReadLine();
                    break;
                case "2":
                    CreateRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "0":
                    exitProgram = true;
                    SC.Clear();
                    SC.WriteLine("Goodbye!");
                    SC.ReadLine();
                    break;
                default:

                    break;
            }
        }
    }

    private static int? GetValidInput()
    {
        List<WaterIntake> records = GetAllRecords();
        List<int> recordIds = records.Select(x => x.Id).ToList();

        PrintAllRecords(records);


        int userInput;
        bool validInput = false;
        do
        {
            if (!int.TryParse(SC.ReadLine(), out userInput))
            {
                SC.WriteLine("\nInvalid input. Please try again.");
                continue;
            }

            if (userInput == 0)
            {
                return null;
            }

            if (!recordIds.Contains(userInput))
            {
                SC.WriteLine("\nThe typed Id does not match any records. Please try again.");
                continue;
            }

            validInput = true;
        } while (!validInput);

        return userInput;
    }

    private static void UpdateRecord()
    {
        try
        {
            SC.Clear();
            SC.WriteLine("\nPlease type the Id of the record you wish to edit or 0 to return to the menu:");

            int? userInput = GetValidInput();

            if (userInput == null)
            {
                return;
            }

            SC.WriteLine("Now, type the new date (Format: dd-mm-yy) for this record:");
            string date = GetDateInput();
            if (date == null)
            {
                return;
            }

            SC.WriteLine("Next, type the new quantity for this record:");
            int? quantity = GetQuantityInput();
            if (quantity == 0)
            {
                return;
            }

            waterRepository.UpdateRecord(userInput, date, quantity);

            SC.WriteLine($"Record {userInput} was successfully updated! Press enter to continue.");
            SC.ReadLine();
        }
        catch
        {
            SC.WriteLine("Oops, an error occurred. Please try again later.");
        }
        
    }

    private static void DeleteRecord()
    {
        try
        {
            SC.WriteLine("\nPlease type the Id of the record you wish to delete or 0 to return to the menu:");

            int? userInput = GetValidInput();

            if (userInput == null)
            {
                return;
            }

            waterRepository.DeleteRecord(userInput);

            SC.Clear();
            SC.WriteLine($"Record {userInput} was successfully deleted! Press enter to continue.");
            SC.ReadLine();
        }
        catch
        {
            SC.WriteLine("Oops, an error occurred. Please try again later.");
        }
    }

    private static void CreateRecord()
    {
        var createInputs = ShowCreateMenu();
        if (createInputs == null)
        {
            return;
        }
        string date = createInputs.Value.Date;
        int quantity = createInputs.Value.Quantity;
        waterRepository.CreateRecord(date, quantity);
        SC.WriteLine("Record created! Press Enter to continue:");
        SC.ReadLine();
    }

    private static List<WaterIntake> GetAllRecords()
    {
        try
        {
            return waterRepository.GetRecords();
        }
        catch
        {
            SC.WriteLine("Error retrieving records.");
            return new List<WaterIntake>();
        }
    }

    private static void PrintAllRecords(List<WaterIntake> records)
    {
        try
        {
            if (records.Count > 0)
            {
                SC.WriteLine("Records:\n\n");
                foreach (WaterIntake record in records)
                {
                    SC.WriteLine($"Id: {record.Id}\tDate: {record.Date}\tQuantity: {record.Quantity}\t");
                }
                
            }
            else
            {
                SC.WriteLine("There are currently no records");
            }
        }
        catch
        {
            SC.WriteLine("Oops, an error occurred while retrieving records. Please try again later.");
        }
    }

    internal static (string Date, int Quantity)? ShowCreateMenu()
    {
        SC.Clear();
        SC.WriteLine("Please enter the date (Format: dd-mm-yy) you drank the water or 0 to return to the previous menu:");

        string dateInput = GetDateInput();
        if (dateInput == null)
        {
            return null;
        }

        SC.Clear();
        SC.WriteLine("\nPlease enter the amount of glasses of water taken (only integers allowed) or 0 to return to the previous menu:");

        int? quantity = GetQuantityInput();
        if (quantity == null)
        {
            return null;
        }

        return (dateInput, quantity.Value);
    }

    internal static string? GetDateInput()
    {
        string dateInput;

        while (true)
        {
            dateInput = SC.ReadLine();

            if (dateInput == "0")
            {
                return null;
            }

            if (DateTime.TryParseExact(dateInput, "dd-MM-yy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime _))
            {
                SC.WriteLine($"You entered {dateInput}. Please press enter to continue:");
                SC.ReadLine();
                return dateInput;
            }
            else
            {
                SC.Clear();
                SC.WriteLine("\nInvalid date format (use dd-mm-yy). Please try again or type 0 to return.");
            }
        }
    }

    internal static int? GetQuantityInput()
    {
        int quantity;

        while (!int.TryParse(SC.ReadLine(), out quantity))
        {
            SC.Clear();
            SC.WriteLine("\nInvalid input. Please try again or type 0 to return to the previous menu:.");
        }

        if (quantity == 0)
        {
            return null;
        }

        SC.WriteLine($"You entered {quantity}. Please press enter to continue:");
        SC.ReadLine();
        return quantity;
    }

}
