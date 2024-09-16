using System.Globalization;

namespace HabitTrackerLibrary
{
    public class Validator
    {
        public static readonly List<string> mainMenuChoices = ["c", "v", "u", "i", "d", "r", "exit"];
        public static readonly List<string> reportsMenuChoices = ["1", "2", "3", "4", "menu"];
        public static string ValidateMenuChoice(List<string> choices)
        {
            string? choice = Console.ReadLine();
            while (choice is null || !choices.Contains(choice.ToLower().Trim()))
            {
                Console.WriteLine($"Please choose a valid option from the menu ({String.Join(", ", choices)}). You entered {choice}.");
                choice = Console.ReadLine();
            }
            return choice;
        }
        public static string ValidateHabitChoice()
        {
            int choice;
            string? readResult = Console.ReadLine();
            while (!int.TryParse(readResult, out choice) || choice < 0 || choice > DatabaseQueries.currentTables.Count)
            {
                Console.WriteLine($"Please choose a valid index number [0-{DatabaseQueries.currentTables.Count - 1}]\n. You entered {readResult}.");
                readResult = Console.ReadLine();
            }
            Console.WriteLine($"\nYou are now viewing Habit: {DatabaseQueries.currentTables[choice]}\n");
            return DatabaseQueries.currentTables[choice];
        }
        public static void ValidateColumnsChoice()
        {
            int counter = 0;
            bool validInput = false;
            while (!validInput)
            {
                List<string> input = SplitAndTrimMultipleValues();

                for (int i = 0; i < input.Count; i++)
                {
                    if (DatabaseQueries.currentTableInfo.ContainsKey(input[i].Trim()))
                    {
                        Options.entryToUpdate.Add(input[i], "");
                        counter++;
                    }
                }                
                if (counter > 0)
                {
                    validInput = true;
                }        
                else
                    Console.WriteLine("\nValues entered do not match columns of this table.\n" +
                        "Please enter valid column names in a comma separated format 'column1, column2'.\n");
            }
        }
        public static int ValidateIdChoice(int rowCount)
        {
            int choice;
            string? readResult = Console.ReadLine();
            while (!int.TryParse(readResult, out choice) || choice < 0 || choice > (rowCount - 1))
            {
                Console.WriteLine($"Please choose a valid occurrenceId (0 - {rowCount - 1})\n. You entered {readResult}.");
                readResult = Console.ReadLine();
            }
            Console.WriteLine($"\nYou are now updating record with occurrenceId {choice}.\n");
            return choice;
        }
        public static int ValidateIntValueToUpdate()
        {
            int value;
            string? readResult = Console.ReadLine();
            while (!int.TryParse(readResult, out value))
            {
                Console.WriteLine($"Please choose a valid integer value ('43').\nYou entered {readResult}.");
                readResult = Console.ReadLine();
            }
            return value;
        }
        public static DateTime ValidateDateTimeValue()
        {
            DateTime dateValue = DateTime.Now;
            if (!ChooseNowAsDate())
            {  
                string? readResult = Console.ReadLine();
                while (!DateTime.TryParseExact(readResult, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateValue))
                {
                    Console.WriteLine($"Please choose a valid DateTime value ('2011-03-21 13:26').\nYou entered {readResult}.");
                    readResult = Console.ReadLine();
                }
            }
            return dateValue;
        }
        public static List<string> SplitAndTrimMultipleValues()
        {
            List<string> inputList = [];

            string? readResult = Console.ReadLine();
            if (readResult is not null)
                inputList = readResult.Split(",").ToList();

            for (int i = 0; i < inputList.Count; i++){
                inputList[i] = inputList[i].Trim();
            }
            return inputList;
        }
        public static void ValidateMultipleIntValuesToDelete()
        {
            List<string> entryList = SplitAndTrimMultipleValues();

            foreach (string entry in entryList)
            {
                if (int.TryParse(entry, out int value))
                {
                    Options.entryToDelete.Add(entry);
                }
                else
                {
                    Console.WriteLine($"Entry {entry} is not a number and will not be included in the deletion query.");
                }
            }
        }
        public static void ValidateMultipleDateTimeValuesToDelete()
        {
            List<string> entryList = SplitAndTrimMultipleValues();
            foreach (string entry in entryList)
            {
                if (DateTime.TryParseExact(entry, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                {
                    Options.entryToDelete.Add(entry);
                }
                else
                {
                    Console.WriteLine($"Entry {entry} is not a date and will not be included in the deletion query.");
                }
            }
        }
        public static void ValidateWithinRange(int maxRange)
        {
            for (int i = 1; i < Options.entryToDelete.Count; i++)
            {
                if (Convert.ToInt32(Options.entryToDelete[i]) > (maxRange - 1))
                {
                    Options.entryToDelete.RemoveAt(i);
                }
            }    
        }
        private static bool ChooseNowAsDate()
        {
            Console.WriteLine("\nDo you want to use the current time as a DateTime input?\nPlease enter 'now', if yes. If not, please press any key, then enter a date.");
            string? readResult = Console.ReadLine();
            if (readResult is not null && readResult.Equals("now"))
                return true;
            else
                return false;
        }
    }
}
