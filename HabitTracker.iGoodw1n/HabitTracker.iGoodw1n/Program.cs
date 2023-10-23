using System.Globalization;

namespace HabitTracker.iGoodw1n;

internal static class Program
{
    private const string dbPath = "Data Source=HabitTracker.db;";
    private static readonly DataStorage _db = new DataStorage(dbPath);
    static void Main(string[] args)
    {
        var closeApp = false;
        while (!closeApp)
        {
            IOHelpers.ShowMenu();

            var input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    break;
                case "1":
                    ShowAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    ShowReport();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

    private static void UpdateRecord()
    {
        ShowAllRecords();

        var id = IOHelpers.GetParsedUserInput<int>("\nPlease type the id of row that you want to update or type 0 to return to Main Menu: ");

        if (id == 0) return;

        var record = _db.GetRecord(id);

        if (record == null)
        {
            Console.WriteLine($"Record with id {id} doesn't exist.");
            return;
        }

        Console.WriteLine(record);

        var language = GetUpdatedValue("Language", record.Language);
        record.Language = string.IsNullOrEmpty(language) ? record.Language : language;

        var lines = GetUpdatedValue("Lines of code", record.Lines.ToString());
        record.Lines = string.IsNullOrEmpty(language) || !int.TryParse(lines, out var linesInt) ? record.Lines : linesInt;

        _db.UpdateRecord(record);
        record = _db.GetRecord(id);
        Console.WriteLine($"Updated record: {record}");

        string GetUpdatedValue(string field, string value)
        {
            Console.WriteLine($"Change {field} or just press Enter");
            Console.WriteLine(value);
            Console.CursorTop--;
            var newValue = Console.ReadLine();
            return newValue!;
        }
    }

    private static void DeleteRecord()
    {
        ShowAllRecords();

        var id = IOHelpers.GetParsedUserInput<int>("\nPlease type the id of row that you want to delete or type 0 to return to Main Menu: ");

        if (id == 0) return;

        var record = _db.GetRecord(id);

        if (record == null) return;

        Console.WriteLine($"This record will be deleted {record}. Are you sure? y/n");
        var input = Console.ReadLine();

        if (!input.Equals("y", StringComparison.OrdinalIgnoreCase)) return;
        
        _db.DeleteRecord(id);
    }

    private static void InsertRecord()
    {
        var language = IOHelpers.GetUserInput("\nPlease enter Programming Language: ");
        var lines = IOHelpers.GetParsedUserInput<int>("\nPlease enter number of lines that you have written: ");

        Console.Write("\nEnter the date (Format: dd-MM-yy) or leave blank to use current Date. Enter 0 to cancel operation and return to Main Menu:");
        var dateInput = Console.ReadLine();
        if (dateInput == "0") return;
        DateTime date;
        if (dateInput == "")
        {
            date = DateTime.UtcNow;
        }
        else
        {
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.Write("\nEnter the date (Format: dd-MM-yy) or leave blank to use current Date: ");
                dateInput = Console.ReadLine();
            }
        }

        var record = new WrittenCode
        {
            Language = language,
            Lines = lines,
            Date = date
        };

        _db.CreateRecord(record);
    }

    private static void ShowAllRecords()
    {
        var records = _db.GetAllRecords();
        if (records == null)
        {
            Console.WriteLine("You don't have any records yet");
            return;
        }

        Console.WriteLine($"{new string('=', 20)} CODE TRACKER RECORDS {new string('=', 20)}");
        Console.WriteLine("\n{0,-2}{1,20}{2,20}{3,20}", "Id", "Language", "Lines of Code", "Date");
        Console.WriteLine(new string('=', 62));
        foreach (var record in records)
        {
            Console.WriteLine("{0,-2}{1,20}{2,20}{3,20:d}",
                record.Id,
                record.Language,
                record.Lines,
                record.Date);
        }
        Console.WriteLine($"\n{new string('=', 62)}");
    }

    private static void ShowReport()
    {
        var records = _db.GetAllRecords();
        if (records == null) return;

        var years = records.GroupBy(r => r.Date.Year).Select(g => g.Key).ToList();

        var year = IOHelpers.GetParsedUserInput<int>($"\nEnter Year to generate annual report. " +
            $"You have records for this years ({string.Join(", ", years)}): ");

        if (!years.Contains(year))
        {
            Console.WriteLine($"You don't have any records for this year: {year}");
        }

        var groupped = records
            .Where(r => r.Date.Year == year)
            .GroupBy(r => r.Language)
            .Select(g => (Language: g.Key, TotalLines: g.Sum(r => r.Lines)))
            .ToList();

        Console.WriteLine($"{new string('=', 10)} YEAR REPORT {new string('=', 10)}");
        Console.WriteLine("\n{0,-15}{1}", "Language", "Total lines of code");
        foreach (var group in groupped)
        {
            Console.WriteLine($"{group.Language + ":", -20} {group.TotalLines}");
        }
        Console.WriteLine($"{new string('=', 10)} YEAR REPORT {new string('=', 10)}");
    }
}