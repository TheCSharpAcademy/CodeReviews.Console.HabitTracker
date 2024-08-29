using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

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
                            HabitTracked = reader.GetInt32(2)
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

        int valueAchieved = 0;
        Console.WriteLine("Please insert the value achieved you want to track.");
        Program.InsertExitPrompt(exitChar);

        bool shouldExitToMenu = Program.AssignSelectionInput(ref valueAchieved, 0, 9999999, skipSelection: exitChar);
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
    internal static void GenerateReport(string habitName)
    {
        string updatingHabit = "";

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

                List<int> codeList = new List<int>();

                foreach (int y in differentYears)
                {
                    List<int> differentMonths = new List<int>();

                    foreach (DataRetreive dt in data)
                    {
                        if (dt.dateTimeFormatDate.Year == y)
                        {
                            int month = dt.dateTimeFormatDate.Month;
                            if (!differentMonths.Contains(month))
                            {
                                differentMonths.Add(month);
                            }
                        }
                    }

                    
                }
            }

            Console.Write("Press any key to return to the previous menu: ");
            Console.ReadKey();

            connection.Close();
        }
    }
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
}

public class DataRetreive
{
    public int Id { get; set; }
    public string Date { get; set; }
    public DateTime dateTimeFormatDate { get; set; }
    public int HabitTracked { get; set; }

}
public class ReportDataCoded
{
    public int occurrences { get; set; }
    public int highiestValue { get; set; }
    public int smallestValue { get; set; }
    public int MeanValue { get; set; }
    public int MedianValue { get; set; }
    public List<int> ModalValues { get; set; }
}