using HabitLogger.Database;
using System.Diagnostics;

namespace HabitLogger;

internal class InsertRecordScreen
{
    private readonly IDatabase _database;

    public InsertRecordScreen(IDatabase database)
    {
        _database = database;
    }

    public void Show()
    {
        const string header = @"Insert Record
=============
";

        const string footer = @"

-------------
Press [Esc] to cancel insertion.";

        const string datePrompt = @"Enter a time and date for this record,
or leave empty for current time: ";

        const string quantityPrompt = "Enter the quantity for this occasion: ";

        string userDateString = "";
        DateTime? userDate = null;
        int? userQuantity = null;

        bool pressedEscape = false;
        while (!pressedEscape && userDate == null)
        {
            Console.Clear();
            Console.WriteLine(header);
            Console.Write(datePrompt);
            var currentPositionX = Console.CursorLeft;
            var currentPositionY = Console.CursorTop;
            Console.WriteLine(footer);

            Console.SetCursorPosition(currentPositionX, currentPositionY);

            (pressedEscape, userDateString, _) = Helpers.ReadInput();

            if (string.IsNullOrWhiteSpace(userDateString))
            {
                var now = DateTime.Now;
                userDate = now;
                userDateString = now.ToString();
            }
            else if (DateTime.TryParse(userDateString, out var parsedDate))
            {
                userDate = parsedDate;
                userDateString = parsedDate.ToString();
            }
        }

        while (!pressedEscape && userQuantity == null)
        {
            Console.Clear();
            Console.WriteLine(header);
            Console.Write(datePrompt);
            Console.WriteLine(userDateString);
            Console.WriteLine();
            Console.Write(quantityPrompt);
            var currentPositionX = Console.CursorLeft;
            var currentPositionY = Console.CursorTop;
            Console.WriteLine(footer);

            Console.SetCursorPosition(currentPositionX, currentPositionY);
            string userQuantityString;
            (pressedEscape, userQuantityString, _) = Helpers.ReadInput();

            if (int.TryParse(userQuantityString, out var parsedQuantity))
            {
                userQuantity = parsedQuantity;
            }
        }

        if (!pressedEscape && userDate != null && userQuantity != null)
        {
            _database.InsertRecord(new Models.HabitRecord() { Date = userDate.Value, Quantity = userQuantity.Value });
            Debug.WriteLine($"Added record: {userQuantity} @ {userDate}");
        }
    }
}
