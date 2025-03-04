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
            List<string[]> habits = db.GetHabits();

            do
            {
                Console.Clear();

                Console.WriteLine("~~~ MANAGE HABITS ~~~");

                //Display all current habits
                string[] habitNames = new string[habits.Count];
                for (int i = 0; i < habits.Count; i++)
                {
                    Console.WriteLine($"|\t{habits[i][0]}");
                    habitNames[i] = habits[i][0].ToUpper();
                }
                Console.WriteLine("|\tType 1 to create a new habit");
                Console.WriteLine("|\tType 0 to return to the main menu");

                Console.Write("Which habit do you want to manage (type habit name):");
                string? option = Console.ReadLine();

                //Show habit manage menu, or creation menu, or repeat for invalid input
                if (option != null && habitNames.Contains(option.ToUpper()))
                {
                    Console.WriteLine($"\n~~~ Managing habit: {option} ~~~");
                    Console.WriteLine("| #1. Update");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    //Create new habit, or exit to menu
                    switch (option)
                    {
                        case "1":   //Create habit
                            string habitName;
                            string habitDescription;
                            string habitUoM;

                            //Gather new habit info, ensure no empty strings
                            Console.WriteLine("\nNew habit name: ");
                            habitName = GetInput();

                            Console.WriteLine("New habit description: ");
                            habitDescription = GetInput();

                            Console.WriteLine("New habit UoM: ");
                            habitUoM = GetInput();

                            try
                            {
                                db.CreateHabit(habitName, habitDescription, habitUoM);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Failed to create habit...");
                                Console.WriteLine(e.Message);
                                Console.WriteLine("Press Enter to return to the menu");
                                continue;
                            }
                            finally
                            {
                                habits.Add(new string[3] { habitName, habitDescription, habitUoM });
                            }
                            break;

                        case "0":   //Exit to menu
                            return;

                        default:
                            break;
                    }
                }
            } while (true);
        }

        /// <summary>
        /// Enforces non-null user input
        /// </summary>
        /// <returns></returns>
        static string GetInput()
        {
            do
            {
                string? input = Console.ReadLine();

                if (input == null || input.Length == 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("\r" + new string(' ', Console.BufferWidth) + "\r");
                }
                else return input;
            } while (true);
        }
    }
}
