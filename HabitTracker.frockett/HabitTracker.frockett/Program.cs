using Microsoft.Data.Sqlite;
using System.Globalization;
using ConsoleTableExt;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitTracker.frockett;

internal class Program
{

    static void Main(string[] args)
    {
        dbOperations db = new dbOperations();
        UserInput input = new UserInput();

        db.SeedTestData();
        input.GetUserInput();
    }


}
