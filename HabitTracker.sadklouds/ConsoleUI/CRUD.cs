using ConsoleUI.Helpers;
using HabbitTrackerLibrary.sadklouds;

namespace ConsoleUI
{
    internal class CRUD
    {
        private readonly SqliteDataAccess _db;

        public CRUD(SqliteDataAccess db)
        {
            _db = db;
        }

        public void GetHabitRecords()
        {
            try
            {
                var habit = _db.LoadHabit();
                if (habit.Count != 0)
                {
                    Console.WriteLine($"Tracking Amount of water drunken");
                    Console.WriteLine("---------------------------------------");
                    foreach (var record in habit)
                    {
                        Console.WriteLine($"Index:{record.Id}, Date:{record.Date.ToShortDateString()}, Quantity:{record.Quantity}  Unit:{record.Unit}");
                        Console.WriteLine("---------------------------------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No record were found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nHabit could not be found!\n");
            }
        }
        public void UpdateHabitRecord()
        {
            int id = (int)UserInputsHelper.GetDoubleInput("Please enter record ID to update: ");
            string date = UserInputsHelper.GetHabitDateInput();
            double quantity = UserInputsHelper.GetDoubleInput("Enter quantity of habit: ");
            string unit = UserInputsHelper.GetStringInput("Enter unit of measure (KM, miles, number of glasses etc) ");
            string result = _db.UpdateRecord(id, date, quantity, unit);
            Console.WriteLine(result);
        }

        public void DeleteHabitRecord()
        {
            int id = (int)UserInputsHelper.GetDoubleInput("Please enter record ID for deletion: ");
            string result = _db.DeleteRecord(id);
            Console.WriteLine(result);
        }
        public void AddHabitRecord()
        {
            string date = UserInputsHelper.GetHabitDateInput();
            double quantity = UserInputsHelper.GetDoubleInput("Enter quantity of habit: ");
            string unit = UserInputsHelper.GetStringInput("Enter unit of measure (KM, miles, number of glasses etc) ");
            try
            {
                _db.InsertHabit(date, quantity, unit);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nError occured inserting Habit\n");
            }

        }

    }
}
