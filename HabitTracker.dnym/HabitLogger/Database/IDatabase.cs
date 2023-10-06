using HabitLogger.Models;

namespace HabitLogger.Database;

internal interface IDatabase
{
    public void InsertRecord(HabitRecord record);
    public HabitRecord? GetRecord(int id);
    public List<HabitRecord> GetRecords(int limit = int.MaxValue, int skip = 0);
    public int GetRecordsCount();
    public void UpdateRecord(HabitRecord record);
    public void DeleteRecord(int id);
}
