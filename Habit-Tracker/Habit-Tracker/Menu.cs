using Habit_Tracker;

namespace Habit_Tracker
{
    internal class Menu
    {
        private readonly TrackerEngine engine;

        public Menu(string dbFile)
        {
            engine = new TrackerEngine(dbFile);
        }        

        internal void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");            

            bool isContinueOn = true;

            do
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Input your command");
                Console.WriteLine("\n");
                Console.WriteLine("0 to exit");
                Console.WriteLine("I to input the record");
                Console.WriteLine("U to update the record");
                Console.WriteLine("D to delete the record");
                Console.WriteLine("V to view the all records");
                Console.WriteLine("-------------------------------------");

                var userSelected = Console.ReadLine();

                Console.Clear();
                switch (userSelected.ToUpper().Trim())
                {
                    case "0":
                        isContinueOn = false;
                        break;
                    case "I":
                        engine.insertDrinkingWater();
                        break;
                    case "U":
                        engine.updateDrinkingWater();
                        break;
                    case "D":
                        engine.deleteDrinkingWater();
                        break;                    
                    case "V":
                        engine.retrieveAllRecords();
                        break;
                    default:
                        Console.WriteLine("Incorrect input, try again.");
                        break;
                }
                Console.WriteLine("-------------------------------------");

            } while (isContinueOn);
        }
    }
}
