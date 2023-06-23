using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    internal class Statistics
    {
        private static string connectionString = @"Data Source=habit-tracker.db";

        /// <summary>
        /// This function returns all HabitRecords within a particular HabitTable that were entered between the start and end dates (inclusive)
        /// </summary>
        /// <param name="habitTable"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static List<HabitRecord> RecordBetweenDates(HabitTable habitTable, DateTime start, DateTime end)
        {
            List<HabitRecord> results = new List<HabitRecord>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM {habitTable.TableName} WHERE Date BETWEEN '{start.Date.ToString("yyyy-MM-dd")}' AND '{end.Date.ToString("yyyy-MM-dd")}'";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        results.Add(new HabitRecord(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2)));
                    }
                }

                connection.Close();
            }

            return results;
        }

        /// <summary>
        /// This function returns a float value that is the sum of the values in each record in the given table that lies between the given dates.
        /// If either of the dates provided is not given, then the entire table sum is calculated.
        /// If any values are determined to not be numeric, this value is simply ignored.
        /// </summary>
        /// <param name="habitTable"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static float SumOfValuesBetweenDates(HabitTable habitTable, DateTime? start = null, DateTime? end = null)
        {
            List<HabitRecord> results;

            if(start != null && end != null)
            {
                results = RecordBetweenDates(habitTable, start.Value, end.Value);
            }
            else
            {
                results = habitTable.GetAllRecords();
            }

            float sum = 0f;

            results.ForEach(record =>
            {
                float value;
                if(float.TryParse(record.Value, out value))
                {
                    sum += value;
                }
            });

            return sum;
        }
    }
}
