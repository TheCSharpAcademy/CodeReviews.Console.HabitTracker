using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitTracker
{
    internal class Driver
    {

        public static void Main(string[] args)
        {

            MainMenu();

        }

        /// <summary>
        /// Displays the Main Menu where the user can:
        /// - Choose a habit to view records of
        /// - Create a new habit to track\
        /// - Quit the program
        /// </summary>
        static void MainMenu()
        {
            ConsoleUtilities.Menu mainMenu = new ConsoleUtilities.Menu("Main Menu. Please select an option");

            mainMenu.AddOption("A", "All Habits...", () => { HabitsMenu(); });
            mainMenu.AddOption("N", "Add Habit", () => { NewHabit(); });
            mainMenu.AddOption("Q", "Quit Program", () => { Environment.Exit(0); });

            mainMenu.SelectOption();
        }

        /// <summary>
        /// This is the old Main Menu, displays:
        /// - All Records
        /// - New Record
        /// - Back to Main Menu
        /// </summary>
        static void HabitsMenu()
        {

        }

        /// <summary>
        /// This menu will contain the form to create a new tracked habit
        /// </summary>
        static void NewHabit()
        {
            ConsoleUtilities.Form newHabit = new ConsoleUtilities.Form((values) =>
            {
                values.ForEach(value => { Console.WriteLine(value); });
                Console.ReadLine();
            });

            newHabit.AddStringQuery("Enter the name of the new habit");
            newHabit.AddChoiceQuery("Select the type of record", "integer", "decimal", "text");
            newHabit.AddStringQuery("Enter the units of this record");

            newHabit.Start();

            MainMenu();
        }
    }
}
