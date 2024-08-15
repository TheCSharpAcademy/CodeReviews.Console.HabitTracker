using HabitLogger.Models;
class Application
{
    private static void Main()
    {
        const string DBNAME = "habit.db";
        string s_path = GetFilePath(DBNAME);
        try
        {

            DBStorage dB = new DBStorage(s_path, DBNAME);
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static string GetCurrentPath()
    {
        return Environment.CurrentDirectory.Replace("bin/Debug/net7.0", "");
    }

    private static string GetFilePath(string tableName)
    {
        return Path.Combine(GetCurrentPath(), tableName);
    }
}