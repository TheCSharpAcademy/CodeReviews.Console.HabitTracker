using Spectre.Console;

namespace HabitLogger;

public class UI
{
    private readonly LogService _logService;

    public UI(LogService logService)
    {
        _logService = logService;
    }

    public void Start()
    {
        AnsiConsole.Write(
        new FigletText("Habit logger")
        .LeftJustified()
        .Color(Color.Blue));

        bool keepGoing = true;
        while (keepGoing)
        {
            PrintMenu();

            string userInput = GetUserInput("Enter a number: ");

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Closing app..");
                    keepGoing = false;
                    break;
                case "1":
                    var logs = _logService.GetAll();
                    PrintAllLogs(logs);
                    break;
                case "2":
                    logs = _logService.GetAll();
                    PrintAllLogs(logs);

                    int inputId = GetIdInput();
                    var log = _logService.GetLog(inputId);

                    PrintLog(log);
                    break;
                case "3":
                    var number = GetNumberInput("Enter number of hours you've been coding: ");
                    var dateCreated = GetDateInput("Enter a date(Format: dd/mm/yyy hh:mm i.e 01/06/1992 14:45): ");

                    _logService.AddLog(new CSharpLog
                    {
                        Hours = number,
                        DateCreated = DateTime.Parse(dateCreated),
                        DateUpdated = DateTime.Parse(dateCreated)
                    });
                    break;
                case "4":
                    logs = _logService.GetAll();
                    PrintAllLogs(logs);

                    int input = GetIdInput();
                    log = _logService.GetLog(input);

                    if (log is null)
                    {
                        Console.WriteLine("No log found.");
                        break;
                    }
                    var updateHours = GetNumberInput("Enter updated hour: ");
                    var updatedDate = GetDateInput("Enter a date(Format: dd/mm/yyy hh:mm i.e 01/06/1992 14:45): ");
                    while (!Validate.IsValidUpdateDate(log.DateUpdated, DateTime.Parse(updatedDate)))
                    {
                        Console.WriteLine("New update date can't be prior to current update date.Try again.");
                        updatedDate = GetDateInput("Enter a date(Format: dd/mm/yyy hh:mm i.e 01/06/1992 14:45): ");
                    }

                    _logService.Update(log.Id, new CSharpLog
                    {
                        Hours = updateHours,
                        DateUpdated = DateTime.Parse(updatedDate)
                    });
                    break;
                case "5":
                    logs = _logService.GetAll();
                    PrintAllLogs(logs);

                    var deleteLogId = GetIdInput();
                    var deleteLog = _logService.GetLog(deleteLogId);

                    if (deleteLog is null)
                    {
                        Console.WriteLine("No log found.");
                        break;
                    }

                    _logService.DeleteLog(deleteLogId);

                    break;
                default:
                    Console.WriteLine("Invalid number.Try again.");
                    break;
            }
        }
    }

    public void PrintAllLogs(List<CSharpLog> logs)
    {
        var table = new Table();

        table.AddColumns("Id", "Hours", "Date Created", "Date Updated");

        foreach (var log in logs)
        {
            table.AddRow(log.Id.ToString(), log.Hours.ToString(), log.DateCreated.ToString(), log.DateUpdated.ToString());
        }

        AnsiConsole.Write(table);
    }

    public void PrintLog(CSharpLog log)
    {
        var table = new Table();

        table.AddColumns("Id", "Hours", "Date Created", "Date Updated");

        table.AddRow(log.Id.ToString(), log.Hours.ToString(), log.DateCreated.ToString(), log.DateUpdated.ToString());

        AnsiConsole.Write(table);
    }

    string? GetUserInput(string message)
    {
        Console.Write(message);
        string userInput = Console.ReadLine();

        return userInput;
    }

    int GetNumberInput(string message)
    {
        int numberinput = 0;

        while (true)
        {
            try
            {
                numberinput = Convert.ToInt32(GetUserInput(message));
                break;
            }
            catch (FormatException ex)
            {

                Console.WriteLine("Invalid number: Try again");
            }
        }

        return numberinput;
    }
    string GetDateInput(string message)
    {
        Console.Write(message);
        string dateInput = Console.ReadLine();
        while (!Validate.IsValidateDate(dateInput))
        {
            Console.WriteLine("Invalid date.Try again.");
            Console.Write(message);
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    int GetIdInput()
    {
        int idInput = 0;

        while (true)
        {
            try
            {
                idInput = Convert.ToInt32(GetUserInput("Enter id: "));
                break;
            }
            catch (FormatException ex)
            {

                Console.WriteLine("Invalid number: Try again");
            }
        }

        return idInput;
    }

    void PrintMenu()
    {
        Console.WriteLine(@"
What would you like to do?
    
Type 0 to Close Application.
Type 1 to View All Records.
Type 2 to View a Record.
Type 3 to Insert Record.
Type 4 to Update Record.
Type 5 to Delete Record");
    }
}

