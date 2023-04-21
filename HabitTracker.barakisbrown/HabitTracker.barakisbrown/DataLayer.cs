namespace HabitTracker.barakisbrown;

public class DataLayer
{
    private readonly string DatabaseName = "readings.db";
    private readonly string TableName = "Sugar Readings";
    private string DataSource;

    public DataLayer() 
    {
        DataSource = $"Data Source={DatabaseName}";
        
    }
}
