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
            Console.WriteLine("Type 5 to View All Habits");
            Console.WriteLine("Type 6 to Insert Habit");
            Console.WriteLine("Type 7 to Delete Habit");
            Console.WriteLine("Type 8 to Update Habit");
            DashLines();
        }
        public static void InvalidCommand()
        {
            Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
        }

        public static void DisplayModel(RecordModel rm)
        {
            Console.WriteLine($"{rm.RecordId} - {rm.HabitName, 14} - {rm.Date.ToString("dd-MMM-yyyy")} - Quantity: {rm.Quantity}");
        }

        public static void DisplayModel(HabitModel hm)
        {
            Console.WriteLine($"{hm.HabitId} - {hm.HabitName}");
        }

        public static void DoesNotExist(int id, string type)
        {
            Console.WriteLine($"\n\n{type} with Id {id} doesn't exist. \n\n");
        }

        public static void DateRequest()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-MM-yy). Type 0 to return to main menu.");
        }

        public static void QuantityRequest()
        {
            Console.WriteLine("\n\nPlease insert number of glasses or other measurement of your choice (no decimals allowed)");
        }

        public static void HabitRequest()
        {
            Console.WriteLine("\n\nPlease insert the name of the habit. Type 0 to return to main menu.");
        }

        public static void UpdateId()
        {
            Console.WriteLine("\n\nPlease type the Id of the record you would like to update. Type 0 to return to main menu.");
        }

        public static void DeleteId(string type)
        {
            Console.WriteLine($"\n\nPlease type the Id of the {type} you want to delete. Type 0 to return to main menu.");
        }

        public static void InvalidDate()
        {
            Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Try again or Type 0 to return to main menu.");
        }

        public static void InvalidNumber()
        {
            Console.WriteLine("\n\nInvalid number. Try again.");
        }

        public static void Deleted(int id, string type)
        {
            Console.WriteLine($"\n\n{type} with Id {id} was deleted. \n\n");
        }

        public static void ForeignKey(int id)
        {
            Console.WriteLine($"\n\nUnable to delete Id {id}, there are still records using this habit.");
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
