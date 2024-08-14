using HabitTracker.Jackua.Model;

namespace HabitTracker.Jackua.View
{
    public class MenuView
    {
        public static void MainMenu()
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Records");
            Console.WriteLine("Type 3 to Delete Record");
            Console.WriteLine("Type 4 to Update Record");
            DashLines();
        }
        public static void InvalidCommand()
        {
            Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
        }

        public static void DisplayDrinkingWater(DrinkingWaterModel dw)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
        }

        public static void DoesNotExist(int recordId)
        {
            Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
        }

        public static void DateRequest()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yy). Type 0 to return to main menu.");
        }

        public static void InvalidDate()
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Try again or Type 0 to return to main menu.\n\n");
        }

        public static void InvalidNumber()
        {
            Console.WriteLine("\n\nInvalid number. Try again.\n\n");
        }

        public static void Deleted(int recordId)
        {
            Console.WriteLine($"\n\nRecord with Id{recordId} was deleted. \n\n");
        }

        public static void DashLines()
        {
            Console.WriteLine("----------------------------------------------------------------\n");
        }

        public static void NoRows()
        {
            Console.WriteLine("No rows found");
        }

        public static void GoodBye()
        {
            Console.WriteLine("\nGoodbye!\n");
        }
    }
}
