using HabitLoggerLibrary.MansoorAZafar.Controllers;
using HabitLoggerLibrary.MansoorAZafar.Models;

namespace HabitLoggerLibrary.MansoorAZafar
{
    public class HabitManager
    {

        private DatabaseManager databaseManager;
        private Reports? reports;

        public HabitManager()
        {
            this.databaseManager = new DatabaseManager();
        }

        public void DoAction(HabitSelections chosenSelection)
        {
            Console.Clear();

            int id = -1;

            switch(chosenSelection)
            {
                case HabitSelections.update:
                    this.AssignValidID(ref id);
                    this.UpdateHabit(ref id);
                    
                    break;
                
                case HabitSelections.delete:
                    this.AssignValidID(ref id);
                    this.DeleteHabit(ref id);
                    
                    break;
                
                case HabitSelections.insert:
                    this.InsertHabit();
                    break;
                
                case HabitSelections.data:
                    
                    Console.WriteLine("Viewing all Data:\n");
                    this.databaseManager.PrintAllData();
                    Utilities.PressToContinue();
                    break;
                
                case HabitSelections.reports:
                    this.reports = new Reports(this.databaseManager);
                    this.reports.HandleReportSelection();
                    break;
            }
            System.Console.Clear();
        }

        private void AssignValidID(ref int id)
        {
            while(!this.databaseManager.IDExists(id))
            {
                Console.WriteLine("That ID doesn't exist!\n");
                Utilities.GetValidQuantity
                (
                    quantity: ref id,
                    message: "Please enter the ID\n> ",
                    errorMessage: "Invalid Answer\n Please Enter a valid ID\n> "
                );
            }
        }

        private bool EntriesExist()
        {
            return this.databaseManager.GetNumberOfEntries() > 0;
        }

        private void DeleteHabit(ref int id)
        {
            if (!this.EntriesExist())
            {
                Console.WriteLine("There are no Entries in the Database\n" +
                    "There is nothing to delete\n");

                Utilities.PressToContinue();
                return;
            }

            this.AssignValidID(ref id);
            this.databaseManager.Delete(id: ref id);
        }

        private void UpdateHabit(ref int id)
        {

            if (!this.EntriesExist())
            {
                Console.WriteLine("There are no Entries in the Database\nThere is nothing to update\n");
                Utilities.PressToContinue();
                return;
            }

            this.AssignValidID(ref id);
            string habitType = "";
            Utilities.GetValidStringInput
            (
                input: ref habitType,
                message: "Please enter the New Habit Type\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Habit Type\n> "
            );

            DateTime inputDate = DateTime.Now;
            Utilities.GetValidDateddMMyyFormat
            (
                date: ref inputDate,
                message: "Please enter the New Date\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Date\n> "
            );

            int quantity = 0;
            Utilities.GetValidQuantity(ref quantity);

            string units = "";
            Utilities.GetValidStringInput
            (
                input: ref units,
                message: "Please enter the New Units\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Units\n> "
            );

            this.databaseManager.Update
            (
                ID: ref id,
                habitType: ref habitType,
                date: ref inputDate,
                quantity: ref quantity,
                units: ref units
            );
        }

        private void InsertHabit()
        {
            string habitType = "";
            Utilities.GetValidStringInput
            (
                input: ref habitType,
                message: "Please enter the New Habit Type\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Habit Type\n> "
            );

            DateTime inputDate = DateTime.Now;
            Utilities.GetValidDateddMMyyFormat
            (
                date: ref inputDate,
                message: "Please enter the New Date\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Date\n> "
            );

            int quantity = 0;
            Utilities.GetValidQuantity(ref quantity);

            string units = "";
            Utilities.GetValidStringInput
            (
                input: ref units,
                message: "Please enter the New Units\n> ",
                errorMessage: "Invalid Answer\nPlease Enter a valid Units\n> "
            );

            this.databaseManager.Insert
            (
                habitType: ref habitType,
                date: ref inputDate,
                quantity: ref quantity,
                units: ref units
            );
        }
    }
}