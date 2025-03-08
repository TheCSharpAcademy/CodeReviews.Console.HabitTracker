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
                Console.WriteLine("|  #2.\tDelete a log");
                Console.WriteLine("|  #3.\tView logs");
                Console.WriteLine("|  #5.\tManage Habits");
                Console.WriteLine("|  #0.\tExit Application");

                Console.Write("Type the option number desired: ");
                string option = GetInput();

                //Don't allow logging unless habits exist
                if (db.GetHabits().Count == 0 && int.TryParse(option, out int optionInt) && optionInt < 5)
                {
                    Console.WriteLine("\nThere are no habits...?");
                    Console.WriteLine("Please create a habit before logging activity");
                    Console.Write("Press Enter to return to the menu: ");
                    Console.ReadLine();
                    continue;
                }

                switch (option)
                {
                    case "1":
                        CreateLog(ref db);
                        break;

                    case "2":
                        DeleteLog(ref db);
                        break;

                    case "3":
                        ViewLogs(ref db);
                        break;

                    case "5":
                        ManageHabits(ref db);
                        break;

                    case "0":   //Exit application
                        return;

                    default:    //Repeat menu for valid input
                        continue;
                }
            } while (true);
        }

        static void CreateLog(ref SQLite db)
        {
            List<string[]> habits = db.GetHabits();

            do
            {
                Console.Clear();
                Console.WriteLine("Select a habit to log.");

                //Display current habits
                string[] habitNames = GetHabitsMenu(ref db);
                Console.WriteLine("|\tType 0 to return to the main menu");

                Console.Write("Which habit do you want to log (type habit name):");
                string option = GetInput();

                if (habitNames.Contains(option.ToUpper()))
                {
                    //Retrieve only the habit being logged
                    string[] habit = habits.Single(arr => arr.First().ToUpper() == option.ToUpper());

                    Console.Write($"\nHow many {habit[2]} would you like to log: ");
                    int count = GetNumber();

                    Console.Write("\nSelect date to be logged (YYYY-MM-DD, or t = today): ");
                    DateTime date = GetDate();

                    Console.WriteLine($"Logging {count} {habit[2]} on date {date.ToShortDateString()} for habit {habit[0]}...");
                    Console.Write("\tDo you wish to proceed (Y/N): ");
                    string confirmation = GetConfirmation();

                    switch (confirmation)
                    {
                        case "Y":
                            try
                            {
                                db.CreateLog(habit[0], count, date);
                                return;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Failed to create log...");
                                Console.WriteLine(e.Message);
                                Console.Write("Press Enter to return to the menu");
                                Console.ReadLine();
                                continue;
                            }

                        case "N":
                            continue;

                        default:
                            continue;
                    }
                }
                else
                {
                    if (option == "0")
                        return;
                    else
                        continue;
                }
            } while (true);

        }

        static void DeleteLog(ref SQLite db)
        {
            List<string[]> logs = db.GetLogs();
            /* logs[0] = ID
             * logs[1] = Habit
             * logs[2] = Count
             * logs[3] = UoM
             * logs[4] = Date
             */

            do
            {
                Console.Clear();

                //Display & select logs
                string[]? log = GetLogFromMenu(logs);
                if (log == null)
                    return;

                try
                {
                    db.DeleteLog(log[0]);
                    logs.Remove(log);
                    Console.WriteLine("Log deleted successfully!");
                    Console.Write("Press Enter to continue: ");
                    Console.ReadLine();
                    continue;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to delete log...");
                    Console.WriteLine(e.Message);
                    Console.Write("Press Enter to continue");
                    Console.ReadLine();
                    continue;
                }
            } while (true);
        }

        static void ViewLogs(ref SQLite db)
        {
            List<string[]> logs = db.GetLogs();
            /* logs[0] = ID
             * logs[1] = Habit
             * logs[2] = Count
             * logs[3] = UoM
             * logs[4] = Date
             */

            int page = 0;
            int maxPage = logs.Count() / 10;

            do
            {
                Console.Clear();

                Console.WriteLine("~~~ Logs ~~~");

                for (int i = 0; i < 10 && (page * 10) + i < logs.Count(); i++)
                {
                    int log = (page * 10) + i;
                    Console.Write($"|\t#{logs[log][0]}. {logs[log][1]} - ");
                    Console.WriteLine($"{logs[log][2]} {logs[log][3]} - {logs[log][4]}");
                }
                Console.WriteLine("\nType 'd' to page down, or 'u' to page up");
                Console.Write("Or type 0 to return to the main menu: ");

                string input = GetInput();

                switch (input.ToUpper())
                {
                    case "D":
                        if (page < maxPage)
                            page++;
                        break;

                    case "U":
                        if (page > 0)
                            page--;
                        break;

                    case "0":
                        return;

                    default:
                        break;
                }
            } while (true);
        }

        static string[]? GetLogFromMenu(List<string[]> logs)
        {
            /* logs[0] = ID
             * logs[1] = Habit
             * logs[2] = Count
             * logs[3] = UoM
             * logs[4] = Date
             */

            int page = 0;
            int maxPage = logs.Count() / 10;

            do
            {
                Console.Clear();

                Console.WriteLine("Select a log to delete.");

                List<string> ids = new List<string>();
                for (int i = 0; i < 10 && (page * 10) + i < logs.Count(); i++)
                {
                    int log = (page * 10) + i;
                    Console.Write($"|\t#{logs[log][0]}. {logs[log][1]} - ");
                    Console.WriteLine($"{logs[log][2]} {logs[log][3]} - {logs[log][4]}");
                    ids.Add(logs[log][0]);
                }
                Console.WriteLine("|\tType 0 to return to the main menu\n");

                Console.WriteLine("Type the ID of the log you would like to select");
                Console.Write("Or type 'd' to page down, or 'u' to page up: ");
                string input = GetInput();
                
                if (int.TryParse(input, out int id))
                {
                    if (ids.Contains(id.ToString()))
                        return logs.Single(arr => arr.First().ToUpper() == id.ToString());
                    else if (id == 0)
                    {
                        return null;
                    }
                    else
                    {
                        continue;
                    }
                }
                //Page Up, Page Down, or redraw on default
                else
                {
                    switch (input.ToUpper())
                    {
                        case "D":
                            if (page < maxPage)
                                page++;
                            break;

                        case "U":
                            if (page > 0)
                                page--;
                            break;

                        default:
                            break;
                    }
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
                string[] habitNames = GetHabitsMenu(ref db);
                Console.WriteLine("|\tType 1 to create a new habit");
                Console.WriteLine("|\tType 0 to return to the main menu");

                Console.Write("Which habit do you want to manage (type habit name):");
                string option = GetInput();

                //Show habit manage menu, or creation menu, or repeat for invalid input
                if (habitNames.Contains(option.ToUpper()))
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
                Console.Write("Press Enter to return to the menu");
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

                    Console.WriteLine("Habit updated successfully!");
                    Console.Write("Press Enter to return to the menu: ");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to update habit...");
                    Console.WriteLine(e.Message);
                    Console.Write("Press Enter to return to the menu");
                    Console.ReadLine();
                    throw;
                }
            } 
            else
            {
                //No changes specified
                Console.WriteLine("No changes made...\nPress Enter to return to the menu: ");
                Console.ReadLine();
            }
        }

        static bool DeleteHabit(ref SQLite db, string habit)
        {
            Console.WriteLine("\nWARNING: This action will delete the habit selected, and all associated logs");
            Console.WriteLine("\tThis action is not reversible");
            Console.Write("\tDo you wish to proceed (Y/N): ");
            string confirmation = GetConfirmation();

            switch (confirmation)
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
                        Console.Write("Press Enter to return to the menu");
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
        /// Displays and returns list of habit names in upper case
        /// </summary>
        /// <param name="db"></param>
        /// <returns>string[] habitNames</returns>
        static string[] GetHabitsMenu(ref SQLite db)
        {
            List<string[]> habits = db.GetHabits();

            string[] habitNames = new string[habits.Count];
            for (int i = 0; i < habits.Count; i++)
            {
                Console.WriteLine($"|\t{habits[i][0]}");
                habitNames[i] = habits[i][0].ToUpper();
            }

            return habitNames;
        }

        /// <summary>
        /// Enforces non-null user input
        /// </summary>
        /// <returns></returns>
        static string GetInput()
        {
            (int, int) cursor = Console.GetCursorPosition();
            
            do
            {
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
                else return input.ToUpper();
            } while (true);
        }

        /// <summary>
        /// Enforces valid date input
        /// </summary>
        /// <returns></returns>
        static DateTime GetDate()
        {
            (int, int) cursor = Console.GetCursorPosition();
            do
            {
                string? input = Console.ReadLine();

                if (DateTime.TryParse(input, out DateTime date))
                    return date;
                else if (input == "t")
                {
                    return DateTime.Today;
                }
                else
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                }

            } while (true);
        }

        /// <summary>
        /// Enforces valid number input
        /// </summary>
        /// <returns></returns>
        static int GetNumber()
        {
            (int, int) cursor = Console.GetCursorPosition();
            do
            {
                string? input = Console.ReadLine();

                if (Int32.TryParse(input, out int number))
                    return number;
                else
                {
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(cursor.Item1, cursor.Item2);
                }

            } while (true);
        }
    }
}
