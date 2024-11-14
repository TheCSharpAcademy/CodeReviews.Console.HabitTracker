using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace HabitLoggerConsole;

internal class ReportClass
{
    internal static void GenerateReportV2(string habitName)
    {
        bool[] options = new bool[8];

        options = ReportOptionsMenu(habitName);
        if (options == null)
        {
            return;
        }
        
        List<Tuple<int, ReportDataCoded[]>> data = GatherData(habitName);
        GenerateReportOnScreen(options, data);


        Console.Write($"Report for ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{HabitCommands.TableNameToDisplayableFormat(habitName)}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" habit report have been genereted. Press any key to return to the previous menu: ");
        Console.ReadKey();

    }
    private static bool[] ReportOptionsMenu(string habitName)
    {
        Console.Clear();
        bool[] userOptions = new bool[8];

        using (var connection = new SqliteConnection(Program.ConnectionString))
        {
            connection.Open();

            var tblCmd = connection.CreateCommand();
            tblCmd.CommandText = $"SELECT COUNT(*) FROM '{habitName}'";
            int recordsNumber = (int)(long)tblCmd.ExecuteScalar();

            if (recordsNumber < 10)
            {
                Console.WriteLine("No sufficient data to generate your report. Your habit has to have at least 10 insertions.");
                Console.Write("Please press any button to return to a previous menu: ");
                Console.ReadKey();
                return null;
            }
            connection.Close();
        }

        userOptions = UserOptionsData.RetreiveUserDefaultOptions();
        string[] menuStars = new string[8];

        for (int i = 0; i < 8; i++)
        {
            if (userOptions[i] == true)
            {
                menuStars[i] = "*";
            }
            else
            {
                menuStars[i] = " ";
            }
        }


        while (true)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("You are currently in the report options selection menu.");
            Console.ResetColor();
            Console.Write("Please select display options for each month for the ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{HabitCommands.TableNameToDisplayableFormat(habitName)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" report.\n");
            Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");

            Console.WriteLine($"[{menuStars[0]}] 0 - Display number of records.");
            Console.WriteLine($"[{menuStars[1]}] 1 - Display sum of records.");
            Console.WriteLine($"[{menuStars[2]}] 2 - Display maximal value.");
            Console.WriteLine($"[{menuStars[3]}] 3 - Display minimal value.");
            Console.WriteLine($"[{menuStars[4]}] 4 - Display mean value.");
            Console.WriteLine($"[{menuStars[5]}] 5 - Display median value.");
            Console.WriteLine($"[{menuStars[6]}] 6 - Display modal value.");
            Console.WriteLine($"[{menuStars[7]}] 7 - Display yearly summation.\n");
            Console.WriteLine($"    8 - Save those options as default.");
            Console.WriteLine($"    9 - Run the report.\n");

            Console.WriteLine($"{new string('-', Console.BufferWidth)}\n");

            Program.InsertExitPrompt(Program.exitChar, backMenuAlteration: true);
            int userInput = 0;

            bool exitMenu = Program.AssignSelectionInput(ref userInput, 0, 9, skipSelection: Program.exitChar);
            if (exitMenu)
            {
                return null;
            }

            switch (userInput)
            {
                case 8:
                    if (userInput == 8)
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;

                        var match = Regex.Match(UserOptionsData.path, @"(.*)(?=\\)");

                        System.IO.Directory.CreateDirectory(match.Value);
                        using (StreamWriter writeStream = File.CreateText(UserOptionsData.path))
                        {
                            serializer.Serialize(writeStream, userOptions);
                        }
                    }
                    Console.Clear();
                    Console.WriteLine("Your default options have been saved!");
                    break;
                case 9:
                    Console.Clear();
                    return userOptions;
                default:
                    Console.Clear();
                    if (userOptions[userInput] == true)
                    {
                        userOptions[userInput] = false;
                        menuStars[userInput] = " ";
                    }
                    else
                    {
                        userOptions[userInput] = true;
                        menuStars[userInput] = "*";
                    }
                    break;
            }    
        }
    }
    private static void GenerateReportOnScreen(bool[] options, List<Tuple<int, ReportDataCoded[]>> data)
    {
        int tableVerticalSpace = 0;
        for (int i = 0; i < 7; i++)
        {
            if (options[i] == true)
            {
                tableVerticalSpace++;
            }
        }

        Console.Clear();

        WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.BUp, 0, 1);
        WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.Blank, 0, 1);
        WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.BMiddle, 0, 1);

        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < tableVerticalSpace; j++)
            {
                WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.Blank, 0, 1);
            }
            if (i + 1 == data.Count)
            {
                WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.BDown, 0, 1);
            }
            else
            {
                WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.Middle, 0, 1);
            }
        }

        //  Create left edge content (years) and table edges:
        //      Top bit:
        Console.SetCursorPosition(1, 0);
        for (int i = 0; i < 7; i++)
        {
            WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BUp, 1, 0);
        }
        Console.SetCursorPosition(1, 2);
        for (int i = 0; i < 7; i++)
        {
            WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BMiddle, 1, 0);
        }

        //      Year bit:
        for (int i = 0; i < data.Count; i++)
        {
            int yearPosition = 0;
            yearPosition = tableVerticalSpace % 2 == 0 ? yearPosition = tableVerticalSpace / 2 : yearPosition = (tableVerticalSpace + 1) / 2;

            Console.SetCursorPosition(2, Console.CursorTop + yearPosition);
            Console.Write(data[i].Item1);
            Console.Write(":");

            //          Bottom edge:
            Console.SetCursorPosition(1, Console.CursorTop + tableVerticalSpace - yearPosition + 1);
            if (i + 1 == data.Count)
            {
                for (int j = 0; j < 7; j++)
                {
                    WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BDown, 1, 0);
                }
            }
            else
            {
                for (int j = 0; j < 7; j++)
                {
                    WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.Middle, 1, 0);
                }
            }
        }

        //      Close the year bit:
        Console.SetCursorPosition(8, 0);
        WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BUp, 0, 1);
        WriteTableWall(TableWallsHorizontal.Right, TableWallsVertical.Blank, 0, 1);
        WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BMiddle, 0, 1);

        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 0; j < tableVerticalSpace; j++)
            {
                WriteTableWall(TableWallsHorizontal.Right, TableWallsVertical.Blank, 0, 1);
            }
            if (i + 1 == data.Count)
            {
                WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BDown, 0, 1);
            }
            else
            {
                WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.Middle, 0, 1);
            }
        }

        //  Create table contents:

        int alterations = options[7] ? 13 : 12;
        for (int i = 0; i < alterations; i++)
        {
            Console.SetCursorPosition(Console.CursorLeft + 2, 1);
            int lineupPosition = Console.CursorLeft - 1;
            int longestInsert = 2;

            if (i == 12)
            {
                InputTableContent("Yearly:", ref longestInsert);
            }
            else
            {
                string currentMonth = Enum.GetName(typeof(Months), i);
                InputTableContent(currentMonth + ":", ref longestInsert);
            }
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 2);

            //  First, insert the data:
            for (int j = 0; j < data.Count; j++)
            {
                ReportDataCoded monthData = data[j].Item2[i];
                for (int k = 0; k < 7; k++)
                {
                    if (options[k] != true)
                    {
                        continue;
                    }

                    string? dataToPresent;
                    string presentator;

                    switch (k)
                    {
                        case 0:
                            if (monthData.Occurrences == null)
                            {
                                dataToPresent = "0";
                            }
                            else
                            {
                                dataToPresent = monthData.Occurrences.ToString();
                            }
                            presentator = "Records";
                            break;
                        case 1:
                            dataToPresent =  System.String.Format("{0:#.##}", monthData.Sum);
                            presentator = "Sum";
                            break;
                        case 2:
                            dataToPresent = System.String.Format("{0:#.##}", monthData.HighiestValue);
                            presentator = "Max";
                            break;
                        case 3:
                            dataToPresent = System.String.Format("{0:#.##}", monthData.SmallestValue);
                            presentator = "Min";
                            break;
                        case 4:
                            dataToPresent = System.String.Format("{0:#.##}", monthData.MeanValue);
                            presentator = "Mean";
                            break;
                        case 5:
                            dataToPresent = System.String.Format("{0:#.##}", monthData.MedianValue);
                            presentator = "Median";
                            break;
                        case 6:
                            dataToPresent = System.String.Format("{0:#.##}", monthData.ModalValue);
                            presentator = "Modal";
                            break;
                        default:
                            presentator = "?";
                            dataToPresent = "-";
                            break;
                    }
                    if (dataToPresent == "NaN" || dataToPresent == null)
                    {
                        dataToPresent = "-";
                    }
                    InputTableContent(presentator + ": " + dataToPresent, ref longestInsert);
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
            }

            // Then, create horizontal sides of the table:
            Console.SetCursorPosition(lineupPosition, 0);
            for (int j = 0; j < longestInsert; j++)
            {
                WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BUp, 1, 0);
            }
            Console.SetCursorPosition(lineupPosition, 2);

            for (int j = 0; j < longestInsert; j++)
            {
                WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BUp, 1, 0);
            }
            Console.SetCursorPosition(lineupPosition, Console.CursorTop + tableVerticalSpace + 1);

            for (int j = 0; j < data.Count; j++)
            {
                
                if (j + 1 != data.Count)
                {
                    for (int k = 0; k < longestInsert; k++)
                    {
                        WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.Middle, 1, 0);
                    }
                }
                else
                {
                    for (int k = 0; k < longestInsert; k++)
                    {
                        WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BDown, 1, 0);
                    }
                }
                Console.SetCursorPosition(lineupPosition, Console.CursorTop + tableVerticalSpace + 1);
            }
            //  Then, close the montly (yearly) section wiht a verical wall:
            Console.SetCursorPosition(lineupPosition + longestInsert, 0);

            if (i + 1 == alterations)
            {
                WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.BUp, 0, 1);
                WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.Blank, 0, 1);
                WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.BMiddle, 0, 1);
            }
            else
            {
                WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BUp, 0, 1);
                WriteTableWall(TableWallsHorizontal.Right, TableWallsVertical.Blank, 0, 1);
                WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BMiddle, 0, 1);
            }
            
            for (int j = 0; j < data.Count; j++)
            {
                for (int k = 0; k < tableVerticalSpace; k++)
                {
                    if (i + 1 == alterations)
                    {
                        WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.Blank, 0, 1);
                    }
                    else
                    {
                        WriteTableWall(TableWallsHorizontal.Right, TableWallsVertical.Blank, 0, 1);
                    }
                }
                if (j + 1 == data.Count)
                {
                    if (i + 1 == alterations)
                    {
                        WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.BDown, 0, 1);
                    }
                    else
                    {
                        WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.BDown, 0, 1);
                    }
                }
                else
                {
                    if (i + 1 == alterations)
                    {
                        WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.Middle, 0, 1);
                    }
                    else
                    {
                        WriteTableWall(TableWallsHorizontal.Middle, TableWallsVertical.Middle, 0, 1);
                    }
                }
            }
            if (i + 1 == alterations)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 2);
            }
            else
            {
                Console.SetCursorPosition(lineupPosition + longestInsert, 0);
            }
        }
    }
    public static void InputTableContent(string content, ref int inputLength)
    {
        int x = Console.CursorLeft;
        int y = Console.CursorTop;

        Console.WriteLine(content);
        if (content.Length + 2 > inputLength)
        {
            inputLength = content.Length + 2;
        }

        Console.SetCursorPosition(x, y);
    }
    public static List<Tuple<int, ReportDataCoded[]>> GatherData(string habitName)
    {
        List<Tuple<int, ReportDataCoded[]>> data = new List<Tuple<int, ReportDataCoded[]>>();

        using (var connection = new SqliteConnection(Program.ConnectionString))
        {
            connection.Open();

            var tblCmd = connection.CreateCommand();

            tblCmd.CommandText = $"SELECT COUNT(*) FROM (SELECT DISTINCT SUBSTR(Date, LENGTH(Date) - 3, 4) FROM '{habitName}')";

            tblCmd.CommandText = $"SELECT DISTINCT SUBSTR(Date, LENGTH(Date) - 3, 4) FROM '{habitName}'";

            SqliteDataReader reader = tblCmd.ExecuteReader();

            List<int> years = new List<int>();

            while (reader.Read())
            {
                years.Add(Convert.ToInt32(reader.GetString(0)));
            }
            reader.Close();

            years.Sort();

            tblCmd.CommandText = $"SELECT * FROM '{habitName}'";
            reader = tblCmd.ExecuteReader();
            reader.Read();
            string trackedName = reader.GetName(2);
            reader.Close();

            tblCmd.CommandText = $"SELECT DISTINCT SUBSTR(Date, LENGTH(Date) - 3, 4) FROM '{habitName}'";

            foreach (int year in years)
            {
                ReportDataCoded[] yearlySummation = new ReportDataCoded[13];
                for (int i = 0; i < yearlySummation.Length; i++)
                {
                    yearlySummation[i] = new ReportDataCoded();
                }

                foreach (string currentlyCheckedMonth in Enum.GetNames<Months>())
                {
                    DataCalculator(habitName, connection, tblCmd, trackedName, year, ref yearlySummation, false, currentlyCheckedMonth);
                }
                DataCalculator(habitName, connection, tblCmd, trackedName, year, ref yearlySummation, true);


                reader.Close();
                data.Add(new Tuple<int, ReportDataCoded[]>(year, yearlySummation));
            } 

            connection.Close();
        }
        return data;
    }
    private static void DataCalculator(string habitName, SqliteConnection connection, SqliteCommand tblCmd, string trackedName, int year, ref ReportDataCoded[] yearlySummation, bool yearlyReport, string currentlyCheckedMonth = "")
    {
        string yearlyCalculator = yearlyReport ? "" : $"AND Date LIKE '%{currentlyCheckedMonth}%'";

        SqliteDataReader reader;
        tblCmd.CommandText = $"SELECT * FROM '{habitName}' WHERE Date LIKE '%{year}'{yearlyCalculator}";
        reader = tblCmd.ExecuteReader();

        if (reader.HasRows)
        {
            double sumValue = 0;
            int occurrences = 0;
            double meanValue = 0;
            double modalValue = 0;
            double medianValue = 0;
            double highiestValue = 0;
            double smallestValue = 0;

            var summationTblCmd = connection.CreateCommand();

            summationTblCmd.CommandText = $"SELECT SUM({trackedName}) FROM '{habitName}' WHERE Date LIKE '%{year}'{yearlyCalculator}";
            sumValue = (double)summationTblCmd.ExecuteScalar();

            summationTblCmd.CommandText = $"SELECT COUNT(*) FROM '{habitName}' WHERE Date LIKE '%{year}'{yearlyCalculator}";
            occurrences = Convert.ToInt32(summationTblCmd.ExecuteScalar());

            meanValue = sumValue / occurrences;

            summationTblCmd.CommandText = $"SELECT MAX({trackedName}) FROM '{habitName}' WHERE DATE LIKE '%{year}'{yearlyCalculator}";
            highiestValue = (double)summationTblCmd.ExecuteScalar();

            summationTblCmd.CommandText = $"SELECT MIN({trackedName}) FROM '{habitName}' WHERE DATE LIKE '%{year}'{yearlyCalculator}";
            smallestValue = (double)summationTblCmd.ExecuteScalar();

            List<double> recordList = new List<double>();
            while (reader.Read())
            {
                recordList.Add(reader.GetDouble(2));
            }
            recordList.Sort();

            if (recordList.Count == 1)
            {
                modalValue = medianValue = 1;
            }
            else
            {
                if (recordList.Count() % 2 == 0)
                    medianValue = (recordList[recordList.Count / 2] + recordList[(recordList.Count / 2) - 1]) / 2;
                else
                    medianValue = recordList[(recordList.Count / 2) - 1];

                Dictionary<double, int> numericCount = new Dictionary<double, int>();

                foreach (double n in recordList)
                {
                    if (!numericCount.ContainsKey(n))
                        numericCount.Add(n, 1);
                    else
                        numericCount[n]++;
                }

                List<KeyValuePair<double, int>> numericCountList = numericCount.ToList();
                numericCountList = numericCountList.OrderBy(x => x.Value).ToList();

                if (numericCountList[0].Value == numericCountList[1].Value)
                    modalValue = double.NaN;
                else
                    modalValue = numericCountList[0].Value;                
            }

            if (yearlyReport)
            {
                yearlySummation[12] = new ReportDataCoded()
                {
                    Occurrences = occurrences,
                    Sum = sumValue,
                    HighiestValue = highiestValue,
                    SmallestValue = smallestValue,
                    MeanValue = meanValue,
                    MedianValue = medianValue,
                    ModalValue = modalValue
                };
            }
            else
            {
                yearlySummation[(int)Enum.Parse(typeof(Months), currentlyCheckedMonth)] = new ReportDataCoded()
                {
                    Occurrences = occurrences,
                    Sum = sumValue,
                    HighiestValue = highiestValue,
                    SmallestValue = smallestValue,
                    MeanValue = meanValue,
                    MedianValue = medianValue,
                    ModalValue = modalValue
                };
            }
        }
        else
        {
            yearlySummation[(int)Enum.Parse(typeof(Months), currentlyCheckedMonth)] = new ReportDataCoded()
            {
                Occurrences = null,
                Sum = double.NaN,
                HighiestValue = double.NaN,
                SmallestValue = double.NaN,
                MeanValue = double.NaN,
                MedianValue = double.NaN,
                ModalValue = double.NaN
            };
        }
        reader.Close();
    }
    internal static void WriteTableWall(TableWallsHorizontal horizontal, TableWallsVertical vertical, int moveRight, int moveDown)
    {
        switch (horizontal)
        {
            case TableWallsHorizontal.Left:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("┌");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("└");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╒");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╘");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("├");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╞");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("│");
                        break;
                }
                break;
            case TableWallsHorizontal.Right:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("┐");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("┘");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╕");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╛");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("┤");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╡");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("│");
                        break;
                }
                break;
            case TableWallsHorizontal.BRight:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("╖");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("╜");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╗");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╝");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("╢");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╣");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("║");
                        break;
                }
                break;
            case TableWallsHorizontal.BLeft:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("╓");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("╙");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╔");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╚");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("╟");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╠");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("║");
                        break;
                }
                break;
            case TableWallsHorizontal.Middle:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("┬");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("┴");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╤");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╧");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("┼");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╪");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("─");
                        break;
                }
                break;
            case TableWallsHorizontal.BMiddle:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("╥");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("╨");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("╦");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("╩");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("╫");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("╬");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write("═");
                        break;
                }
                break;
            case TableWallsHorizontal.Blank:
                switch (vertical)
                {
                    case TableWallsVertical.Up:
                        Console.Write("─");
                        break;
                    case TableWallsVertical.Down:
                        Console.Write("─");
                        break;
                    case TableWallsVertical.BUp:
                        Console.Write("═");
                        break;
                    case TableWallsVertical.BDown:
                        Console.Write("═");
                        break;
                    case TableWallsVertical.Middle:
                        Console.Write("─");
                        break;
                    case TableWallsVertical.BMiddle:
                        Console.Write("═");
                        break;
                    case TableWallsVertical.Blank:
                        Console.Write(" ");
                        break;
                }
                break;
        }
        Console.SetCursorPosition(Console.CursorLeft - 1 + moveRight, Console.CursorTop + moveDown);
    }
    public class ReportDataCoded
    {
        public int? Occurrences
        {
            get; set;
        }
        public double? Sum
        {
            get; set;
        }
        public double HighiestValue
        {
            get; set;
        }
        public double SmallestValue
        {
            get; set;
        }
        public double MeanValue
        {
            get; set;
        }
        public double MedianValue
        {
            get; set;
        }
        public double ModalValue
        {
            get; set;
        }
    }
    public enum TableWallsHorizontal
    {
        Right,
        Left,
        BRight,
        BLeft,
        Middle,
        BMiddle,
        Blank,
        Special
    }
    public enum TableWallsVertical
    {
        Up,
        Down,
        BUp,
        BDown,
        Middle,
        BMiddle,
        Blank,
        Special
    }
    public enum Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
