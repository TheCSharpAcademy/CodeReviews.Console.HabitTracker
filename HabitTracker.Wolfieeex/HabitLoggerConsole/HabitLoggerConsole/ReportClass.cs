using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;

namespace HabitLoggerConsole;

internal class ReportClass
{
    internal static void GenerateReportV2(string habitName)
    {
        List<Tuple<int, ReportDataCoded[]>> data = GatherData(habitName);
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

                foreach (int year in years)
                {
                    ReportDataCoded[] yearlySummation = new ReportDataCoded[13];
                    for (int i = 0; i < yearlySummation.Length; i++)
                    {
                        yearlySummation[i] = new ReportDataCoded();
                    }

                    foreach (string currentlyCheckedMonth in Enum.GetNames<Months>())
                    {
                        tblCmd.CommandText = $"SELECT * FROM '{habitName}' WHERE Date LIKE '%{year}' AND Date LIKE '%{currentlyCheckedMonth}%'";
                        reader = tblCmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(1));
                            }
                        }
                        else
                        {
                            yearlySummation[(int)Enum.Parse(typeof(Months), currentlyCheckedMonth)] = new ReportDataCoded()
                            {
                                occurrences = null,
                                highiestValue = double.NaN,
                                smallestValue = double.NaN,
                                MeanValue = double.NaN,
                                MedianValue = double.NaN,
                                ModalValue = double.NaN
                            };
                        }
                        reader.Close();
                    }
                    data.Add(new Tuple<int, ReportDataCoded[]>(year, yearlySummation));
                }
                Console.ReadKey();
            }
            connection.Close();
        }
        return data;
    }

    public class ReportDataCoded
    {
        public int? occurrences
        {
            get; set;
        }
        public double highiestValue
        {
            get; set;
        }
        public double smallestValue
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
