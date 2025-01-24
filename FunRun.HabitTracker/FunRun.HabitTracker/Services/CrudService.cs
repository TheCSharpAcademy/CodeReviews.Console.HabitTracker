using Microsoft.Extensions.Logging;
using FunRun.HabitTracker.Data.Model;
using FunRun.HabitTracker.Services.Interfaces;
using Spectre.Console;

namespace FunRun.HabitTracker.Services;

public class CrudService : ICrudService
{
    private ILogger<CrudService> _log;
    private ISQLOperations _sql;
    public CrudService(ILogger<CrudService> log, ISQLOperations sql)
    {
        _log = log;
        _sql = sql;
    }


    public List<HabitModel> ReturnAllHabits()
    {
        try
        {
            _log.LogInformation("Looking for all habits");

            var result = _sql.SQLReadAllHabits();

            return result;
        }
        catch (Exception ex)
        {
            _log.LogError("An Error happend looking for all Habbits: {mes}", ex.Message);
            return new List<HabitModel>();
        }
    }

    public void CreateOneHabit(HabitModel newHabit)
    {
        try
        {
             _sql.SQLCreateHabit(newHabit);
            _log.LogInformation($"Success: newHabit:[{newHabit.HabitName}; {newHabit.HabitDescription}; {newHabit.HabitCounter}]");

        }
        catch (Exception ex)
        {
            _log.LogError("An Error happend CreateOneHabit: {mes}", ex.Message);
        }
    }

    public void UpdateOneHabit(HabitModel newHabit)
    {
        try
        {
            _sql.SQLUpdateHabit(newHabit);
            _log.LogInformation($"Success: newHabit:[{newHabit.HabitName}; {newHabit.HabitDescription}; {newHabit.HabitCounter}]");

        }
        catch (Exception ex)
        {
            _log.LogError("An Error happend UpdateOneHabit: {mes}", ex.Message);
        }
    }

    public void DeleteOneHabit(HabitModel newHabit)
    {
        try
        {
            _sql.SQLDeleteHabit(newHabit);
            _log.LogInformation($"Success: DeleteOneHabit:[{newHabit.HabitName}; {newHabit.HabitDescription}; {newHabit.HabitCounter}]");

        }
        catch (Exception ex)
        {
            _log.LogError("An Error happend DeleteOneHabit: {mes}", ex.Message);
        }
    }


}
