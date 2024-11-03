using Spectre.Console;
namespace DatabaseController;
public static class OperationController
{
    enum HabitProperties
    {
        HabitName,
        Quantity,
        OcurranceDate,
        MeasurementUnit,
        SaveChanges
    }
    enum DateEntry
    {
        SpecificDate,
        TodaysDate
    }
    enum ReportType
    {
        XDaysAgo,
        StartDateUptoTodayDate,
        StartDateUptoEndDate,
        TotalOfAllTime,
        TotalOfGivenMonth,
        Today,
        Exit
    }

    // Retrieves the habit ID from a given row passed by as a string in a specific format.
    private static int ExtractHabitID(string recordRow)
    {
        string[] selectedHabitValues = recordRow.Split(',');
        string ID = selectedHabitValues[0].Substring(selectedHabitValues[0].IndexOf('=') + 1);
        return int.Parse(ID);
    }
    // Retrieves the habitname from a given row passed by as a string in a specific format.
    private static string ExtractHabitName(string recordRow)
    {
        string[] selectedHabitValues = recordRow.Split(',');
        string habitName = selectedHabitValues[1].Substring(selectedHabitValues[1].IndexOf('=') + 1);
        return habitName;
    }

    // shows the table rows as selectable items to further process upon them.
    private static string SelectRecordList(string databasePath, bool distinctRows = false)
    {
        SqliteQuery sqliteQuery = new SqliteQuery();
        string queryResult = "";
        string query;

        if (distinctRows)
        {
            query =
            @"
                SELECT id, habitname, quantity, quantityunit, date
                FROM habit
                GROUP BY habitname
                ORDER BY habitname;

                
            ";
        }
        else
        {
            query =
           @"
                SELECT * FROM habit
                ORDER BY habitname;
                
            ";
        }


        queryResult = SqliteQuery.ExecuteReadQuery(query, databasePath);
        // convert the record rows in the database to a list
        var recordList = queryResult.Split('.')
            .Where(row => !String.IsNullOrWhiteSpace(row))
            .ToList();

        // make the list a selection prompt for the user to select a habit to delete.
        var selectedHabit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[red]Choose a habit:[/]")
            .AddChoices(recordList)
            );
        return selectedHabit;
    }
    private static void QueryTableView(string queryResult, TableBorder tableBorder, char rowDelimiter = '.', char columnDelimiter = ',')
    {
        string[] queryRows = queryResult.Split(rowDelimiter);
        var table = new Table();
        table.Border(tableBorder);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Habit's name[/]");
        table.AddColumn("[yellow]Quantity[/]");
        table.AddColumn("[yellow]Unit of measurement[/]");
        table.AddColumn("[yellow]Ocurrance date[/]");

        foreach (var row in queryRows)
        {
            if (row != "")
            {
                string[] columnValue = row.Split(",");
                string ID = columnValue[0].Substring(columnValue[0].IndexOf('=') + 1);
                string habitName = columnValue[1].Substring(columnValue[1].IndexOf('=') + 1);
                string quantity = columnValue[2].Substring(columnValue[2].IndexOf('=') + 1);
                string measurementUnit = columnValue[3].Substring(columnValue[3].IndexOf('=') + 1);
                string ocurranceDate = columnValue[4].Substring(columnValue[4].IndexOf('=') + 1);
                table.AddRow(

                $"[cyan]{ID}[/]",
                $"[cyan]{habitName}[/]",
                $"[green]{quantity}[/]",
                $"[green]{measurementUnit}[/]",
                $"[blue]{ocurranceDate}[/]"

                );
            }


        }
        AnsiConsole.Write(table);

    }
    public static void InsertRecord(string databasePath)
    {
        string dateFormat = "yyyy-MM-dd";
        SqliteQuery sqliteQuery = new SqliteQuery();
        String outputDate = "";

        bool queryIsSuccessfull = default;

        AnsiConsole.MarkupLine("[red]Add a new habit![/]");
        do
        {
            // AnsiConsole Ask statement forces the type of the variable on the user , it displays an error message if a user attempts to enter a value with invalid format
            var habitName = AnsiConsole.Ask<string>("[red]Enter the habit name:[/]\n");

            var quantity = AnsiConsole.Ask<int>("[red]Enter the quantity:[/]\n");
            if (quantity <= 0)
            {
                Console.WriteLine("Error: the number of ocurrances is unlogical , can't be 0 or  below");
                do
                {
                    quantity = AnsiConsole.Ask<int>("[red]Enter the amount of times you did this today (value can't be 0 or below 0):[/]\n");
                }
                while (quantity <= 0);
            }

            var measurementUnit = AnsiConsole.Ask<string>("[red]Enter the measurement unit of this habit:\n(Type 0 if this habit can't be measured by a unit)[/]\n");

            // gives the user an option to enter a specific date for when the habit ocurred or just use today's date.
            var dateEntryChoice = AnsiConsole.Prompt(
                new SelectionPrompt<DateEntry>()
                .Title("[red]Would you like to enter a specific date or today's date?[/]")
                .AddChoices(Enum.GetValues<DateEntry>()));

            switch (dateEntryChoice)
            {
                case DateEntry.SpecificDate:
                    var ocurranceDate = AnsiConsole.Ask<DateOnly>("[red]Enter the date when you did that habit (use this format mm-dd-yyyy):[/]");
                    // converts the enforced date input to a string of a specific date format.
                    outputDate = ocurranceDate.ToString(dateFormat);

                    break;
                case DateEntry.TodaysDate:
                    //used datetime to get today's date and convert it to a string with a specified format.
                    outputDate = DateTime.Now.ToString(dateFormat);

                    break;
            }
            // Creating the insert query string.
            string newRecordQuery =
                @"
                INSERT INTO habit 
                (habitname,quantity,quantityunit,date) VALUES (@habitname,@quantity,@measurementunit,@outputdate);
                ";

            // uses a helper method to execute the insert query.
            queryIsSuccessfull = SqliteQuery.ExecuteWriteQuery(newRecordQuery, databasePath, habitName, quantity, measurementUnit, outputDate);

        }
        while (!queryIsSuccessfull);

        AnsiConsole.MarkupLine("[green]Record inserted successfully!.\nPress any key to continue.[/]");
        Console.ReadKey();

    }

    // Second CRUD operation , view all records
    public static void ViewAllRecords(string databasePath)
    {

        SqliteQuery sqliteQuery = new SqliteQuery();
        string queryResult = "";

        string query =
            @"
                SELECT * FROM habit 
                ORDER BY habitname;
                
            ";

        queryResult = SqliteQuery.ExecuteReadQuery(query, databasePath);

        QueryTableView(queryResult, TableBorder.Square, '.', ',');

        AnsiConsole.MarkupLine("Press Any Key to Continue.");
        Console.ReadKey();
    }

    public static void DeleteRecord(string databasePath)
    {
        SqliteQuery sqliteQuery = new SqliteQuery();
        string queryResult = "";

        // Show all the records to the user
        string query =
            @"
                SELECT * FROM habit ORDER BY habitname;
                
            ";

        queryResult = SqliteQuery.ExecuteReadQuery(query, databasePath);

        // convert the record rows in the database to a list
        var recordList = queryResult.Split('.')
            .Where(row => !String.IsNullOrWhiteSpace(row))
            .ToList();
        // make the list a selection prompt for the user to select a habit to delete.
        var selectedHabit = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[red]Choose a habit to delete:[/]")
            .AddChoices(recordList)
            );

        // Following the format => columns are divided by commas, i know ID is the first column , i parse that colum into an Int variable habitID
        string[] selectedHabitValues = selectedHabit.Split(',');
        string ID = selectedHabitValues[0].Substring(selectedHabitValues[0].IndexOf('=') + 1);
        int habitID = int.Parse(ID);

        // Queries the database with the selected habit's ID to delete the record.
        query =
            @"
                DELETE FROM habit
                    WHERE id = @id
            ";

        SqliteQuery.ExecuteDeleteQuery(query, databasePath, habitID);

        AnsiConsole.MarkupLine("[green]The record has been deleted successfully!.\nPress any key to continue.[/]");
        Console.ReadKey();
    }

    public static void UpdateRecord(string databasePath)
    {
        string dateFormat = "yyyy-MM-dd";
        SqliteQuery sqliteQuery = new SqliteQuery();

        string selectedHabit = SelectRecordList(databasePath);

        // Following the format => columns are divided by commas, assuming  ID is the first column , parse that column into an Int variable habitID

        int habitID = ExtractHabitID(selectedHabit);

        string habitName = "";
        int quantity = -1;
        string MeasurementUnit = "";
        string outputDate = "";

        // Queries the database with the selected habit's ID to update the record.
        string query =
            @"
                UPDATE habit
                SET habitname = @habitname, quantity = @quantity,quantityunit = @measurementunit, date = @newdate
                WHERE id = @id
            ";

        bool updateFinished = default;

        do
        {
            var habitPropertyChoice = AnsiConsole.Prompt(
                        new SelectionPrompt<HabitProperties>()
                        .Title("[red]what habit property would you like to edit?\n[yellow]Click save changes to apply.[/][/]")
                        .AddChoices(Enum.GetValues<HabitProperties>()));
            switch (habitPropertyChoice)
            {
                case HabitProperties.HabitName:
                    habitName = AnsiConsole.Ask<string>("[red]Enter a new name for this habit: [/]");
                    break;
                case HabitProperties.Quantity:
                    quantity = AnsiConsole.Ask<int>("[red]Edit the number of ocurrances for this habit: [/]");
                    break;
                case HabitProperties.MeasurementUnit:
                    MeasurementUnit = AnsiConsole.Ask<string>("[red]Enter a new measurement unit for this habit:\n(Type 0 if this habit can't be measured by a unit)[/]");
                    break;
                case HabitProperties.OcurranceDate:
                    var ocurranceDate = AnsiConsole.Ask<DateOnly>("[red]Enter a new date for this habit: [/]");
                    outputDate = ocurranceDate.ToString(dateFormat);
                    break;
                case HabitProperties.SaveChanges:
                    updateFinished = SqliteQuery.ExecuteUpdateQuery(query, databasePath, habitID, habitName, quantity, MeasurementUnit, outputDate);
                    AnsiConsole.MarkupLine("[green]Changes Saved![/]");
                    break;
            }


        }
        while (!updateFinished);






        AnsiConsole.MarkupLine("[green]The record has been updated successfully!.\nPress any key to continue.[/]");
        Console.ReadKey();

    }

    public static void ViewReports(string databasePath)
    {
        SqliteQuery sqliteQuery = new SqliteQuery();
        string reportRows = "";
        bool exitMenu = default;
        string dateFormat = "yyyy-MM-dd";

        string selectedHabit = SelectRecordList(databasePath, true);
        string selectedHabitName = ExtractHabitName(selectedHabit);

        // completes the query and execuetes it,then it shows the result in a table
        void CloseUpQueryShowResult(string query)
        {
            if (query.Contains("WHERE"))
                query += "AND habitname = @habitname";
            else
                query += "WHERE habitname = @habitname";

            reportRows = SqliteQuery.ExecuteReadQuery(query, databasePath, selectedHabitName);
            QueryTableView(reportRows, TableBorder.Square, '.', ',');

            AnsiConsole.MarkupLine("Press Any Key to Continue.");
            Console.ReadKey();

        }


        do
        {
            string reportQuery = "SELECT * FROM habit ";
            var reportType = AnsiConsole.Prompt(
            new SelectionPrompt<ReportType>()
            .Title("[yellow]Select a report type to view \n(or select exit to leave the report menu):[/]\n")
            .AddChoices(Enum.GetValues<ReportType>())
            );
            switch (reportType)
            {
                case ReportType.XDaysAgo:
                    var pastXDays = AnsiConsole.Ask<int>("[red]past X days = [/]");
                    DateTime reportDate = DateTime.Now.AddDays(-pastXDays);
                    string outputDate = reportDate.ToString(dateFormat);

                    reportQuery += $"WHERE date >= '{outputDate}' ";
                    break;
                case ReportType.StartDateUptoTodayDate:
                    var startingDate = AnsiConsole.Ask<DateOnly>("[red]Starting date = [/]");
                    string fromDate = startingDate.ToString(dateFormat);
                    string todayDate = DateTime.Today.ToString(dateFormat);

                    reportQuery += $"WHERE date BETWEEN  '{fromDate}' AND '{todayDate}' ";
                    break;
                case ReportType.StartDateUptoEndDate:
                    var inputstartDate = AnsiConsole.Ask<DateOnly>("[red]Starting date = [/]");
                    string startDate = inputstartDate.ToString(dateFormat);
                    var endingDate = AnsiConsole.Ask<DateOnly>("[red]Upto date = [/]");
                    string endDate = endingDate.ToString(dateFormat);

                    reportQuery += $"WHERE date BETWEEN  '{startDate}' AND '{endDate}' ";

                    break;
                case ReportType.TotalOfGivenMonth:
                    var inputMonth = AnsiConsole.Ask<DateOnly>("[red]Enter the starting date of the month (e.g., 2024-11-01) = [/]");

                    int inputYear = inputMonth.Year;
                    int inputMonthNumber = inputMonth.Month;

                    reportQuery += $"WHERE strftime('%Y', date) = '{inputYear}' AND strftime('%m', date) = '{inputMonthNumber:D2}' ";


                    break;
                case ReportType.TotalOfAllTime:
                    // pass because the report already shows the full table for the given habit name
                    break;
                case ReportType.Today:
                    string dateOfToday = DateTime.Today.ToString(dateFormat);
                    reportQuery += $"WHERE date = '{dateOfToday}' ";
                    break;
                case ReportType.Exit:
                    exitMenu = true;
                    continue;
            }
            CloseUpQueryShowResult(reportQuery);
        }
        while (!exitMenu);

    }
}

