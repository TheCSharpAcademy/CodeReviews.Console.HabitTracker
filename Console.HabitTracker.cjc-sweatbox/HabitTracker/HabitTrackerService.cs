// --------------------------------------------------------------------------------------------------
// HabitTracker.HabitTrackerService
// --------------------------------------------------------------------------------------------------
// Provides interations from view to database.
// --------------------------------------------------------------------------------------------------
using HabitTracker.Data.Managers;
using HabitTracker.Models;

namespace HabitTracker;

public class HabitTrackerService
{
    #region Fields

    private SqliteDataManager _dataManager;

    #endregion
    #region Constructors
    
    public HabitTrackerService(string databaseConnectionString)
    {
        _dataManager = new SqliteDataManager(databaseConnectionString);
    }

    #endregion
    #region Methods: Public - Habit

    public void AddHabit(string name, string measure)
    {
        _dataManager.AddHabit(name, measure);
    }

    public Habit? GetHabit(int id)
    {
        var output = _dataManager.GetHabit(id);

        return output == null ? null : new Habit(output);
    }

    public List<Habit> GetHabits()
    {
        var output = new List<Habit>();

        foreach (var entity in _dataManager.GetHabits())
        {
            output.Add(new Habit(entity));
        }

        return output;
    }

    public List<Habit> GetHabitsByIsActive(bool isActive)
    {
        var output = new List<Habit>();

        foreach (var entity in _dataManager.GetHabitsByIsActive(isActive))
        {
            output.Add(new Habit(entity));
        }

        return output;
    }

    public void SetHabit(int id, string name, string measure)
    {
        _dataManager.SetHabit(id, name, measure);
    }

    public void SetHabitIsActive(int id, bool isActive)
    {
        _dataManager.SetHabitIsActive(id, isActive);
    }

    #endregion
    #region Methods: Public - Habit Log

    public void AddHabitLog(int habitId, DateTime date, int quantity)
    {
        _dataManager.AddHabitLog(habitId, date, quantity);
    }

    public HabitLog? GetHabitLog(int id)
    {
        var output = _dataManager.GetHabitLog(id);

        return output == null ? null : new HabitLog(output);
    }

    public List<HabitLog> GetHabitLogs()
    {
        var output = new List<HabitLog>();

        foreach (var entity in _dataManager.GetHabitLogs())
        {
            output.Add(new HabitLog(entity));
        }

        return output;
    }

    public void SetHabitLog(int id, int habitId, DateTime date, int quantity)
    {
        _dataManager.SetHabitLog(id, habitId, date, quantity);
    }

    public void DeleteHabitLog(int id)
    {
        _dataManager.DeleteHabitLog(id);
    }

    #endregion
    #region Methods: Public - HabitReport

    public List<HabitReport> GetHabitReport()
    {
        var output = new List<HabitReport>();

        foreach (var entity in _dataManager.GetHabitReport())
        {
            output.Add(new HabitReport(entity));
        }

        return output;
    }

    #endregion
    #region Methods: Public - HabitLogReport

    public List<HabitLogReport> GetHabitLogReport()
    {
        var output = new List<HabitLogReport>();

        foreach (var entity in _dataManager.GetHabitLogReport())
        {
            output.Add(new HabitLogReport(entity));
        }

        return output;
    }

    public List<HabitLogReport> GetHabitLogReportByHabitId(int habitId)
    {
        var output = new List<HabitLogReport>();

        foreach (var entity in _dataManager.GetHabitLogReportByHabitId(habitId))
        {
            output.Add(new HabitLogReport(entity));
        }

        return output;
    }

    public List<HabitLogSumReport> GetHabitLogSumReport(DateTime dateFrom, DateTime dateTo)
    {
        var output = new List<HabitLogSumReport>();

        foreach (var entity in _dataManager.GetHabitLogSumReport(dateFrom, dateTo))
        {
            output.Add(new HabitLogSumReport(entity));
        }

        return output;
    }

    public List<HabitLogSumReport> GetHabitLogSumReportByHabitId(DateTime dateFrom, DateTime dateTo, int habitId)
    {
        var output = new List<HabitLogSumReport>();

        foreach (var entity in _dataManager.GetHabitLogSumReportByHabitId(dateFrom, dateTo, habitId))
        {
            output.Add(new HabitLogSumReport(entity));
        }

        return output;
    }

    #endregion
    #region Methods: Public - SeedDatabase

    public void SeedDatabase()
    {
        _dataManager.SeedDatabase();
    }

    #endregion

}
