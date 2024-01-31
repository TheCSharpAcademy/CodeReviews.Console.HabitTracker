namespace HabitTracker.frockett;

internal class UserInput
{
    DbOperations db = new DbOperations();

    public void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;

        while (!closeApp)
        {
            Console.WriteLine("\t\tMain Menu");
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine("Please select an option from the menu below\n");
            Console.WriteLine("1. View Records");
            Console.WriteLine("2. Add New Habit");
            Console.WriteLine("3. Insert Record");
            Console.WriteLine("4. Delete Record");
            Console.WriteLine("5. Update Record");
            Console.WriteLine("0. Close Application");
            Console.WriteLine("_______________________________________________\n");
            string? readResult = Console.ReadLine();

            if (!int.TryParse(readResult, out int input))
            {
                Console.WriteLine("Invalid selection, please input a valid integer choice");
                readResult = Console.ReadLine();
            }

            switch (input)
            {
                case 1:
                    db.PrintAllRecords();
                    break;
                case 2:
                    db.InsertHabit();
                    break;
                case 3:
                    db.InsertRecord();
                    break;
                case 4:
                    db.DeleteRecord();
                    break;
                case 5:
                    db.UpdateRecord();
                    break;
                case 0:
                    closeApp = true;
                    break;
                default:
                    Console.WriteLine("Selection not recognized, please input a selection between 1 and 5");
                    break;
            }
        }
        return;
    }
}
