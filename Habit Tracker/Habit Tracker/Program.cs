using SQLite;

namespace Habit_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SQLite.SQLite db = new SQLite.SQLite();

            do
            {
                Console.Clear();
                
                Console.WriteLine("~~~ MAIN MENU ~~~\n");
                Console.WriteLine("What would you like to do?");

                Console.WriteLine("|  #1.\tCreate a log");
                Console.WriteLine("|  #2.\tUpdate a log");
                Console.WriteLine("|  #3.\tDelete a log");
                Console.WriteLine("|  #4.\tView logs");
                Console.WriteLine("|  #5.\tManage Habits");
                Console.WriteLine("|  #99.\tInitalize random habit data (Will delete current data!)");
                Console.WriteLine("|  #0.\tExit Application");

                Console.Write("Type the option number desired:");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        break;

                    case "2":
                        break;

                    case "3":
                        break;

                    case "4":
                        break;

                    case "5":
                        ManageHabits(ref db);
                        break;

                    case "99":
                        break;

                    case "0":   //Exit application
                        return;

                    default:    //Repeat menu for valid input
                        continue;
                }
            } while (true);
        }

        static void ManageHabits(ref SQLite.SQLite db)
        {
            Console.Clear();

            command
            List<string[]> habits = db.runSelect()
            
            Console.WriteLine("~~~ MANAGE HABITS ~~~\n");

            Console.WriteLine("")
            Console.Write("Type the option number desired:");

        }
    }
}
