using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace habit_tracker;

internal class RandomData
{
    SqlCommands sqlCommands = new();
    internal void GenerateRandomData(string tableName)
    {
        Random random = new Random();
        int day = random.Next(1, 28);
        int month = random.Next(1, 12);
        int year = random.Next(24, 25);

        int[] values = GetMinMaxRandomValues();
        int minimumRandom = values[0];
        int maximumRandom = values[1];

        string date = $"{day}-{month}-{year}";

        for(int i = 0; i < 365;  i++)
        {
            int randomValues = random.Next(minimumRandom, maximumRandom);
            sqlCommands.SqlInsertAction(tableName,date,randomValues);
        }


    }

    internal int[] GetMinMaxRandomValues()
    {
        int[] values = new int[2];
        Console.WriteLine("What is your minimum unit threshold you would like to generate?");
        string userInput = Console.ReadLine();
        while (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out _))
        {
            Console.WriteLine("You did not enter a number value, please try again.");
            userInput = Console.ReadLine();
        }
        values[0] = Convert.ToInt32(userInput);
        Console.WriteLine("What is your maximum unit threshold you would like to generate?");
        userInput = Console.ReadLine();
        while (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out _))
        {
            Console.WriteLine("You did not enter a number value, please try again.");
            userInput = Console.ReadLine();
        }
        values[1] = Convert.ToInt32(userInput);

        return values;
    }
}
