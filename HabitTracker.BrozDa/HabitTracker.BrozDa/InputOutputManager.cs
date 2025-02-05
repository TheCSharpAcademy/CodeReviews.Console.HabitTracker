using System.Data.Common;
using System.Text.RegularExpressions;

namespace HabitTracker.BrozDa
{
    
    internal class InputOutputManager
    {
        private readonly string _format = "{0,-15:N}";
        private readonly int _IDcolumnWidth = 5;
        private readonly int _OtherColumnsWidth = 15;
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

            Console.Write($"Please enter value for Date: ");
            newRecord.Date = Console.ReadLine();
            Console.Write($"Please enter value for Volume: ");
            newRecord.Volume = Console.ReadLine();

            return newRecord;
        }
        public int GetRecordIdForUpdate(List<DatabaseRecord> records)
        {

            Console.Write("Please enter ID of the record you wish to update:");
            string? input = Console.ReadLine();
            // MISSING INPUT VERIFICATION CODE
            while (!ValidateInput(input, 1, records.Count))
            {
                Console.WriteLine();
                Console.Write("Please enter ID of the record you wish to update:");
                input = Console.ReadLine();
            }
            return Convert.ToInt32(input);
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
        public DatabaseRecord GetNewValuesForRecord(DatabaseRecord oldRecord, int columnNumber)
        {
            DatabaseRecord newRecord = new DatabaseRecord(oldRecord.ID, oldRecord.Date, oldRecord.Volume);

            if (columnNumber == 0)
            {
                Console.Write("Enter new value for column Date: ");
                newRecord.Date = Console.ReadLine();
                Console.Write("Enter new value for column Glasses: ");
                newRecord.Volume = Console.ReadLine();
            }
            if (columnNumber == 1)
            {
                Console.Write("Enter new value for column Date: ");
                newRecord.Date = Console.ReadLine();
            }
            if (columnNumber == 2) 
            {
                Console.Write("Enter new value for column Glasses: ");
                newRecord.Volume = Console.ReadLine();
            }

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
        
    }
}
