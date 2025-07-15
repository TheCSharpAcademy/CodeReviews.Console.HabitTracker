namespace habit_logger;

public static class Constants
{
    //name of the database file
    public const string ConnectionString = @"Data Source=habit-logger.db";
    // setting this to true generates generic habits and random habit records for them.
    // numbers are totally random between 1-999
    public const bool DebugMode = true;
}