using HabitLogger.Models;

namespace HabitLogger;

internal static class ManageRecordScreen
{
    public enum Action
    {
        Modify,
        Delete,
        Back
    }

    public static Action Show(HabitRecord record)
    {
        Action returnAction = Action.Back;
        while (true)
        {
            Console.Clear();
            Console.WriteLine(@$"Viewing Record
==============

Date: {record.Date}
Quantity: {record.Quantity}

--------------
Press [M] to modify the record,
[D] to delete,
or [Esc] to go back to the main menu.");
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.M:
                    if (ModifyRecordScreen.Show(record))
                    {
                        returnAction = Action.Modify;
                    }
                    break;
                case ConsoleKey.D:
                    return Action.Delete;
                case ConsoleKey.Escape:
                    return returnAction;
            }
        }
    }
}
