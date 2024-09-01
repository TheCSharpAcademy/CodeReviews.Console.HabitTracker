namespace HabitTracker;

public static class Queries
{
    public const string InsertRecordCommand = """
                                       INSERT INTO habits (date, habit, unit, quantity) 
                                       VALUES (@date, @habit, @unit, @quantity)
                                       """;
    public const string ViewAllRecordsCommand = "SELECT id, date, habit, unit, quantity FROM habits";
    public const string UpdateRecordCommand = """
                                       UPDATE habits 
                                       SET date = @updatedDate, habit = @updatedHabit, 
                                           unit = @updatedUnit, quantity = @updatedQuantity 
                                       WHERE id = @id
                                       """;
    public const string DeleteRecordCommand = "DELETE FROM habits WHERE id = @id";
    public const string CheckTableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;";
    public const string CreateTableCommand = """
                                              CREATE TABLE habits (
                                                  id INTEGER PRIMARY KEY,
                                                  date DATE NOT NULL,
                                                  habit TEXT NOT NULL,
                                                  unit TEXT NOT NULL,
                                                  quantity INT NOT NULL
                                              )
                                              """;
}