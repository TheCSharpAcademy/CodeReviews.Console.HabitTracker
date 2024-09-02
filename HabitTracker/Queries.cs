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
    
    public const string SelectDistinctHabitsCommand = "SELECT DISTINCT habit, unit FROM habits;";
    
    public const string CountTotalOccurrencesOfTheHabit =
        "SELECT COUNT(id) FROM habits WHERE habit=@habit AND unit=@unit;";
    
    public const string CountTotalQuantityOfTheHabit =
        "SELECT SUM(quantity) FROM habits WHERE habit=@habit AND unit=@unit;";
    
    public const string CountTotalOccurrencesOfTheHabitThisYear =
        "SELECT COUNT(id) FROM habits WHERE habit=@habit AND unit=@unit AND DATE BETWEEN @startDate AND @endDate;";
    
    public const string CountTotalQuantityOfTheHabitThisYear =
        "SELECT SUM(quantity) FROM habits WHERE habit=@habit AND unit=@unit AND DATE BETWEEN @startDate AND @endDate;";

    public const string GetLongestDailyStreakOfHabit = """
                                                  WITH HabitDates AS (
                                                      SELECT
                                                        date(date) AS habit_date
                                                      FROM
                                                        habits
                                                      WHERE
                                                        habit = @habit AND unit=@unit
                                                  ),
                                                  DateWithRowNumber AS (
                                                      SELECT
                                                        habit_date,
                                                        ROW_NUMBER() OVER (ORDER BY habit_date) AS rn
                                                      FROM
                                                        HabitDates
                                                  ),
                                                  GroupHabit AS (
                                                      SELECT 
                                                        habit_date,
                                                        rn,
                                                        DATE(habit_date, '-' || (ROW_NUMBER() OVER (ORDER BY habit_date) - 1) || ' days') AS GroupingSet
                                                      FROM DateWithRowNumber
                                                  )
                                                  
                                                  SELECT 
                                                    COUNT(GroupingSet) AS streak,
                                                    min(habit_date) AS start_date,
                                                    max(habit_date) AS end_date
                                                  FROM 
                                                    GroupHabit
                                                  GROUP BY 
                                                    GroupingSet
                                                  ORDER BY 
                                                    COUNT(GroupingSet) DESC, start_date DESC
                                                  LIMIT 1;
                                                  """;
}