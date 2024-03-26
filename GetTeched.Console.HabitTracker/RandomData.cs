namespace habit_tracker;

internal class RandomData
{
    SqlCommands sqlCommands = new();
    internal void GenerateRandomData(string tableName = "", bool initial = true)
    {
        Random random = new Random();

        List<string> habits = new List<string>() { "Drinking Coffee", "Kilometers Run", "Active Minutes", "Steps Per Day", "Hours of Sleep" };
        List<string> unitName = new List<string>() { "Cups", "Kilometers", "Minutes", "Steps", "Hours" };
        List<int> units = new List<int>() { 8, 20, 120, 30000, 12 };

        int loggedDays; string measurementUnit; int day; int month; int year; string date; int randomValues;

        bool tableExists = false;


        if (initial)
        {
            for (int i = 0; i <= habits.Count - 1; i++)
            {
                loggedDays = random.Next(100, 365);
                tableName = habits[i].ToLower().Trim().Replace(" ", "_");
                measurementUnit = unitName[i];
                tableExists = sqlCommands.SqlInitialize(tableName, measurementUnit);
                
                if(!tableExists)
                {
                    for (int j = 1; j <= loggedDays; j++)
                    {
                        day = random.Next(1, 28);
                        month = random.Next(1, 12);
                        year = 23;
                        date = $"{day.ToString("D2")}-{month.ToString("D2")}-{year}";

                        randomValues = random.Next(1, units[i]);
                        sqlCommands.SqlInsertAction(tableName, date, randomValues);
                    }
                }
                
            }
        }
        else
        {
            loggedDays = random.Next(100, 365);
            for(int i = 0;i <= loggedDays; i++)
            {
                day = random.Next(1, 28);
                month = random.Next(1, 12);
                year = 23;
                date = $"{day.ToString("D2")}-{month.ToString("D2")}-{year}";

                randomValues = random.Next(1, 30);
                sqlCommands.SqlInsertAction(tableName, date, randomValues);
            }
        }
        

    }

}
