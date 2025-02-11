using System.Globalization;
using System.Text.RegularExpressions;
namespace HabitTracker.BrozDa
{

    internal class OutputManager
    {
        public string DateFormat { get; init; }

        private const string FormatSpecifier = "{0,-15:N}";
        private const int HorizonalLineLength = 5 + 15 + 15 + 4; //5 for ID, 2x15 for Date and Volume, +4 horizontalLines
        public readonly int MainMenuLength = 5;
        public readonly int HabitMenuLength = 7;
        public OutputManager(string dateFormat)
        {
            DateFormat = dateFormat;
        }
        public void PrintMainMenu()
        {
            ClearAndSetCursor();
            Console.WriteLine("Welcome to habit tracker application");
            Console.WriteLine("Habit tracker can be used to track custom habits");
            Console.WriteLine();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. Check tracked habits");
            Console.WriteLine("\t2. Manage existing habit");
            Console.WriteLine("\t3. Create new habit");
            Console.WriteLine("\t4. Delete habit");
            Console.WriteLine("\t5. Exit the application");
            Console.Write("Your selection: ");
        }
        public void PrintHabitMenu()
        {
            ClearAndSetCursor();
            Console.WriteLine("Please select the operation:");
            Console.WriteLine("\t1. View all records");
            Console.WriteLine("\t2. Insert record");
            Console.WriteLine("\t3. Update record");
            Console.WriteLine("\t4. Delete record");
            Console.WriteLine("\t5. Get Report");
            Console.WriteLine("\t6. Exit to main menu");
            Console.WriteLine("\t7. Close the application");

            Console.Write("Your selection: ");

        }
        private void ClearAndSetCursor()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }
        
       
        public void PrintRecordsFromTable(string table, List<DatabaseRecord> records, string unit)
        {
            ClearAndSetCursor();
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
            Console.Write(string.Format(FormatSpecifier, "Date"));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, "Volume"));
            Console.Write('|');
            Console.WriteLine();
        }
       
        public void PrintRecord(DatabaseRecord record, string unit) {

            Console.Write('|');
            Console.Write(string.Format("{0,-5}", record.ID));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, record.Date.ToString(DateFormat)));
            Console.Write('|');
            Console.Write(string.Format(FormatSpecifier, record.Volume + " " + unit));
            Console.WriteLine('|');

        }
        public void PrintHorizonalLine()
        {
            Console.WriteLine(new string('-', HorizonalLineLength));
        }
        private void PrintTableHeader(string table)
        {
            string text = $"Table: {table}";
            int totalSpaces = (HorizonalLineLength - 2 - text.Length);
            int leftpadding = totalSpaces / 2;
            int rightPadding = totalSpaces - leftpadding;

            
            Console.WriteLine("|" + new string(' ', leftpadding) + text + new string(' ', rightPadding) + "|");
        }
        


        public void PrintTablesInDatabase(List<string> listOfTables)
        {
            ClearAndSetCursor();
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
