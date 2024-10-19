using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitLoggerConsole;

internal class ReportClass
{
    internal static void GenerateReportV2(string habitName)
    {
        //ReportOptionsMenu();

        bool[] options = new bool[6];
        List<Tuple<int, ReportDataCoded[]>> data = GatherData(habitName);
        GenerateReportOnScreen(options, data);

    }
    private static void ReportOptionsMenu()
    {
        throw new NotImplementedException();
    }
    private static void GenerateReportOnScreen(bool[] options, List<Tuple<int, ReportDataCoded[]>> data)
    {
        WriteTableWall(TableWallsHorizontal.BLeft, TableWallsVertical.BUp);
        WriteTableWall(TableWallsHorizontal.Blank, TableWallsVertical.BUp);
        WriteTableWall(TableWallsHorizontal.BRight, TableWallsVertical.BUp);
        Console.ReadKey();
    }
    public static List<Tuple<int, ReportDataCoded[]>> GatherData(string habitName)
    {
        List<Tuple<int, ReportDataCoded[]>> data = new List<Tuple<int, ReportDataCoded[]>>();
        CultureInfo culture = new CultureInfo("en-GB");

        int longestInsert = 3;

        using (var connection = new SqliteConnection(Program.connectionString))
        {
            connection.Open();

            var tblCmd = connection.CreateCommand();
            tblCmd.CommandText = $"SELECT COUNT(*) FROM '{habitName}'";
            int recordsNumber = (int)(long)tblCmd.ExecuteScalar();

            if (recordsNumber < 10)
            {
                Console.WriteLine("No sufficient data to generate your report. Your habit has to have at least 10 insertions.");
                return null;
            }
            else
            {
                int numberOfYears = 0;
                int[] longestWordInColumn = new int[12];

                tblCmd.CommandText = $"SELECT COUNT(*) FROM (SELECT DISTINCT SUBSTR(Date, LENGTH(Date) - 3, 4) FROM '{habitName}')";
                int differentYearsCount = (int)(long)tblCmd.ExecuteScalar();

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
                Console.ReadKey();
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

            ReportDataCoded currentMonthSummation = new ReportDataCoded();
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

                if (yearlyReport)
                {
                    yearlySummation[12] = new ReportDataCoded()
                    {
                        Occurrences = occurrences,
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
                        HighiestValue = highiestValue,
                        SmallestValue = smallestValue,
                        MeanValue = meanValue,
                        MedianValue = medianValue,
                        ModalValue = modalValue
                    };
                }
            }
        }
        else
        {
            yearlySummation[(int)Enum.Parse(typeof(Months), currentlyCheckedMonth)] = new ReportDataCoded()
            {
                Occurrences = null,
                HighiestValue = double.NaN,
                SmallestValue = double.NaN,
                MeanValue = double.NaN,
                MedianValue = double.NaN,
                ModalValue = double.NaN
            };
        }
        reader.Close();
    }
    internal static void WriteTableWall(TableWallsHorizontal horizontal, TableWallsVertical vertical)
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
                        Console.Write("╫");
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
                        Console.Write("");
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
    }
    public class ReportDataCoded
    {
        public int? Occurrences
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
        Blank
    }
    public enum TableWallsVertical
    {
        Up,
        Down,
        BUp,
        BDown,
        Middle,
        BMiddle,
        Blank
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
