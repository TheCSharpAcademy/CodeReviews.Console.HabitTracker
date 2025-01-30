using Microsoft.Extensions.Logging;
using HabitTracker.FunRunRushFlush.Data.Model;
using HabitTracker.FunRunRushFlush.Services.Interfaces;
using Spectre.Console;

namespace HabitTracker.FunRunRushFlush.Services;

public class CrudService : ICrudService
{
    private ILogger<CrudService> _log;
    private ISqlOperations _sql;
    public CrudService(ILogger<CrudService> log, ISqlOperations sql)
    {
        _log = log;
        _sql = sql;
    }


    public List<HabitModel> ReturnAllHabits()
    {
        try
        {
            _log.LogInformation("Looking for all habits");

            var result = _sql.SqlReadAllHabits();

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
             _sql.SqlCreateHabit(newHabit);
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
            _sql.SqlUpdateHabit(newHabit);
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
            _sql.SqlDeleteHabit(newHabit);
            _log.LogInformation($"Success: DeleteOneHabit:[{newHabit.HabitName}; {newHabit.HabitDescription}; {newHabit.HabitCounter}]");

        }
        catch (Exception ex)
        {
            _log.LogError("An Error happend DeleteOneHabit: {mes}", ex.Message);
        }
    }


}
