
namespace Habit_Tracker
{
    internal class TrackerEngine
    {
        internal DateOnly InputDrinkingDate()
        {
            DateOnly drinkingDate = default;
            bool validInput = false;

            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("Please input the date (YYYY-MM-DD):");
                if (DateOnly.TryParse(Console.ReadLine(), out drinkingDate))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            return drinkingDate;
        }

        internal int InputWaterGlassesQuantity()
        {
            int quantity = 0;
            bool validInput = false;

            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("Please input the number of water glasses on that day:");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive integer.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            return quantity;
        }

        readonly private string dbFile;
        readonly DatabaseManager DB;
        public TrackerEngine(string dbFile)
        {
            this.dbFile = dbFile;
            this.DB = new DatabaseManager(dbFile);            
        }
        internal void insertDrinkingWater()
        {
            DateOnly drinkingDate = InputDrinkingDate();
            int quantity = InputWaterGlassesQuantity();

            // Insert the drinking record into the database
            if (this.DB.InsertDrinkingWater(drinkingDate, quantity))
            {
                Console.WriteLine("The record was inserted!");
            }
            else
            {
                Console.WriteLine("Failed to insert the record.");
            }

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadLine();
        }

        internal void updateDrinkingWater()
        {
            DateOnly drinkingDate = InputDrinkingDate();
            int quantity = InputWaterGlassesQuantity();
                        
            // Update the drinking record in the database
            if (this.DB.UpdateDrinkingWater(drinkingDate, quantity))
            {
                Console.WriteLine("The record was updated!");
            }
            else
            {
                Console.WriteLine("Failed to update the record.");
            }

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadLine();
        }

        internal void deleteDrinkingWater()
        {            
            DateOnly drinkingDate = InputDrinkingDate();

            // delete the drinking water record in the database
            if (this.DB.DeleteDrinkingWater(drinkingDate))
            {
                Console.WriteLine("The record was deleted!");
            }
            else
            {
                Console.WriteLine("Failed to delete the record.");
            }

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadLine();
        }

        internal void retrieveAllRecords()
        {
            // retrieve the all drinking water records in the database
            this.DB.RetrieveAllRecords();            

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadLine();
        }
    }
}
