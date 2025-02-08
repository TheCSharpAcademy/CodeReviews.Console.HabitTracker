
using System.Globalization;
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
        public InputOutputManager(string dateTimeFormat) 
        {
            DateTimeFormat = dateTimeFormat;
        }
        public void PrintHabitMenu()
        {
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker tracks number of glasses of water drank during the day");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View all records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Close the application");

            Console.Write("Your selection: ");

        }
        public void PrintRecords(List<string> columns, List<DatabaseRecord> records)
        {
            int verticalLineLength = _IDcolumnWidth + (columns.Count - 1) * _OtherColumnsWidth + columns.Count + 1;

            Console.Clear();
            Console.SetCursorPosition(0, 0);


            //horizonal line
            Console.WriteLine(new string('-', verticalLineLength));


            //column names
            Console.Write('|');
            Console.Write(string.Format("{0,-5:N}",columns[0]));
            Console.Write('|');
            Console.Write(string.Format(_format, columns[1]));
            Console.Write('|');
            Console.Write(string.Format(_format, columns[2]));
            Console.Write('|');
            Console.WriteLine();

            //horizonal line
            Console.WriteLine(new string('-', verticalLineLength));

            // records
            foreach (var record in records)
            {
                Console.Write('|');
                Console.Write(string.Format("{0,-5}", record.ID));
                Console.Write('|');
                Console.Write(string.Format(_format, record.Date));
                Console.Write('|');
                Console.Write(string.Format(_format, record.Volume));
                Console.WriteLine('|');
            }

            //horizonal line
            Console.WriteLine(new string('-', verticalLineLength));

        }
        public DatabaseRecord GetNewRecord(string table)
        {
            DatabaseRecord newRecord = new DatabaseRecord();
            Console.WriteLine();
            //Console.Write($"Please enter date value in DD-MM-YYYY format, or enter \"today\" for today's date: ");
            newRecord.Date = GetDateForRecord();

            Console.Write($"Please enter value for Volume: ");
            newRecord.Volume = Console.ReadLine();

            return newRecord;
        }
        public DateTime GetDateForRecord()
        {
            DateTime date = DateTime.Now;

            while (true)
            {
                Console.Write($"Please enter date value in DD-MM-YYYY format, or enter \"today\" for today's date: ");
                string? userInput = Console.ReadLine()?.Trim().ToLower();

                if (userInput == null || userInput == string.Empty)
                {
                    Console.WriteLine("Invalid input. Please enter a valid date.");
                }
                else if (userInput == "today")
                {
                    date = DateTime.Now;
                    break;
                }
                else if (DateTime.TryParseExact(userInput, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }

            return date;
        }

        public int GetRecordIdFromUser(string operation)
        {
            Console.Write($"Please valid ID of the record you wish to {operation}:");
            string? input = Console.ReadLine();
            int numericInput;
            while(!int.TryParse(input, out numericInput))
            {
                Console.Write($"Please valid ID of the record you wish to {operation}:");
                input = Console.ReadLine();
            }
            return numericInput;
        }
        public DatabaseRecord GetNewValuesForRecord(DatabaseRecord oldRecord)
        {
            DatabaseRecord newRecord = new DatabaseRecord(oldRecord.ID, oldRecord.Date, oldRecord.Volume);
            newRecord.Date = GetDateForRecord();
            Console.Write("Enter new value for column Glasses: ");
            newRecord.Volume = Console.ReadLine();

            return newRecord;
        }
       
        public bool ValidateInput(string? input, int minVal, int maxVal) 
        {
            int numInput;

            if (!int.TryParse(input, out numInput))
            {
                return false;
            }
            if (numInput < minVal) 
            {
                return false;
            }
            if (numInput > maxVal) 
            {
                return false;
            }

            return true;
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
        public void PrintRecord(DatabaseRecord record) {

            Console.Write('|');
            Console.Write(string.Format("{0,-5}", record.ID));
            Console.Write('|');
            Console.Write(string.Format(_format, record.Date.ToString(_dateFormat)));
            Console.Write('|');
            Console.Write(string.Format(_format, record.Volume));
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

            PrintHorizonalLine();
            Console.WriteLine("|" + new string(' ', leftpadding) + text + new string(' ', rightPadding) + "|");
        }
        
        
    }
}
