using HabitLogger.Database;

namespace HabitLogger;

internal class ManageRecordsMenu
{
    private readonly IDatabase _database;

    public ManageRecordsMenu(IDatabase database)
    {
        _database = database;
    }

    public void Show()
    {
        const string headerBase = "Records";
        string header;

        const string pgUp = @"[PgUp] to go to the previous page,
";

        const string pgDown = @"[PgDown] to go to the next page,
";

        const string esc = "[Esc] to go back to the main menu";

        const string footerBase = @"

------------------
Press {}.";
        string footer;

        const string prompt = "Select a record to manage: ";
        const string noRecords = "No records to manage.";

        int recordsCount;
        int skipped = 0;
        int perPage = Math.Max(1, Console.WindowHeight - 11);
        Helpers.Signal? signal;

        bool pressedEscape = false;
        while (!pressedEscape)
        {
            recordsCount = _database.GetRecordsCount();
            var subset = _database.GetRecords(perPage, skipped);
            int left = recordsCount - skipped - subset.Count;
            var quantityWidth = 0;
            if (subset.Count > 0)
            {
                quantityWidth = subset.Max(r => r.Quantity.ToString().Length);
            }

            Console.Clear();

            if (recordsCount <= perPage)
            {
                string divider = new('=', headerBase.Length);
                header = $"{headerBase}\n{divider}\n";
            }
            else
            {
                var totalPages = (int)Math.Ceiling(recordsCount / (double)perPage);
                var currentPage = (int)Math.Ceiling((skipped + 1) / (double)perPage);
                header = headerBase + $" (page {currentPage}/{totalPages})";
                string divider = new('=', header.Length);
                header = $"{header}\n{divider}\n";
            }
            Console.WriteLine(header);

            if (skipped == 0 && left == 0)
            {
                footer = footerBase.Replace("{}", esc);
            }
            else if (skipped == 0)
            {
                footer = footerBase.Replace("{}", $"{pgDown}or {esc}");
            }
            else if (left == 0)
            {
                footer = footerBase.Replace("{}", $"{pgUp}or {esc}");
            }
            else
            {
                footer = footerBase.Replace("{}", $"{pgUp}{pgDown}or {esc}");
            }

            if (subset.Count > 0)
            {
                for (int i = 0; i < subset.Count; i++)
                {
                    var record = subset[i];
                    var number = i + 1;
                    var numberString = number.ToString().PadLeft(perPage.ToString().Length);
                    var quantityString = record.Quantity.ToString().PadLeft(quantityWidth);
                    Console.WriteLine($"{numberString}. {quantityString} @ {record.Date}");
                }
            }
            else
            {
                Console.Write(noRecords);
            }

            int currentPositionX = 0;
            int currentPositionY = 0;
            if (subset.Count > 0)
            {
                Console.WriteLine();
                Console.Write(prompt);
                currentPositionX = Console.CursorLeft;
                currentPositionY = Console.CursorTop;
            }

            Console.WriteLine(footer);
            string userSelection;
            if (subset.Count > 0)
            {
                Console.SetCursorPosition(currentPositionX, currentPositionY);
            }
            (pressedEscape, userSelection, signal) = Helpers.ReadInput();
            if (signal == Helpers.Signal.PG_UP && skipped > 0)
            {
                skipped = Math.Max(0, skipped - perPage);
            }
            else if (signal == Helpers.Signal.PG_DOWN && left > 0)
            {
                skipped = Math.Min(recordsCount - 1, skipped + perPage);
            }
            else if (int.TryParse(userSelection, out int recordNumber) && recordNumber >= 1 && recordNumber <= perPage && recordNumber <= subset.Count)
            {
                var record = subset[recordNumber - 1];
                var action = ManageRecordScreen.Show(record);
                if (action == ManageRecordScreen.Action.Delete)
                {
                    _database.DeleteRecord(record.Id);
                    if (subset.Count == 1)
                    {
                        skipped = Math.Max(0, skipped - perPage);
                    }
                }
                else if (action == ManageRecordScreen.Action.Modify)
                {
                    _database.UpdateRecord(record);
                }
            }
        }
    }
}
