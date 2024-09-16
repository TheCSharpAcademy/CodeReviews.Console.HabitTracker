namespace HabitTrackerLibrary
{
    public class Options
    {
        public static Dictionary<string, string> entryToUpdate = [];
        public static List<string> entryToDelete = [];
        static DatabaseQueries databaseQueries = new DatabaseQueries();
        public static void CreateHabit()
        {
            Console.WriteLine("Please provide a name for your habit: \n");
            string habitName = Console.ReadLine() ?? "Habit name";
            Console.WriteLine("Please choose a unit of measurement for your habit: \n");
            string chosenMeasurement = Console.ReadLine() ?? "Distance";
            databaseQueries.CreateHabitQuery(habitName, chosenMeasurement);
        }
        public static void ViewHabit()
        {  
            databaseQueries.GetCurrentTables();
            Console.WriteLine("Which habit do you want to view?\n");
            ListAllHabits();
            databaseQueries.ViewHabitQuery(Validator.ValidateHabitChoice());
        }
        public static void UpdateHabit()
        {
            string habitChoice = ChooseHabitToEdit();

            Console.WriteLine("Which columns do you want to update? This table consists of: \n");
            databaseQueries.GetTableInfo(habitChoice);
            
            Console.WriteLine("\nPlease use a comma separated format 'column1, column2'.\n");
            Validator.ValidateColumnsChoice();

            int rowCount = databaseQueries.GetRowCount(habitChoice);
            Console.WriteLine($"Which entry would you like to update? Please specify occurrenceId (0 - {rowCount - 1}): \n");
            int occurrenceId = Validator.ValidateIdChoice(rowCount);

            foreach (string entry in entryToUpdate.Keys)
            {
                if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entry) == "INT")
                {
                    Console.WriteLine($"Please provide a value (integer '23') to update an entry in column {entry} at row {occurrenceId}: ");
                    entryToUpdate[entry] = (Validator.ValidateIntValueToUpdate()).ToString();
                }
                else if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entry) == "TEXT" && entry.ToLower().Contains("date"))
                {
                    Console.WriteLine($"Please provide a value (DateTime 'yyyy-MM-dd hh-mm') to update an entry in column {entry} at row {occurrenceId}: ");
                    entryToUpdate[entry] = (Validator.ValidateDateTimeValue()).ToString("yyyy-MM-dd HH:mm");
                }
            }

            databaseQueries.UpdateHabitQuery(habitChoice, entryToUpdate, occurrenceId);
            entryToUpdate.Clear();
        }
        public static void InsertIntoHabit()
        {
            string habitChoice = ChooseHabitToEdit();
            databaseQueries.GetTableInfo(habitChoice);

            int rowCount = databaseQueries.GetRowCount(habitChoice);
            entryToUpdate.Add("occurrenceId", rowCount.ToString());

            foreach (string entry in DatabaseQueries.currentTableInfo.Keys)
            {
                if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entry) == "INT" && entry != "occurrenceId")
                {
                    Console.WriteLine($"Please provide a value (integer '23') to insert an entry in column {entry} at row {rowCount}: ");
                    entryToUpdate.Add(entry, (Validator.ValidateIntValueToUpdate()).ToString());
                }
                else if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entry) == "TEXT" && entry.ToLower().Contains("date"))
                {
                    Console.WriteLine($"Please provide a value (DateTime 'yyyy-MM-dd HH:mm') to insert an entry in column {entry} at row {rowCount}: ");
                    entryToUpdate.Add(entry, (Validator.ValidateDateTimeValue()).ToString("yyyy-MM-dd HH:mm"));
                }
            }

            databaseQueries.InsertIntoHabitQuery(habitChoice, entryToUpdate, rowCount);
            entryToUpdate.Clear();
        }
        public static void DeleteFromHabit()
        {
            string habitChoice = ChooseHabitToEdit();
            databaseQueries.GetTableInfo(habitChoice);
            int rowCount = databaseQueries.GetRowCount(habitChoice);

            Console.WriteLine("Based on a value from which column would you like to delete records?\n");
            Validator.ValidateColumnsChoice();

            entryToDelete.Add(entryToUpdate.Keys.ElementAt(0));

            if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entryToDelete[0]) == "INT" && entryToDelete[0] != "occurrenceId")
            {
                Console.WriteLine($"Please provide values separated by commas (integer '23') to delete all rows that have a matching value in column {entryToDelete[0]}.");
                Validator.ValidateMultipleIntValuesToDelete();
            }
            else if (DatabaseQueries.currentTableInfo.GetValueOrDefault(entryToDelete[0]) == "TEXT" && entryToDelete[0].ToLower().Contains("date"))
            {
                Console.WriteLine($"Please provide values separated by commas (DateTime 'yyyy-MM-dd HH:mm') to delete all rows that have a matching value in column {entryToDelete[0]}.");
                Validator.ValidateMultipleDateTimeValuesToDelete();
            }
            else if (entryToDelete[0] == "occurrenceId")
            {
                Console.WriteLine($"Please provide values separated by commas (integer '23') to delete the rows that have a matching value (0 - {rowCount - 1}) in column {entryToDelete[0]}.");
                Validator.ValidateMultipleIntValuesToDelete();
                Validator.ValidateWithinRange(rowCount);
            }
            else
            {
                Console.WriteLine($"Please provide values separated by commas to delete the rows that have a matching value in column {entryToDelete[0]}.");
                foreach (string entry in Validator.SplitAndTrimMultipleValues())
                {
                    entryToDelete.Add(entry);
                }            
            }
            if (entryToDelete.Count > 0) {
                databaseQueries.DeleteFromQuery(habitChoice, entryToDelete, rowCount);
            }
            entryToDelete.Clear();
            entryToUpdate.Clear();
        }
        public static void GenerateReportTotal()
        {
            string habitChoice = ChooseHabitToEdit();
            (DateTime startDate, DateTime endDate) = ChooseTimespan();
            databaseQueries.GenerateReportTotalQuery(habitChoice, startDate, endDate);
        }
        public static void GenerateReportTotalAmount()
        {
            string habitChoice = ChooseHabitToEdit();
            (DateTime startDate, DateTime endDate) = ChooseTimespan();
            databaseQueries.GenerateReportTotalAmountQuery(habitChoice, startDate, endDate);
        }
        public static void GenerateReportAverage()
        {
            string habitChoice = ChooseHabitToEdit();
            (DateTime startDate, DateTime endDate) = ChooseTimespan();
            databaseQueries.GenerateReportAverageQuery(habitChoice, startDate, endDate);
        }
        public static void GenerateReportFiveGreatest()
        {
            string habitChoice = ChooseHabitToEdit();
            databaseQueries.GenerateRaportFiveGreatestQuery(habitChoice);
        }
        private static string ChooseHabitToEdit()
        {
            databaseQueries.GetCurrentTables();
            Console.WriteLine("Which habit do you want to access?\n");
            ListAllHabits();
            string habitChoice = Validator.ValidateHabitChoice();
            return habitChoice;
        }
        private static (DateTime, DateTime) ChooseTimespan()
        {
            Console.WriteLine("Please provide a start date (yyyy-MM-dd HH:mm): ");
            DateTime startDate = Validator.ValidateDateTimeValue();
            Console.WriteLine("Please provide an end date (yyyy-MM-dd HH:mm): ");
            DateTime endDate = Validator.ValidateDateTimeValue();
            if (endDate <= startDate)
            {
                Console.WriteLine("End date must be later than start date.");
                endDate = Validator.ValidateDateTimeValue();
            }
            return (startDate, endDate);
        }
        private static void ListAllHabits()
        {
            databaseQueries.GetCurrentTables();
            for (int i = 0; i < DatabaseQueries.currentTables.Count; i++)
            {
                Console.WriteLine($"\t[{i}]. {DatabaseQueries.currentTables[i]}\n");
            }
            Console.WriteLine($"Please choose a corresponding index number [0-{DatabaseQueries.currentTables.Count - 1}]\n");
        }
    }
}
