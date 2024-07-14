namespace IbraheemKarim.HabitLogger
{
    public static class ActionsProcessor
    {
        public static void ProcessActionType(ActionType action)
        {                                                                             
            switch (action)
            {
                case ActionType.AddNewRecord:
                    Console.Clear();
                    AddNewDatabaseRowFromUserInput();
                    HelperMethods.PauseAppUntilUserPressesAkey();
                    break;
                case ActionType.DeleteRecord:
                    Console.Clear();
                    DeleteDatabaseRowAccordingToUserInput();
                    HelperMethods.PauseAppUntilUserPressesAkey();
                    break;
                case ActionType.ShowAllPreviousRecords:
                    Console.Clear();
                    DatabaseOperations.PrintAllRows();
                    HelperMethods.PauseAppUntilUserPressesAkey();
                    break;
                case ActionType.UpdateRecord:
                    Console.Clear();
                    UpdateDatabaseRowAccordingToUserInput();
                    HelperMethods.PauseAppUntilUserPressesAkey();
                    break;
                case ActionType.ExitApp:
                    ExitTheApp();
                    break;
            }
        }

        private static void AddNewDatabaseRowFromUserInput()
        {
            var quantity = HelperMethods.GetPositiveIntegerFromUser("quantity");
            var date = HelperMethods.GetDateFromUser();
            DatabaseOperations.InsertRecord(quantity,date);
        }

        private static void DeleteDatabaseRowAccordingToUserInput()
        {
            DatabaseOperations.PrintAllRows();
            Console.WriteLine();
            var rowId = HelperMethods.GetPositiveIntegerFromUser("the ID of the row to be deleted");
            
            if(IsValidRowId(rowId))
                DatabaseOperations.DeleteRecord(rowId);
            else
                Console.WriteLine("\nThis row ID doesn't exist");
        }             

        private static void UpdateDatabaseRowAccordingToUserInput()
        {
            DatabaseOperations.PrintAllRows();
            Console.WriteLine();
            var rowId = HelperMethods.GetPositiveIntegerFromUser("the ID of the row to be updated");

            if (IsValidRowId(rowId))
            {
                Console.Clear();
                var quantity = HelperMethods.GetPositiveIntegerFromUser("quantity");
                var date = HelperMethods.GetDateFromUser();
                DatabaseOperations.UpdateRecord(rowId, quantity, date);
            }
            else
            {
                Console.WriteLine("\nThis row ID doesn't exist");
            }
        }      
        
        private static void ExitTheApp()
        {
            Console.Clear();
            Console.WriteLine("\nGoodbye!");
            Environment.Exit(0);
        }

        private static bool IsValidRowId(int rowId)
        {
            HashSet<int> availableIds = DatabaseOperations.GetAvailableRowIds();
            return availableIds.Contains(rowId);
        }
    }
}
