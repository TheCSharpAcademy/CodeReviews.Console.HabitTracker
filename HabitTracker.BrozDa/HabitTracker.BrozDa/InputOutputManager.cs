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

        public void ShowRecords(List<string> columns, List<DatabaseRecord> records)
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
    }
}
