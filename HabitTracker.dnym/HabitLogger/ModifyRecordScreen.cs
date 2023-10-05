using HabitLogger.Models;
using System.Diagnostics;

namespace HabitLogger;

internal static class ModifyRecordScreen
{
    public static bool Show(HabitRecord record)
    {
        const string header = @"Modify Record
=============
";

        const string footer = @"

-------------
Press [Esc] to cancel modification.";

        string datePrompt = @$"Enter a time and date for this record, or leave empty
for unchanged time [{record.Date}]: ";

        string quantityPrompt = @$"Enter the quantity for this occasion, or leave empty
for unchanged quantity [{record.Quantity}]: ";

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
                userDate = record.Date;
                userDateString = record.Date.ToString();
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

            if (string.IsNullOrWhiteSpace(userQuantityString))
            {
                userQuantity = record.Quantity;
            }
            else if (int.TryParse(userQuantityString, out var parsedQuantity))
            {
                userQuantity = parsedQuantity;
            }
        }

        var output = false;
        if (!pressedEscape && userDate != null && userQuantity != null && (userDate != record.Date || userQuantity != record.Quantity))
        {
            record.Date = (DateTime)userDate;
            record.Quantity = (int)userQuantity;
            Debug.WriteLine($"Modified record: {userQuantity} @ {userDate}");
            output = true;
        }
        return output;
    }
}
