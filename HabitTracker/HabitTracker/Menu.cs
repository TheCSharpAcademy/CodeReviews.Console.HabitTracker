using System;

namespace HabitTracker
{
    internal class Menu
    {
        internal void ProjectMenu()
        {
            var isGameOn = true;
            do
            {
                Console.Clear();
                Console.WriteLine($@"Hello What would you like to do today? Choose from options below:
                    0-ViewRecords
                    1-InsertRecord
                    2-DeleteRecord
                    3-UpdateRecord
                    4-Exit Application");
                Console.WriteLine("-------------------------------------------");

                var selection = Console.ReadLine().ToLower().Trim();

                switch (selection)
                {
                    case "0":
                        App.ViewRecords();
                        break;
                    case "1":
                        App.InsertRecord(); break;
                    case "2":
                        App.DeleteRecord(); break;
                    case "3":
                        App.UpdateRecord(); break;
                    case "4":
                        Console.WriteLine("Goodbye");
                        isGameOn = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Input.Press any key to continue.");
                        Console.ReadLine();
                        break;
                }

            } while (isGameOn);
        }
    }
}
