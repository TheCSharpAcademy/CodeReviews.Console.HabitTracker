namespace FunRun.HabitTracker.Data.Model;

public static class HabitTable
{
    //TODO: maybe using a struc/subclass for each column
    // columnName, ColumnType, CreationOption, etc...

    public static string TableName = "HabitTable";
    public static string Id = "Id";
    public static string HabitName = "HabitName";
    public static string HabitDescription = "HabitDescription";
    public static string HabitCounter = "HabitCounter";

    public static string SqlCreateTable()
    {
        
        string tableCreateStatment = $@"
            CREATE TABLE IF NOT EXISTS {TableName}(
                {Id} INTEGER PRIMARY KEY AUTOINCREMENT,
                {HabitName} NVARCHAR(100) NOT NULL,
                {HabitDescription} NVARCHAR(250),
                {HabitCounter} INT NOT NULL
            );
        ";

        return tableCreateStatment;
    }

}
