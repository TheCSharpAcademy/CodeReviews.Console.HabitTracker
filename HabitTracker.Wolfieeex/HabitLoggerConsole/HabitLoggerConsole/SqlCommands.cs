using Microsoft.Data.Sqlite;
using HabitLoggerConsole;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitLoggerConsole;

internal class SqlCommands
{
    private static char exitChar = 'E';

    internal static Dictionary<int, int> GetAllRecords(bool pauseScreening)
    {
        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            int lastRowIndex = 0;

            tableCmd.CommandText = $"SELECT * FROM going_to_gym";

            List<GoingToGym> tableData = new(); 
            Dictionary<int, int>? idMap = new Dictionary<int, int>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lastRowIndex++;
                    tableData.Add(
                        new GoingToGym
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                            Sets = reader.GetInt32(2)
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
                Console.WriteLine($"{lastRowIndex} - {dw.Date.ToString("dd-MMM-yyyy")} - Sets: {dw.Sets}");
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
    internal static void Insert()
    {
        string date = GetDateInput();
        if (date.ToLower() == exitChar.ToString().ToLower())
        {
            return;
        }

        Console.Clear();

        int numberOfSets = 0;
        Console.WriteLine("Please insert number of sets perfromed during the exercise: ");
        InsertExitPrompt(exitChar);

        bool shouldExitToMenu = Program.AssignSelectionInput(ref numberOfSets, 0, 999, skipSelection: exitChar);
        if (shouldExitToMenu)
        {
            return;
        }

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO going_to_gym(Date, Sets) VALUES('{date}', '{numberOfSets}')";
            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    internal static void Delete()
    {
        bool executeProgram = true;
        while (executeProgram)
        {
            var idMap = GetAllRecords(false);

            if (idMap == null)
            {
                Console.Write("There are no records to delete! Please press any key to return to the main menu: ");
                Console.ReadKey();
                return;
            }

            int lastRowId = idMap.Count;
            int selectedRow = 0;
            Console.WriteLine("Choose which record to delete by selecting its index number");
            InsertExitPrompt(exitChar);
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

                tableCmd.CommandText = $"DELETE FROM going_to_gym WHERE Id = '{rowCount}'";
                rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Console.Clear();
            Console.Write($"Record with index {selectedRow} has been deleted.\n");
        }
    }
    internal static void Update()
    {
        bool executeProgram = true;
        while (executeProgram)
        {
            var idMap = GetAllRecords(false);

            if (idMap == null)
            {
                Console.Write("There are no records to update! Please press any key to return to the main menu: ");
                Console.ReadKey();
                return;
            }

            int lastRowId = idMap.Count;
            int selectedRow = 0;
            Console.WriteLine("Choose which record to update by selecting its index number");
            InsertExitPrompt(exitChar);
            bool shouldExit = Program.AssignSelectionInput(ref selectedRow, 1, lastRowId, skipSelection: exitChar);
            if (shouldExit)
            {
                return;
            }

            int rowCount = idMap[selectedRow];
            using (var connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();

                string date = GetDateInput();
                if (date.ToLower() == exitChar.ToString().ToLower())
                {
                    return;
                }

                int sets = 0;
                shouldExit = Program.AssignSelectionInput(ref sets, 1, 999, skipSelection: exitChar);
                if (shouldExit)
                {
                    return;
                }

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE going_to_gym SET Date = '{date}', Sets = '{sets}' WHERE Id = '{rowCount}'";

                connection.Close();
            }

            Console.Clear();
            Console.WriteLine($"Record with index {selectedRow} has been updated.");
        }
    }

    private static string GetDateInput()
    {
        Console.WriteLine("Please insert the date of the operation (Format that is accepted: dd-mm-yyyy).");
        InsertExitPrompt(exitChar);

        string? dateInput = Console.ReadLine();
        string optional = exitChar.ToString().ToLower();

        while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _) && dateInput.ToLower() != optional)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write($"{new string(' ', Console.BufferWidth)}");
            Console.SetCursorPosition(0, Console.CursorTop);

            Console.Write($"Invalid option. Please insert a date in a correct format or choose '{exitChar}' to return to main menu: ");
            dateInput = Console.ReadLine();
        }

        return dateInput;
    }

    private static void InsertExitPrompt(char exitChar)
    {
        Console.WriteLine($"Optionally, insert '{exitChar}' to return to the main menu.");
        Console.Write("\nYour option: ");
    }
}

public class GoingToGym
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Sets { get; set; }
}