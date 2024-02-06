using CodingTracker;
using Spectre.Console;

namespace HabitLogger;

class Program
{
    private static bool openApp = true;

    static void Main(string[] args)
    {
        DataBase.Connect();

        WelcomeMessage();

        do
        {
            char option = GetOption();
            switch (option)
            {
                case '1':
                    WelcomeMessage();
                    OpenShowWindow();
                    break;
                case '2':
                    WelcomeMessage();
                    OpenInsertWindow();
                    break;
                case '3':
                    WelcomeMessage();
                    OpenUpdateWindow();
                    break;
                case '4':
                    WelcomeMessage();
                    OpenDeleteWindow();
                    break;
                case '0':
                    openApp = false;
                    Console.WriteLine("Exit Program");
                    break;
                default:
                    Console.WriteLine("please Enter valid input");
                    break;
            }
        } while (openApp);

    }
    // display welcom message for user
    private static void WelcomeMessage()
    {
        Console.Clear();
        Console.WriteLine($@"       Welcome To Habit Logger
+++++++++++++++++++++++++++++++++++++++++");
    }
    // display main menu for user to choose from it what action user want to do
    // return char the number of option user choose
    static char GetOption()
    {
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("What would you like to do?")
                        .AddChoices(new[]
                        {
                                "1 - Show all records",
                                "2 - Insert new record",
                                "3 - Update record",
                                "4 - Delete record",
                                "0 - exit"
                        }));
        return option[0];
    }


    private static void OpenShowWindow()
    {
        HabitLoggerController controller = new HabitLoggerController();
        var records = controller.GetAllRecords();
        var table = new Table();

        table.AddColumn(new TableColumn("ID").Centered());
        table.AddColumn(new TableColumn("Date").Centered());
        table.AddColumn(new TableColumn("Kilometers").Centered());

        foreach (var record in records)
        {
            table.AddRow($"{record.Id}", record.Date.ToString("MM/dd/yyyy"),  $"{record.Kilometers} km");
        }
        AnsiConsole.Write(table);
    }
    private static void OpenInsertWindow()
    {
        string date = Validations.GetValidatedDate("Please Enter Date in Excat date format (MM/dd/yyyy) : ");
        int kilometers = Validations.GetValidatedKilometers("Please Enter number of Kilometers you run ");

        var controller = new HabitLoggerController();
        controller.Insert(date, kilometers);

        // display success message
        Console.WriteLine();
        AnsiConsole.MarkupLine("[green]Record has been inserted successfully[/]");
        Console.WriteLine();
    }
    private static void OpenUpdateWindow()
    {
        OpenShowWindow();
        int id = Validations.GetValidatedInteger("Enter record Id you want to update");

        if (!DataBase.IsExist(id))
        {
            AnsiConsole.MarkupLine($"[red]Couldn't find record with id {id} , Please valid ID[/]");
            OpenUpdateWindow();
        }

        Console.WriteLine("-------------------------------------------------\n");
        string date = Validations.GetValidatedDate("Please Enter Date in Excat date format (MM/dd/yyyy) : ");
        int kilometers = Validations.GetValidatedKilometers("Please Enter number of Kilometers you run ");

        var controller = new HabitLoggerController();
        controller.Update(id, date, kilometers);

        // display success message
        Console.WriteLine();
        AnsiConsole.MarkupLine("[green]Record has been Updated successfully[/]");
        Console.WriteLine();
    }
    private static void OpenDeleteWindow()
    {
        OpenShowWindow();
        int id = Validations.GetValidatedInteger("Enter record Id you want to Delete");

        if (!DataBase.IsExist(id))
        {
            AnsiConsole.MarkupLine($"[red]Couldn't find record with id {id} , Please valid ID[/]");
            OpenDeleteWindow();
        }

        var controller = new HabitLoggerController();
        controller.Delete(id);

        // display success message
        Console.WriteLine();
        AnsiConsole.MarkupLine("[green]Record has been Deleted successfully[/]");
        Console.WriteLine();
    }
}
