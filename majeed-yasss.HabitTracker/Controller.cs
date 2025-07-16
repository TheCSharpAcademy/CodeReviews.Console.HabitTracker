namespace majeed_yasss.HabitTracker;
internal class Controller
{
    public static void Run()
    {
        Model.LoadDatabase();

        while (true)
        {
            Option option = View.MainMenu();
            Execute(option);
        }
    }
    private static void Execute(Option command)
    {
        switch (command)
        {
            case Option.None:
                View.Exit();
                break;

            case Option.ViewLogs:
                GetAllRecords();
                break;

            case Option.CreateLog:
                Insert();
                break;

            case Option.DeleteLog:
                {
                    bool d = Delete(); // Ideally this would be in a View.DeletionSuccess/Failure
                    if (d) Console.WriteLine($"\n\nRecord was deleted successfuly.\n\n");
                    else Console.WriteLine($"\n\nRecord doesn't exist.\n\n");
                    Console.WriteLine("press any key to continue"); Console.ReadKey();
                }
                break;

            case Option.UpdateLog:
                { 
                    bool u = Update(); // Ideally this would be in a View.UpdateSuccess/Failure
                    if (u) Console.WriteLine($"\n\nRecord was updated successfuly.\n\n");
                    else Console.WriteLine($"\n\nRecord doesn't exist.\n\n");
                    Console.WriteLine("press any key to continue"); Console.ReadKey();
                }
                break;

            default:
                View.Exit();
                break;
        }
    }
    private static void GetAllRecords()
    {
        List<DrinkingWater> tableData = Model.GetAllRecords();
        View.Records(tableData);
    }
    private static void Insert()
    {
        Console.Clear();
        int quantity = View.GetPositiveInt("\n\nNumber of glasses (no decimals allowed)\n\n");
        string date = View.GetDateInput();
        Model.Insert(date, quantity);
        GetAllRecords();
    }
    private static bool Delete()
    {
        DrinkingWater? record = SelectRecord();
        return record != null && Model.Delete(record.Id);
    }
    private static bool Update()
    {
        DrinkingWater? record = SelectRecord();
        return record != null && Model.Update(record.Id);
    }
    private static DrinkingWater? SelectRecord()
    {
        List<DrinkingWater> tableData = Model.GetAllRecords();
        if (tableData.Count == 0) return null;
        return View.SelectFromRecords(tableData);
    }
}

