
using System.Globalization;
using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{

    internal class InputManager
    {
        public string DateTimeFormat { get; init; }
        public InputManager(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }

        public DatabaseRecord GetValuesForRecord()
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.Date = GetDateForRecord();


            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        public DatabaseRecord GetValuesForRecord(int ID)
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.ID = ID;
            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        public int GetVolumeForRecord()
        {

            string prompt = "Please enter positive numeric value for volume: ";
            int numericInput = GetValidIntegerInput(prompt, 0, int.MaxValue);

            return numericInput;
        }
        private int GetValidIntegerInput(string prompt, int minValue, int maxValue)
        {
            int numericInput;
            string? input;

            Console.Write(prompt);
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || numericInput < minValue || numericInput > maxValue)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }

            return numericInput;
        }
        private int GetValidIntegerInput(string prompt, HashSet<int> IDs)
        {
            int numericInput;
            string? input;

            Console.Write(prompt);
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || (!IDs.Contains(numericInput) && numericInput != 0))
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }

            return numericInput;
        }
        public DateTime GetDateForRecord()
        {
            DateTime date;
            while (true)
            {
                Console.Write($"Please enter date value in {DateTimeFormat.ToUpper()} format, or enter \"today\" for today's date: ");

                string? userInput = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                    continue;
                }
                else if (userInput == "today")
                {
                    return DateTime.Now;
                }
                else if (DateTime.TryParseExact(userInput, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }

                Console.WriteLine("Invalid date format. Please try again.");
            }
        }

        public string GetTableNameById(List<string> tables)
        {
            string prompt = "Please enter ID of the table for new record (or press 0 to return): ";
            int numericInput = GetValidIntegerInput(prompt,0,tables.Count);

            return numericInput == 0 ? string.Empty : tables[numericInput - 1];
        }
        public int GetRecordIdFromUser(List<DatabaseRecord> records, string operation)
        {
            int numericInput;
            HashSet<int> IDs = new HashSet<int>(records.Select(record => record.ID));

            string prompt = $"Please valid ID of the record you wish to {operation}, or enter \"0\" to return to menu:";

            numericInput = GetValidIntegerInput(prompt, IDs);
            return numericInput;
        }
        public int GetInputInMenu(int maxValue)
        {
            
            int numericInput = GetValidIntegerInput("", 1, maxValue);
            return numericInput;
        }
        public string GetNewTableName(List<string> existingTables)
        {
            string? name;
            Console.WriteLine("Name of the habit can contain only alpha-numeric characters or space between words, leading or trailing spaces are not permitted");
            Console.Write("Please enter habit name: ");
            name = Console.ReadLine();
            while (!IsTableNameValid(name) || existingTables.Contains(name))
            {
                Console.WriteLine("Name invalid or already exists");
                Console.Write("Please enter valid name: ");
                name = Console.ReadLine();

            }
            return name;

        }
        public string GetExistingTableName(List<string> existingTables)
        {
            string? name;
            Console.Write("Please enter name of the table to be deleted (or enter \"0\" to return to main menu): ");
            name = Console.ReadLine();
            while (!existingTables.Contains(name) && name != "0")
            {
                Console.Write("Please enter valid name (or enter 0 to return to main menu): ");
                name = Console.ReadLine();

            }
            return name;

        }
        private bool IsTableNameValid(string name)
        {
            if (name == null || name.Length == 0)
                return false;

            return Regex.IsMatch(name, @"^[a-zA-Z0-9]+( [a-zA-Z0-9]+)*$");
        }
        public string GetNewTableUnit()
        {
            string? name;
            Console.Write("Please enter measurement unit: ");
            name = Console.ReadLine();

            while (name == null)
            {
                Console.Write("Please enter valid unit");
            }

            return name;
        }
    }
}
