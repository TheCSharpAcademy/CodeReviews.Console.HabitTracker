using HabitLoggerLibrary.MansoorAZafar;
using HabitLoggerLibrary.MansoorAZafar.Models;

namespace HabitLogger.MansoorAZafar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1. Show a Menu 
            //2. Let user select an option
            //3. Show Result of said option
            //4. Loop till they select end

            bool endProgram = false;
            HabitManager habits = new();
            HabitSelections choiceSelection = HabitSelections.None;


            while (!endProgram)
            {
                Menu();
                GetAndValidateInput(ref choiceSelection);
                if (choiceSelection == HabitSelections.exit)
                {
                    endProgram = true;
                    continue;
                }
                habits.DoAction(choiceSelection);
            }


            System.Console.WriteLine("\nGoodbye!\n");

        }

        public static void Menu()
        {
            System.Console.Clear();
            System.Console.WriteLine
            (
                "*".PadRight(29, '*')
                + "\n"
                + "Input your Selection,\nEither the number or the action (exit, update, delete, insert, data, reports)\n\n"
                + "0:   to exit\n"
                + "1:   to update hours\n"
                + "2:   to delete a date\n"
                + "3:   to insert a date\n"
                + "4:   to view data\n"
                + "5:   to view reports\n"
                + "*".PadRight(29, '*')
            );
        }

        public static void GetAndValidateInput(ref HabitSelections selection, string message = "> ", string errorMessage = "\nInvalid Selection, Please select a Valid input\n> ")
        {
            Console.Write(message);
            while (!(Enum.TryParse(System.Console.ReadLine()?.ToLower(), out selection))
                || selection == HabitSelections.None
                || !Enum.IsDefined(typeof(HabitSelections), selection))
                Console.Write(errorMessage);
        }
    }
}
