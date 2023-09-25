
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace HabitTracker.Paul_W_Saltzman
{
    internal static class Menu
    {
        public static void GetMenu()
        {

            bool closeApp = false;
            Settings settings = Data.GetSettings();
            while (!closeApp)
            {
                Console.Clear();
                Console.WriteLine("           MAIN MENU");
                Console.WriteLine("________________________________________\n");
                Console.WriteLine("0: Close Application");
                Console.WriteLine("1: Log a Habit");
                Console.WriteLine("2: View / Edit Logs");
                Console.WriteLine("3: Manage Habits");
                Console.WriteLine("4: Manage Unit Types");
                Console.WriteLine("5: Settings");
                if (settings.TestMode) // Test mode removable in settings
                {
                    Console.WriteLine("6: Generate random logs for testing (TESTING MODE ENABLED)");
                }
                Console.WriteLine("________________________________________\n");
                string entry = Console.ReadLine();
                entry = entry.ToLower().Trim();

                switch (entry)
                {
                    case "0":
                        Console.WriteLine("GoodBye");
                        closeApp = true;
                        break;
                    case "1":
                        LoggedHabitView.CLoggedHabit(); //Create (log) Habit (you can also add habits or units (or you will be able to when implemented)
                        break;
                    case "2":
                        LoggedHabitView.RUDLoggedHabits(); //Read, Update, Delete Habits (view,edit,delete)
                        break;
                    case "3":
                        Habits.CRUDHabit();
                        break;
                    case "4":
                        UnitType.CRUDUnitTypes();
                        break;
                    case "5":
                        settings = Settings.ProgramSettings(settings);
                        break;
                    case "6":
                        if (settings.TestMode)
                            //Test mode removable in settings
                        {
                            LoggedHabitView.RadomLogHabit();
                        }
                        else { }
                        break;

                    default:

                        break;

                }

            }



        }


    }
}
