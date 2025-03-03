using Microsoft.Data.Sqlite;

namespace HabitTracker.StressedBread
{
    class AutomaticSeeding
    {
        DatabaseService databaseService = new();

        internal void IsTableEmpty()
        {
            int count = -1;
            string commandText = @"SELECT COUNT(*) FROM habits";

            using (SqliteDataReader? reader = databaseService.ExecuteRead(commandText))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        count = reader.GetInt32(0);
                    }
                }
            }

            if (count == 0)
            {
                Seed();
            }
        }
        internal void Seed()
        {
            string[,] habitList =
            {
                {"Drinking Water", "Glasses"},
                {"Running", "Km"},
                {"Reading", "Pages"},
                {"Exercise", "Minutes"},
                {"Sleep", "Hours"},
                {"Walking", "Steps"},
                {"Writing", "Words"},
                {"Meditating", "Minutes"},
                {"Guitar Practice", "Minutes"},
                {"Healthy Snacks", "Servings"}
            };

            for (int i = 0; i < habitList.GetLength(0); i++)
            {
                string name = habitList[i, 0];
                string unit = habitList[i, 1];

                string commandText = @"INSERT INTO habits (HabitName, Unit)
                           VALUES (@name, @unit)
                           ";
                List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                    new SqliteParameter(@"name", name),
                    new SqliteParameter(@"unit", unit)
            };
                databaseService.ExecuteCommand(commandText, parameters);
            }

            for (int i = 0; i < 100; i++)
            {
                Random random = new();

                int year = random.Next(2000, 2100);
                int month = random.Next(1, 12);
                int daysInMonth = DateTime.DaysInMonth(year, month);
                int day = random.Next(1, daysInMonth);
                int habitId = random.Next(1, 11);
                int quantity = random.Next(1, 100);
                string date = new DateTime(year, month, day).ToString("yyyy-MM-dd");

                string commandText = @"INSERT INTO habit_data (HabitId, Date, Quantity)
                           VALUES (@habitId, @date, @quantity)
                           ";
                List<SqliteParameter> parameters = new List<SqliteParameter>()
            {
                    new SqliteParameter(@"habitId", habitId),
                    new SqliteParameter(@"date", date),
                    new SqliteParameter(@"quantity", quantity)
            };
                databaseService.ExecuteCommand(commandText, parameters);
            }
        }
    }
}
