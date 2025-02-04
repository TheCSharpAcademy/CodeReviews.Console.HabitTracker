using System.Data.Common;

namespace HabitTracker.BrozDa
{
    internal class DisplayManager
    {
        private readonly string _format = "{0,-15:N}";
        public DisplayManager() { }

        public void ShowRecords(List<string> columns, List<DatabaseRecord> records)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            Console.WriteLine(new string('-',columns.Count*15 +columns.Count+1));

            Console.Write('|');
            foreach (var column in columns) {
                Console.Write(string.Format(_format,column));
                Console.Write('|');
            }
            Console.WriteLine();

            Console.WriteLine(new string('-', columns.Count * 15 + columns.Count + 1));
            foreach (var record in records)
            {
                Console.Write('|');
                Console.Write(string.Format(_format, record.Date));
                Console.Write('|');
                Console.Write(string.Format(_format, record.Volume));
                Console.WriteLine('|');
            }
            Console.WriteLine(new string('-', columns.Count * 15 + columns.Count + 1));

        }
    }
}
