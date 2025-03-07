using Database;

namespace Habit_Tracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SQLite db = new SQLite();

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

                Console.Write("Type the option number desired: ");
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

        static void ManageHabits(ref SQLite db)
        {
            List<string[]> habits = db.GetHabits();

            do
            {
                HabitMenu:
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
                string option = GetInput();

                //Show habit manage menu, or creation menu, or repeat for invalid input
                if (option != null && habitNames.Contains(option.ToUpper()))
                {
                    //Retrieve only the habit being managed
                    string[] habit = habits.Single(arr => arr.First().ToUpper() == option.ToUpper());

                    do
                    {
                        Console.Clear();
                        
                        Console.WriteLine($"~~~ Managing habit: {habit[0]} ~~~");
                        Console.WriteLine("| #1. Update");
                        Console.WriteLine("| #2. Delete");
                        Console.WriteLine("| #0. Return to Habit Menu");
                        Console.Write("Type the option number desired: ");
                        string input = GetInput();
                        
                        switch (input)
                        {
                            //Update
                            case "1":
                                try
                                {
                                    UpdateHabit(ref db, ref habit);

                                    //Reset habits list to read in new updates
                                    habits = db.GetHabits();
                                }
                                catch
                                {
                                    //Menu handling done inside UpdateHabit()
                                    continue;
                                }
                                break;

                            //Delete
                            case "2":
                                try
                                {
                                    if (DeleteHabit(ref db, habit[0]))
                                        goto HabitMenu;
                                }
                                catch
                                {
                                    //Menu handling done inside UpdateHabit()
                                    continue;
                                }
                                continue;

                            case "0":
                                //Not ideal, but was the cleanest solution for breaking out of both the switch & do-loop in one step
                                goto HabitMenu;

                            default:
                                continue;

                        }
                    } while (true);
                }
                //Create new habit, or exit to menu
                else
                {
                    switch (option)
                    {
                        //Create habit
                        case "1":
                            try
                            {
                                habits.Add(CreateHabit(ref db));
                            }
                            catch
                            {
                                //Menu handling done inside CreateHabit()
                                continue;
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

        static string[] CreateHabit(ref SQLite db)
        {
            string habitName;
            string habitDescription;
            string habitUoM;

            //Gather new habit info, ensure no empty strings
            Console.Write("\nNew habit name: ");
            habitName = GetInput();

            Console.Write("New habit description: ");
            habitDescription = GetInput();

            Console.Write("New habit UoM: ");
            habitUoM = GetInput();

            try
            {
                db.CreateHabit(habitName, habitDescription, habitUoM);
                return new string[3] { habitName, habitDescription, habitUoM };
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to create habit...");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Enter to return to the menu");
                Console.ReadLine();
                throw;
            }
        }

        static void UpdateHabit(ref SQLite db, ref string[] habit)
        {
            /* habit[0] = Habit Name
             * habit[1] = Description
             * habit[2] = UoM
             */

            Console.WriteLine($"\nHabit Name: {habit[0]}");
            Console.Write("Please enter a new name (leave blank to stay the same): ");
            string habitName = Console.ReadLine() ?? "";

            Console.WriteLine($"\nHabit Description: {habit[1]}");
            Console.Write("Please enter a new description (leave blank to stay the same): ");
            string habitDescription = Console.ReadLine() ?? "";

            Console.WriteLine($"\nHabit UoM: {habit[2]}");
            Console.Write("Please enter a new UoM (leave blank to stay the same): ");
            string habitUoM = Console.ReadLine() ?? "";

            if (habitName != "" || habitDescription != "" || habitUoM != "")
            {
                try
                {
                    db.UpdateHabit(habit[0], habitName, habitDescription, habitUoM);

                    //Return new habit back to parent function for menu update
                    if (habitName != "")
                        habit[0] = habitName;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to update habit...");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Press Enter to return to the menu");
                    Console.ReadLine();
                    throw;
                }
            } 
            else
            {
                //No changes specified
                Console.WriteLine("No changes made...\nPress Enter to return to the menu: ");
                Console.ReadLine();
                //return habit[0];
            }
        }

        static bool DeleteHabit(ref SQLite db, string habit)
        {
            Console.WriteLine("\nWARNING: This action will delete the habit selected, and all associated logs");
            Console.WriteLine("\tThis action is not reversible");
            Console.Write("\tDo you wish to proceed (Y/N): ");
            string input = GetConfirmation();

            switch (input)
            {
                case "Y":
                    try
                    {
                        db.DeleteHabit(habit);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to delete habit...");
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Press Enter to return to the menu");
                        Console.ReadLine();
                        return false;
                    }

                case "N":
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Enforces non-null user input
        /// </summary>
        /// <returns></returns>
        static string GetInput()
        {
            do
            {
                (int, int) cursor = Console.GetCursorPosition();
                string? input = Console.ReadLine();

                if (input == null || input.Length == 0)
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                }
                else return input;
            } while (true);
        }
        
        /// <summary>
        /// Enforces Y or N user input
        /// </summary>
        /// <returns></returns>
        static string GetConfirmation()
        {
            do
            {
                (int, int) cursor = Console.GetCursorPosition();
                string? input = Console.ReadLine();

                if (input == null || (input.ToUpper() != "Y" && input.ToUpper() != "N"))
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                }
                else return input;
            } while (true);
        }

    }
}
