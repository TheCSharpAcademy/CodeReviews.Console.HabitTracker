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
                        continue;

                    case "2":
                        continue;

                    case "3":
                        continue;

                    case "4":
                        continue;

                    case "5":
                        continue;

                    case "99":
                        continue;

                    case "0":
                        return;

                    default:
                        continue;
                }
            } while (true);
        }
    }
}
