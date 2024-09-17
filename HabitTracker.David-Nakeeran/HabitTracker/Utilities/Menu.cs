using HabitTracker.Database;
namespace HabitTracker.Utilities;
internal class Menu
{
    private readonly HabitTrackerOperationHandler _operationHandler;
    private readonly DatabaseManager _database;

    internal Menu(HabitTrackerOperationHandler operationHandler, DatabaseManager database)
    {
        _operationHandler = operationHandler;
        _database = database;
    }
    internal void ShowMenu()
    {
        bool closeApp = false;
        while (!closeApp)
        {
            Console.WriteLine("\n\nMain Menu");
            Console.WriteLine("\nWhat would you like to do");
            Console.WriteLine("\nType 0 to Close Application");
            Console.WriteLine("Type 1 to View all Records");
            Console.WriteLine("Type 2 to Insert a Record");
            Console.WriteLine("Type 3 to Delete a Record");
            Console.WriteLine("Type 4 to Update a Record");
            Console.WriteLine("--------------------------------");

            string? readResult = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(readResult))
            {
                Console.WriteLine("Please make a selection");
                readResult = Console.ReadLine();
            }

            switch (readResult)
            {
                case "0":
                    Console.WriteLine("\n Goodbye \n");
                    closeApp = true;
                    break;
                case "1":
                    _database.GetAllRecords();
                    break;
                case "2":
                    string dateInput = _operationHandler.GetDateInput("Please enter a date in the format of: dd/mm/yy, Enter 0 to return to Main Menu");
                    if (dateInput == "0")
                    {
                        ShowMenu();
                    }
                    else
                    {
                        int numberInput = _operationHandler.GetNumberInput("\n\n Please insert number of glasses or other measurement of your choice (no decimals allowed)\n\n");
                        _database.Insert(dateInput, numberInput);
                    }
                    break;
                case "3":
                    Console.Clear();
                    _database.GetAllRecords();
                    int recordId = _operationHandler.GetNumberInput("\n\n Please type the Id of the you want to delete or type 0 to go back to the Main Menu\n\n");
                    if (recordId == 0)
                    {
                        ShowMenu();
                    }
                    else
                    {
                        _database.Delete(recordId);
                    }
                    break;
                case "4":
                    Console.Clear();
                    _database.GetAllRecords();
                    recordId = _operationHandler.GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main menu.\n\n");
                    if (recordId == 0)
                    {
                        ShowMenu();
                    }
                    else
                    {
                        if (!_database.DoesRecordExist(recordId))
                        {
                            Console.WriteLine($"\n\nRecord with Id: {recordId} does not exist\n\n");
                            ShowMenu();
                        }
                        dateInput = _operationHandler.GetDateInput("Please enter a date in the format of: dd/mm/yy");
                        int quantity = _operationHandler.GetNumberInput("\n\n Please insert number of glasses or other measure of your choice (no decimals allowed)\n\n");
                        _database.Update(recordId, dateInput, quantity);
                    }
                    Console.WriteLine("Update record");
                    break;
                default:
                    Console.WriteLine("Invalid entry. Please type a number from 0 to 4. \n");
                    break;
            }
        }
    }
}
