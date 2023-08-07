using ConsoleTableExt;
using HabitTracker.MartinL_no.Services;

namespace HabitTracker.MartinL_no;

internal class HabitTrackerApplication
{
    private readonly HabitService _service;

    internal HabitTrackerApplication(HabitService service)
    {
        _service = service;
    }

    internal void Execute()
    {
        while (true)
        {
            ShowMainMenuOptions();
            var op = Ask("Your choice: ");

            switch (op.ToLower())
            {
                case "h":
                    AddHabit();
                    break;
                case "a":
                    AddDate();
                    break;
                case "u":
                    UpdateDate();
                    break;
                case "d":
                    DeleteDate();
                    break;
                case "v":
                    ViewRecords();
                    break;
                case "0":
                    Console.WriteLine("Program ended");
                    return;
                default:
                    ShowMessage("Invalid option, please try again");
                    break;
            }
        }
    }

    private void ShowMainMenuOptions()
    {
        ShowHeader("Welcome to the Habit Tracker app!");

        Console.WriteLine("""

            Select an option:
            h - Add/replace habit
            a - Add date
            u - Update date
            d - Delete date
            v - View records
            0 - Exit program

            """);

        Console.WriteLine("---------------------------------");
    }

    private void AddHabit()
    {
        while (true)
        {
            ShowHeader("Add/replace habit");

            var name = Ask("Enter your habit name: ");

            try
            {
                _service.Add(name);
                ShowMessage($"{name} added as habit");
                break;
            }
            catch (ArgumentException)
            {
                ShowMessage($"Invalid entry please try again");
            }
            catch (InvalidOperationException)
            {
                var op = Ask($"Another habit is already stored in the system, do you want to replace it with {name} (Enter y)? ");

                if (op.ToLower() == "y") ReplaceHabit(name);

                break;
            }
        }
    }

    private void AddDate()
    {
        while (true)
        {
            ShowHeader("Add date");

            try
            {
                var habit = _service.Get();

                var dateString = Ask("Which date would you like to add a record to (yyyy-MM-dd)? ");
                var date = DateOnly.Parse(dateString);

                if (habit.Dates.Exists(d => d.Date == date))
                {
                    ShowMessage("Record already exists for this date, if you wish to change it update it via the main menu");
                    return;
                }

                var repetitionsString = Ask("How many times did you repeat the habit that day? ");
                var repetitions = Int32.Parse(repetitionsString);

                _service.AddDate(date, repetitions);
                ShowMessage("Entry added!");
                break;
            }
            catch (FormatException)
            {
                ShowMessage("Invalid date or repetitions entry, please try again");
            }
            catch (InvalidOperationException)
            {
                ShowMessage("No habit to add a record to, add habit first");
                break;
            }
        }
    }

    private void UpdateDate()
    {
        while (true)
        {
            ShowHeader("Update date");

            try
            {
                var habit = _service.Get();

                var dateString = Ask("Which date would you like to update (yyyy-MM-dd)? ");
                var date = DateOnly.Parse(dateString);

                if (!habit.Dates.Exists(d => d.Date == date))
                {
                    ShowMessage("Record does not exist for this date, add it via the option in the main menu");
                    return;
                }

                var repetitionsString = Ask("How many times did you repeat the habit that day? ");
                var repetitions = Int32.Parse(repetitionsString);

                _service.UpdateDate(date, repetitions);
                ShowMessage("Entry updated!");
                break;
            }
            catch (FormatException)
            {
                ShowMessage("Invalid date or repetitions entry, please try again");
            }
            catch (InvalidOperationException)
            {
                ShowMessage("No habit currently being recorded, add habit first");
                break;
            }
        }
    }

    private void DeleteDate()
    {
        while (true)
        {
            ShowHeader("Delete date");

            try
            {
                var dateString = Ask("Which date would you like to delete (yyyy-MM-dd)? ");
                var date = DateOnly.Parse(dateString);

                _service.DeleteDate(date);
                ShowMessage("Entry deleted!");
                break;
            }
            catch (FormatException)
            {
                ShowMessage("Invalid date or repetitions entry, please try again");
            }
        }
    }

    private void ViewRecords()
    {
        ShowHabitRecordsMenu();

        var op = Ask("Your choice: ");

        switch (op.ToLower())
        {
            case "a":
                ViewAllRecords();
                break;
            case "t":
                ViewTotal();
                break;
            case "d":
                ViewTotalSinceSpecifiedDate();
                break;
        }
    }

    private void ViewAllRecords()
    {
        try
        {
            var habit = _service.Get();
            if (habit.Dates.Count == 0) throw new InvalidOperationException();

            var tableData = habit.Dates.Select(d => new List<object> { d.Date, d.Count }).ToList();
            ShowRecordsTable($"All {habit.Name} habit records", tableData, "Date", "Count");
        }
        catch (InvalidOperationException)
        {
            ShowMessage("No habit currently being recorded, add habit and/or records first");
        }
    }

    private void ViewTotal()
    {
        try
        {
            Console.Clear();

            var habit = _service.Get();
            var habitTotal = _service.GetTotal(habit.Id);
            var tableData = new List<List<object>>
            {
                new List<object> { "All dates", habitTotal.Total }
            };

            ShowRecordsTable($"Total {habit.Name} habit repetitions", tableData, "From Date", "Total");
        }
        catch (InvalidOperationException)
        {
            ShowMessage("No matching records found");
        }
    }

    private void ViewTotalSinceSpecifiedDate()
    {
        while (true)
        {
            try
            {
                Console.Clear();

                var habit = _service.Get();

                var dateString = Ask("From which date do you wish to see the total amount of records from  (yyyy-MM-dd)? ");

                var date = DateOnly.Parse(dateString);
                var habitTotal = _service.GetTotalSinceDate(habit.Id, date);
                var tableData = new List<List<object>>
                {
                    new List<object> { date, habitTotal.Total }
                };

                ShowRecordsTable($"Total {habit.Name} habit repetitions", tableData, "From Date", "Total");
                break;

            }
            catch (FormatException)
            {
                ShowMessage("Invalid date, please try again");

            }
            catch (InvalidOperationException)
            {
                ShowMessage("No matching records found");
                break;
            }
        }
    }

    private void ShowHabitRecordsMenu()
    {
        ShowHeader("View habit records");

        Console.WriteLine("""

            Select an option:
            a - View all records
            t - Total from all time
            d - Total since specified date

            """);

        Console.WriteLine("---------------------------------");
    }

    private void ShowRecordsTable(string pageTitle, List<List<object>> tableDate, string colOne, string colTwo)
    {
        Console.Clear();
        Console.WriteLine($"{pageTitle}\n");

        ConsoleTableBuilder
            .From(tableDate)
            .WithColumn(colOne, colTwo)
            .ExportAndWriteLine();

        Console.Write("\nPress any key to return to the main menu");
        Console.ReadKey();
    }

    private void ReplaceHabit(string name)
    {
        _service.Delete();
        _service.Add(name);
        ShowMessage($"You have changed your habit to {name}");
    }

    private static void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("---------------------------------");
    }

    private void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(2500);
    }

    private string Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}
