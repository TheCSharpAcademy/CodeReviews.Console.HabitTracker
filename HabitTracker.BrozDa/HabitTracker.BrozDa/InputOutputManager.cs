
using System.Dynamic;
using System.Globalization;
using System.Text.RegularExpressions;
namespace HabitTracker.BrozDa
{

    internal class InputOutputManager
    {
        private readonly string _dateFormat = "dd-MM-yyyy";
        public string DateTimeFormat { get; init; }

        private readonly string _format = "{0,-15:N}";
        private readonly int _IDcolumnWidth = 5;
        private readonly int _OtherColumnsWidth = 15;
        private readonly int _horizonalLineLength = 5 + 15 + 15 + 4; //5 for ID, 2x15 for Date and Volume, +4 horizontalLines
        public readonly int MainMenuLength = 5;
        public readonly int HabitMenuLength = 6;
        public InputOutputManager(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }
        public void PrintMainMenu()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker can be used to track custom habits");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. Check tracked habits");
            Console.WriteLine("\t2. Manage existing habit");
            Console.WriteLine("\t3. Create new habit");
            Console.WriteLine("\t4. Delete table");
            Console.WriteLine("\t5. Exit the application");
            Console.Write("Your selection: ");
        }
        public void PrintHabitMenu()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View all records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Exit to main menu");
            Console.WriteLine("\t6. Close the application");

            Console.Write("Your selection: ");

        }
        public DatabaseRecord GetValuesForNewRecord()
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.Date = GetDateForRecord();


            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        public DatabaseRecord GetValuesForNewRecord(int ID)
        {
            DatabaseRecord newRecord = new DatabaseRecord();

            newRecord.ID = ID;
            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        public int GetVolumeForRecord()
        {
            Console.Write($"Please enter positive numeric value for volume: ");

            int numericInput;
            string? input = Console.ReadLine();


            while (!int.TryParse(input, out numericInput) || numericInput < 0)
            {
                Console.Write("Enter valid, positive numeric  value: ");
                input = Console.ReadLine();
            }

            return numericInput;
        }
        public DateTime GetDateForRecord()
        {
            DateTime date = DateTime.Now;
            bool isInputValid = false;
            while (!isInputValid)
            {
                Console.Write($"Please enter date value in {_dateFormat.ToUpper()} format, or enter \"today\" for today's date: ");
                string? userInput = Console.ReadLine()?.Trim().ToLower();

                if (userInput == "today")
                {
                    date = DateTime.Now;
                    isInputValid = true;
                }
                else if (DateTime.TryParseExact(userInput, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    isInputValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }
            return date;
        }
        public string GetTableNameFromUser(List<string> tables)
        {
            string? input;
            int numericInput;

            Console.Write("Please enter ID of the table for new record (or press 0 to return): ");
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || numericInput < 0 || numericInput > tables.Count)
            {
                Console.Write("Please enter valid ID: ");
                input = Console.ReadLine();
            }

            return numericInput == 0 ? string.Empty : tables[numericInput - 1];
        }
        public int GetRecordIdFromUser(List<DatabaseRecord> records, string operation)
        {
            string? input;
            int numericInput;

            Console.Write($"Please valid ID of the record you wish to {operation}:");
            input = Console.ReadLine();

            while (!int.TryParse(input, out numericInput) || !IsRecordIdPresent(numericInput, records))
            {
                Console.Write($"Please valid ID of the record you wish to {operation}:");
                input = Console.ReadLine();
            }
            return numericInput;
        }
        private bool IsRecordIdPresent(int id, List<DatabaseRecord> records)
        {
            foreach (DatabaseRecord record in records)
            {
                if (record.ID == id)
                {
                    return true;
                }
            }
            return false;
        }
        public DatabaseRecord GetNewValuesForExistingRecord(DatabaseRecord oldRecord)
        {
            DatabaseRecord newRecord = new DatabaseRecord(oldRecord.ID, oldRecord.Date, oldRecord.Volume);

            newRecord.Date = GetDateForRecord();
            newRecord.Volume = GetVolumeForRecord();

            return newRecord;
        }
        public void PrintTable(string table, List<DatabaseRecord> records, string unit)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            PrintHorizonalLine();
            PrintTableHeader(table);
            PrintHorizonalLine();
            PrintTableColumns();
            PrintHorizonalLine();
            foreach (DatabaseRecord record in records) 
            { 
                PrintRecord(record, unit);
            }
            PrintHorizonalLine();

        }
        public void PrintTableColumns()
        {
            Console.Write('|');
            Console.Write(string.Format("{0,-5:N}", "ID"));
            Console.Write('|');
            Console.Write(string.Format(_format, "Date"));
            Console.Write('|');
            Console.Write(string.Format(_format, "Volume"));
            Console.Write('|');
            Console.WriteLine();
        }
        
        public void PrintTableColumns(List<string> columns, string table)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            PrintTableHeader(table);
            PrintHorizonalLine();

            //column names
            Console.Write('|');
            Console.Write(string.Format("{0,-5:N}", columns[0]));
            Console.Write('|');
            Console.Write(string.Format(_format, columns[1]));
            Console.Write('|');
            Console.Write(string.Format(_format, columns[2]));
            Console.Write('|');
            Console.WriteLine();

            PrintHorizonalLine();
        }
        public void PrintRecord(DatabaseRecord record, string unit) {

            Console.Write('|');
            Console.Write(string.Format("{0,-5}", record.ID));
            Console.Write('|');
            Console.Write(string.Format(_format, record.Date.ToString(_dateFormat)));
            Console.Write('|');
            Console.Write(string.Format(_format, record.Volume + " " + unit));
            Console.WriteLine('|');

        }
        public void PrintHorizonalLine()
        {
            Console.WriteLine(new string('-', _horizonalLineLength));
        }
        private void PrintTableHeader(string table)
        {
            string text = $"Table: {table}";
            int totalSpaces = (_horizonalLineLength - 2 - text.Length);
            int leftpadding = totalSpaces / 2;
            int rightPadding = totalSpaces - leftpadding;

            
            Console.WriteLine("|" + new string(' ', leftpadding) + text + new string(' ', rightPadding) + "|");
        }
        public int GetInputInMenu(int maxValue)
        {
            string? input = Console.ReadLine();
            int numericInput;

            while (!int.TryParse(input, out numericInput) || numericInput < 1 || numericInput > maxValue)
            {
                Console.Write("Please enter valid value: ");
                input = Console.ReadLine();
            }

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
            Console.Write("Please enter name of the table to be deleted (or enter 0 to return to main menu): ");
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
            if(name == null || name.Length == 0)
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
        
        public void PrintTables(List<string> listOfTables)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Currently tracked habits: ");
            if (listOfTables == null || listOfTables.Count == 0)
            {
                Console.WriteLine("There are not tables in database yet");
            }
            else
            {
                for (int i = 0; i < listOfTables.Count; i++)
                {
                    Console.WriteLine($"\t{i + 1}: {listOfTables[i]}");
                    

                }
            }
            Console.WriteLine();
        }

        
    }
}
