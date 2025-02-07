using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{
    
    internal class InputOutputManager
    {
        private readonly string _format = "{0,-15:N}";
        private readonly int _IDcolumnWidth = 5;
        private readonly int _OtherColumnsWidth = 15;
        private readonly int _horizonalLineLength = 5 + 15 + 15 + 4; //5 for ID, 2x15 for Date and Volume, +4 horizontalLines
        public InputOutputManager() { }

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
            Console.Write($"Please enter value for Date: ");
            newRecord.Date = Console.ReadLine();
            Console.Write($"Please enter value for Volume: ");
            newRecord.Volume = Console.ReadLine();

            return newRecord;
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
        // returns number representing position of columns;
        public int GetRecordColumnForUpdate(List<string> columns)
        {
            Console.WriteLine("Please select column you with to update");
            Console.WriteLine("0.\tAll columns");
            for (int i = 1; i < columns.Count; i++) {
                Console.WriteLine($"{i}.\t{columns[i]}");
            }
            Console.Write("Please enter your choice: ");
            string? input = Console.ReadLine();
            while (!ValidateInput(input, 0, columns.Count-1)) { 
                Console.Write("Please enter VALID choice: ");
                input = Console.ReadLine();
            }

            return Convert.ToInt32(input);
        }
        public DatabaseRecord GetNewValuesForRecord(DatabaseRecord oldRecord)
        {
            DatabaseRecord newRecord = new DatabaseRecord(oldRecord.ID, oldRecord.Date, oldRecord.Volume);
            Console.Write("Enter new value for column Date: ");
            newRecord.Date = Console.ReadLine();
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
            Console.Write(string.Format(_format, record.Date));
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
