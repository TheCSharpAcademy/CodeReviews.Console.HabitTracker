using System.Runtime.CompilerServices;
using System.Xml;
using static HabitTracker.S1m0n32002.Models.Habit;

namespace HabitTracker.S1m0n32002.Models
{
    public class Habit
    {
        public const string TabName = "habits";

        public enum Periodicities { None = -1, Daily = 1, Weekly, Monthly, Yearly }

        /// <summary>
        /// Id of habit
        /// </summary>
        public int Id { get; set; } = -1;
        /// <summary>
        /// Name of habit
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Periodicities of habit
        /// </summary>
        public Periodicities Periodicity { get; set; } = Periodicities.None;

        /// <summary>
        /// Average times per period
        /// </summary>
        public double TimesPerPeriod
        {
            get
            {
                if (Occurrences.Count == 0) return 0;

                List<int> timesPerSpan = [];

                switch (Periodicity)
                {
                    case Periodicities.Daily:
                        foreach (var item in Occurrences.CountBy(x => x.Date.Date))
                            timesPerSpan.Add(item.Value);
                        break;

                    case Periodicities.Weekly:
                        // from copilot
                        var bySpan = Occurrences.CountBy(x => new
                        {
                            x.Date.Year,
                            Week = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(x.Date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                        });

                        foreach (var item in bySpan)
                            timesPerSpan.Add(item.Value);
                        break;

                    case Periodicities.Monthly:
                        foreach (var item in Occurrences.CountBy(x => new DateTime(x.Date.Year, x.Date.Month, 1)))
                            timesPerSpan.Add(item.Value);
                        break;

                    case Periodicities.Yearly:
                        foreach (var item in Occurrences.CountBy(x => new DateTime(x.Date.Year, 1, 1)))
                            timesPerSpan.Add(item.Value);
                        break;

                    case Periodicities.None:
                        throw new InvalidOperationException("Periodicity was not set");
                    default:
                        throw new Exception("Invalid periodicity");
                }

                return timesPerSpan.Average();
            }
        }

        /// <summary>
        /// List of dates when habit was done
        /// </summary>
        public List<Occurrence> Occurrences { get; set; } = [];
        
        /// <summary>
        /// Class that represents the occurrence of a habit
        /// </summary>
        public class Occurrence
        {
            public const string TabName = "occurrences";

            /// <summary>
            /// Id of occurrance
            /// </summary>
            public int Id { get; set; } = -1;
            /// <summary>
            /// date when habit was done
            /// </summary>
            public DateTime Date { get; set; } = new DateTime(0);
            /// <summary>
            /// Id of habit
            /// </summary>
            public int HabitId { get; set; } = -1;

        }
    }
}
