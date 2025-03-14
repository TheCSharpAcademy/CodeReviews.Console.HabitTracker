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
            HabitsPrompts.ProcessSelectedMenu(selectedMenu, db);
        }
    }
}