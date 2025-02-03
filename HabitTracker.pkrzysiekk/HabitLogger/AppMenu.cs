

namespace HabitLogger
{
    internal class AppMenu
    {
        public static void ShowMenu()
        {
            Console.Clear();

            bool closeApp = false;

            while (!closeApp)
            {
                Console.WriteLine("\n\nMenu:");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close the app");
                Console.WriteLine("Type 1 to View all Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete Record");
                Console.WriteLine("Type 4 to Update Record");
                Console.WriteLine("Type 5 to sum all water you've drunk");
                Console.WriteLine("Type 6 to sum all water you've drunk in the last 7 days");
                Console.WriteLine("----------------------------\n");

                string? commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        DBController.GetAllRecords();
                        break;
                    case "2":
                        DBController.InsertData();
                        break;
                    case "3":
                        DBController.Delete();
                        break;
                    case "4":
                        DBController.Update();
                        break;
                    case "5":
                        DBController.SumWater();
                        break;
                    case "6":
                        DBController.LastWeekData();
                        break;


                }
            }
        }
    }
}
