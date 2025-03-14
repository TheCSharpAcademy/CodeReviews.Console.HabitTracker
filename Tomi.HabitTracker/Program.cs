using Tomi.HabitTracker.Data;
using Tomi.HabitTracker.Prompts;


class Progrram
{
    public static void Main(string[] args)
    {
        DBHelper db = new("Data Source=habbit-logger.db;Version=3;");
        bool endApp = false;

        while (!endApp)
        {
            string selectedMenu = HabitsPrompts.PromptForMenu();
            if (selectedMenu == "0")
            {
                Console.WriteLine("bye!");
                break;
            }
            ProcessSelectedMenu(selectedMenu, db);
        }
    }

    public static void ProcessSelectedMenu(string selectedMenu, DBHelper db)
    {
        switch (selectedMenu)
        {
            case "1":
                //View All Records
                try
                {
                    var habitGists = db.ViewAllRecords();
                    Console.WriteLine($"{"Id".PadRight(10)} | {"Habit".PadRight(20)} | {"Quantity".PadRight(10)} | {"Habit Date".PadRight(15)}");

                    Console.WriteLine(new string('-', 10 + 20 + 10 + 15 + 3));

                    foreach (HabitCompactGist gist in habitGists)
                    {
                        gist.PrintHabitGist();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Can't read habit gist at the moment: {e}");
                }
                break;
            case "2":
                //Insert Record
                try
                {
                    var (dateInsertField, habitInsertField, quantityInsertField) = HabitsPrompts.GetHabitDetails();
                    db.InsertRecord((dateInsertField, habitInsertField, quantityInsertField));
                    Console.WriteLine("Habit Gist Added Successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Can't insert habit gist at the moment: {e}");
                }

                break;
            case "3":
                //Delete Record
                try
                {
                    int deleteId = HabitsPrompts.PromptForId();
                    db.DeleteRecord(deleteId);
                    Console.WriteLine("Record deleted successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Can't delete habit gist at the moment: {e}");
                }
                break;
            case "4":
                try
                {
                    int updateId = HabitsPrompts.PromptForId();
                    var (dateInsertField, habitInsertField, quantityInsertField) = HabitsPrompts.GetHabitDetails();

                    db.UpdateRecord(updateId, (dateInsertField, habitInsertField, quantityInsertField));
                    Console.WriteLine("Habit Gist Successfully!");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Can't update habit gist at the moment: {e}");
                }
                break;
            default:
                Console.WriteLine("Invalid Selected Menu");
                break;
        }
    }
}