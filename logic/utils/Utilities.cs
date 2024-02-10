using System.Globalization;
using System.Text;
using HabitLogger.enums;
using Spectre.Console;

namespace HabitLogger.logic.utils;

/// <summary>
/// The Utilities class provides utility methods for various operations.
/// </summary>
internal static class Utilities
{
    #region Inner Classes and Records

    /// <summary>
    /// The <see cref="ExitToMainException"/> class represents an exception that is thrown when an operation needs to exit to the main menu.
    /// </summary>
    public sealed class ExitToMainException(string message = "Exiting to main menu.") : Exception(message);

    /// <summary>
    /// Represents an exception that is thrown when the application needs to exit.
    /// </summary>
    public sealed class ExitFromAppException(string message = "Exiting the application.") : Exception(message);

    /// <summary>
    /// Represents the input data for generating a report.
    /// </summary>
    private sealed record ReportInputData(
        ReportType ReportType, int Id,
        string? Date = null, string? StartDate = null, string? EndDate = null, int? Month = null, int? Year = null
    );
    #endregion

    #region Methods

    /// <summary>
    /// Validates a number input from the user.
    /// </summary>
    /// <param name="message">The message to display to the user.</param>
    /// <param name="maximum">The maximum valid number.</param>
    /// <param name="minimum">The minimum valid number.</param>
    /// <returns>The validated number input from the user.</returns>
    internal static int ValidateNumber(string message = "Enter a positive number:", 
                                        int maximum = int.MaxValue, int minimum = 0)
    {
        int output;
        bool isValid;

        do
        {
            Console.WriteLine(message);
            var numberInput = Console.ReadLine();

            try
            {
                if (numberInput != null)
                {
                    CheckForZero(numberInput);
                }
            } 
            catch (ExitToMainException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            isValid = int.TryParse(numberInput, out output) && output >= minimum && output <= maximum;

            if (!isValid)
            {
                Console.WriteLine("\nInvalid input. ");
            }
            
        } while(!isValid);
        
        return output;
    }

    /// <summary>
    /// Validates a date inputted by the user. prompts user to enter a date in the past in the format "yyyy-MM-dd", will keep asking until a valid date is entered.
    /// </summary>
    /// <param name="message">The message to display to the user asking for the date.</param>
    /// <returns>A string representation of the validated date in the format "yyyy-MM-dd".</returns>
    internal static string ValidateDate(string message = "Enter the date (yyyy-MM-dd):")
    {
        DateTime dateValue;
        bool isValid;

        do
        {
            Console.WriteLine(message);
            var dateInput = Console.ReadLine();

            try
            {
                if (dateInput != null)
                {
                    CheckForZero(dateInput);
                }
            } 
            catch (ExitToMainException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
            isValid = DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dateValue) && dateValue <= DateTime.Now && dateValue >= DateTime.Now.AddYears(-1);


            if (!isValid)
            {
                Console.WriteLine(
                    "Invalid input or future date. Please enter a date in the past in the format 'dd-MM-yyyy'.");
            }
        } while (!isValid);

        return dateValue.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Validates the user input for text fields.
    /// </summary>
    /// <param name="str">The name of the text field.</param>
    /// <returns>The validated user input.</returns>
    internal static string ValidateTextInput(string str)
    {
        try
        {
            var input = AnsiConsole.Ask<string>($"Enter the {str}:");

            while (string.IsNullOrWhiteSpace(input))
            {
                input = AnsiConsole.Ask<string>($"Please enter a valid input for the {str}:");
            }
            
            CheckForZero(input);
            
            return input;
        } catch (ExitToMainException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    /// <summary>
    /// Constructs an SQL UPDATE query for a given database table.
    /// </summary>
    /// <param name="databaseName">The name of the database table to update.</param>
    /// <param name="parameters">A dictionary containing the column names and their corresponding values to update.</param>
    /// <returns>The constructed SQL UPDATE query.</returns>
    internal static string UpdateQueryBuilder(string databaseName, Dictionary<string, object> parameters)
    {
        StringBuilder query = new();
        var keysList = new List<string>(parameters.Keys).Except(new List<string> { "@id", "@Id" }).ToList();

        query.Append($"UPDATE {databaseName} SET");

        foreach (var key in keysList)
        {
            query.Append($" {string.Concat(key.Substring(1, 1).ToUpper(), key.AsSpan(2))} = {key},");
        }
        
        query.Remove(query.Length - 1, 1);
        query.Append(" WHERE Id = @id");
        
        return query.ToString();
    }

    /// <summary>
    /// Builds a query to retrieve records based on specified criteria.
    /// </summary>
    /// <param name="reportData">The input data for building the query.</param>
    /// <returns>A string representing the generated SQL query.</returns>
    private static string ReportQueryBuilder(ReportInputData reportData)
    {
        StringBuilder query = new();
        
        query.Append($"SELECT * FROM records WHERE");

        switch (reportData.ReportType)
        {
            case ReportType.DateToToday:
            case ReportType.DateToDate:
                query.Append($" Date");
                query.Append(reportData.ReportType == ReportType.DateToToday ? $" >= @date AND" : $" BETWEEN @startDate AND @endDate AND");
                break;   
            case ReportType.TotalForMonth:
                query.Append($" strftime('%m', Date) = @month AND strftime('%Y', Date) = @year AND");
                break;
            case ReportType.YearToDate:
            case ReportType.TotalForYear:
                query.Append(" strftime('%Y', Date) = @year");
                query.Append(reportData.ReportType == ReportType.YearToDate ? " AND Date <= @date AND" : " AND");
                break;
            case ReportType.Total:
            case ReportType.ReturnToMainMenu:
                break;
            default:
                Console.WriteLine("Problem with query builder occured (query section).");
                break;
        }
        
        query.Append(" HabitId = @id  ORDER BY Date ASC");
        
        return query.ToString();
    }

    /// <summary>
    /// Builds the dictionary of query parameters based on the report data.
    /// </summary>
    /// <param name="reportData">The report input data containing the report type, id, and optional date filters.</param>
    /// <returns>
    /// The dictionary of query parameters where the keys are the parameter names and the values are the parameter values.
    /// </returns>
    private static Dictionary<string, object> ReportQueryParametersBuilder(ReportInputData reportData)
    {
        Dictionary<string, object> parameters = new();
        
        switch (reportData.ReportType)
        {
            case ReportType.DateToToday:
                parameters.Add("@date", reportData.Date!);
                break;
            case ReportType.DateToDate:
                parameters.Add("@startDate", reportData.StartDate!);
                parameters.Add("@endDate", reportData.EndDate!);
                break;
            case ReportType.TotalForMonth:
                parameters.Add("@month", (reportData.Month ?? -1).ToString("00"));
                parameters.Add("@year", (reportData.Year ?? DateTime.Now.Year).ToString()); 
                break;
            case ReportType.YearToDate:
                parameters.Add("@year", reportData.Year.ToString()!);
                parameters.Add("@date", reportData.Date!);
                break;
            case ReportType.TotalForYear:
                parameters.Add("@year", (reportData.Year ?? DateTime.Now.Year).ToString());
                break;
            case ReportType.Total:
            case ReportType.ReturnToMainMenu:
                break;
            default:
                Console.WriteLine("Problem with query builder occured (Parameters section).");
                break; 
        }
        
        parameters.Add("@id", reportData.Id);

        return parameters;
    }

    /// <summary>
    /// Creates a report query based on the specified report type and ID.
    /// </summary>
    /// <param name="reportType">The type of report to create.</param>
    /// <param name="id">The ID of the report.</param>
    /// <returns>
    /// A tuple containing the generated query and parameters in the form:
    /// Item1 (string): The generated query.
    /// Item2 (Dictionary&lt;string, object&gt;): The query parameters.
    /// </returns>
    internal static (string? query, Dictionary<string, object>? parameters)
        CreateReportQuery(ReportType reportType, int id)
    {
        ReportInputData reportInputData;
        string? query;
        Dictionary<string, object>? parameters;
        string endDate;
        int year;
            
        switch (reportType)
        {
            case ReportType.DateToToday:
                var date = ValidateDate("Enter the start date (yyyy-MM-dd):");
                reportInputData = new ReportInputData(ReportType: reportType, Id: id, Date: date);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            case ReportType.DateToDate:
                var startDate = ValidateDate("Enter the start date (yyyy-MM-dd):");
                endDate = ValidateDate("Enter the end date (yyyy-MM-dd):");
                reportInputData = new ReportInputData(ReportType: reportType, Id: id, StartDate: startDate,
                    EndDate: endDate);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            case ReportType.TotalForMonth:
                var month = ValidateNumber("Enter the month (1-12):", maximum: 12);
                year = ValidateNumber("Enter the year (yyyy):", minimum: DateTime.Now.Year - 1,
                    maximum: DateTime.Now.Year);
                reportInputData = new ReportInputData(ReportType: reportType, Id: id, Month: month, Year: year);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            case ReportType.YearToDate:
                year = ValidateNumber("Enter the year (yyyy):", minimum: DateTime.Now.Year - 1,
                    maximum: DateTime.Now.Year);
                endDate = ValidateDate("Enter the end date (yyyy-MM-dd):");
                reportInputData = new ReportInputData(ReportType: reportType, Id: id, Date: endDate, Year: year);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            case ReportType.TotalForYear:
                year = ValidateNumber("Enter the year (yyyy):", minimum: DateTime.Now.Year - 1,
                    maximum: DateTime.Now.Year);
                reportInputData = new ReportInputData(ReportType: reportType, Id: id, Year: year);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            case ReportType.Total:
                reportInputData = new ReportInputData(ReportType: reportType, Id: id);

                query = ReportQueryBuilder(reportInputData);
                parameters = ReportQueryParametersBuilder(reportInputData);
                break;
            default:
                Console.WriteLine("No records found.");
                query = null;
                parameters = null;
                
                break;
        }

        return (query, parameters);
    }

    /// <summary>
    /// Checks if the input string is equal to "0" and throws an exception if it is.
    /// </summary>
    /// <param name="input">The input string to check.</param>
    private static void CheckForZero(string input)
    {
        if (input.Equals("0"))
        {
            throw new ExitToMainException();
        }
    }

    /// <summary>
    /// Saves the provided table as a CSV file.
    /// </summary>
    /// <param name="table">The table to be saved.</param>
    internal static void SaveReportToFile(Table table)
    {
        var reportName = table.Title?.Text;
        table.Title("");
        
        var textWriter = new StringWriter();
        
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(textWriter)
        });
        
        console.Write(table);
        console.WriteLine($"{reportName}\n");
        console.WriteLine($"Generated on {DateTime.Now:f}");

        var actualTime = DateTime.Now
                                .ToString("HH:mm:ss")
                                .Split(":")
                                .Aggregate((x, y) => x + "-" + y);
        
        File.WriteAllText($"report-{DateTime.Now.Date:yyyy-MM-dd}-{actualTime}.csv", textWriter.ToString());

        AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        
        AnsiConsole.WriteLine("Save complete.");
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    #endregion
}