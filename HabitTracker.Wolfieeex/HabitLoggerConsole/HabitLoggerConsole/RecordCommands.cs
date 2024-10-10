using HabitLoggerConsole.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using System.Numerics;

namespace HabitLoggerConsole;

internal class RecordCommands
{
    private static char exitChar = 'E';

    internal static Dictionary<int, int> GetAllRecords(bool pauseScreening, string habit)
    {
        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            int lastRowIndex = 0;

            tableCmd.CommandText = $"SELECT * FROM '{habit}'";

            string? measurementType = null;
            string? habitName = null;
            List<DataRetreive> tableData = new(); 
            Dictionary<int, int>? idMap = new Dictionary<int, int>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                bool accessOnce = true;
                while (reader.Read())
                {
                    if (accessOnce)
                    {
                        accessOnce = false;
                        measurementType = reader.GetName(3);
                        habitName = reader.GetName(2);
                    }
                    lastRowIndex++;
                    tableData.Add(
                        new DataRetreive
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            HabitTracked = reader.GetDouble(2)
                        });

                    idMap.Add(lastRowIndex, reader.GetInt32(0));
                }
            }
            else
            {
                connection.Close();
                if (!pauseScreening)
                {
                    return null;
                }
                Console.Write("No rows found! Please press any key to return to the main menu: ");
                Console.ReadKey();
                return null;
            }

            connection.Close();

            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine();
            lastRowIndex = 0;
            foreach (var dw in tableData)
            {
                lastRowIndex++;
                Console.Write($"{(lastRowIndex + " - " + dw.Date).PadRight(40)} - {HabitCommands.TableNameToDisplayableFormat(habitName)}: {dw.HabitTracked}");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{measurementType}");
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.WriteLine($"{new string('-', Console.BufferWidth)}");
            Console.WriteLine();

            if (pauseScreening)
            {
                Console.Write("Please press any key to return to the main menu: ");
                Console.ReadKey();
            }
            return idMap;
        }
    }
    internal static void Insert(string habit)
    {
        string date = GetDateInput();
        if (date.ToLower() == exitChar.ToString().ToLower())
        {
            return;
        }

        Console.Clear();

        double valueAchieved = 0;
        Console.WriteLine("Please insert the value achieved you want to track.");
        Program.InsertExitPrompt(exitChar);

        bool shouldExitToMenu = Program.AssingDoubleInput(ref valueAchieved, 0, 9999999, skipSelection: exitChar);
        if (shouldExitToMenu)
        {
            return;
        }

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM '{habit}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            string habitTrackingName = reader.GetName(2);

            reader.Close();

            tableCmd.CommandText = $"INSERT INTO '{habit}'(Date, '{habitTrackingName}') VALUES('{date}', '{valueAchieved}')";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void Delete(string habit)
    {
        bool executeProgram = true;
        while (executeProgram)
        {
            var idMap = GetAllRecords(false, habit);

            if (idMap == null)
            {
                Console.Write("There are no records to delete! Please press any key to return to the main menu: ");
                Console.ReadKey();
                return;
            }

            int lastRowId = idMap.Count;
            int selectedRow = 0;
            Console.WriteLine("Choose which record to delete by selecting its index number.");
            Program.InsertExitPrompt(exitChar);
            bool shouldExit = Program.AssignSelectionInput(ref selectedRow, 1, lastRowId, skipSelection: exitChar);
            if (shouldExit)
            {
                return;
            }

            int rowCount = idMap[selectedRow];
            using (var connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM '{habit}' WHERE Id = '{rowCount}'";
                rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.Clear();
            Console.Write($"Record with index {selectedRow} has been deleted.\n");
        }
    }
    internal static void Update(string habit)
    {
        bool executeProgram = true;
        while (executeProgram)
        {
            var idMap = GetAllRecords(false, habit);

            if (idMap == null)
            {
                Console.Write("There are no records to update! Please press any key to return to the main menu: ");
                Console.ReadKey();
                return;
            }

            int lastRowId = idMap.Count;
            int selectedRow = 0;
            Console.WriteLine("Choose which record to update by selecting its index number.");
            Program.InsertExitPrompt(exitChar);
            bool shouldExit = Program.AssignSelectionInput(ref selectedRow, 1, lastRowId, skipSelection: exitChar);
            if (shouldExit)
            {
                return;
            }

            int rowCount = idMap[selectedRow];
            Console.Clear();
            shouldExit = RunUpdateMenu(habit, selectedRow, rowCount);
        }
    }
    /*internal static void GenerateReport(string habitName)
    {
        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var tblCmd = connection.CreateCommand();
            tblCmd.CommandText = $"SELECT COUNT(*) FROM '{habitName}'";
            int recordsNumber = (int)(long)tblCmd.ExecuteScalar();

            if (recordsNumber < 10)
            {
                Console.WriteLine("No sufficient data to generate your report. Your habit has to have at least 10 insertions.");
            }
            else
            {
                tblCmd.CommandText = $"SELECT * FROM '{habitName}'";

                SqliteDataReader reader = tblCmd.ExecuteReader();

                string valueTrackedName = "";
                string measurementType = "";
                List<DataRetreive> data = new List<DataRetreive>();

                reader.Read();
                valueTrackedName = reader.GetName(2);
                measurementType = reader.GetName(3);

                data.Add(new DataRetreive
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetString(1),
                    dateTimeFormatDate = DateTime.Parse(reader.GetString(1)),
                    HabitTracked = reader.GetInt32(2)
                });

                while (reader.Read())
                {
                    data.Add(new DataRetreive
                    {
                        Id = reader.GetInt32(0),
                        Date = reader.GetString(1),
                        dateTimeFormatDate = DateTime.Parse(reader.GetString(1)),
                        HabitTracked = reader.GetInt32(2)
                    });
                }
                
                List<int> differentYears = new List<int>();

                foreach (DataRetreive dt in data)
                {
                    int year = dt.dateTimeFormatDate.Year;
                    if (!differentYears.Contains(year))
                    {
                        differentYears.Add(year);
                    }
                }

                differentYears.Sort();

                Dictionary<int, ReportDataCoded[]> report = new Dictionary<int, ReportDataCoded[]>();

                foreach (int y in differentYears)
                {
                    Dictionary<int, List<double>> valuesByMonth = new Dictionary<int, List<double>>();
                    for (int i = 1; i <= 12; i++)
                    {
                        valuesByMonth.Add(i, new List<double>());
                    }

                    foreach (DataRetreive dt in data)
                    {
                        if (y == dt.dateTimeFormatDate.Year)
                        {
                            int month = dt.dateTimeFormatDate.Month;
                            valuesByMonth[month].Add(dt.HabitTracked);
                        }
                    }

                    ReportDataCoded[] currentYearReport = new ReportDataCoded[12];
                    for (int i = 0; i < 12; i++)
                    {
                        currentYearReport[i] = new ReportDataCoded();
                    }

                    foreach (var monthBreakDown in valuesByMonth)
                    {
                        var arrayValue = monthBreakDown.Key - 1;
                        var currentMonthList = monthBreakDown.Value;

                        if (currentMonthList.Any())
                        {
                            currentYearReport[arrayValue].highiestValue = currentMonthList.Max();

                            currentYearReport[arrayValue].smallestValue = currentMonthList.Min();

                            int recordCount = currentYearReport[arrayValue].occurrences = currentMonthList.Count();

                            double recordSum = currentMonthList.Sum();
                            currentYearReport[arrayValue].MeanValue = (double)recordSum / recordCount;

                            currentMonthList.Sort();

                            if (recordCount == 1)
                            {
                                currentYearReport[arrayValue].MedianValue = currentMonthList[0];
                            }
                            else if (recordCount % 2 == 0)
                            {
                                currentYearReport[arrayValue].MedianValue = ((double)currentMonthList[recordCount / 2] + currentMonthList[(recordCount / 2) - 1]) / 2.0d;
                            }
                            else
                            {
                                currentYearReport[arrayValue].MedianValue = (double)currentMonthList[((recordCount - 1) / 2)];
                            }

                            if (recordCount == 1)
                            {
                                currentYearReport[arrayValue].ModalValue = currentMonthList[0];
                            }
                            else
                            {
                                Dictionary<double, int> valueOccurrences = new Dictionary<double, int>();
                                foreach (double val in currentMonthList)
                                {
                                    if (!valueOccurrences.ContainsKey(val))
                                        valueOccurrences.Add(val, 1);
                                    else
                                        valueOccurrences[val]++;
                                }

                                if (valueOccurrences.Any())
                                {
                                    valueOccurrences = valueOccurrences.OrderByDescending(x => x.Value).ToDictionary<double, int>();
                                    var list = valueOccurrences.ToList<KeyValuePair<double, int>>();

                                    if (valueOccurrences.Count == 1)
                                    {
                                        currentYearReport[arrayValue].MedianValue = list[0].Key;
                                    }
                                    else
                                    {
                                        if (list[0].Key != list[1].Key)
                                            currentYearReport[arrayValue].MedianValue = list[0].Key;
                                        else
                                            currentYearReport[arrayValue].ModalValue = double.NaN;
                                    }
                                }
                                else
                                {
                                    currentYearReport[arrayValue].ModalValue = double.NaN;
                                }
                            }
                        }
                        else
                        {
                            currentYearReport[arrayValue] = new ReportDataCoded
                            {
                                occurrences = 0,
                                highiestValue = double.NaN,
                                smallestValue = double.NaN,
                                MeanValue = double.NaN,
                                MedianValue = double.NaN,
                                ModalValue = double.NaN,
                            };
                        }
                    }
                    report.Add(y, currentYearReport);
                }

                int longestWord = FindLongestWord(report);
                PopulateTable(report, 20);
                Console.ReadKey();
                BuildReportTable(20, 3);
                Console.ReadKey();
            }

            Console.Write("Press any key to return to the previous menu: ");
            Console.ReadKey();

            connection.Close();
        }
    }*/
    private static bool RunUpdateMenu(string habit, int selectedRow, int rowCount)
    {
        bool shouldExit;
        while (true)
        {
            using (var connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT * FROM '{habit}' WHERE Id = '{rowCount}'";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                reader.Read();
                string recordDate = reader.GetString(1);
                string recordValue = reader.GetString(2);
                string recordValueName = reader.GetName(2);
                reader.Close();

                Console.Write($"You are updating ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{HabitCommands.TableNameToDisplayableFormat(habit).ToLower()}");
                Console.ResetColor();
                Console.WriteLine($" habit with record inserted on {System.String.Format(recordDate, "dddd, dd MMMM, yyyy")} with its value {recordValue}.\n");
                Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");
                Console.WriteLine("0 - Update date");
                Console.WriteLine("1 - Update value achieved\n");
                Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");
                Program.InsertExitPrompt(exitChar, backMenuAlteration: true);

                int userInput = -1;
                shouldExit = Program.AssignSelectionInput(ref userInput, 0, 1, skipSelection: exitChar);
                if (shouldExit)
                {
                    connection.Close();
                    break;
                }

                switch (userInput)
                {
                    case 0:
                        Console.Clear();
                        string date = GetDateInput();
                        if (date.ToLower() == exitChar.ToString().ToLower())
                        {
                            Console.Clear();
                            break;
                        }

                        tableCmd.CommandText = $"UPDATE '{habit}' SET Date = '{date}' WHERE Id = '{rowCount}'";
                        tableCmd.ExecuteNonQuery();

                        Console.Clear();
                        Console.WriteLine($"Record with index {selectedRow} has been updated.\n");

                        break;
                    case 1:
                        Console.Clear();
                        int updatedValueAchieved = 0;
                        Console.WriteLine("Please insert an updated value that was achieved on that time.");
                        Program.InsertExitPrompt(exitChar);
                        shouldExit = Program.AssignSelectionInput(ref updatedValueAchieved, 1, 9999999, skipSelection: exitChar);
                        if (shouldExit)
                        {
                            Console.Clear();
                            break;
                        }
                        tableCmd.CommandText = $"UPDATE '{habit}' SET '{recordValueName}' = '{updatedValueAchieved}' WHERE Id = '{rowCount}'";
                        tableCmd.ExecuteNonQuery();

                        Console.Clear();
                        Console.WriteLine($"Record with index {selectedRow} has been updated.\n");

                        break;
                }
                connection.Close();
            }
        }
        return shouldExit;
    }
    private static string GetDateInput()
    {
        Console.WriteLine("Please insert the date of the operation, or type in \"Now\" to accept today's date instead:");
        Program.InsertExitPrompt(exitChar);

        string? dateInput = Console.ReadLine();
        string optional = exitChar.ToString().ToLower();

        while (!DateTime.TryParse(dateInput, out _) && dateInput.ToLower() != optional && dateInput.ToLower() != "now")
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"{new string(' ', Console.BufferWidth)}");
            Console.SetCursorPosition(0, Console.CursorTop);

            Console.Write($"Invalid option. Please insert a date in a correct format, or type in \"Now\" to accept today's date: ");
            dateInput = Console.ReadLine();
        }

        if (dateInput.ToLower() == "now")
        {
            dateInput = DateTime.Now.ToString();
        }

        if (dateInput.ToLower() != optional)
        {
            var date = DateTime.Parse(dateInput, new CultureInfo("en-GB"), DateTimeStyles.None);
            dateInput = date.ToString("dddd, dd MMMM, yyyy");
        }

        return dateInput;
    }
    /*private static void BuildReportTable(int longestWord, int numberOfYears)
    {
        int startCursorPositionX = Console.CursorLeft;
        int startCursorPositionY = Console.CursorTop;
        
        
        
        Console.WriteLine($"{new string('_', 16 + (13 * (longestWord + 3)))}");
        Console.Write("|");
        Console.SetCursorPosition(Console.CursorLeft + 14, Console.CursorTop);
        Console.Write("|");
        for (int i = 0; i < 13; i++)
        {
            Console.SetCursorPosition(Console.CursorLeft + longestWord + 2, Console.CursorTop);
            Console.Write("|");
        }
        Console.WriteLine();
        Console.WriteLine($"{new string('_', 16 + (13 * (longestWord + 3)))}");
        for (int i = 0; i < numberOfYears; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Console.Write("|");
                Console.SetCursorPosition(Console.CursorLeft + 14, Console.CursorTop);
                Console.Write("|");
                for (int k = 0; k < 13; k++)
                {
                    Console.SetCursorPosition(Console.CursorLeft + longestWord + 2, Console.CursorTop);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"{new string('_', 16 + (13 * (longestWord + 3)))}");
        }

        //Console.SetCursorPosition(startCursorPositionX, startCursorPositionY);
    }
    private static void PopulateTable(Dictionary<int, ReportDataCoded[]> report, int longestWord)
    {
        Console.SetBufferSize(20 + (13 * (longestWord + 3)), 600);

        int startCursorPositionX = Console.CursorLeft;
        int startCursorPositionY = Console.CursorTop;
        Console.SetCursorPosition(startCursorPositionX + 1, startCursorPositionY + 1);

        string lineString = "";
        lineString += $"Year\u2193   Month\u2192 ";
        for (int i = 0; i < 12; i++)
        {
            lineString += (Enum.GetName(typeof(Months), (Months)i) + ":").PadRight(longestWord + 3);
        }
        lineString += $"Total for the year:";
        Console.WriteLine(lineString);

        Console.SetCursorPosition(startCursorPositionX, startCursorPositionY);
    }
    private static int FindLongestWord(Dictionary<int, ReportDataCoded[]> report)
    {
        return 0;
    }*/
}

public class DataRetreive
{
    public int Id { get; set; }
    public string Date { get; set; }
    public DateTime dateTimeFormatDate { get; set; }
    public double HabitTracked { get; set; }

}

